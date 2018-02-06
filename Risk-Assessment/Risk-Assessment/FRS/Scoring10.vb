Imports System.Reflection
Imports System.Runtime.CompilerServices

Namespace FRS

    Public Module Scoring10

        Friend Class Coefficients

            Public age, sbp, bmi, smoker, diabetes As Double
            Public gender, trtbp As Double

            Public Shared ReadOnly Keys As String()
            Public Shared ReadOnly fieldReader As Dictionary(Of String, Func(Of Coefficients, Double))

            Default Public ReadOnly Property Item(fieldName As String) As Double
                <MethodImpl(MethodImplOptions.AggressiveInlining)>
                Get
                    Return getField(Me, fieldName)
                End Get
            End Property

            Private Shared Function getField(c As Coefficients, fieldName$) As Double
                With LCase(fieldName)
                    If fieldReader.ContainsKey(fieldName) Then
                        Return fieldReader(fieldName)(c)
                    Else
                        Return 0
                    End If
                End With
            End Function

            Shared Sub New()
                Keys = GetType(Coefficients) _
                    .GetFields(BindingFlags.Public Or BindingFlags.Instance) _
                    .Select(Function(f) f.Name) _
                    .ToArray
                fieldReader = GetType(Coefficients) _
                    .GetFields(BindingFlags.Public Or BindingFlags.Instance) _
                    .ToDictionary(Function(field) field.Name,
                                  Function(field) As Func(Of Coefficients, Double)
                                      Return Function(c As Coefficients) CDbl(field.GetValue(c))
                                  End Function)
            End Sub

            Sub New()
            End Sub

            Sub New(mine As FRSModel)
                Me.age = mine.Age
                Me.bmi = mine.BMI
                Me.diabetes = If(mine.Diabetes, 1, 0)
                Me.gender = If(mine.GenderMale, 1, 0)
                Me.sbp = mine.SBP
                Me.smoker = If(mine.Smoking, 1, 0)
                Me.trtbp = If(mine.TreatedHypertension, 1, 0)
            End Sub

            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Public Function copyObject() As Coefficients
                Return New Coefficients With {
                    .age = age,
                    .bmi = bmi,
                    .diabetes = diabetes,
                    .sbp = sbp,
                    .smoker = smoker,
                    .gender = gender,
                    .trtbp = trtbp
                }
            End Function
        End Class

#Region "Coefficients"

        ReadOnly coefsMaleNoTrtbp As New Coefficients With {
            .age = 3.11296,
            .sbp = 1.85508,
            .bmi = 0.79277,
            .smoker = 0.70953,
            .diabetes = 0.5316
        }
        ReadOnly coefsFemaleNoTrtbp As New Coefficients With {
            .age = 2.72107,
            .sbp = 2.81291,
            .bmi = 0.51125,
            .smoker = 0.61868,
            .diabetes = 0.77763
        }

        Const coefSbpMaleTrtbp = 1.92672   ' replaces sbp coef if being treated
        Const coefSbpFemaleTrtbp = 2.88267 ' replaces sbp coef if being treated
#End Region

        ' Is contribution to some coef*data Or coef*ln(data)?
        ReadOnly useLogData As New Coefficients With {
            .age = 1,
            .sbp = 1,
            .bmi = 1,
            .smoker = 0,
            .diabetes = 0
        }

        ' Constant term in sum
        Const betaZeroMale = -23.9388
        Const betaZeroFemale = -26.0145

        ' Base for pow() calculation
        Const baseMale = 0.88431
        Const baseFemale = 0.94833

        ' Baseline values. Need to fill in gender And age
        ReadOnly baselineNormalData As New Coefficients With {
            .gender = 0,
            .age = 30,
            .sbp = 125,
            .bmi = 22.5,
            .smoker = 0,
            .diabetes = 0,
            .trtbp = 0
        }

        ReadOnly baselineOptimalData As New Coefficients With {
            .gender = 0,
            .age = 30,
            .sbp = 110,
            .bmi = 22,
            .smoker = 0,
            .diabetes = 0,
            .trtbp = 0
        }

        Private Function calcRisk(data As Coefficients) As Double
            Dim coefs As New Coefficients
            Dim base As Double
            Dim betaZero As Double

            If (data.gender = 1.0R) Then
                ' male
                betaZero = betaZeroMale
                base = baseMale
                coefs = coefsMaleNoTrtbp.copyObject()

                If (data.trtbp = 1) Then
                    coefs.sbp = coefSbpMaleTrtbp
                End If
            Else
                betaZero = betaZeroFemale
                base = baseFemale
                coefs = coefsFemaleNoTrtbp.copyObject()

                If (data.trtbp = 1) Then
                    coefs.sbp = coefSbpFemaleTrtbp
                End If
            End If

            ' do computation
            Dim betaSum = betaZero

            For Each k In Coefficients.Keys
                Dim m = data(k)
                If (useLogData(k) <> 0) Then
                    m = Math.Log(m)
                End If

                Dim dBeta = coefs(k) * m
                betaSum += dBeta
            Next

            Dim Risk = 1.0 - Math.Pow(base, Math.Exp(betaSum))
            Return Risk
        End Function

        ' Binary search to find "normal" age for the given score
        Private Function calcHeartAge(riskVal As Double, gender As Double) As Double
            Dim loAge = 10  ' no real minimum bound, but 10 Is a practical one
            Dim hiAge = 86 ' 85 Is max
            Dim testAge#
            Dim testData = baselineNormalData.copyObject()

            testData.gender = gender

            ' threshold should be < half of the desired accuracy (.5 in this case)
            Do While ((hiAge - loAge) > 0.2)

                testAge = (hiAge + loAge) / 2.0
                testData.age = testAge

                Dim testRisk = calcRisk(testData)

                If (testRisk < riskVal) Then
                    loAge = testAge
                ElseIf (testRisk > riskVal) Then
                    hiAge = testAge
                Else
                    hiAge = testAge
                    loAge = testAge
                End If
            Loop

            Return testAge
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function HeartAge(risk As Risk, genderMale As Boolean) As Double
            Return calcHeartAge(risk.risk, If(genderMale, 1, 0))
        End Function

        <Extension>
        Public Function BMICalc(mine As FRSModel) As Risk
            Dim data As New Coefficients(mine:=mine)
            Dim risk = calcRisk(data)

            ' 'normal' risk    
            Dim testData = baselineNormalData.copyObject()
            testData.age = data.age
            testData.gender = data.gender
            Dim normalRisk = calcRisk(testData)

            ' 'optimal' risk
            testData = baselineOptimalData.copyObject()
            testData.age = data.age
            testData.gender = data.gender
            Dim optRisk = calcRisk(testData)

            Return New Risk With {
                .risk = risk,
                .normalRisk = normalRisk,
                .optimalRisk = optRisk
            }
        End Function
    End Module
End Namespace
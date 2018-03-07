Imports System.Runtime.CompilerServices

Namespace FRS

    Public MustInherit Class Scoring10

#Region "Coefficients"
        Protected ReadOnly coefsMaleNoTrtbp As Coefficients
        Protected ReadOnly coefsFemaleNoTrtbp As Coefficients
        Protected ReadOnly coefSbpMaleTrtbp#   ' replaces sbp coef if being treated
        Protected ReadOnly coefSbpFemaleTrtbp# ' replaces sbp coef if being treated
#End Region

        ''' <summary>
        ''' Is contribution to some coef*data Or coef*ln(data)?
        ''' </summary>
        Protected ReadOnly useLogData As Coefficients

        ' Constant term in sum
        Protected ReadOnly betaZeroMale#
        Protected ReadOnly betaZeroFemale#

        ' Base for pow() calculation
        Protected ReadOnly baseMale#
        Protected ReadOnly baseFemale#

        ' Baseline values. Need to fill in gender And age
        Protected ReadOnly baselineNormalData As Coefficients
        Protected ReadOnly baselineOptimalData As Coefficients

        Protected Sub New(coefsMaleNoTrtbp As Coefficients,
                          coefsFemaleNoTrtbp As Coefficients,
                          coefSbpMaleTrtbp#,
                          coefSbpFemaleTrtbp#,
                          useLogData As Coefficients,
                          betaZeroMale#,
                          betaZeroFemale#,
                          baseMale#,
                          baseFemale#,
                          baselineNormalData As Coefficients,
                          baselineOptimalData As Coefficients)

            Me.baseFemale = baseFemale
            Me.baselineNormalData = baselineNormalData
            Me.baselineOptimalData = baselineOptimalData
            Me.baseMale = baseMale
            Me.betaZeroFemale = betaZeroFemale
            Me.betaZeroMale = betaZeroMale
            Me.coefSbpFemaleTrtbp = coefSbpFemaleTrtbp
            Me.coefSbpMaleTrtbp = coefSbpMaleTrtbp
            Me.coefsFemaleNoTrtbp = coefsFemaleNoTrtbp
            Me.coefsMaleNoTrtbp = coefsMaleNoTrtbp
            Me.useLogData = useLogData
        End Sub

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
            Dim loAge# = 10  ' no real minimum bound, but 10 Is a practical one
            Dim hiAge# = 86 ' 85 Is max
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
        Public Function HeartAge(risk As Risk, genderMale As Boolean) As Double
            Return calcHeartAge(risk.risk, If(genderMale, 1, 0))
        End Function

        Public Function CalcRisk(mine As FRSModel) As Risk
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
    End Class
End Namespace
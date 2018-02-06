Imports System.Reflection
Imports System.Runtime.CompilerServices

Namespace FRS

    Partial Class Scoring10

        Protected Friend Class Coefficients

            Public age, sbp, bmi, smoker, diabetes As Double
            Public gender, trtbp As Double
            Public tcl, hdl As Double

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
                Me.tcl = mine.TC
                Me.hdl = mine.HDL
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
                    .trtbp = trtbp,
                    .hdl = hdl,
                    .tcl = tcl
                }
            End Function
        End Class
    End Class
End Namespace
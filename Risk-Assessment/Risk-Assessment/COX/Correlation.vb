Imports Microsoft.VisualBasic.Data.csv.IO

Namespace COX

    Public Class Correlation : Inherits EntityObject

        Public Property Name As String

        Public Function Correlation(name As String) As Integer
            Dim value$

            If Not Properties.ContainsKey(name) Then
                value = Properties _
                    .Where(Function(tuple) tuple.Key.TextEquals(name)) _
                    .FirstOrDefault _
                    .Value
            Else
                value = Me(name)
            End If

            If value.StringEmpty Then
                Return 0
            ElseIf value = "+" Then
                Return 1
            ElseIf value = "-" Then
                Return -1
            Else
                Throw New NotImplementedException
            End If
        End Function
    End Class
End Namespace

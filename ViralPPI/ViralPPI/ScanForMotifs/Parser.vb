Imports System.Text.RegularExpressions

Namespace SFM

    Public Module Parser

        Const deliHeader As String = "#+-+"

        Public Function ParseFile(path As String) As Output
            Dim text As String = path.ReadAllText
            Dim parts As String() = Regex.Split(text, deliHeader)

        End Function
    End Module
End Namespace
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace SFM

    Public Class Inputs

        Public Property bufs As Dictionary(Of String, String())

        Public Overrides Function ToString() As String
            Return bufs.GetJson
        End Function
    End Class
End Namespace
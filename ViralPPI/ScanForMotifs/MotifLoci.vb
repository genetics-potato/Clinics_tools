Imports Microsoft.VisualBasic.Serialization.JSON

Namespace SFM

    Public Class MotifLoci

        Public Property Identifier As String
        Public Property Sequence_header As String
        Public Property UTR_Start As String
        Public Property UTR_END As String
        Public Property MSA_Start As String
        Public Property MSA_END As String
        Public Property UTR_Sequence_Fragment As String
        Public Property MSA_Sequence_Fragment As String
        Public Property additions As Dictionary(Of String, String)

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace
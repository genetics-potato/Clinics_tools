Imports Microsoft.VisualBasic.Data.csv
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.genomics.SequenceModel.FASTA

Namespace SFM

    Public Class Output

        ''' <summary>
        ''' Input detalis
        ''' </summary>
        ''' <returns></returns>
        Public Property inputs As Inputs
        ''' <summary>
        ''' Input Sequence/Alignement
        ''' </summary>
        ''' <returns></returns>
        Public Property Alignment As FastaFile

#Region "#####-------------------------------------------------- Output -----------------------------------------------------------------------------------------"

        ''' <summary>
        ''' Identified regulatory elements from Transterm
        ''' </summary>
        ''' <returns></returns>
        Public Property Regulatory As MotifLoci()
        ''' <summary>
        ''' identified protein binding sites from RBPDB
        ''' </summary>
        ''' <returns></returns>
        Public Property RBPDB As MotifLoci()
        ''' <summary>
        ''' Identified 8 base seed sequence targets from human microRNA's (miRBase)
        ''' </summary>
        ''' <returns></returns>
        Public Property miRBase As MotifLoci()
#End Region

        ''' <summary>
        ''' Save data files into a data export directory.
        ''' </summary>
        ''' <param name="EXPORT"></param>
        Public Sub Save(EXPORT As String)
            Call inputs.bufs.GetJson.SaveTo(EXPORT & "/inputDetails.json")
            Call Alignment.Save(EXPORT & "/inputAlignments.fasta", Encodings.ASCII)
            Call Regulatory.SaveTo(EXPORT & "/regulatory_elementary.csv")
            Call RBPDB.SaveTo(EXPORT & "/protein_bindings.csv")
            Call miRBase.SaveTo(EXPORT & "/human_microRNA.csv")
        End Sub
    End Class
End Namespace
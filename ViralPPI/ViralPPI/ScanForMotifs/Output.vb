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
    End Class
End Namespace
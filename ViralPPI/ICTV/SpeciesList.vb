Imports Microsoft.VisualBasic.Data.csv.StorageProvider.Reflection
Imports Microsoft.VisualBasic.Serialization.JSON

''' <summary>
''' ``ICTV Master Species List 2015 v1.csv``::``ICTV 2015 Master Species #30``
''' </summary>
Public Class SpeciesList

    Public Property Order As String
    Public Property Family As String
    Public Property Subfamily As String
    Public Property Genus As String
    Public Property Species As String
    <Column("Type Species?")> Public Property TypeSpecies As Boolean
    <Column("Exemplar Accession Number")> Public Property ExemplarAccessionNumber As String
    <Column("Exemplar Isolate")> Public Property ExemplarIsolate As String
    <Column("Genome Composition")> Public Property GenomeComposition As String
    <Column("Last Change")> Public Property LastChange As String
    <Column("MSL Of Last Change")> Public Property MSLOfLastChange As String
    Public Property Proposal As String
    <Column("Taxon History URL")> Public Property TaxonHistoryURL As String

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function
End Class

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.Data.csv
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.UnixBash
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization
Imports SMRUCC.Clinic.ViralPPI.SFM
Imports SMRUCC.genomics.Analysis.SequenceTools.SequencePatterns.Topologically
Imports SMRUCC.genomics.ComponentModel.Loci

Public Module SiteMatches

    Public Function LoadPalindrome(DIR As String, Optional maxMatch As Integer = 3) As Dictionary(Of String, ImperfectPalindrome())
        Dim files = ls - l - r - wildcards("*.csv") <= DIR
        Dim gp = From path As String
                 In files
                 Select path.BaseName, path
                 Group path By id = BaseName.Split("_"c).First.ToLower Into Group
        Dim out As New Dictionary(Of String, ImperfectPalindrome())

        For Each id In gp
            Dim data As ImperfectPalindrome() =
                id _
                .Group _
                .Select(Function(file) file.LoadCsv(Of ImperfectPalindrome)) _
                .MatrixToVector
            out(id.id) = data _
                .Where(Function(x) x.MaxMatch >= maxMatch) _
                .ToArray
        Next

        Return out
    End Function

    <Extension>
    Public Function MatchPalindrome(ByRef locis As MotifLoci(), palindrome As Dictionary(Of String, ImperfectPalindrome()), title As String) As MotifLoci()
        Dim GetValues = DynamicsConfiguration.ToDictionary(Of ImperfectPalindrome)()

        For Each loci As MotifLoci In locis
            Dim id As String = loci.Sequence_header.Split("|"c).First.ToLower
            If Not palindrome.ContainsKey(id) Then
                Continue For
            End If
            Dim data As ImperfectPalindrome() = palindrome(id)
            Dim lociPos As New Location(
                {loci.UTR_Start, loci.MSA_Start}.Min,
                {loci.UTR_END, loci.MSA_END}.Max)

            If lociPos.Left = 0 OrElse lociPos.Right = 0 Then
                Continue For
            End If

            Dim site = LinqAPI.DefaultFirst(Of ImperfectPalindrome) <=
                From x As ImperfectPalindrome
                In data.AsParallel
                Let pos = x.MappingLocation
                Where pos.InsideOrOverlapWith(lociPos) OrElse
                    lociPos.InsideOrOverlapWith(pos)
                Select x

            If Not site Is Nothing Then
                Dim value = GetValues(site)

                For Each val As KeyValuePair(Of String, String) In value
                    loci.additions.Add(title & "." & val.Key, val.Value)
                Next
            End If
        Next

        Return locis
    End Function
End Module

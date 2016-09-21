Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.Data.csv
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.UnixBash
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Serialization
Imports SMRUCC.Clinic.ViralPPI.SFM
Imports SMRUCC.genomics.Analysis.SequenceTools.SequencePatterns.LociFilter
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

    Public Function LoadRepeats(Of T As RepeatsView)(DIR As String,
                                                     Optional interval As Integer = 2000,
                                                     Optional compare As Compares = Compares.Interval) As Dictionary(Of String, T())
        Dim files As IEnumerable(Of String) = ls - l - r - wildcards("*.csv") <= DIR
        Dim gp = From path As String
                 In files
                 Select path.BaseName, path
                 Group path By id = BaseName.Split("_"c).First.ToLower Into Group
        Dim out As New Dictionary(Of String, T())

        For Each id In gp
            Dim data As T() =
                id _
                .Group _
                .Select(Function(file) file.LoadCsv(Of T)) _
                .MatrixToVector

            If GetType(T).Equals(GetType(RepeatsView)) Then
                out(id.id) = data.Filtering(interval, compare, False).ToArray
            Else
                Dim revs As RevRepeatsView() = data _
                    .ToArray(Function(x) DirectCast(DirectCast(x, Object), RevRepeatsView))
                out(id.id) = FilteringRev(revs, interval, compare, False) _
                    .ToArray(Function(x) DirectCast(DirectCast(x, Object), T))
            End If
        Next

        Return out
    End Function

    <Extension>
    Public Function MatchRepeats(ByRef locis As MotifLoci(), repeats As Dictionary(Of String, RepeatsView()), title As String) As MotifLoci()
        Dim GetValues = DynamicsConfiguration.ToDictionary(Of RepeatsView)()

        For Each loci As MotifLoci In locis
            Dim id As String = loci.Sequence_header.Split("|"c).First.ToLower
            If Not repeats.ContainsKey(id) Then
                Continue For
            End If
            Dim data As RepeatsView() = repeats(id)
            Dim lociPos As New Location(
                {loci.UTR_Start, loci.MSA_Start}.Min,
                {loci.UTR_END, loci.MSA_END}.Max)

            If lociPos.Left = 0 OrElse lociPos.Right = 0 Then
                Continue For
            End If

            Dim site = LinqAPI.DefaultFirst(Of RepeatsView) <=
                From x As RepeatsView
                In data.AsParallel
                Let len As Integer = x.SequenceData.Length
                Let poss = {
                    New Location(x.Left, x.Left + len) +
                    x.Locis.ToList(Function(l) New Location(l, l + len))
                }.MatrixAsIterator
                Where Not (From pos As Location
                           In poss
                           Where pos.InsideOrOverlapWith(lociPos) OrElse
                               lociPos.InsideOrOverlapWith(pos)
                           Select pos).FirstOrDefault Is Nothing
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

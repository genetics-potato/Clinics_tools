Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Data.csv
Imports SMRUCC.Clinic.ViralPPI.SFM
Imports SMRUCC.genomics.Analysis.SequenceTools.SequencePatterns.Topologically

Module Program

    Public Function Main() As Integer
        Return GetType(CLI).RunCLI(App.CommandLine)
    End Function
End Module

Module CLI

    <ExportAPI("/Match.Sites.Palindrome", Usage:="/Match.Sites.Palindrome /in <motifs.csv> /pals <palindrome.DIR> [/cut <3> /out <out.csv>]")>
    Public Function MatchSitesPalindrome(args As CommandLine) As Integer
        Dim [in] As String = args("/in")
        Dim pals As String = args("/pals")
        Dim cut As Integer = args.GetValue("/cut", 3)
        Dim out As String = args.GetValue("/out", [in].TrimSuffix & $".{pals.BaseName},cut={cut}.csv")
        Dim sites = [in].LoadCsv(Of MotifLoci).ToArray
        Dim palData = SiteMatches.LoadPalindrome(pals, cut)

        Return SiteMatches.MatchPalindrome(
            sites, palData, pals.BaseName).SaveTo(out).CLICode
    End Function

    <ExportAPI("/Group.Count", Usage:="/Group.Count /in <motifLoci.csv> [/out <out.csv>]")>
    Public Function GroupCounts(args As CommandLine) As Integer
        Dim [in] As String = args("/in")
        Dim out As String = args.GetValue("/out", [in].TrimSuffix & ".group_count.csv")
        Dim data = [in].LoadCsv(Of MotifLoci)
        Dim key As String = data.First.__getMathKey
        Dim gm = From x As MotifLoci
                 In data
                 Select x
                 Group x By x.Sequence_header Into Group
        Dim views = From x
                    In gm
                    Let sites As MotifLoci() = x.Group.ToArray
                    Let has As MotifLoci() = sites.Where(
                        Function(s) s.additions.ContainsKey(key) AndAlso
                        Not String.IsNullOrEmpty(s.additions(key))).ToArray
                    Select x.Sequence_header,
                        count = sites.Length,
                        matchs = has.Length,
                        avgMatch = If(
                            has.Length = 0,
                            0,
                            has.Average(
                            Function(s) CInt(Val(s.additions(key)))))

        Return views.ToArray.SaveTo(out).CLICode
    End Function

    <Extension>
    Private Function __getMathKey(s As MotifLoci) As String
        For Each key As String In s.additions.Keys
            If InStr(key, NameOf(ImperfectPalindrome.MaxMatch)) > 1 Then
                Return key
            End If
        Next

        Return Nothing
    End Function
End Module
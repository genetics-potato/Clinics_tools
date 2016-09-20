Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.DocumentFormat.Csv
Imports SMRUCC.Clinic.ViralPPI.SFM

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
End Module
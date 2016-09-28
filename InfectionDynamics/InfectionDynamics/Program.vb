Imports System.IO
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.Data.Bootstrapping
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Mathematical
Imports Microsoft.VisualBasic.Mathematical.BasicR
Imports Microsoft.VisualBasic.Mathematical.diffEq
Imports Microsoft.VisualBasic.Mathematical.Plots
Imports Microsoft.VisualBasic.Mathematical.SyntaxAPI.MathExtension
Imports Microsoft.VisualBasic.Serialization.JSON
Imports RDotNET.Extensions.Bioinformatics.deSolve
Imports RDotNET.Extensions.Bioinformatics.deSolve.API
Imports RDotNET.Extensions.VisualBasic
Imports RDotNET.Extensions.VisualBasic.API.base
Imports RDotNET.Extensions.VisualBasic.API.utils

Module Program

    Sub Main()

        Dim observation As ODEsOut = New Kinetics_of_influenza_A_virus_infection_in_humans().Solve(10000, 0, 100)

        Call MonteCarlo.Iterations(GetType(Model), observation, 1000, 20, ).GetJson.__DEBUG_ECHO


        'Dim sfsfds = ODEsOut.LoadFromDataFrame("X:\RNA-seq\be\1\1V.csv")

        'Dim ppppaspdasds = Analysis.GroupsAnalysis("X:\RNA-seq\be\", Kinetics_of_influenza_A_virus_infection_in_humans.GetAnalysis, 5)

        'Call ppppaspdasds.GetJson.SaveTo("x:\afsdfsd.json")


        'Dim result = New Kinetics_of_influenza_A_virus_infection_in_humans().Solve(10000, 0, 7)

        ''result.y("V") = New NamedValue(Of Double()) With {
        ''.Name = "V",
        ''.x = Log(New Vector(result.y("V").x), 2).ToArray
        ''}

        ''result.y("I") = New NamedValue(Of Double()) With {
        ''.Name = "I",
        ''.x = VectorMath.Log(New Vector(result.y("I").x)).ToArray
        '' }
        ''result.y("T") = New NamedValue(Of Double()) With {
        ''.Name = "T",
        ''.x = VectorMath.Log(New Vector(result.y("T").x)).ToArray
        '' }

        'Call result.DataFrame.Save("./Kinetics_of_influenza_A_virus_infection_in_humans.csv", Encodings.ASCII)
        'Dim rnd As New PreciseRandom(10 ^ -23, 300)

        'Using ffdfdsf As StreamWriter = "x:\sdfsdfdsfs.txt".OpenWriter
        '    For Isdfdsfs = 0 To 1000000
        '        Call ffdfdsf.WriteLine(rnd.NextNumber)
        '    Next
        'End Using
        'Pause()
        ' bootstrapping test

        'Dim uid As New Uid


        'Dim allRange As New PreciseRandom(-10, 10)   ' 1e-10 -> 1e10
        'Dim I As New NamedValue(Of PreciseRandom) With {.Name = NameOf(I), .x = New PreciseRandom(-10, 3)}
        'Dim T As New NamedValue(Of PreciseRandom) With {.Name = NameOf(T), .x = allRange} 'New PreciseRandom(10 ^ 3, 10 ^ 10)
        'Dim V As New NamedValue(Of PreciseRandom) With {.Name = NameOf(V), .x = allRange} 'New PreciseRandom(10 ^ -6, 10)

        'Dim p As New NamedValue(Of PreciseRandom) With {.Name = NameOf(p), .x = allRange} ' New PreciseRandom(10 ^ -5, 1)
        'Dim c As New NamedValue(Of PreciseRandom) With {.Name = NameOf(c), .x = allRange} 'New PreciseRandom(10 ^ -5, 10)
        'Dim beta As New NamedValue(Of PreciseRandom) With {.Name = NameOf(beta), .x = allRange} ' New PreciseRandom(10 ^ -10, 1)
        'Dim delta As New NamedValue(Of PreciseRandom) With {.Name = NameOf(delta), .x = allRange} ' New PreciseRandom(10 ^ -5, 150)



        'For Each x In Bootstrapping(Of Kinetics_of_influenza_A_virus_infection_in_humans)({p, c, beta, delta}, {T, I, V}, Long.MaxValue, 10000, 0, 200)
        '    Dim id As String = ++uid
        '    Dim path As String = App.HOME & $"/be/{id.First}/{id}.csv"
        '    Call x.DataFrame.Save(path, Encodings.ASCII)
        'Next

        'Pause()



        '  Dim result = New Kinetics_of_influenza_A_virus_infection_in_humans().Solve(10000, 0, 7)

        'result.y("V") = New NamedValue(Of Double()) With {
        '.Name = "V",
        '.x = Log(New Vector(result.y("V").x), 2).ToArray
        '}

        'result.y("I") = New NamedValue(Of Double()) With {
        '.Name = "I",
        '.x = VectorMath.Log(New Vector(result.y("I").x)).ToArray
        ' }
        'result.y("T") = New NamedValue(Of Double()) With {
        '.Name = "T",
        '.x = VectorMath.Log(New Vector(result.y("T").x)).ToArray
        ' }

        '   Call result.DataFrame.Save("./Kinetics_of_influenza_A_virus_infection_in_humans.csv", Encodings.ASCII)
        'Call Scatter.Plot(result.FromODEs(, 10, 10)).SaveAs("./Kinetics_of_influenza_A_virus_infection_in_humans.png")


        'result = New Ebola_virus_infection_modeling_and_identifiability_problems().Solve(10000, 0, 7)
        'Call result.DataFrame.Save("./Ebola_virus_infection_modeling_and_identifiability_problems.csv", Encodings.ASCII)
        'Call Scatter.Plot(result.FromODEs(, 10, 10)).SaveAs("./Ebola_virus_infection_modeling_and_identifiability_problems.png")


        'With New ddd

        '    Dim x As New RDotNET.Extensions.VisualBasic.var

        '    .call = "x3 <- ff"




        'End With
    End Sub
End Module
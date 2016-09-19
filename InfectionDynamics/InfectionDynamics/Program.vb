Imports RDotNET.Extensions.VisualBasic
Imports RDotNET.Extensions.VisualBasic.API.base
Imports RDotNET.Extensions.VisualBasic.API.utils
Imports RDotNET.Extensions.Bioinformatics.deSolve.API
Imports RDotNET.Extensions.Bioinformatics.deSolve
Imports Microsoft.VisualBasic.Mathematical.Plots
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Mathematical.BasicR
Imports Microsoft.VisualBasic.Mathematical.SyntaxAPI.MathExtension
Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.Mathematical.diffEq
Imports Microsoft.VisualBasic.Language

Module Program

    Sub Main()

        ' bootstrapping test

        Dim T As New NamedValue(Of DoubleRange) With {.Name = NameOf(T), .x = New DoubleRange(10 ^ 5, 10 ^ 10)}
        Dim I As New NamedValue(Of DoubleRange) With {.Name = NameOf(I), .x = New DoubleRange(0, 10 ^ 3)}
        Dim V As New NamedValue(Of DoubleRange) With {.Name = NameOf(V), .x = New DoubleRange(10 ^ -10, 1)}

        Dim p As New NamedValue(Of DoubleRange) With {.Name = NameOf(p), .x = New DoubleRange(10 ^ -4, 10)}
        Dim c As New NamedValue(Of DoubleRange) With {.Name = NameOf(c), .x = New DoubleRange(10 ^ -2, 10)}
        Dim beta As New NamedValue(Of DoubleRange) With {.Name = NameOf(beta), .x = New DoubleRange(10 ^ -8, 1)}
        Dim delta As New NamedValue(Of DoubleRange) With {.Name = NameOf(delta), .x = New DoubleRange(10 ^ -2, 10)}
        Dim uid As New Uid

        For Each x In BootstrapEstimate.Bootstrapping(Of Kinetics_of_influenza_A_virus_infection_in_humans)({p, c, beta, delta}, {T, I, V}, Integer.MaxValue, 1000000, 0, 50)
            Dim id As String = ++uid
            Dim path As String = App.HOME & $"/be/{id.First}/{id}.csv"
            Call x.DataFrame.Save(path, Encodings.ASCII)
        Next

        Pause()



        Dim result = New Kinetics_of_influenza_A_virus_infection_in_humans().Solve(10000, 0, 7)

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

        Call result.DataFrame.Save("./Kinetics_of_influenza_A_virus_infection_in_humans.csv", Encodings.ASCII)
        Call Scatter.Plot(result.FromODEs(, 10, 10)).SaveAs("./Kinetics_of_influenza_A_virus_infection_in_humans.png")


        result = New Ebola_virus_infection_modeling_and_identifiability_problems().Solve(10000, 0, 7)
        Call result.DataFrame.Save("./Ebola_virus_infection_modeling_and_identifiability_problems.csv", Encodings.ASCII)
        Call Scatter.Plot(result.FromODEs(, 10, 10)).SaveAs("./Ebola_virus_infection_modeling_and_identifiability_problems.png")


        With New ddd

            Dim x As New RDotNET.Extensions.VisualBasic.var

            .call = "x3 <- ff"




        End With
    End Sub


    Class ddd

        WriteOnly Property [call] As String
            Set(value As String)

            End Set
        End Property
    End Class
End Module
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

Module Program

    Sub Main()

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

    End Sub
End Module
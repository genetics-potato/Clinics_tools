Imports RDotNET.Extensions.VisualBasic
Imports RDotNET.Extensions.VisualBasic.API.base
Imports RDotNET.Extensions.VisualBasic.API.utils
Imports RDotNET.Extensions.Bioinformatics.deSolve.API
Imports RDotNET.Extensions.Bioinformatics.deSolve
Imports Microsoft.VisualBasic.Mathematical.Plots
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Mathematical.BasicR

Module Program

    Sub Main()

        Dim result = New Kinetics_of_influenza_A_virus_infection_in_humans().Solve(1000, 0, 8)

        'result.y("V") = New NamedValue(Of Double()) With {
        '    .Name = "V",
        '    .x = -Log(New Vector(result.y("V").x))
        '}

        Call result.DataFrame.Save("./Kinetics_of_influenza_A_virus_infection_in_humans.csv", Encodings.ASCII)
        Call Scatter.Plot(result.FromODEs).SaveAs("./Kinetics_of_influenza_A_virus_infection_in_humans.png")

        Pause()

        Call $"p      <- {p}".丶
        Call $"beta   <- {beta}".丶
        Call $"c      <- {c}".丶
        Call $"delta  <- {delta}".丶
        Call $"U0     <- {U0}".丶
        Call $"rho    <- {rho}".丶
        Call $"lambda <- {lambda}".丶

        ' The initial value for infected cells(I0) is set to zero.
        ' The best model based on the Akaike Information Criterion(AIC) is presented in Figure3, providing an estimate of 9 ffu/ml for V0.
        ' The initial number of susceptible cells(U0) can be taken from the experiment in Half mannetal. (2008) as 5 × 10^5.
        Dim yini = "c(U=U0, I=0, V=9)"
        Dim model = [function](
            {
                "t",
                "y",
                "params"
            },
 _
             "with(as.list(y), {
    
                dU <- lambda - rho * U - beta * U * V
                dI <- beta * U * V - delta * I
                dV <- p * I - c * V

                list(c(dU, dI, dV))
})")

        require("deSolve")

        Dim times = seq([from]:=0, [to]:=5.6, by:=0.01)
        Dim out = ode(y:=yini,
                      times:=times,
                      func:=model,
                      parms:=NULL,
                      method:=integrator.rk4)

        Call write.csv(out, "x:\ebola_test.csv")

        Pause()
    End Sub

    ''' <summary>
    ''' EBOV is known to replicate at an unusually high rate that
    ''' overwhelms the protein synthesis of infected cells(Sanchez,
    ''' 2001).Consistent with this observation, bootstrap estimates
    ''' revealed a very high rate of viral replication, p = 62(95%CI 
    ''' 31 − 580)(Table1).
    ''' </summary>
    Const p As Double = 378 '62

    Dim I, V, U As Double

    ''' <summary>
    ''' Although the scatter plot in Figure5 shows
    ''' that the estimate of p can be decreased given a higher effective
    ''' infection rate(β),a replication rate of at least 31.8 ffu/ml cell−1
    ''' day−1 is still needed to achieve a good fit of the viral replication
    ''' kinetics in Figure3.
    ''' </summary>G:\R.Bioinformatics\RDotNET.Extensions.VisualBasic\API\utils\
    Const beta As Double = 1.91 '31.8

    ''' <summary>
    ''' Distributions of
    ''' the model parameters are shown in Figure 5. Bootstrap estimates
    ''' for the viral clearance(median c = 1.05 day−1) is slightly below
    ''' other viral infection results(Table1).
    ''' </summary>
    Const c As Double = 8.02 '1.05

    ''' <summary>
    ''' There is experimental evidence that the half-life
    ''' of epithelium cells in lung is 17–18 months in average(Rawlins
    ''' and Hogan,2008).In view of this,the infected cell death rate(δ)
    ''' is fixed at 10−3.
    ''' </summary>
    Const delta As Double = 10 ^ -3

    ''' <summary>
    ''' The initial number of susceptible cells(U0) can be taken from the experiment in Half mannetal. (2008) as 5 × 10^5.
    ''' </summary>
    Const U0 As Double = 5 * 10 ^ 5
    ''' <summary>
    ''' The parameter ρ is fixed from liter-
    ''' ature as 0.001 day−1 (Moehleretal.,2005).
    ''' </summary>
    Const rho As Double = 0.001

    ''' <summary>
    ''' Note that the condition λ = U0ρ should be satisfied to
    ''' guarantee homeostasis in the absence of viral infection,such that
    ''' only ρ is a parameter to be determined.
    ''' </summary>
    Const lambda As Double = rho * U0

    Public Function dU(x As Double, U As Double) As Double
        Return lambda - rho * U - beta * U * V
    End Function

    Public Function dI(x As Double, I As Double) As Double
        Return beta * U * V - delta * I
    End Function

    Public Function dV(x As Double, V As Double) As Double
        Return p * I - c * V
    End Function

End Module
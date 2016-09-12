﻿Imports RDotNET.Extensions.VisualBasic

Module Program

    Sub Main()

        Call $"p      <- {p}".ζ
        Call $"beta   <- {beta}".ζ
        Call $"c      <- {c}".ζ
        Call $"delta  <- {delta}".ζ
        Call $"U0     <- {U0}".ζ
        Call $"rho    <- {rho}".ζ
        Call $"lambda <- {lambda}".ζ

        ' The initial value for infected cells(I0) is set to zero.
        ' The best model based on the Akaike Information Criterion(AIC) is presented in Figure3, providing an estimate of 9 ffu/ml for V0.
        ' The initial number of susceptible cells(U0) can be taken from the experiment in Half mannetal. (2008) as 5 × 10^5.
        Dim yini = "c(U=U0, I=0, V=9)"
        Dim model = [function](
            {"t", "y", "params"},
 _
             "with(as.list(y), {
    
                dU <- lambda - rho * U - beta * U * V
                dI <- beta * U * V - delta * I
                dV <- p * I - c * V

                list(c(dU, dI, dV))
})")

        Dim times = seq(from = 0, To = 5.6, by = 0.01)
        out   <- ode(y = yini, times = times, func = Lorenz, parms = NULL)

        Dim fU As New ODEs.ODE With {.df = AddressOf dU, .SetY = Sub(x) U = x, .y0 = U0}
        Dim fI As New ODEs.ODE With {.df = AddressOf dI, .SetY = Sub(x) I = x, .y0 = 0R}
        Dim fV As New ODEs.ODE With {.df = AddressOf dV, .SetY = Sub(x) V = x, .y0 = 9}
        Dim solver As New ODEs With {.ODEs = {fU, fI, fV}}
        Call solver.Solve(100, 0, 6)
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
    ''' </summary>
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
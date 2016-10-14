Public NotInheritable Class GlobalMembersGauss
    Private Sub New()
    End Sub
    ' cdf/gauss.c
    '     *
    '     * Copyright (C) 2002, 2004 Jason H. Stover.
    '     *
    '     * This program is free software; you can redistribute it and/or modify
    '     * it under the terms of the GNU General Public License as published by
    '     * the Free Software Foundation; either version 3 of the License, or (at
    '     * your option) any later version.
    '     *
    '     * This program is distributed in the hope that it will be useful, but
    '     * WITHOUT ANY WARRANTY; without even the implied warranty of
    '     * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    '     * General Public License for more details.
    '     *
    '     * You should have received a copy of the GNU General Public License
    '     * along with this program; if not, write to the Free Software
    '     * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307, USA.
    '     


    '
    '     * Computes the cumulative distribution function for the Gaussian
    '     * distribution using a rational function approximation.  The
    '     * computation is for the standard Normal distribution, i.e., mean 0
    '     * and standard deviation 1. If you want to compute Pr(X < t) for a
    '     * Gaussian random variable X with non-zero mean m and standard
    '     * deviation sd not equal to 1, find gsl_cdf_ugaussian ((t-m)/sd).
    '     * This approximation is accurate to at least double precision. The
    '     * accuracy was verified with a pari-gp script.  The largest error
    '     * found was about 1.4E-20. The coefficients were derived by Cody.
    '     *
    '     * References:
    '     *
    '     * W.J. Cody. "Rational Chebyshev Approximations for the Error
    '     * Function," Mathematics of Computation, v23 n107 1969, 631-637.
    '     *
    '     * W. Fraser, J.F Hart. "On the Computation of Rational Approximations
    '     * to Continuous Functions," Communications of the ACM, v5 1962.
    '     *
    '     * W.J. Kennedy Jr., J.E. Gentle. "Statistical Computing." Marcel Dekker. 1980.
    '     * 
    '     *  
    '     





    '
    '     * IEEE double precision dependent constants.
    '     *
    '     * GAUSS_EPSILON: Smallest positive value such that 
    '     *                gsl_cdf_gaussian(x) > 0.5.
    '     * GAUSS_XUPPER: Largest value x such that gsl_cdf_gaussian(x) < 1.0.
    '     * GAUSS_XLOWER: Smallest value x such that gsl_cdf_gaussian(x) > 0.0.
    '     




    Friend Function get_del(x As Double, rational As Double) As Double
        Dim xsq As Double = 0.0
        Dim del As Double = 0.0
        Dim result As Double = 0.0

        xsq = Math.Floor(x * DefineConstants.GAUSS_SCALE) / DefineConstants.GAUSS_SCALE
        del = (x - xsq) * (x + xsq)
        del *= 0.5

        result = Math.Exp(-0.5 * xsq * xsq) * Math.Exp(-1.0 * del) * rational

        Return result
    End Function

    '
    '     * Normal cdf for fabs(x) < 0.66291
    '     

    Friend Function gauss_small(x As Double) As Double
        Dim i As UInteger
        Dim result As Double = 0.0
        Dim xsq As Double
        Dim xnum As Double
        Dim xden As Double

        Dim a As Double() = {2.23525203546068, 161.028231068556, 1067.68948546037, 18154.9812533436, 0.0656823379182074}
        Dim b As Double() = {47.2025819046882, 976.098551737777, 10260.932208619, 45507.7893350267}

        xsq = x * x
        xnum = a(4) * xsq
        xden = xsq

        For i = 0 To 2
            xnum = (xnum + a(i)) * xsq
            xden = (xden + b(i)) * xsq
        Next

        result = x * (xnum + a(3)) / (xden + b(3))

        Return result
    End Function

    '
    '     * Normal cdf for 0.66291 < fabs(x) < sqrt(32).
    '     

    Friend Function gauss_medium(x As Double) As Double
        Dim i As UInteger
        Dim temp As Double = 0.0
        Dim result As Double = 0.0
        Dim xnum As Double
        Dim xden As Double
        Dim absx As Double

        Dim c As Double() = {0.398941512088135, 8.88314979438838, 93.5066561321779, 597.2702763948, 2494.53758529037, 6848.19045053628,
            11602.6514376473, 9842.71483838398, 0.0000000107655767737202}
        Dim d As Double() = {22.2666880443281, 235.387901782625, 1519.37759940755, 6485.55829826676, 18615.5716408851, 34900.952721146,
            38912.0032860933, 19685.42967686}

        absx = Math.Abs(x)

        xnum = c(8) * absx
        xden = absx

        For i = 0 To 6
            xnum = (xnum + c(i)) * absx
            xden = (xden + d(i)) * absx
        Next

        temp = (xnum + c(7)) / (xden + d(7))

        result = GlobalMembersGauss.get_del(x, temp)

        Return result
    End Function

    '
    '     * Normal cdf for 
    '     * {sqrt(32) < x < GAUSS_XUPPER} union { GAUSS_XLOWER < x < -sqrt(32) }.
    '     

    Friend Function gauss_large(x As Double) As Double
        Dim i As Integer
        Dim result As Double
        Dim xsq As Double
        Dim temp As Double
        Dim xnum As Double
        Dim xden As Double
        Dim absx As Double

        Dim p As Double() = {0.215898534057957, 0.127401161160247, 0.0222352778706498, 0.00142161919322789, 0.0000291128749511688, 0.0230734417649402}
        Dim q As Double() = {1.28426009614491, 0.468238212480865, 0.0659881378689286, 0.00378239633202758, 0.0000729751555083966}

        absx = Math.Abs(x)
        xsq = 1.0 / (x * x)
        xnum = p(5) * xsq
        xden = xsq

        For i = 0 To 3
            xnum = (xnum + p(i)) * xsq
            xden = (xden + q(i)) * xsq
        Next

        temp = xsq * (xnum + p(4)) / (xden + q(4))
        temp = (M_1_SQRT2PI - temp) / absx

        result = GlobalMembersGauss.get_del(x, temp)

        Return result
    End Function

    Public Function gsl_cdf_ugaussian_P(x As Double) As Double
        Dim result As Double
        Dim absx As Double = Math.Abs(x)

        If absx < DefineConstants.GSL_DBL_EPSILON / 2 Then
            result = 0.5
            Return result
        ElseIf absx < 0.66291 Then
            result = 0.5 + GlobalMembersGauss.gauss_small(x)
            Return result
        ElseIf absx < 4.0 * M_SQRT2 Then
            result = GlobalMembersGauss.gauss_medium(x)

            If x > 0.0 Then
                result = 1.0 - result
            End If

            Return result
        ElseIf x > DefineConstants.GAUSS_XUPPER Then
            result = 1.0
            Return result
        ElseIf x < DefineConstants.GAUSS_XLOWER Then
            result = 0.0
            Return result
        Else
            result = GlobalMembersGauss.gauss_large(x)

            If x > 0.0 Then
                result = 1.0 - result
            End If
        End If

        Return result
    End Function

    Public Function gsl_cdf_ugaussian_Q(x As Double) As Double
        Dim result As Double
        Dim absx As Double = Math.Abs(x)

        If absx < DefineConstants.GSL_DBL_EPSILON / 2 Then
            result = 0.5
            Return result
        ElseIf absx < 0.66291 Then
            result = GlobalMembersGauss.gauss_small(x)

            If x < 0.0 Then
                result = Math.Abs(result) + 0.5
            Else
                result = 0.5 - result
            End If

            Return result
        ElseIf absx < 4.0 * M_SQRT2 Then
            result = GlobalMembersGauss.gauss_medium(x)

            If x < 0.0 Then
                result = 1.0 - result
            End If

            Return result
        ElseIf x > -(DefineConstants.GAUSS_XLOWER) Then
            result = 0.0
            Return result
        ElseIf x < -(DefineConstants.GAUSS_XUPPER) Then
            result = 1.0
            Return result
        Else
            result = GlobalMembersGauss.gauss_large(x)

            If x < 0.0 Then
                result = 1.0 - result

            End If
        End If

        Return result
    End Function

    Public Function gsl_cdf_gaussian_P(x As Double, sigma As Double) As Double
        Return GlobalMembersGauss.gsl_cdf_ugaussian_P(x / sigma)
    End Function

    Public Function gsl_cdf_gaussian_Q(x As Double, sigma As Double) As Double
        Return GlobalMembersGauss.gsl_cdf_ugaussian_Q(x / sigma)
    End Function
End Class


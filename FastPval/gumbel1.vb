
Public NotInheritable Class GlobalMembersGumbel1
	Private Sub New()
	End Sub
	' cdf/gumbel1.c
'     * 
'     * Copyright (C) 2003, 2007, 2009 Brian Gough
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
'     * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
'     



	Public Shared Function gsl_cdf_gumbel1_P(x As Double, a As Double, b As Double) As Double
		Dim u As Double = a * x - Math.Log(b)
		Dim P As Double = Math.Exp(-Math.Exp(-u))
		Return P
	End Function

	Public Shared Function gsl_cdf_gumbel1_Q(x As Double, a As Double, b As Double) As Double
		Dim u As Double = a * x - Math.Log(b)
		Dim Q As Double
		Dim P As Double = Math.Exp(-Math.Exp(-u))

		If P < 0.5 Then
			Q = 1 - P
		Else
			Q = -expm1(-Math.Exp(-u))
		End If

		Return Q
	End Function
End Class


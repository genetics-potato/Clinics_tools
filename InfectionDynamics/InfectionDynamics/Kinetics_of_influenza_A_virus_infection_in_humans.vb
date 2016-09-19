Imports Microsoft.VisualBasic.Mathematical.BasicR
Imports Microsoft.VisualBasic.Mathematical.diffEq

''' <summary>
''' ##### Kinetics of influenza A virus infection in humans
'''
''' > **DOI** 10.3390/v7102875
''' </summary>
Public Class Kinetics_of_influenza_A_virus_infection_in_humans : Inherits ODEs

    Dim T As var
    Dim I As var
    Dim V As var

    Dim p As Double = 3 * 10 ^ -2
    Dim c As Double = 2
    Dim beta As Double = 8.8 * 10 ^ -6
    Dim delta As Double = 2.6

    Protected Overrides Sub func(dx As Double, ByRef dy As Vector)
        dy(T) = -beta * T * V
        dy(I) = beta * T * V - delta * I
        dy(V) = p * I - c * V
    End Sub

    Protected Overrides Function y0() As var()
        Return {
            V = 1.4 * 10 ^ -2,
            T = 4 * 10 ^ 8,
            I = 0
        }
    End Function
End Class

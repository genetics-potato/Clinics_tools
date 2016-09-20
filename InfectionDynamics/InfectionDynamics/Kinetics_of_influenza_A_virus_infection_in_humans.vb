Imports Microsoft.VisualBasic.Data.Bootstrapping.Analysis
Imports Microsoft.VisualBasic.Mathematical.BasicR
Imports Microsoft.VisualBasic.Mathematical.diffEq
Imports Microsoft.VisualBasic.Mathematical
Imports Microsoft.VisualBasic.Language

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

    ''' <summary>
    ''' 通过数据特征来分析结果
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetAnalysis() As Dictionary(Of String, GetPoints)
        Dim I As GetPoints = Function(data)
                                 Dim a = data.FirstIncrease
                                 Dim iMax = data.MaxIndex
                                 Dim z = data.Skip(iMax).Reach(data.First) + iMax
                                 Return {a, iMax, z}
                             End Function
        Dim T As GetPoints = Function(data)
                                 Dim a = data.FirstDecrease
                                 Dim b = data.Reach(data.First * 0.01)
                                 Return {a, b}
                             End Function
        Dim V As GetPoints = Function(data)
                                 Dim a = data.FirstIncrease
                                 Dim b = data.MaxIndex
                                 Return {a, b}
                             End Function

        Return New Dictionary(Of String, GetPoints) From {
            {NameOf(I), I},
            {NameOf(T), T},
            {NameOf(V), V}
        }
    End Function
End Class

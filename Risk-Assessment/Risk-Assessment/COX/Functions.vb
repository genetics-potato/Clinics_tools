Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Algorithm.base
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Bootstrapping
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Math

Namespace COX

    Public Module Functions

        ''' <summary>
        ''' 生存函数：
        ''' 
        ''' ```
        ''' S(t) = t时刻仍存活的人数/观察的总人数
        ''' ```
        ''' </summary>
        ''' <param name="model"></param>
        ''' <returns></returns>
        <Extension>
        Public Function SurvivalModel(model As IEnumerable(Of Model), Optional offsets# = 1) As PointF()
            Dim timeGroups = model _
                .GroupBy(evaluate:=Function(m) m.time, offsets:=offsets) _
                .OrderBy(Function(t) Val(t.Name)) _
                .ToArray
            Dim ALL = Aggregate time In timeGroups Into Sum(time.Length) ' 总人数
            Dim survival = ALL
            Dim points As New List(Of PointF)

            points += New PointF(Val(timeGroups.First.Name), 1)

            For Each time As NamedCollection(Of Model) In timeGroups
                Dim die = time.Where(Function(m) m.status = COXstatus.Die).Count
                survival -= die
                points += New PointF(Val(time.Name), survival / ALL)
            Next

            Return points
        End Function

        <Extension>
        Public Function SurvivalFit(model As IEnumerable(Of Model), factors%, Optional offsets# = 1) As FitResult
            Dim points = model.SurvivalModel(offsets)
            Dim regression As FitResult = LeastSquares.PolyFit(points.X, points.Y, poly_n:=factors)
            Return regression
        End Function

        <Extension>
        Public Function SurvivalCurve(model As IEnumerable(Of Model), Optional offsets# = 1, Optional pointSize! = 5, <CallerMemberName> Optional name$ = Nothing) As GraphicsData
            Dim points = model.SurvivalModel(offsets)
            Dim curve As New List(Of PointF)

            For Each point In points.SlideWindows(slideWindowSize:=2)
                Dim A = point(0)
                Dim B = point(1)

                curve.Add(A)
                curve.Add(New PointF(B.X, A.Y))
            Next

            Dim serial As New SerialData With {
                .color = Color.Blue,
                .lineType = DashStyle.Solid,
                .PointSize = pointSize,
                .pts = curve _
                    .Select(Function(p)
                                Return New PointData With {
                                    .pt = p
                                }
                            End Function) _
                    .ToArray,
                .title = name
            }

            Return Scatter.Plot({serial})
        End Function
    End Module
End Namespace

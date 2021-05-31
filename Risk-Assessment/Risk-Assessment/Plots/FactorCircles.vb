Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.Scripting.Runtime

Public Module FactorCircles

    ''' <summary>
    ''' 在这里假设因素之间都可以直接进行比较，则面积就与因素的和成正相关
    ''' </summary>
    ''' <param name="factors"></param>
    ''' <param name="size$"></param>
    ''' <param name="margin$"></param>
    ''' <param name="bg$"></param>
    ''' <param name="circleLogo">The logo image or fill color.
    ''' (如果这个是图片的话，应该是``1：1``的正方形图片)</param>
    ''' <returns></returns>
    ''' 
    <Extension>
    Public Function Plot(factors As Factors,
                         Optional size$ = "2700,2000",
                         Optional margin$ = g.DefaultPadding,
                         Optional bg$ = "white",
                         Optional circleLogo$ = "#C8004C",
                         Optional beneficialsColor$ = "#A4D867",
                         Optional detrimentalsColor$ = "#504E53",
                         Optional innerCircle! = 0.5,
                         Optional shadowDistance# = 120,
                         Optional shadowAngle# = 45,
                         Optional scoreFontCSS$ = "font-style: strong; font-size: 300; font-family: " & FontFace.MicrosoftYaHei & ";") As GraphicsData

        Dim scoreFont As Font = CSSFont.TryParse(scoreFontCSS)
        Dim plotInternal =
            Sub(ByRef g As IGraphics, region As GraphicsRegion)
                Dim plotRect As Rectangle = region.PlotRegion
                Dim radius! = Math.Min(plotRect.Width, plotRect.Height) / 2 * 0.95
                Dim innerRadius = radius * innerCircle
                Dim center As PointF = plotRect.Centre

                ' 首先确定内圈的位置
                Dim innerTopLeft As New PointF With {
                    .X = center.X - innerRadius,
                    .Y = center.Y - innerRadius
                }
                Dim innerRect As New RectangleF(innerTopLeft, New SizeF(innerRadius * 2, innerRadius * 2))
                Dim outerTopLeft As New PointF With {
                    .X = center.X - radius,
                    .Y = center.Y - radius
                }

                Dim outerRect = New RectangleF(outerTopLeft, New SizeF(radius * 2, radius * 2))

                ' 首先需要进行阴影的绘制
                With outerRect.Location.MovePoint(shadowAngle, shadowDistance)
                    Dim circle As New GraphicsPath

                    Call circle.AddEllipse(.X, .Y, CSng(radius * 2.05), CSng(radius * 2.05))
                    Call circle.CloseAllFigures()
                    Call Shadow.DropdownShadows(g, polygon:=circle)
                End With

                ' 然后绘制中间的logo
                If circleLogo.FileExists Then
                    Using logoImage As Image = circleLogo.LoadImage
                        Call g.DrawImage(logoImage, innerRect)
                    End Using
                Else
                    Call g.FillRectangle(circleLogo.GetBrush, innerRect)
                End If

                ' 然后填充外圈区域
                ' 先计算出区域的大小
                Dim sumAll = factors.SumAll
                ' 根据和来计算出弧度大小
                Dim beneficials = 360 * factors.beneficials.Values.Sum / sumAll
                Dim detrimentals = 360 * factors.detrimentals.Values.Sum / sumAll

                ' 在中间绘制总得分
                Dim score$ = Math.Round(factors.beneficials.Values.Sum / sumAll * 100)
                Dim textWidth = g.MeasureString(score, scoreFont).Width
                Dim x = innerRect.Left + (innerRect.Width - textWidth) / 2
                Dim y = innerRect.Top + innerRect.Height / 3

                Call g.DrawString(score, scoreFont, Brushes.White, x, y)

                If Val(score) < 70 Then
                    With New Rectangle(New Point(x + textWidth - innerRadius / 4, innerRect.Top + innerRect.Height / 3), New Size(innerRadius / 3, innerRadius / 3))
                        Call g.DrawImage(My.Resources.sign_warning_icon, .ByRef)
                    End With
                End If

                ' 然后开始绘制圆弧
                ' 需要计算出开始的弧度
                Dim startAngle = 0 - beneficials / 2
                ' 然后构建出path对象用于进行填充
                Dim arc As New GraphicsPath
                Dim outerStart As PointF = center.MovePoint(startAngle, radius)
                Dim outerStop As PointF = center.MovePoint(startAngle + beneficials, radius)
                Dim innerStart As PointF = center.MovePoint(startAngle, innerRadius)
                Dim innerStop As PointF = center.MovePoint(startAngle + detrimentals, radius)

                Call arc.AddArc(outerRect, startAngle, beneficials)
                'Call arc.AddLine(outerStop, innerStop)
                Call arc.AddArc(innerRect, startAngle + beneficials, -beneficials)
                'Call arc.AddLine(innerStart, outerStart)
                Call arc.CloseAllFigures()

                Call g.FillPath(beneficialsColor.GetBrush, arc)

                startAngle += beneficials

                arc = New GraphicsPath
                arc.AddArc(outerRect, startAngle, detrimentals)
                arc.AddArc(innerRect, startAngle + detrimentals, -detrimentals)

                Call g.FillPath(detrimentalsColor.GetBrush, arc)

            End Sub

        Return g.GraphicsPlots(
            size.SizeParser,
            margin, bg,
            plotInternal
        )
    End Function
End Module

Public Structure Factors

    ''' <summary>
    ''' 有益的因素
    ''' </summary>
    Dim beneficials As NamedValue(Of Double)()
    ''' <summary>
    ''' 有害的因素
    ''' </summary>
    Dim detrimentals As NamedValue(Of Double)()

    Public ReadOnly Property IsEmpty As Boolean
        Get
            Return beneficials.Length = 0 AndAlso detrimentals.Length = 0
        End Get
    End Property

    Public ReadOnly Property SumAll As Double
        Get
            Return beneficials.Values.Sum + detrimentals.Values.Sum
        End Get
    End Property

End Structure
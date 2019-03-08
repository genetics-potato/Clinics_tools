Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.ValueTypes

Namespace Marker

    Public Module Interpreter

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="range">浓度区间定义</param>
        ''' <param name="quantify">定量结果</param>
        ''' <param name="scoreEdges">
        ''' 得分计算的边界点：``[yellow, red]``
        ''' 
        ''' ```
        '''     red      yellow      green
        ''' |---------|----------|-----------|
        ''' 0     edge:red   edge:yellow   100
        ''' ```
        ''' </param>
        ''' <returns></returns>
        <Extension>
        Public Function GetDescription(range As IRange,
                                       quantify#,
                                       scoreEdges As (yellow#, red#),
                                       Optional mode As InterpreterTypes = InterpreterTypes.GreenBetter) As Description

            Dim description As (percentage#, location As RangeLocations)
            Dim score#
            Dim percentRange As DoubleRange = {0, 100}

            Select Case mode
                Case InterpreterTypes.LowerBetter
                    description = lowerBetter(range, quantify)
                Case InterpreterTypes.HigherBetter
                    description = higherBetter(range, quantify)
                Case Else
                    description = normal(range, quantify)
            End Select

            Select Case description.location
                Case RangeLocations.Green
                    score = percentRange.ScaleMapping(description.percentage, {scoreEdges.yellow, 100})
                Case RangeLocations.HiYellow, RangeLocations.LowYellow
                    score = percentRange.ScaleMapping(description.percentage, {scoreEdges.red, scoreEdges.yellow})
                Case Else
                    score = percentRange.ScaleMapping(description.percentage, {0, scoreEdges.red})
            End Select

            Return New Description With {
                .Location = description.location,
                .Percentage = description.percentage,
                .Score = score
            }
        End Function

        Private Function lowerBetter(range As IRange, value#) As (percentage#, location As RangeLocations)
            If value <= range.Green.Min Then
                Return (1, RangeLocations.Green)
            ElseIf range.Green.IsInside(value) Then
                Return (range.Green.Percentage(value), RangeLocations.Green)
            ElseIf range.Yellow.IsInside(value) Then
                Return (range.Yellow.Percentage(value), RangeLocations.HiYellow)
            ElseIf range.Red.IsInside(value) Then
                Return (range.Red.Percentage(value), RangeLocations.HiRed)
            Else
                Return (0, RangeLocations.HiRed)
            End If
        End Function

        Private Function higherBetter(range As IRange, value#) As (percentage#, location As RangeLocations)

        End Function

        Private Function normal(range As IRange, value#) As (percentage#, location As RangeLocations)
            Dim green As DoubleRange = range.Green
            Dim yellow As DoubleRange = range.Yellow
            Dim red As DoubleRange = range.Red
            Dim rl As DoubleRange = {red.Min, yellow.Min}
            Dim yl As DoubleRange = {yellow.Min, green.Min}
            Dim yh As DoubleRange = {green.Max, yellow.Max}
            Dim rh As DoubleRange = {yellow.Max, red.Max}
            Dim percentage#
            Dim location As RangeLocations

            ' 这里的百分比描述的是数据的偏离性
            '
            ' | red_low | yellow_low |    green    | yellow_hi | red_hi |
            ' 1 <----- 0|1 <------- 0|1 <-- 0 --> 1| 0 -----> 1| 0 ---> 1 
            '
            If rl.IsInside(value) OrElse value <= rl.Min Then
                ' 定量结果值已经远远小于最低值了
                If value <= rl.Min Then
                    percentage = 100
                Else
                    percentage = (1 - rl.Percentage(value)) * 100
                End If

                location = RangeLocations.LowRed
            ElseIf yl.IsInside(value) Then
                percentage = (1 - yl.Percentage(value)) * 100
                location = RangeLocations.LowYellow
            ElseIf green.IsInside(value) Then

                ' 只要存在于正常范围区间内，无所谓百分比大小？
                ' 都是百分百
                ' 使用中点作为零点
                ' 半长作为度量
                percentage = Math.Abs(green.Average - value) / (green.Length / 2) * 100
                location = RangeLocations.Green
            ElseIf yh.IsInside(value) Then

                percentage = yh.Percentage(value) * 100
                location = RangeLocations.HiYellow

            Else

                If value > rh.Max Then
                    percentage = 100
                Else
                    percentage = rh.Percentage(value) * 100
                End If

                location = RangeLocations.HiRed
            End If

            Return (percentage, location)
        End Function
    End Module

    Public Class Description

        ''' <summary>
        ''' 区间位置
        ''' </summary>
        ''' <returns></returns>
        Public Property Location As RangeLocations
        ''' <summary>
        ''' 在该区间内的偏移百分比
        ''' </summary>
        ''' <returns></returns>
        Public Property Percentage As Double
        ''' <summary>
        ''' 计算出来的偏移对得分的mapping结果
        ''' </summary>
        ''' <returns></returns>
        Public Property Score As Double

        Public Overrides Function ToString() As String
            Return $"{Score} ({Location}: {Percentage.ToString("F2")}%)"
        End Function

    End Class

    Public Interface IRange

        Property Red As DoubleRange
        Property Yellow As DoubleRange
        Property Green As DoubleRange

    End Interface
End Namespace
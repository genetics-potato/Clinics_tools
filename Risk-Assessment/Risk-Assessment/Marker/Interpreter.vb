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
        ''' <returns></returns>
        ''' 
        <Extension>
        Public Function GetDescription(range As IRange, quantify#, mode As InterpreterTypes)
            Select Case mode
                Case InterpreterTypes.LowerBetter
                    Return lowerBetter(range, quantify)
                Case InterpreterTypes.HigherBetter
                    Return higherBetter(range, quantify)
                Case Else
                    Return normal(range, quantify)
            End Select
        End Function

        Private Function lowerBetter(range As IRange, value#) As (percentage#, location As RangeLocations)

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
            ' | red_low | yellow_low |   green   | yellow_hi | red_hi |
            ' 1 <----- 0 1 <------- 0 1 <- 0 -> 1  0 -----> 1  0 ---> 1 

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

    End Class

    Public Interface IRange

        Property Red As DoubleRange
        Property Yellow As DoubleRange
        Property Green As DoubleRange

    End Interface
End Namespace
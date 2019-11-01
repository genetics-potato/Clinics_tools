Imports System.ComponentModel

Namespace Marker

    Public Enum RangeLocations
        <Description("red")> LowRed
        <Description("yellow")> LowYellow
        <Description("green")> Green
        <Description("yellow")> HiYellow
        <Description("red")> HiRed
    End Enum

    ''' <summary>
    ''' 解释器的工作形式
    ''' </summary>
    Public Enum InterpreterTypes As Integer
        ''' <summary>
        ''' ``1``: 区间为 0 红 黄 绿 无上限
        ''' </summary>
        HigherBetter = 1
        ''' <summary>
        ''' ``-1``: 区间为 0 绿 黄 红 无上限
        ''' </summary>
        LowerBetter = -1
        ''' <summary>
        ''' ``0``: 区间为 红 黄 绿 黄 红，默认形式
        ''' </summary>
        GreenBetter = 0
    End Enum
End Namespace


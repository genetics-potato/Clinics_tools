Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model

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
        Public Function GetDescription(range As IRange, quantify#)

        End Function
    End Module

    Public Interface IRange

        Property Red As DoubleRange
        Property Yellow As DoubleRange
        Property Green As DoubleRange

    End Interface
End Namespace
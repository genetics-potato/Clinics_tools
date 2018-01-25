Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.Bootstrapping
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
        Public Function SurvivalModel(model As IEnumerable(Of Model)) As FitResult
            Dim timeGroups = model.Select(Function(m) m.time).GroupBy(offsets:=0.1)

        End Function
    End Module
End Namespace

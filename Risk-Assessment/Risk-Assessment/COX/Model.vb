Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Data.csv.StorageProvider.Reflection

Namespace COX

    Public Class Model : Inherits DataSet

        Public Property time As Double
        <Column("status", GetType(Coefficients.statusCode))>
        Public Property status As COXstatus

        Public Overrides Function ToString() As String
            Return $"[{ID}] {time}, {status.Description}"
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function Project(names$()) As Model
            Return New Model With {
                .Properties = Properties.Subset(keys:=names),
                .ID = ID,
                .status = status,
                .time = time
            }
        End Function
    End Class

    ''' <summary>
    ''' + 1 是正常无患病的截尾数据
    ''' + 2 表示患病了
    ''' </summary>
    Public Enum COXstatus As Integer

        ''' <summary>
        ''' 没有发生死亡事件的截尾数据
        ''' </summary>
        Censored = 1
        ''' <summary>
        ''' 发生了死亡事件
        ''' </summary>
        Die = 2
    End Enum
End Namespace
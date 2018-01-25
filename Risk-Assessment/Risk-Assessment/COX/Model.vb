Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Data.csv.StorageProvider.Reflection

Public Class Model : Inherits DataSet

    Public Property time As Double
    <Column("status", GetType(COX.statusCode))>
    Public Property status As COXstatus

    Public Overrides Function ToString() As String
        Return $"[{ID}] {time}, {status.Description}"
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
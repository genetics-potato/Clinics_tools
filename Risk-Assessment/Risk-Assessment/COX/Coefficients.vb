Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.Data.csv
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports Microsoft.VisualBasic.Text
Imports RDotNET.Extensions.Bioinformatics
Imports RDotNET.Extensions.VisualBasic.API.base
Imports RDotNET.Extensions.VisualBasic.API.utils
Imports RDotNET.Extensions.VisualBasic.SymbolBuilder

Namespace COX

    ''' <summary>
    ''' 比例风险回归模型(proportional hazards model，简称Cox模型)
    ''' 
    ''' > https://en.wikipedia.org/wiki/Proportional_hazards_model
    ''' </summary>
    Public Module Coefficients

        Public Function Hazard(h0#, b As Dictionary(Of String, Double), X As Dictionary(Of String, Double)) As Double
            Dim names$() = b.Keys.ToArray
            Dim coeff As Vector = names.Select(Function(key) b(key)).AsVector
            Dim factors As Vector = names.Select(Function(key) X(key)).AsVector
            Dim ht = h0 * Math.Exp((coeff * factors).Sum)
            Return ht
        End Function

        ''' <summary>
        ''' 通过这个训练函数会生成H(t)，危险率函数，实际上是一个条件瞬间死亡率。
        ''' </summary>
        ''' <param name="model"></param>
        ''' <param name="names">
        ''' The factor name alias, the key in this dictionary table should be the key names in <see cref="Model.Properties"/>.
        ''' </param>
        ''' <returns></returns>
        <Extension>
        Public Function Training(model As IEnumerable(Of Model), Optional names As Dictionary(Of String, String) = Nothing) As Dictionary(Of String, Double)

            Dim table As Model() = model.ToArray
            Dim data = read.csv(file:=table.tempData)

            ' R server code
            ' 进行COX模型的回归训练
            require("survival")

            ' 回归模型的方程
            ' formula <- Surv(time, status) ~ taxonomy1 + taxonomy2 + ... - 1
            Dim factors$ = table(Scan0) _
                .Properties _
                .Keys _
                .JoinBy(" + " & ASCII.LF)
            Dim formula$ = $"Surv(time, status) ~ {factors} - 1"
            Dim cox = survival.coxph(formula, data:=data)
            Dim coeff = survival.GetCoefficients(cox)

            With names Or table.factorNames.AsDefault

                ' 还需要对字符串做一下额外的处理
                ' 因为R会自动对非法字符做一些转换，在这里需要将
                ' 属性名字之中的非法字符都转换回来
                coeff = .ToDictionary(Function(t) t.Value,
                                      Function(t)
                                          Dim id$ = t.Key.AsRid
                                          Return coeff(id)
                                      End Function)
            End With

            ' 返回相关性因子
            Return coeff
        End Function

        ''' <summary>
        ''' 在这里会自动对R语言的变量名之中的非法字符做一些替换
        ''' </summary>
        ''' <param name="table"></param>
        ''' <returns></returns>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Private Function factorNames(table As Model()) As Dictionary(Of String, String)
            Return table(Scan0) _
                .Properties _
                .Keys _
                .ToDictionary(Function(key) key,
                              Function(raw) raw)
        End Function

        <Extension>
        Private Function tempData(model As Model()) As String
            With App.GetAppSysTempFile(".csv", App.PID)
                Call model.SaveTo(.ref, strict:=False, encoding:=Encoding.ASCII)
                Return .ref
            End With
        End Function

        Friend Class statusCode : Implements IParser

            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Public Overloads Function ToString(obj As Object) As String Implements IParser.ToString
                Return CInt(DirectCast(obj, COXstatus)).ToString
            End Function

            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Public Function TryParse(cell As String) As Object Implements IParser.TryParse
                Return CType(CInt(Val(cell)), COXstatus)
            End Function
        End Class
    End Module
End Namespace
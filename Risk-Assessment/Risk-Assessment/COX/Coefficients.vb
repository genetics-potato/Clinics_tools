Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.Data.csv
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.Scripting
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports Microsoft.VisualBasic.Text
'Imports RDotNET.Extensions.Bioinformatics
'Imports RDotNET.Extensions.VisualBasic.API.base
'Imports RDotNET.Extensions.VisualBasic.API.utils
'Imports RDotNET.Extensions.VisualBasic.SymbolBuilder
Imports csv = Microsoft.VisualBasic.Data.csv.IO.File

Namespace COX

    ''' <summary>
    ''' 比例风险回归模型(proportional hazards model，简称Cox模型)
    ''' 
    ''' > https://en.wikipedia.org/wiki/Proportional_hazards_model
    ''' </summary>
    Public Module Coefficients

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function Power(b As NamedVector, X As NamedVector) As Double
            Return Math.Exp((b * X).Sum)
        End Function

        Public Function Hazard(h0#, b As NamedVector, X As NamedVector) As Double
            Dim ht = h0 * Power(b, X)
            Return ht
        End Function

        Public Function SurvivalProbability(h0#, b As NamedVector, X As NamedVector) As Double
            Dim s0 = 1 - h0
            Dim st = s0 ^ Power(b, X)
            Return st
        End Function

        ''' <summary>
        ''' FoldChange compare between the condition <paramref name="X1"/> and <paramref name="X2"/>.
        ''' </summary>
        ''' <param name="b"></param>
        ''' <param name="X1"></param>
        ''' <param name="X2"></param>
        ''' <returns></returns>
        Public Function FoldChange(b As NamedVector, X1 As NamedVector, X2 As NamedVector) As Double
            Dim x = Power(b, X1)
            Dim y = Power(b, X2)
            Return x / y
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
        Public Function Training(model As IEnumerable(Of Model), Optional project$() = Nothing, Optional names As Dictionary(Of String, String) = Nothing) As Dictionary(Of String, Double)
            'Dim keys$() = project Or model.First.Properties.Keys.ToArray.AsDefault
            'Dim table As Model() = model _
            '    .Select(Function(m)
            '                Return m.Project(names:=keys)
            '            End Function) _
            '    .ToArray
            'Dim data = read.csv(file:=table.tempData)

            '' R server code
            '' 进行COX模型的回归训练
            'require("survival")

            '' 回归模型的方程
            '' formula <- Surv(time, status) ~ taxonomy1 + taxonomy2 + ... - 1
            'Dim factors$ = table(Scan0) _
            '    .Properties _
            '    .Keys _
            '    .JoinBy(" + " & ASCII.LF)
            'Dim formula$ = $"Surv(time, status) ~ {factors} - 1"
            'Dim cox = survival.coxph(formula, data:=data)
            'Dim coeff = survival.GetCoefficients(cox)

            'With names Or table.factorNames.AsDefault

            '    ' 还需要对字符串做一下额外的处理
            '    ' 因为R会自动对非法字符做一些转换，在这里需要将
            '    ' 属性名字之中的非法字符都转换回来
            '    coeff = .ToDictionary(Function(t) t.Value,
            '                          Function(t)
            '                              Dim id$ = t.Key.AsRid
            '                              Return coeff(id)
            '                          End Function)
            'End With

            '' 返回相关性因子
            'Return coeff
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
            Dim csv As csv = Nothing

            With TempFileSystem.GetAppSysTempFile(".csv", App.PID)
                Call model.SaveTo(.ByRef, strict:=False, encoding:=Encoding.ASCII)
                Call csv.SetValue(
                    File.Load(.ByRef) _
                        .Columns _
                        .Select(AddressOf stripNA) _
                        .JoinColumns)
                Call csv.Save(.ByRef, Encoding.ASCII)

                Return .ByRef
            End With
        End Function

        Private Function stripNA(s As String) As String
            If s.TextEquals("NaN") OrElse s = "非数字" OrElse s = "???" Then
                Return "NA"
            ElseIf Val(s).IsNaNImaginary Then
                Return "NA"
            Else
                Return s
            End If
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function stripNA(col As String()) As String()
            Return col _
                .Select(AddressOf stripNA) _
                .ToArray
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
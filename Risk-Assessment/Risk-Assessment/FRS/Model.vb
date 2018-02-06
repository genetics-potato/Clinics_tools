Imports System.Text

Namespace FRS

    Public Class FRSModel

        Public Property GenderMale As Boolean
        ''' <summary>
        ''' Systolic blood pressure, mm Hg
        ''' </summary>
        ''' <returns></returns>
        Public Property SBP As Double
        Public Property Age As Double
        Public Property Diabetes As Boolean
        Public Property Smoking As Boolean
        Public Property TreatedHypertension As Boolean

        ''' <summary>
        ''' Total cholesterol, mg/dL
        ''' </summary>
        ''' <returns></returns>
        Public Property TC As Double
        ''' <summary>
        ''' HDL cholesterol, mg/dL
        ''' </summary>
        ''' <returns></returns>
        Public Property HDL As Double
        Public Property BMI As Double

        Public Overrides Function ToString() As String
            Dim html As New StringBuilder
            Dim lipidfull = Me.LipidFullCVDRisk
            Dim lipidhard = Me.LipidHardCVDRisk
            Dim BMIfull = Me.BMIFullCVDRisk
            Dim BMIhard = Me.BMIHardCVDRisk

            html.AppendLine(<h3>Lipids-Based Results</h3>)
            html.AppendLine(
                <table>
                    <thead>
                        <tr>
                            <th>Item</th>
                            <th>Percentage%</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr><td>Your Risk of Full CVD:</td><td><%= lipidfull.risk %> %</td></tr>
                        <tr><td>Optimal Risk of Full CVD:</td><td><%= lipidfull.optimalRisk %> %</td></tr>
                        <tr><td>Normal Risk of Full CVD:</td><td><%= lipidfull.normalRisk %> %</td></tr>
                        <tr><td>Your Risk of Hard CVD:</td><td><%= lipidhard.risk %> %</td></tr>
                        <tr><td>Optimal Risk of Hard CVD:</td><td><%= lipidhard.optimalRisk %> %</td></tr>
                        <tr><td>Normal Risk of Hard CVD:</td><td><%= lipidhard.normalRisk %> %</td></tr>
                    </tbody>
                </table>)

            html.AppendLine(<h3>BMI-Based Results</h3>)
            html.AppendLine(
                <table>
                    <thead>
                        <tr>
                            <th>Item</th>
                            <th>Percentage%</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr><td>Your Risk of Full CVD:</td><td><%= BMIfull.risk %> %</td></tr>
                        <tr><td>Optimal Risk of Full CVD:</td><td><%= BMIfull.optimalRisk %> %</td></tr>
                        <tr><td>Normal Risk of Full CVD:</td><td><%= BMIfull.normalRisk %> %</td></tr>
                        <tr><td>Your Risk of Hard CVD:</td><td><%= BMIhard.risk %> %</td></tr>
                        <tr><td>Optimal Risk of Hard CVD:</td><td><%= BMIhard.optimalRisk %> %</td></tr>
                        <tr><td>Normal Risk of Hard CVD:</td><td><%= BMIhard.normalRisk %> %</td></tr>
                    </tbody>
                </table>)

            Return html.ToString
        End Function
    End Class
End Namespace


Imports Microsoft.VisualBasic.Serialization.JSON

Namespace FRS

    ''' <summary>
    ''' + https://www.framinghamheartstudy.org/risk-functions/cardiovascular-disease/30-year-risk.php#
    ''' + https://www.framinghamheartstudy.org/risk-functions/scripts/framingham30.js
    ''' </summary>
    ''' <remarks>
    ''' ###### Lipids-Based Results
    ''' 
    ''' |item                    |percentage|
    ''' |------------------------|----------|
    ''' |Your Risk Of Full CVD   |	 53 %   |
    ''' |Optimal Risk Of Full CVD|	 13 %   |
    ''' |Normal Risk Of Full CVD |	 23 %   |
    ''' |Your Risk Of Hard CVD   |	 41 %   |
    ''' |Optimal Risk Of Hard CVD|	 7  %   |
    ''' |Normal Risk Of Hard CVD |	 13 %   |
    ''' 
    ''' ###### BMI-Based Results
    ''' 
    ''' |item                    |percentage|
    ''' |------------------------|----------|
    ''' |Your Risk Of Full CVD   |	 75 %   |
    ''' |Optimal Risk Of Full CVD|	 18 %   |
    ''' |Normal Risk Of Full CVD |	 23 %   |
    ''' |Your Risk Of Hard CVD   | 	 65 %   |
    ''' |Optimal Risk Of Hard CVD|	 11 %   |
    ''' |Normal Risk Of Hard CVD |	 13 %   |
    ''' </remarks>
    Public Class Score30

        Public Property risk As Double
        Public Property optimalRisk As Double
        Public Property normalRisk As Double

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class
End Namespace
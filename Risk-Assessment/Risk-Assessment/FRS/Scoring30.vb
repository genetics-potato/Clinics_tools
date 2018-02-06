Namespace FRS

    ''' <summary>
    ''' ##### Framingham Risk Score
    ''' 
    ''' The Framingham Risk Score is a gender-specific algorithm used to estimate the 10-year cardiovascular risk of an individual. 
    ''' The Framingham Risk Score was first developed based on data obtained from the Framingham Heart Study, to estimate the 10-year 
    ''' risk of developing coronary heart disease.[1] In order to assess the 10-year cardiovascular disease risk, cerebrovascular 
    ''' events, peripheral artery disease and heart failure were subsequently added as disease outcomes for the 2008 Framingham 
    ''' Risk Score, on top of coronary heart disease.
    ''' 
    ''' > https://en.wikipedia.org/wiki/Framingham_Risk_Score
    ''' </summary>
    Public Module Scoring30

        ' Project - CVHCWeb, Mayo Clinic Rochester
        ' Created on Mar 30, 2011
        '
        ' These calculations are based On the framingham 30 year risk study located at
        ' http://circ.ahajournals.org/cgi/content/short/CIRCULATIONAHA.108.816694v1 Specifically, these calculations are a
        ' reverse engineering Of the spreadsheet located at
        ' http://circ.ahajournals.org/content/vol0/issue2009/images/data/CIRCULATIONAHA.108.816694/DC1/CI816694.DSriskcalculator.xls

        ' SPECIAL NOTE: As of 4/8/2011 there Is a bug on the spreadsheet listed above regarding the normal risk scores.  These risk 
        '               scores do Not take into account the difference between male/female.  This code has been updated to address
        '               this issue, however the spreadsheet may Not.

        ' Authors: Michael J. Pencina PhD, Ralph B. D'Agostino Sr PhD, Martin G. Larson ScD, Joseph M. Massaro PhD, and
        ' Ramachandran S. Vasan MD.

        ' Excel spreadsheet calculations have been translated To Java by Aaron Vaneps (Vaneps.Michael@mayo.edu), And I make no
        ' guarantee that there are no bugs In the translation. Use at your own risk.

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="age"></param>
        ''' <param name="isMale"></param>
        ''' <param name="systolicBp"></param>
        ''' <param name="cholesterol"></param>
        ''' <param name="hdl"></param>
        ''' <param name="smoker"></param>
        ''' <param name="treatedBp"></param>
        ''' <param name="diabetic"></param>
        ''' <returns></returns>
        Public Function calculateFullRiskNoBmi(age, isMale, systolicBp, cholesterol, hdl, smoker, treatedBp, diabetic) As Score30
            Dim returnObj As New Score30 With {.risk = -1.0, .optimalRisk = -1.0, .normalRisk = -1.0}
            returnObj.risk = calculateFullRiskNoBmiInternal(age, isMale, systolicBp, cholesterol, hdl, smoker, treatedBp, diabetic)
            returnObj.optimalRisk = calculateFullRiskNoBmiInternal(age, isMale, 110, 160, 60, False, False, False)
            returnObj.normalRisk = calculateFullRiskNoBmiInternal(age, isMale, 125, 180, 45, False, False, False)
            Return returnObj
        End Function
    End Module

    Public Class Score30

        Public Property risk As Double
        Public Property optimalRisk As Double
        Public Property normalRisk As Double

    End Class
End Namespace
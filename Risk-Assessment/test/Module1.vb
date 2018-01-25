Imports Microsoft.VisualBasic.Data.Bootstrapping
Imports SMRUCC.Clinic.RiskAssessment.COX

Module Module1

    Sub Main()
        Call survialTest()
    End Sub

    Sub survialTest()
        Dim modelData = Model.LoadDataSet(Of Model)("D:\Clinics_tools\Risk-Assessment\lung.csv", "inst").ToArray
        Dim S As FitResult = modelData.SurvivalModel


        Pause()
    End Sub
End Module

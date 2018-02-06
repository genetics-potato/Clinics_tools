Namespace FRS

    Public Class BMICalc : Inherits Scoring10

        Sub New()
            Call MyBase.New(
                coefsMaleNoTrtbp:=New Coefficients With {
                    .age = 3.11296,
                    .sbp = 1.85508,
                    .bmi = 0.79277,
                    .smoker = 0.70953,
                    .diabetes = 0.5316
                },
                coefsFemaleNoTrtbp:=New Coefficients With {
                    .age = 2.72107,
                    .sbp = 2.81291,
                    .bmi = 0.51125,
                    .smoker = 0.61868,
                    .diabetes = 0.77763
                },
                coefSbpMaleTrtbp:=1.92672,
                coefSbpFemaleTrtbp:=2.88267,
                useLogData:=New Coefficients With {
                    .age = 1,
                    .sbp = 1,
                    .bmi = 1,
                    .smoker = 0,
                    .diabetes = 0
                },
                betaZeroMale:=-23.9388,
                betaZeroFemale:=-26.0145,
                baseMale:=0.88431,
                baseFemale:=0.94833,
                baselineNormalData:=New Coefficients With {
                    .gender = 0,
                    .age = 30,
                    .sbp = 125,
                    .bmi = 22.5,
                    .smoker = 0,
                    .diabetes = 0,
                    .trtbp = 0
                },
                baselineOptimalData:=New Coefficients With {
                    .gender = 0,
                    .age = 30,
                    .sbp = 110,
                    .bmi = 22,
                    .smoker = 0,
                    .diabetes = 0,
                    .trtbp = 0
                })
        End Sub
    End Class
End Namespace
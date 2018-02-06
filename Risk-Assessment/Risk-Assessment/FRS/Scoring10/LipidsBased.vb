Namespace FRS

    Public Class LipidsCalc : Inherits Scoring10

        Sub New()
            Call MyBase.New(
             coefsMaleNoTrtbp:=New Coefficients With {
        .age = 3.06117, .sbp = 1.93303, .tcl = 1.1237, .hdl = -0.93263, .smoker = 0.65451, .diabetes = 0.57367
    },
      coefsFemaleNoTrtbp:=New Coefficients With {
        .age = 2.32888, .sbp = 2.76157, .tcl = 1.20904, .hdl = -0.70833, .smoker = 0.52873, .diabetes = 0.69154
    },
   coefSbpMaleTrtbp:=1.99881,
             coefSbpFemaleTrtbp:=2.82263,
           useLogData:=New Coefficients With {
       .age = 1, .sbp = 1, .tcl = 1, .hdl = 1, .smoker = 0, .diabetes = 0
    },
            betaZeroMale:=-23.9802,
             betaZeroFemale:=-26.1931,
             baseMale:=0.88936,
             baseFemale:=0.95012,
          baselineNormalData:=New Coefficients With {
       .gender = 0, .age = 30, .sbp = 125, .tcl = 180, .hdl = 45, .smoker = 0, .diabetes = 0, .trtbp = 0
    },
      baselineOptimalData:=New Coefficients With {
       .gender = 0, .age = 30, .sbp = 110, .tcl = 160, .hdl = 60, .smoker = 0, .diabetes = 0, .trtbp = 0
    })
        End Sub
    End Class
End Namespace
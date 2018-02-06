Imports System.Runtime.CompilerServices

Namespace FRS

    Public Module Extensions

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function LipidFullCVDRisk(mine As FRSModel) As Risk
            Return Scoring30.calculateFullRiskNoBmi(
                mine.Age,
                mine.GenderMale,
                mine.SBP,
                mine.TC,
                mine.HDL,
                mine.Smoking,
                mine.TreatedHypertension,
                mine.Diabetes
            )
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function LipidHardCVDRisk(mine As FRSModel) As Risk
            Return Scoring30.calculateHardRiskNoBmi(
                mine.Age,
                mine.GenderMale,
                mine.SBP,
                mine.TC,
                mine.HDL,
                mine.Smoking,
                mine.TreatedHypertension,
                mine.Diabetes
            )
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function BMIFullCVDRisk(mine As FRSModel) As Risk
            Return Scoring30.calculateFullRiskBmi(
                mine.Age,
                mine.GenderMale,
                mine.SBP,
                mine.Smoking,
                mine.TreatedHypertension,
                mine.Diabetes,
                mine.BMI
            )
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function BMIHardCVDRisk(mine As FRSModel) As Risk
            Return Scoring30.calculateHardRiskBmi(
                mine.Age,
                mine.GenderMale,
                mine.SBP,
                mine.Smoking,
                mine.TreatedHypertension,
                mine.Diabetes, mine.BMI
            )
        End Function
    End Module
End Namespace


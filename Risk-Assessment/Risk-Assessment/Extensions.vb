Imports System.Runtime.CompilerServices

Public Module Extensions

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function calcBMI(height#, mass#) As Double
        Dim bmi = 703.0814062 * mass / (height * height)
        Return bmi
    End Function
End Module

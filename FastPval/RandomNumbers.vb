'----------------------------------------------------------------------------------------
'	Copyright © 2006 - 2012 Tangible Software Solutions Inc.
'	This class can be used by anyone provided that the copyright notice remains intact.
'
'	This class provides the ability to simulate the behavior of the C/C++ functions for 
'	generating random numbers, using the .NET Framework System.Random class.
'	'rand' converts to the parameterless overload of NextNumber
'	'random' converts to the single-parameter overload of NextNumber
'	'randomize' converts to the parameterless overload of Seed
'	'srand' converts to the single-parameter overload of Seed
'----------------------------------------------------------------------------------------
Friend NotInheritable Class RandomNumbers
	Private Sub New()
	End Sub
	Private Shared r As System.Random

	Friend Shared Function NextNumber() As Integer
		If r Is Nothing Then
			Seed()
		End If

		Return r.[Next]()
	End Function

	Friend Shared Function NextNumber(ceiling As Integer) As Integer
		If r Is Nothing Then
			Seed()
		End If

		Return r.[Next](ceiling)
	End Function

	Friend Shared Sub Seed()
		r = New System.Random()
	End Sub

	Friend Shared Sub Seed(seed__1 As Integer)
		r = New System.Random(seed__1)
	End Sub
End Class

Public NotInheritable Class GlobalMembersUtil
	Private Sub New()
	End Sub
	'
'     * util.h
'     *
'     *  Created on: May 4, 2010
'     *      Author: jun
'     




	Public Shared Function compareint(a As System.IntPtr, b As System.IntPtr) As Integer
		Return (CInt(a) - CInt(b))
	End Function

	Public Shared Function comparefloat(a As System.IntPtr, b As System.IntPtr) As Integer
		If CSng(a) < CSng(b) Then
			Return -1
		End If
		Return CSng(a).Target > CSng(b)

	End Function

	Public Shared Function producerand(times As Integer, range As Integer) As Integer()
		Dim randarray As Integer() = New Integer(times - 1) {}
		Dim i As Integer
		'init
		RandomNumbers.Seed((time(0)))

		For i = 0 To times - 1
			randarray(i) = CInt(Math.Truncate(range * (RandomNumbers.NextNumber() / CDbl(RAND_MAX))))
		Next

		Return randarray
	End Function

	Public Shared Function readline(f As FILE) As String
		'
'         * calloc is like malloc, except it allocates cleared memory
'         * so you don't have random bytes in the memory space
'         

		'C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
		Dim line As SByte() = DirectCast(calloc(0, 1), String)
		Dim c As SByte
		Dim len As Integer = 0

		'
'         * If the readline reaches the end of a file,
'         * I want it to stop just the same
'         

		While (InlineAssignHelper(c, fgetc(f))) <> EOF AndAlso c <> ControlChars.Lf
			'
'             * The actual size allocated needs to be 1 more
'             * character than the string itself because '\0'
'             * needs to be added to the end of the string
'             * to prevent garbage
'             

			'C++ TO C# CONVERTER TODO TASK: The memory management function 'realloc' has no equivalent in C#:
			line = DirectCast(realloc(line, 1 * (len + 2)), String)
			line(System.Math.Max(System.Threading.Interlocked.Increment(len),len - 1)) = c
			line(len) = CSByte(AscW(ControlChars.NullChar))
		End While
		Return line
	End Function

	Public Shared Sub moveline(f As FILE)
		Dim c As SByte
		While (InlineAssignHelper(c, fgetc(f))) <> EOF AndAlso c <> ControlChars.Lf
			

		End While
	End Sub

	Public Shared Function wc(file As String) As Integer
		Dim fp As FILE = fopen(file, "r")
		Dim buf As New String(New Char(BUFSIZ - 1) {})
		Dim len As Integer = 0
		Dim c As Integer = 0
		While (InlineAssignHelper(len, fread(buf, 1, BUFSIZ, fp))) <> 0
			'C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on this variable, so pointers on this variable are left unchanged.
			Dim p As Pointer(Of SByte) = buf
			'C++ TO C# CONVERTER TODO TASK: The memory management function 'memchr' has no equivalent in C#:
			While (InlineAssignHelper(p, DirectCast(memchr(CType(p, System.IntPtr), ControlChars.Lf, buf.Substring(len) - p), String))) <> 0
				p += 1
				c += 1
			End While
		End While
		Return c
	End Function


	Public Shared Function binary_search(ByRef a As Single, ByRef b As Single, n As Single) As Single
		'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
		Dim m As Single
		While b > a
			m = a + (b - a) / 2
			If m < n Then
				a = m + 1
			ElseIf m > n Then
				b = m
			Else
				Exit While
			End If
		End While
		If a = b Then
			m = a
		End If
		'C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on this variable, so pointers on this variable are left unchanged.
		Dim flag As Pointer(Of Single) = m
		While m = (System.Threading.Interlocked.Decrement(flag)).Target
			

		End While
		Return flag + 1
	End Function


	Public Shared Function mean(data As Single(), stride As UInteger, size As UInteger) As Single
        ' Compute the arithmetic mean of a dataset using the recurrence relation
        '         mean_(n) = mean(n-1) + (data[n] - mean(n-1))/(n+1)   


        Dim meanX As Single = 0F
        Dim i As UInteger

		For i = 0 To size - 1
            meanX += (data(i * stride) - meanX) / (i + 1)
        Next

        Return meanX
    End Function

	Public Shared Function variance(data As Single(), stride As UInteger, n As UInteger, mean As Single) As Single
        ' takes a dataset and finds the variance 


        Dim varianceX As Single = 0F

        Dim i As UInteger

		' find the sum of the squares 

		For i = 0 To n - 1
			Dim delta As Single = (data(i * stride) - mean)
            varianceX += (delta * delta - varianceX) / (i + 1)
        Next

        Return varianceX
    End Function
	Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
		target = value
		Return value
	End Function
End Class


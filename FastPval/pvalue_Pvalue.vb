' #define GSL_DBL_EPSILON
' #define SQRT32
' #define GAUSS_EPSILON
' #define GAUSS_XUPPER
' #define GAUSS_XLOWER
' #define GAUSS_SCALE


Public NotInheritable Class GlobalMembersPvalue_Pvalue
    Private Sub New()
    End Sub
    '
    '     * Class:     pvalue_Pvalue
    '     * Method:    computePvalue
    '     * Signature: (Ljava/lang/String;Ljava/lang/String;IILjava/lang/String;I)V
    '     

    Public Sub Java_pvalue_Pvalue_computePvalue(env As JNIEnv, obj As jobject, rawFile As jstring, sampleFile As jstring, samplingNumber As jint, samplingCutoff As jint,
        outputFile As jstring, assumeDisType As jint)
        Dim timer1 As time_t = time(Nothing)

        '	produceNumber();
        Dim fp As FILE
        Dim i As Integer = 0
        Dim selectednum As Integer = samplingNumber
        Dim cutoff As Integer = samplingCutoff
        Dim rawf As String = env.GetStringUTFChars(env, rawFile, JNI_FALSE)
        Dim samplef As String = env.GetStringUTFChars(env, sampleFile, JNI_FALSE)
        Dim outputf As String = env.GetStringUTFChars(env, outputFile, JNI_FALSE)
        Dim assumeDis As Integer = assumeDisType
        'compute the total number
        Dim doclines As Integer = GlobalMembersUtil.wc(rawf)
        Console.Write("The size is {0:D}" & vbLf, doclines)
        'produce the random number and sort
        'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
        Dim randomarray As Integer = GlobalMembersUtil.producerand(selectednum, doclines)
        qsort(randomarray, selectednum, 4, AddressOf GlobalMembersUtil.compareint)
        'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
        'C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
        Dim selectedarray As Single = CSng(malloc(selectednum * 4))

        If (InlineAssignHelper(fp, fopen(rawf, "r"))) Is Nothing Then
            Return
        End If

        'read the randomized data from given dataset
        Dim filehandle As Integer = 1
        For i = 0 To selectednum - 1
            Dim flag As Integer = 0
            Dim current As Integer = randomarray(i)
            Dim distance As Integer = current - filehandle
            If distance < 0 Then
                selectedarray(i) = selectedarray(i - 1)
                Continue For
            End If
            While flag < distance
                GlobalMembersUtil.moveline(fp)
                filehandle += 1
                flag += 1
            End While
            'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
            Dim line As SByte = GlobalMembersUtil.readline(fp)
            filehandle += 1
            selectedarray(i) = Convert.ToDouble(line)
            'C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(line)
        Next
        fclose(fp)
        qsort(selectedarray, selectednum, 4, AddressOf GlobalMembersUtil.comparefloat)

        'compute and get the cutoff data
        Dim cutoffpoint As Single = selectedarray(selectednum - cutoff + 1)
        Dim cutoffnum As Integer = 0
        Console.Write("The cutoff point is {0:f}" & vbLf, cutoffpoint)

        If (InlineAssignHelper(fp, fopen(rawf, "r"))) Is Nothing Then
            Return
        End If
        Dim cutoffarray As Single() = New Single(-1) {}
        For i = 0 To doclines - 1
            If cutoffnum Mod BUFSIZ = 0 Then
                'C++ TO C# CONVERTER TODO TASK: The memory management function 'realloc' has no equivalent in C#:
                cutoffarray = CSng(realloc(cutoffarray, 4 * (cutoffnum + BUFSIZ)))
            End If
            'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
            Dim line As SByte = GlobalMembersUtil.readline(fp)
            If Convert.ToDouble(line) > cutoffpoint Then
                '			cutoffarray = (float*) realloc(cutoffarray, sizeof(float)
                '					* (cutoffnum + 1));
                cutoffarray(cutoffnum) = Convert.ToDouble(line)
                cutoffnum += 1
            End If
            'C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(line)
        Next
        fclose(fp)
        Console.Write("There are {0:D} data above the cutoff" & vbLf, cutoffnum)
        qsort(cutoffarray, cutoffnum, 4, AddressOf GlobalMembersUtil.comparefloat)

        Dim maxInDataset As Single = cutoffarray(cutoffnum - 1)
        Dim maxPvalue As Single = 1 / CSng(doclines)
        Dim selectMean As Single = GlobalMembersUtil.mean(selectedarray, 1, selectednum)
        Dim selectVariance As Single
        Dim selectBeta As Single
        Dim selectMu As Single
        If assumeDis = 0 Then
            selectVariance = GlobalMembersUtil.variance(selectedarray, 1, selectednum, selectMean)
        End If
        If assumeDis = 1 Then
            selectVariance = GlobalMembersUtil.variance(selectedarray, 1, selectednum, selectMean)
            selectBeta = Math.Sqrt(6 * selectVariance / (3.14159265358979 * 3.14159265358979))
            selectMu = selectMean - selectBeta * 0.577215664901533
        End If

        'search and compute p-value
        '	float* p1 = binary_search(cutoffarray, cutoffarray + cutoffnum - 1, 899.60);

        'compute the sample p-value
        Dim fps As FILE
        Dim s As Single = cutoff / CSng(selectednum)
        Dim n As Single = 1 / CSng(doclines)
        Dim z As Single = n / s
        Dim k As Single = (1 / z) / CSng(cutoffnum)
        Dim samplelines As Integer = GlobalMembersUtil.wc(samplef)
        If (InlineAssignHelper(fps, fopen(samplef, "r"))) Is Nothing Then
            Return
        End If

        Dim fpo As FILE
        If (InlineAssignHelper(fpo, fopen(outputf, "w+"))) Is Nothing Then
            Return
        End If
        For i = 0 To samplelines - 1
            'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
            Dim line As SByte = GlobalMembersUtil.readline(fps)
            Dim cnum As Single = Convert.ToDouble(line)
            'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
            Dim m As Single
            Dim location As Single
            Dim pvalue As Single
            Dim cnumP As Single
            Dim maxP As Single
            Dim temp As Single
            If cnum > cutoffpoint Then
                If cnum > maxInDataset Then
                    If assumeDis = 0 Then
                        cnumP = GlobalMembersGauss.gsl_cdf_gaussian_Q(cnum, selectVariance)
                        maxP = GlobalMembersGauss.gsl_cdf_gaussian_Q(maxInDataset, selectVariance)
                    End If
                    If assumeDis = 1 Then
                        cnumP = GlobalMembersGumbel1.gsl_cdf_gumbel1_Q(cnum, selectMu, selectBeta)
                        maxP = GlobalMembersGumbel1.gsl_cdf_gumbel1_Q(maxInDataset, selectMu, selectBeta)
                    End If
                    If cnumP > maxP Then
                        temp = cnumP
                        maxP = cnumP
                        cnumP = temp
                    End If
                    If maxP > 0F Then
                        pvalue = cnumP * maxPvalue / maxP
                    End If
                    fprintf(fpo, "%E" & vbTab & "%s" & vbLf, pvalue, line)
                Else
                    m = GlobalMembersUtil.binary_search(cutoffarray, cutoffarray + cutoffnum - 1, cnum)
                    location = m - cutoffarray + 1
                    location = k * location
                    location = (1 / z) - location + 1
                    If location > (1 / z) Then
                        location = (1 / z)
                    End If
                    pvalue = s * location / (1 / z) / k
                    fprintf(fpo, "%E" & vbTab & "%s" & vbLf, pvalue, line)
                End If
            Else
                m = GlobalMembersUtil.binary_search(selectedarray, selectedarray + selectednum - 1, cnum)
                location = m - selectedarray + 1
                pvalue = (selectednum - location + 1) / CSng(selectednum)
                fprintf(fpo, "%E" & vbTab & "%s" & vbLf, pvalue, line)
            End If
            'C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(line)
        Next
        fclose(fps)
        fclose(fpo)
        '	printf("find %f at pos:%d\n", 899.60, p1 - cutoffarray);
        Dim timer2 As time_t = time(Nothing)
        Dim dt As Double = difftime(timer2, timer1)
        Console.Write("The running time is {0:f}s" & vbLf, dt)

        Return
    End Sub

    '
    '     * Class:     pvalue_Pvalue
    '     * Method:    generateBackground
    '     * Signature: (Ljava/lang/String;IILjava/lang/String;)V
    '     

    Public Sub Java_pvalue_Pvalue_generateBackground(env As JNIEnv, obj As jobject, rawFile As jstring, samplingNumber As jint, samplingCutoff As jint, firstOutputFile As jstring,
        secondOutputFile As jstring)
        Dim timer1 As time_t = time(Nothing)

        Dim fp As FILE
        Dim i As Integer = 0
        Dim selectednum As Integer = samplingNumber
        Dim cutoff As Integer = samplingCutoff
        Dim rawf As String = env.GetStringUTFChars(env, rawFile, JNI_FALSE)
        Dim firstoutputf As String = env.GetStringUTFChars(env, firstOutputFile, JNI_FALSE)
        Dim secondoutputf As String = env.GetStringUTFChars(env, secondOutputFile, JNI_FALSE)
        'compute the total number
        Dim doclines As Integer = GlobalMembersUtil.wc(rawf)
        Console.Write("The size is {0:D}" & vbLf, doclines)
        'produce the random number and sort
        'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
        Dim randomarray As Integer = GlobalMembersUtil.producerand(selectednum, doclines)
        qsort(randomarray, selectednum, 4, AddressOf GlobalMembersUtil.compareint)
        'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
        'C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
        Dim selectedarray As Single = CSng(malloc(selectednum * 4))

        If (InlineAssignHelper(fp, fopen(rawf, "r"))) Is Nothing Then
            Return
        End If

        'read the randomized data from given dataset
        Dim filehandle As Integer = 1
        For i = 0 To selectednum - 1
            Dim flag As Integer = 0
            Dim current As Integer = randomarray(i)
            Dim distance As Integer = current - filehandle
            If distance < 0 Then
                selectedarray(i) = selectedarray(i - 1)
                Continue For
            End If
            While flag < distance
                GlobalMembersUtil.moveline(fp)
                filehandle += 1
                flag += 1
            End While
            'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
            Dim line As SByte = GlobalMembersUtil.readline(fp)
            filehandle += 1
            selectedarray(i) = Convert.ToDouble(line)
            'C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(line)
        Next
        fclose(fp)
        qsort(selectedarray, selectednum, 4, AddressOf GlobalMembersUtil.comparefloat)

        'compute and get the cutoff data
        Dim cutoffpoint As Single = selectedarray(selectednum - cutoff + 1)
        Dim cutoffnum As Integer = 0
        Console.Write("The cutoff point is {0:f}" & vbLf, cutoffpoint)

        If (InlineAssignHelper(fp, fopen(rawf, "r"))) Is Nothing Then
            Return
        End If
        Dim cutoffarray As Single() = New Single(-1) {}
        For i = 0 To doclines - 1
            If cutoffnum Mod BUFSIZ = 0 Then
                'C++ TO C# CONVERTER TODO TASK: The memory management function 'realloc' has no equivalent in C#:
                cutoffarray = CSng(realloc(cutoffarray, 4 * (cutoffnum + BUFSIZ)))
            End If
            'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
            Dim line As SByte = GlobalMembersUtil.readline(fp)
            If Convert.ToDouble(line) > cutoffpoint Then
                '			cutoffarray = (float*) realloc(cutoffarray, sizeof(float)
                '					* (cutoffnum + 1));
                cutoffarray(cutoffnum) = Convert.ToDouble(line)
                cutoffnum += 1
            End If
            'C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(line)
        Next
        fclose(fp)
        Console.Write("There are {0:D} data above the cutoff" & vbLf, cutoffnum)
        qsort(cutoffarray, cutoffnum, 4, AddressOf GlobalMembersUtil.comparefloat)

        Dim firstbacf As FILE
        If (InlineAssignHelper(firstbacf, fopen(firstoutputf, "w+"))) Is Nothing Then
            Return
        End If

        Dim secondbacf As FILE
        If (InlineAssignHelper(secondbacf, fopen(secondoutputf, "w+"))) Is Nothing Then
            Return
        End If

        fprintf(firstbacf, "%d" & vbLf, doclines)
        fprintf(firstbacf, "%d" & vbLf, selectednum)
        fprintf(firstbacf, "%d" & vbLf, cutoff)
        fprintf(firstbacf, "%d" & vbLf, cutoffnum)
        fprintf(firstbacf, "%f" & vbLf, cutoffpoint)
        fprintf(secondbacf, "%d" & vbLf, doclines)
        fprintf(secondbacf, "%d" & vbLf, selectednum)
        fprintf(secondbacf, "%d" & vbLf, cutoff)
        fprintf(secondbacf, "%d" & vbLf, cutoffnum)
        fprintf(secondbacf, "%f" & vbLf, cutoffpoint)

        For i = 0 To selectednum - 1
            fprintf(firstbacf, "%f" & vbLf, selectedarray(i))
        Next

        For i = 0 To cutoffnum - 1
            fprintf(secondbacf, "%f" & vbLf, cutoffarray(i))
        Next

        fclose(firstbacf)
        fclose(secondbacf)
        '	printf("find %f at pos:%d\n", 899.60, p1 - cutoffarray);
        Dim timer2 As time_t = time(Nothing)
        Dim dt As Double = difftime(timer2, timer1)
        Console.Write("The running time is {0:f}s" & vbLf, dt)

        Return
    End Sub

    '
    '     * Class:     pvalue_Pvalue
    '     * Method:    computePvalueByBackground
    '     * Signature: (Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;I)V
    '     

    Public Sub Java_pvalue_Pvalue_computePvalueByBackground(env As JNIEnv, obj As jobject, firstOutputFile As jstring, secondOutputFile As jstring, sampleFile As jstring, outputFile As jstring,
        assumeDisType As jint)
        Dim timer1 As time_t = time(Nothing)

        Dim i As Integer = 0
        Dim firstoutputf As String = env.GetStringUTFChars(env, firstOutputFile, JNI_FALSE)
        Dim secondoutputf As String = env.GetStringUTFChars(env, secondOutputFile, JNI_FALSE)
        Dim samplef As String = env.GetStringUTFChars(env, sampleFile, JNI_FALSE)
        Dim outputf As String = env.GetStringUTFChars(env, outputFile, JNI_FALSE)
        Dim assumeDis As Integer = assumeDisType

        Dim firstbacf As FILE
        If (InlineAssignHelper(firstbacf, fopen(firstoutputf, "r"))) Is Nothing Then
            Return
        End If
        Dim secondbacf As FILE
        If (InlineAssignHelper(secondbacf, fopen(secondoutputf, "r"))) Is Nothing Then
            Return
        End If

        Dim doclines As Integer
        Dim selectednum As Integer
        Dim cutoff As Integer
        Dim cutoffnum As Integer
        Dim cutoffpoint As Single
        Dim firstdoclines As Integer = Convert.ToInt32(GlobalMembersUtil.readline(firstbacf))
        Dim seconddoclines As Integer = Convert.ToInt32(GlobalMembersUtil.readline(secondbacf))
        Dim firstselectednum As Integer = Convert.ToInt32(GlobalMembersUtil.readline(firstbacf))
        Dim secondselectednum As Integer = Convert.ToInt32(GlobalMembersUtil.readline(secondbacf))
        Dim firstcutoff As Integer = Convert.ToInt32(GlobalMembersUtil.readline(firstbacf))
        Dim secondcutoff As Integer = Convert.ToInt32(GlobalMembersUtil.readline(secondbacf))
        Dim firstcutoffnum As Integer = Convert.ToInt32(GlobalMembersUtil.readline(firstbacf))
        Dim secondcutoffnum As Integer = Convert.ToInt32(GlobalMembersUtil.readline(secondbacf))
        Dim firstcutoffpoint As Integer = Convert.ToInt32(GlobalMembersUtil.readline(firstbacf))
        Dim secondcutoffpoint As Integer = Convert.ToInt32(GlobalMembersUtil.readline(secondbacf))

        If (firstdoclines = seconddoclines) AndAlso (firstselectednum = secondselectednum) AndAlso (firstcutoff = secondcutoff) AndAlso (firstcutoffnum = secondcutoffnum) AndAlso (firstcutoffpoint = secondcutoffpoint) Then
            doclines = firstdoclines
            selectednum = firstselectednum
            cutoff = firstcutoff
            cutoffnum = secondcutoffnum
            cutoffpoint = firstcutoffpoint
        Else
            Console.Write("The first background is not identical to the second background" & vbLf)
            Return
        End If

        Dim selectedarray As Single() = New Single(selectednum - 1) {}
        'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
        'C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
        Dim cutoffarray As Single = CSng(malloc(cutoffnum * 4))

        For i = 0 To selectednum - 1
            'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
            Dim line As SByte = GlobalMembersUtil.readline(firstbacf)
            selectedarray(i) = Convert.ToDouble(line)
            'C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(line)
        Next
        fclose(firstbacf)

        For i = 0 To cutoffnum - 1
            'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
            Dim line As SByte = GlobalMembersUtil.readline(secondbacf)
            cutoffarray(i) = Convert.ToDouble(line)
            'C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(line)
        Next
        fclose(secondbacf)

        Dim maxInDataset As Single = cutoffarray(cutoffnum - 1)
        Dim maxPvalue As Single = 1 / CSng(doclines)
        Dim selectMean As Single = GlobalMembersUtil.mean(selectedarray, 1, selectednum)
        Dim selectVariance As Single
        Dim selectBeta As Single
        Dim selectMu As Single
        If assumeDis = 0 Then
            selectVariance = GlobalMembersUtil.variance(selectedarray, 1, selectednum, selectMean)
        End If
        If assumeDis = 1 Then
            selectVariance = GlobalMembersUtil.variance(selectedarray, 1, selectednum, selectMean)
            selectBeta = Math.Sqrt(6 * selectVariance / (3.14159265358979 * 3.14159265358979))
            selectMu = selectMean - selectBeta * 0.577215664901533
        End If

        'search and compute p-value
        '	float* p1 = binary_search(cutoffarray, cutoffarray + cutoffnum - 1, 899.60);

        'compute the sample p-value
        Dim fps As FILE
        Dim s As Single = cutoff / CSng(selectednum)
        Dim n As Single = 1 / CSng(doclines)
        Dim z As Single = n / s
        Dim k As Single = (1 / z) / CSng(cutoffnum)
        Dim samplelines As Integer = GlobalMembersUtil.wc(samplef)
        If (InlineAssignHelper(fps, fopen(samplef, "r"))) Is Nothing Then
            Return
        End If

        Dim fpo As FILE
        If (InlineAssignHelper(fpo, fopen(outputf, "w+"))) Is Nothing Then
            Return
        End If
        For i = 0 To samplelines - 1
            'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
            Dim line As SByte = GlobalMembersUtil.readline(fps)
            Dim cnum As Single = Convert.ToDouble(line)
            'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
            Dim m As Single
            Dim location As Single
            Dim pvalue As Single
            Dim cnumP As Single
            Dim maxP As Single
            Dim temp As Single
            If cnum > cutoffpoint Then
                If cnum > maxInDataset Then
                    If assumeDis = 0 Then
                        cnumP = GlobalMembersGauss.gsl_cdf_gaussian_Q(cnum, selectVariance)
                        maxP = GlobalMembersGauss.gsl_cdf_gaussian_Q(maxInDataset, selectVariance)
                    End If
                    If assumeDis = 1 Then
                        cnumP = GlobalMembersGumbel1.gsl_cdf_gumbel1_Q(cnum, selectMu, selectBeta)
                        maxP = GlobalMembersGumbel1.gsl_cdf_gumbel1_Q(maxInDataset, selectMu, selectBeta)
                    End If
                    If cnumP > maxP Then
                        temp = cnumP
                        maxP = cnumP
                        cnumP = temp
                    End If
                    If maxP > 0F Then
                        pvalue = cnumP * maxPvalue / maxP
                    End If
                    fprintf(fpo, "%E" & vbTab & "%s" & vbLf, pvalue, line)
                Else
                    m = GlobalMembersUtil.binary_search(cutoffarray, cutoffarray + cutoffnum - 1, cnum)
                    location = m - cutoffarray + 1
                    location = k * location
                    location = (1 / z) - location + 1
                    If location > (1 / z) Then
                        location = (1 / z)
                    End If
                    pvalue = s * location / (1 / z) / k
                    fprintf(fpo, "%E" & vbTab & "%s" & vbLf, pvalue, line)
                End If
            Else
                m = GlobalMembersUtil.binary_search(selectedarray, selectedarray + selectednum - 1, cnum)
                location = m - selectedarray + 1
                pvalue = (selectednum - location + 1) / CSng(selectednum)
                fprintf(fpo, "%E" & vbTab & "%s" & vbLf, pvalue, line)
            End If
            'C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(line)
        Next
        fclose(fps)
        fclose(fpo)
        '	printf("find %f at pos:%d\n", 899.60, p1 - cutoffarray);
        Dim timer2 As time_t = time(Nothing)
        Dim dt As Double = difftime(timer2, timer1)
        Console.Write("The running time is {0:f}s" & vbLf, dt)

        Return
    End Sub

    '
    '     * Class:     pvalue_Pvalue
    '     * Method:    computeExactPvalue
    '     * Signature: (Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)V
    '     

    Public Sub Java_pvalue_Pvalue_computeExactPvalue(env As JNIEnv, obj As jobject, rawFile As jstring, sampleFile As jstring, outputFile As jstring)
        Dim timer1 As time_t = time(Nothing)

        Dim rawf As String = env.GetStringUTFChars(env, rawFile, JNI_FALSE)
        Dim samplef As String = env.GetStringUTFChars(env, sampleFile, JNI_FALSE)
        Dim outputf As String = env.GetStringUTFChars(env, outputFile, JNI_FALSE)

        Dim fp As FILE
        Dim i As Integer
        Dim doclines As Integer = GlobalMembersUtil.wc(rawf)
        Console.Write("The size is {0:D}" & vbLf, doclines)

        If (InlineAssignHelper(fp, fopen(rawf, "r"))) Is Nothing Then
            Return
        End If
        Dim selectedarray As Single() = New Single(doclines - 1) {}
        For i = 0 To doclines - 1
            'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
            Dim line As SByte = GlobalMembersUtil.readline(fp)
            selectedarray(i) = Convert.ToDouble(line)
            'C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(line)
        Next
        fclose(fp)
        qsort(selectedarray, doclines, 4, AddressOf GlobalMembersUtil.comparefloat)

        Dim fps As FILE
        Dim samplelines As Integer = GlobalMembersUtil.wc(samplef)
        If (InlineAssignHelper(fps, fopen(samplef, "r"))) Is Nothing Then
            Return
        End If

        Dim fpo As FILE
        If (InlineAssignHelper(fpo, fopen(outputf, "w+"))) Is Nothing Then
            Return
        End If

        For i = 0 To samplelines - 1
            'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
            Dim line As SByte = GlobalMembersUtil.readline(fps)
            Dim cnum As Single = Convert.ToDouble(line)
            'C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent for pointers to value types:
            Dim m As Single = GlobalMembersUtil.binary_search(selectedarray, selectedarray + doclines - 1, cnum)
            Dim location As Single = m - selectedarray + 1
            Dim pvalue As Single = (doclines - location + 1) / CSng(doclines)

            fprintf(fpo, "%E" & vbTab & "%s" & vbLf, pvalue, line)
        Next
        fclose(fps)
        fclose(fpo)

        Dim timer2 As time_t = time(Nothing)
        Dim dt As Double = difftime(timer2, timer1)
        Console.Write("The running time is {0:f}s" & vbLf, dt)
        Return
    End Sub
End Class



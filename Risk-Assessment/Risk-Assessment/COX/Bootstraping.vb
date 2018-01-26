Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Math

Namespace COX

    Public Module Bootstraping

        Public Delegate Sub SetIndividualStatus(individual As Model)

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="pcts">``{time -> hazard_ratio}``</param>
        ''' <param name="sampleSize%">观测的总人数</param>
        ''' <returns></returns>
        ''' 
        <Extension>
        Public Function CreateSample(pcts As Dictionary(Of Double, Double), timeBegin#, setHazard As SetIndividualStatus, setCensored As SetIndividualStatus, Optional sampleSize% = 1000) As Model()
            Dim samples As New List(Of Model)
            Dim rand As New Random
            Dim n%
            Dim timeRange As DoubleRange
            Dim lastRange As Double

            For Each time As (time#, pct#) In pcts _
                .OrderBy(Function(t) t.Key) _
                .EnumerateTuples

                timeRange = {timeBegin, time.time}
                n = sampleSize * time.pct

                For i As Integer = 0 To n - 1
                    Dim individual As New Model With {
                        .time = rand.NextDouble(timeRange),
                        .status = COXstatus.Die
                    }

                    Call setHazard(individual)
                    Call samples.Add(individual)
                Next

                lastRange = timeBegin
                timeBegin = time.time
                sampleSize -= n
            Next

            timeRange = {
                timeBegin,
                timeBegin + 2 * (timeBegin - lastRange)
            }

            For i As Integer = 0 To sampleSize - 1
                Dim individual As New Model With {
                    .time = rand.NextDouble(timeRange),
                    .status = COXstatus.Die
                }

                Call setCensored(individual)
                Call samples.Add(individual)
            Next

            Return samples
        End Function
    End Module
End Namespace
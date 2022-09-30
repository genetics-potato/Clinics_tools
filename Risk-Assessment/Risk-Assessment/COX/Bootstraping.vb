Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Math

Namespace COX

    Public Module Bootstraping

        Public Delegate Sub SetIndividualStatus(individual As Model)

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="pcts">
        ''' The hazard ratio in different time span. ``{time -> hazard_ratio}``
        ''' </param>
        ''' <param name="sampleSize%">观测的总人数</param>
        ''' <returns></returns>
        ''' 
        <Extension>
        Public Function CreateSample(pcts As Dictionary(Of Double, Double),
                                     timeBegin#,
                                     setHazard As SetIndividualStatus,
                                     setCensored As SetIndividualStatus,
                                     Optional sampleSize% = 1000) As Model()

            Dim samples As New List(Of Model)
            Dim rand As New Random
            Dim n%
            Dim timeRange As DoubleRange
            Dim lastRange As Double
            Dim timestamp As Double
            Dim pct As Double

            cat("\n\n")

            For Each time As KeyValuePair(Of Double, Double) In pcts.OrderBy(Function(t) t.Key)
                timestamp = time.Key
                pct = time.Value
                timeRange = {timeBegin, timestamp}
                n = sampleSize * pct

                For i As Integer = 0 To n - 1
                    Dim individual As New Model With {
                        .time = rand.NextDouble(timeRange),
                        .status = COXstatus.Die
                    }

                    Call setHazard(individual)
                    Call samples.Add(individual)
                Next

                lastRange = timeBegin
                timeBegin = timestamp
                sampleSize -= n

                ' 为了进行分隔，在所打印的消息后面是有一个空格的
                cat($"{n}/{sampleSize}  ")
            Next

            cat("\n\n")

            timeRange = {
                timeBegin,
                timeBegin + 2 * (timeBegin - lastRange)
            }

            For i As Integer = 0 To sampleSize - 1
                Dim individual As New Model With {
                    .time = rand.NextDouble(timeRange),
                    .status = COXstatus.Censored
                }

                Call setCensored(individual)
                Call samples.Add(individual)
            Next

            Dim uid As New Uid(caseSensitive:=True)

            For Each individual As Model In samples
                individual.ID = ++uid
            Next

            Return samples
        End Function
    End Module
End Namespace
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Text
Imports SMRUCC.genomics.SequenceModel.FASTA
Imports Microsoft.VisualBasic

Namespace SFM

    ''' <summary>
    ''' http://bioanalysis.otago.ac.nz/sfm/sfm_main.pl
    ''' </summary>
    Public Module Parser

        Const deliHeader As String = "#+-+\s*"
        Const inputAlignment As String = "Input Sequence/Alignement -------------------------------------------------------------------------------"

        Public Function ParseFile(path As String) As Output
            Dim text As String = path.ReadAllText
            Dim parts As String() = Regex.Split(text, deliHeader).Skip(1).ToArray
            Dim inputs As Inputs = InputsParser(parts(0))
            Dim mla As FastaFile = __parsingMla(parts(1))
            Dim out As New Output With {
                .inputs = inputs,
                .Alignment = mla
            }

            For Each part As String In parts.Skip(3)
                If InStr(part, "Identified regulatory elements from Transterm") > 0 Then
                    out.Regulatory = MotifParser(part).ToArray
                ElseIf InStr(part, "identified protein binding sites from RBPDB") > 0 Then
                    out.RBPDB = MotifParser(part).ToArray
                ElseIf InStr(part, "Identified 8 base seed sequence targets from human microRNA's (miRBase)") > 0 Then
                    out.miRBase = MotifParser(part).ToArray
                Else
                    Throw New NotImplementedException(part.lTokens.First)
                End If
            Next

            Return out
        End Function

        Private Function __parsingMla(part As String) As FastaFile
            Dim data As String = Mid(part, inputAlignment.Length + 1).Trim
            Dim fa As FastaFile = FastaFile.ParseDocument(data)
            Return fa
        End Function

        Public Function InputsParser(part As String) As Inputs
            Dim lines As String() = part.lTokens
            Dim s As New Value(Of String)
            Dim i As int = 5
            Dim inputs As New Inputs With {
                .bufs = New Dictionary(Of String, String())
            }
            Dim values As New List(Of String)
            Dim tt As String = New String(ASCII.TAB, 2)

            Do While Not (s = lines(++i)).IsBlank
                Dim key As String = (+s).Trim(ASCII.TAB, " "c, ":"c)

                Call values.Clear()

                Do While InStr(s = lines(++i), tt) = 1
                    values += (+s).Trim(ASCII.TAB, " "c)
                Loop

                i -= 1
                inputs.bufs(key) = values.ToArray
            Loop

            Return inputs
        End Function

        Public Iterator Function MotifParser(part As String) As IEnumerable(Of MotifLoci)
            Dim lines As String() = part.lTokens.Skip(1).ToArray
            Dim i As Integer

            For i = 0 To lines.Length - 1
                If lines(i).First <> "#"c Then
                    Exit For
                End If
            Next

            For Each line As String In lines.Skip(i)
                If line = "//" Then
                    Exit For
                End If

                Dim tokens As String() = line.Split(ASCII.TAB)

                Yield New MotifLoci With {
                    .Identifier = tokens(0),
                    .Sequence_header = tokens(1),
                    .UTR_Start = tokens(2),
                    .MSA_END = tokens(3),
                    .MSA_Start = tokens(4),
                    .UTR_END = tokens(5),
                    .UTR_Sequence_Fragment = tokens(6),
                    .MSA_Sequence_Fragment = tokens(7)
                }
            Next
        End Function
    End Module
End Namespace
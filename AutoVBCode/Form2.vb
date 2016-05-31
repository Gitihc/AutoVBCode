Imports System.Text.RegularExpressions
Imports System.Text

Public Class Form2

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Using dg As New OpenFileDialog
            With dg
                .Filter = "*|*.prt;*.rpt"
                .Multiselect = False
                If DialogResult.OK = .ShowDialog Then
                    HandFile(.FileName)
                End If
            End With
        End Using
    End Sub

    Public Sub HandFile(ByVal file)
        Dim str As String = System.IO.File.ReadAllText(file)
        Dim sp As Integer = str.IndexOf("Items Field")
        Dim ep As Integer = str.IndexOf("Items Column")
        str = str.Substring(sp, ep - sp)

        Dim reg As New System.Text.RegularExpressions.Regex("Item(\s.*){1,5}End")

        Dim matchs As MatchCollection = reg.Matches(str)
        Dim sb As New StringBuilder("")


        For Each itm In matchs
            Dim k = HandCode(itm.ToString)

            sb.AppendFormat("Rst.fields.Append ""{0}"", adVarChar,255, 32", k).AppendLine()
        Next

        sb.AppendLine().AppendLine()
        sb.Append("Rst.AddNew").AppendLine()
        For Each itm In matchs
            Dim k = HandCode(itm.ToString)
            sb.AppendFormat("rst.Fields(""{0}"")=", k).AppendLine()
        Next
        sb.Append("rst.Update")
        RichTextBox1.Text = sb.ToString
    End Sub

    Public Function HandCode(ByVal str As String) As String
        Dim ep As Integer = str.LastIndexOf("'")
        Return str.Substring(6, ep - 6)
    End Function

 
End Class
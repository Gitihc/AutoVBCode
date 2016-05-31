Imports System.Text.RegularExpressions
Imports System.Text
Imports HlLibrary

Public Class Form1

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

        Dim reg As New System.Text.RegularExpressions.Regex("Name='.*'")
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
        sb.Append("rst.Update").AppendLine.AppendLine()
        RichTextBox1.Text = sb.ToString
    End Sub

    Public Function HandCode(ByVal str As String) As String
        Dim ep As Integer = str.LastIndexOf("'")
        Return str.Substring(6, ep - 6)
    End Function

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSub()
        GetClass()
    End Sub

    Public Sub GetSub()
        Using stream As New System.IO.StreamReader("VB的转换函数.txt")
            Dim vbcode = stream.ReadToEnd
            RichTextBox2.Text = vbcode.ToString()
        End Using
        Using stream2 As New System.IO.StreamReader("模版.txt")
            Dim vbmode = stream2.ReadToEnd
            RichTextBox3.Text = vbmode
        End Using
    End Sub

    'vb转换函数
    Private Sub RichTextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RichTextBox2.TextChanged
        Try
            Dim vbcode As String = RichTextBox2.Text
            System.IO.File.WriteAllText("VB的转换函数.txt", vbcode)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    '模版
    Private Sub RichTextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RichTextBox3.TextChanged
        Try
            Dim vbcode As String = RichTextBox3.Text
            System.IO.File.WriteAllText("VB模版.txt", vbcode)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub


    Private Sub GetClass()
        '加载程序集(dll文件地址)，使用Assembly类   
        Dim assembly As Reflection.Assembly = Reflection.Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory & "HlLibrary.dll")
        'ComboBox1.DisplayMember = ""
        'ComboBox1.ValueMember

        For Each tmp In assembly.GetTypes
            'ComboBox1.Items.Add(tmp)
            'ComboBox2.Items.Add(tmp.Name)
            Select Case tmp.MemberType
                Case Reflection.MemberTypes.Field
                    ComboBox4.Items.Add(tmp)
                Case Reflection.MemberTypes.Method
                    ComboBox5.Items.Add(tmp)
                Case Reflection.MemberTypes.Property
                    ComboBox6.Items.Add(tmp)
                Case Reflection.MemberTypes.Event
                    ComboBox7.Items.Add(tmp)
                Case Reflection.MemberTypes.TypeInfo
                    ComboBox1.Items.Add(tmp)
            End Select
        Next

        ''获取类型，参数（名称空间 类）   
        'Dim type As Type = assembly.GetType("assembly_name.assembly_class")

        ''创建该对象的实例，object类型，参数（名称空间 类）   
        'Dim instance As Object = assembly.CreateInstance("assembly_name.assembly_class")

        ''设置Show_Str方法中的参数类型，Type[]类型；如有多个参数可以追加多个   
        'Dim params_type() As Type = New Type(1) {}
        'params_type(0) = type.GetType("System.String")
        ''设置Show_Str方法中的参数值；如有多个参数可以追加多个   
        'Dim params_obj() As Object = New Object(1) {}
        'params_obj(0) = "jiaopeng"
        ''执行Show_Str方法()
        'Dim value As Object = type.GetMethod("Show_Str", params_type).Invoke(instance, params_obj)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim type As Type = ComboBox1.SelectedItem
        If type.IsClass Then
            RichTextBox4.Text = type.FullName
        End If
    End Sub
End Class

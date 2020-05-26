Imports System.ComponentModel

Public Class FrmMessages

    Private Sub FrmMessages_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveSetting("UDP Tool", "Settings", "Add Time", CheckBox1.Checked)
        SaveSetting("UDP Tool", "Settings", "WrapText", CheckBox2.Checked)
        SaveSetting("UDP Tool", "Settings", "Snap", ChkSnap.Checked)
        SaveSetting("UDP Tool", "Settings", "MessagesWidth", Me.Width)
        SaveSetting("UDP Tool", "Settings", "MessagesHeight", Me.Height)
    End Sub

    Private Sub FrmMessages_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckBox2.Checked = GetSetting("UDP Tool", "Settings", "WrapText", True)
        CheckBox1.Checked = GetSetting("UDP Tool", "Settings", "Add Time", False)
        ChkSnap.Checked = GetSetting("UDP Tool", "Settings", "Snap", True)
        '    Timer1.Enabled = ChkSnap.Checked
        Me.Width = GetSetting("UDP Tool", "Settings", "MessagesWidth", 430)
        Me.Height = GetSetting("UDP Tool", "Settings", "MessagesHeight", 500)
        If OS = "WIN10" Then
            '  Me.Left = FrmMain.Left + FrmMain.Width - 6
            If DisplayRatio = "4/3" Then
                Me.Left = FrmMain.Left + FrmMain.Width - 6
            ElseIf DisplayRatio = "16/9" Then
                Me.Left = FrmMain.Left + FrmMain.Width - 15
            Else
                Me.Left = FrmMain.Left - Me.Width
            End If
        Else
            Me.Left = FrmMain.Left + FrmMain.Width
        End If
        '    Me.Left = FrmMain.Left + FrmMain.Width
        Me.Top = FrmMain.Top
        '  Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If OS = "WIN10" Then
            If DisplayRatio = "4/3" Then
                Me.Left = FrmMain.Left + FrmMain.Width - 6
            ElseIf DisplayRatio = "16/9" Then
                Me.Left = FrmMain.Left + FrmMain.Width - 15
            Else
                Me.Left = FrmMain.Left - Me.Width
            End If
        Else
            Me.Left = FrmMain.Left + FrmMain.Width
        End If
        Me.Top = FrmMain.Top
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RichTextBox1.Text = Nothing
    End Sub

    Private Sub BtnPlus_Click(sender As Object, e As EventArgs) Handles BtnPlus.Click

        RichTextBox1.ZoomFactor = RichTextBox1.ZoomFactor + 1



    End Sub

    Private Sub BtnMinus_Click(sender As Object, e As EventArgs) Handles BtnMinus.Click
        If RichTextBox1.ZoomFactor > 1 Then
            RichTextBox1.ZoomFactor = RichTextBox1.ZoomFactor - 1
        End If

    End Sub

    Private Sub BtnZero_Click(sender As Object, e As EventArgs) Handles BtnZero.Click
        RichTextBox1.ZoomFactor = 1
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        RichTextBox1.WordWrap = CheckBox2.Checked
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        SaveFileDialog1.Filter = "TXT Files (*.txt*)|*.txt"
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK _
       Then
            My.Computer.FileSystem.WriteAllText _
         (SaveFileDialog1.FileName, RichTextBox1.Text, True)
        End If


        'Dim file As System.IO.StreamWriter
        'file = My.Computer.FileSystem.OpenTextFileWriter("\save.txt", True)
        'file.WriteLine("Here is the first string.")
        'file.Close()
    End Sub


    Private Sub RichTextBox1_SelectionChanged(sender As Object, e As EventArgs) Handles RichTextBox1.SelectionChanged
        If RichTextBox1.SelectedText <> Nothing Then

            My.Computer.Clipboard.SetText(RichTextBox1.SelectedText)
        End If

    End Sub

    Private Sub ChkSnap_CheckedChanged(sender As Object, e As EventArgs) Handles ChkSnap.CheckedChanged
        Timer1.Enabled = ChkSnap.Checked
    End Sub
End Class
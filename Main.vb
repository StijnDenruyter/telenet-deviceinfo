Module Main
    Private DeviceInfoIcon As NotifyIcon
    Sub Main()
        DeviceInfoIcon = New NotifyIcon
        DeviceInfoIcon.Icon = New Icon("Icon.ico")
        GetDeviceInfo()
        DeviceInfoIcon.Visible = True
        Application.Run()
    End Sub
    Private Sub GetDeviceInfo()
        Dim Hostname As String = Environment.MachineName
        Dim Username As String = Environment.UserName
        DeviceInfoIcon.Text = "Hostname: " & Hostname & vbCrLf & "Username: " & Username
    End Sub
End Module
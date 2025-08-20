Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO

Module Program
    <STAThread()>
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New TrayAppContext())
    End Sub
End Module

Friend NotInheritable Class TrayAppContext
    Inherits ApplicationContext

    Private ReadOnly DeviceInfoIcon As NotifyIcon
    Private ReadOnly SingleClickTimer As Timer
    Private ReadOnly Hostname As String = Environment.MachineName
    Private ReadOnly Username As String = Environment.UserName

    Public Sub New()
        DeviceInfoIcon = New NotifyIcon() With {
            .Icon = LoadIcon(),
            .Visible = True,
            .Text = "Device info"
        }

        AddHandler DeviceInfoIcon.MouseClick, AddressOf OnTrayMouseClick
        AddHandler DeviceInfoIcon.MouseDoubleClick, AddressOf OnTrayMouseDoubleClick

        SingleClickTimer = New Timer() With {
            .Interval = SystemInformation.DoubleClickTime
        }
        AddHandler SingleClickTimer.Tick, AddressOf OnSingleClickTimerTick

        AddHandler Application.ApplicationExit, AddressOf OnApplicationExit
    End Sub

    Private Shared Function LoadIcon() As Icon
        Dim icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icon.ico")
        If File.Exists(icoPath) Then
            Return New Icon(icoPath)
        End If
        ' Fallback: use the executable's icon
        Return Icon.ExtractAssociatedIcon(Application.ExecutablePath)
    End Function

    Private Sub OnTrayMouseClick(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            ' Start a single-click timer; will be canceled if a double-click is detected
            SingleClickTimer.Stop()
            SingleClickTimer.Start()
        End If
    End Sub

    Private Sub OnTrayMouseDoubleClick(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            SingleClickTimer.Stop()
            Clipboard.SetText(Hostname)
            'DeviceInfoIcon.BalloonTipTitle = "Device info"
            DeviceInfoIcon.BalloonTipText = "Hostname copied to clipboard"
            DeviceInfoIcon.BalloonTipIcon = ToolTipIcon.Info
            DeviceInfoIcon.ShowBalloonTip(1500)

        End If
    End Sub

    Private Sub OnSingleClickTimerTick(sender As Object, e As EventArgs)
        SingleClickTimer.Stop()
        'DeviceInfoIcon.BalloonTipTitle = "Device info"
        DeviceInfoIcon.BalloonTipText = $"Hostname: {Hostname}{vbCrLf}Username: {Username}"
        DeviceInfoIcon.BalloonTipIcon = ToolTipIcon.None
        DeviceInfoIcon.ShowBalloonTip(3000)
    End Sub

    Private Sub OnApplicationExit(sender As Object, e As EventArgs)
        DeviceInfoIcon.Visible = False
        DeviceInfoIcon.Dispose()
        SingleClickTimer.Dispose()
    End Sub
End Class

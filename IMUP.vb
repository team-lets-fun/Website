Namespace WorkingArea
    Public Module Windows
        Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Long
        Public Function GetWindowHwnd(ByVal WindowClassVsWindowText As String) As Long
            Dim chwnd As Long
            chwnd = FindWindow(vbNullString, WindowClassVsWindowText)
            If chwnd = 0 Then
                chwnd = FindWindow(WindowClassVsWindowText, vbNullString)
            End If
            Return chwnd
        End Function
    End Module
    Public Class Volume
        <Runtime.InteropServices.DllImport("user32.dll")>
        Private Shared Function SendMessageW(hWnd As IntPtr, Msg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
        End Function
        Public Shared Sub TurnUp(Handle As IntPtr)
            SendMessageW(Handle, &H319, Handle, New IntPtr(&HA0000))
        End Sub
        Public Shared Sub TurnMute(Handle As IntPtr)

            SendMessageW(Handle, &H319, Handle, New IntPtr(&H80000))
        End Sub
        Public Shared Sub TurnDown(Handle As IntPtr)
            SendMessageW(Handle, &H319, Handle, New IntPtr(&H90000))
        End Sub
    End Class
    Public Module Window
        Public Class ProcessProtect
            Declare Function RtlAdjustPrivilege Lib "ntdll.dll" (ByVal Privilege As Integer, ByVal NewValue As Integer, ByVal NewThread As Integer, ByRef OldValue As Integer) As Integer
            Declare Function NtSetInformationProcess Lib "ntdll.dll" (ByVal ProcessHandle As IntPtr, ByVal ProcessInformationClass As Integer, ByRef ProcessInformation As Integer, ByVal ProcessInformationLength As Integer) As Integer
            Private b As Integer = 0
            ''' <summary>
            ''' 获取进程保护状态
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property GetProtectionState As Boolean
                Get
                    Return b = 1
                End Get
            End Property
            ''' <summary>
            ''' 启动进程保护
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub AntiKill()
                b = 1
                NtSetInformationProcess(-1, &H1D, b, 4)
            End Sub
            ''' <summary>
            ''' 卸载进程保护
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub AllowKill()
                b = 0
                NtSetInformationProcess(-1, &H1D, b, 4)
            End Sub
            ''' <summary>
            ''' 初始化进程保护类
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                RtlAdjustPrivilege(20, 1, 0, b)
            End Sub
        End Class
        Public Class RtlAdjustPrivilege
            ''' <summary>
            ''' 权限列表
            ''' </summary>
            ''' <remarks>
            ''' SeAssignPrimaryTokenPrivilege替换进程级记号,允许初始化一个进程,以取代与已启动的子进程相关的默认令牌.<br></br>
            '''SeAuditPrivilege产生安全审核,允许将条目添加到安全日志.<br></br>
            '''SeBackupPrivilege备份文件和目录,不多说了,就是翻阅遍历,执行文件,读取文件和文件夹所有信息的权限<br></br>
            '''SeChangeNotifyPrivilege跳过遍历检查,允许用户来回移动目录,但是不能列出文件夹的内容<br></br>
            '''SeCreatePagefilePrivilege创建页面文件,允许用户创建和改变一个分页文件的大小<br></br>
            '''SeCreatePermanentPrivilege创建永久共享对象<br></br>
            '''SeCreateTokenPrivilege创建令牌对象,允许进程调用NtCreateToken()或者是其他的Token-Creating APIs创建一个访问令牌<br></br>
            '''SeDebugPrivilege允许访问所有进程.<br></br>
            '''SeIncreaseBasePriorityPrivilege更改优先级时,只有获得此权限后才能设置进程优先级为"实时"<br></br>
            '''SeIncreaseQuotaPrivilege调整进程的内存配额<br></br>
            '''SeLoadDriverPrivilege装载和卸载设备驱动程序,允许动态地加载和卸载设备驱动程序.安装即插即用设备的驱动程序时需要此特权.<br></br>
            '''SeLockMemoryPrivilege内存中锁定页,允许使用进程在物理内存中保存数据,从而避免系统将这些数据分页保存到磁盘的虚拟内存中.采用此策略会减少可用的随机存取内存(RAM)总数,从而可能极大地影响系统性能.<br></br>
            '''SeMachineAccountPrivilege域中添加工作站,用于识别 Active Directory 中已有的帐户和组.<br></br>
            '''SeProfileSingleProcessPrivilege配置单一进程,允许使用性能监视工具来监视非系统进程的性能.<br></br>
            '''SeRemoteShutdownPrivilege从远端系统强制关机,允许从网络上的远程位置关闭计算机.<br></br>
            '''SeRestorePrivilege还原文件和目录,允许绕过文件及目录权限来恢复备份文件.<br></br>
            '''SeSecurityPrivilege管理审核和安全日志,允许指定文件,Active Directory对象和注册表项之类的单个资源的对象访问审核选项.还可以查看和清除安全日志.<br></br>
            '''SeShutdownPrivilege 关闭系统<br></br>
            '''SeSystemEnvironmentPrivilege修改固件环境值,查看,修改环境变量SET命令.<br></br>
            '''SeSystemProfilePrivilege配置系统性能,允许监视系统进程的性能.<br></br>
            '''SeSystemtimePrivilege更改系统时间<br></br>
            '''SeTakeOwnershipPrivilege获得文件或对象的所有权,包括 Active Directory 对象,文件和文件夹,打印机,注册表项,进程和线程.<br></br>
            '''SeTcbPrivilege以操作系统方式操作,成为操作系统的一部分<br></br>
            '''SeUnsolicitedInputPrivilege从终端设备读取未经请求的输入<br></br>
            '''SeImpersonatePrivilege身份验证后模拟客户端<br></br>
            '''SeManageVolumePrivilege执行卷维护任务<br></br>
            '''SeUndockPrivilege从插接工作站中取出计算机<br></br>
            '''SeBatchLogonRight作为批处理作业登录<br></br>
            '''SeInteractiveLogonRight本地登录<br></br>
            '''SeNetworkLogonRight从网络访问此计算机<br></br>
            '''SeServiceLogonRight作为服务登录<br></br>
            ''' </remarks>
            Public Enum Privilege
                Unknown = &H1S
                SeCreateTokenPrivilege = &H2S
                SeAssignPrimaryTokenPrivilege = &H3S
                SeLockMemoryPrivilege = &H4S
                SeIncreaseQuotaPrivilege = &H5S
                SeUnsolicitedInputPrivilege = &H0S
                SeMachineAccountPrivilege = &H6S
                SeTcbPrivilege = &H7S
                SeSecurityPrivilege = &H8S
                SeTakeOwnershipPrivilege = &H9S
                SeLoadDriverPrivilege = &HAS
                SeSystemProfilePrivilege = &HBS
                SeSystemtimePrivilege = &HCS
                SeProfileSingleProcessPrivilege = &HDS
                SeIncreaseBasePriorityPrivilege = &HES
                SeCreatePagefilePrivilege = &HFS
                SeCreatePermanentPrivilege = &H10S
                SeBackupPrivilege = &H11S
                SeRestorePrivilege = &H12S
                SeShutdownPrivilege = &H13S
                SeDebugPrivilege = &H14S
                SeAuditPrivilege = &H15S
                SeSystemEnvironmentPrivilege = &H16S
                SeChangeNotifyPrivilege = &H17S
                SeRemoteShutdownPrivilege = &H18S
                SeUndockPrivilege = &H19S
                SeSyncAgentPrivilege = &H1AS
                SeEnableDelegationPrivilege = &H1BS
                SeManageVolumePrivilege = &H1CS
                SeImpersonatePrivilege = &H1DS
                SeCreateGlobalPrivilege = &H1ES
                SeTrustedCredManAccessPrivilege = &H1FS
                SeRelabelPrivilege = &H20S
                SeIncreaseWorkingSetPrivilege = &H21S
                SeTimeZonePrivilege = &H22S
                SeCreateSymbolicLinkPrivilege = &H23S
                SeDelegateSessionUserImpersonatePrivilege = &H24S
            End Enum
            Declare Function RtlAdjustPrivilege Lib "ntdll.dll" (ByVal Privilege As Integer,
            ByVal NewValue As Integer, ByVal NewThread As Integer, ByRef OldValue As Integer) As Integer
            '''<summary>
            '''构造提取函数
            '''</summary>
            '''<remarks></remarks>
            '''<param name="Privilege">Privilege中的项</param>  
            '''<param name="Switch">设置权限状态</param>  
            '''<param name="OnlyCurrentThread">确定是否在该线程/全进程中提取中</param>
            '''<param name="OldPrivilege">指定是否输出该权限的就状态</param>  
            Public Sub New(ByVal Privilege As Integer,
            ByVal Switch As Boolean, ByVal OnlyCurrentThread As Boolean, ByRef OldPrivilege As Boolean)
                RtlAdjustPrivilege(Privilege, Switch, OnlyCurrentThread, OldPrivilege)
            End Sub

        End Class
        Public Class Window
            <Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True)>
            Private Shared Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As Integer) As Boolean
            End Function
            Private Const SWP_NOSIZE As Integer = &H1
            Private Const SWP_NOMOVE As Integer = &H2
            Private Shared ReadOnly HWND_TOPMOST As New IntPtr(-1)
            Private Shared ReadOnly HWND_NOTOPMOST As New IntPtr(-2)
            Public Shared Function SetTopMost(a As IntPtr) As Integer
                SetWindowPos(a, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE)
                Return 0
            End Function
            Public Shared Function SetNormal(a As IntPtr) As Integer
                SetWindowPos(a, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE)
                Return 0
            End Function
        End Class
        Public Class Disk
            Declare Function DeviceIoControl Lib "kernel32" Alias "DeviceIoControl" (ByVal hDevice As IntPtr, ByVal dwIoControlCode As Integer, ByRef lpInBuffer As Object, ByVal nInBufferSize As Integer, ByRef lpOutBuffer As StorageDeviceNumber, ByVal nOutBufferSize As Integer, ByRef lpBytesReturned As Integer, ByVal lpOverlapped As Integer) As Integer

            Declare Function CreateFile Lib "kernel32" Alias "CreateFileA" (ByVal lpFileName As String, ByVal dwDesiredAccess As Integer, ByVal dwShareMode As Integer, ByVal lpSecurityAttributes As IntPtr, ByVal dwCreationDisposition As Integer, ByVal dwFlagsAndAttributes As Integer, ByVal hTemplateFile As Integer) As Integer

            Declare Function CloseHandle Lib "kernel32" Alias "CloseHandle" (ByVal hObject As IntPtr) As Integer

            Public Structure StorageDeviceNumber
                Public DeviceType As UInteger
                Public DeviceNumber As UInteger
                Public PartitionNumber As UInteger
            End Structure

            ''' <summary>
            ''' 从逻辑分区获取物理硬盘
            ''' </summary>
            ''' <param name="DriveId">逻辑分区号</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function GetPhysicalDisk(ByVal DriveId As Char) As UInteger
                Dim DHandle As IntPtr = CreateFile("\\.\" & DriveId & ":", 4 Or 8, 1 Or 2, Nothing, 3, 0, Nothing)
                If DHandle = -1 Then Throw New Exception("错误 - 获取句柄失败")
                Dim DiskInfo As New StorageDeviceNumber
                Dim ReturnedByteLength As Integer = 0
                Dim Result As Boolean = DeviceIoControl(DHandle, &H2D1080, 0, 0, DiskInfo, 122, ReturnedByteLength, Nothing)
                CloseHandle(DHandle)
                If Not Result Then Throw New Exception("错误 - 获取物理磁盘号失败")
                If ReturnedByteLength = 0 Then Throw New Exception("错误 - 返回值为空")
                Return DiskInfo.DeviceNumber
            End Function
        End Class

        Public Class HotKey
            Private Declare Function RegisterHotKey Lib "user32" (ByVal hWnd As Integer, ByVal id As Integer,
                                                    ByVal fsModifiers As Integer, ByVal vk As Integer) As Integer
            Private Declare Function UnregisterHotKey Lib "user32" (ByVal hWnd As Integer, ByVal id As Integer) As Integer

            Public Const Mod_Alt As Short = &H1S
            Public Const Mod_Control As Short = &H2S
            Public Const Mod_Shift As Short = &H4S
            Public Const Mod_Ctrl_Alt As Short = &H3S
            Public Const Mod_Alt_Shift As Short = &H5S
            Public Const Mod_Ctrl_Shift As Short = &H6S
            Public Const Mod_Ctrl_alt_Shift As Short = &H7S
            Public Const WM_HOTKEY As Short = &H312S
            '0=nothing 1 -alt 2-ctrl 3-ctrl+alt 4-shift 5-alt+shift 6-ctrl+shift 7-ctrl+shift+alt

            Public Shared Function UnregisterHotKeyL(Handle As IntPtr, id As Integer)
                Return UnregisterHotKey(Handle, id)
            End Function
            Public Shared Function RegisterHotKeyL(Handle As IntPtr, RegisterId As Integer, Modifiers As Short, uVirtKey1 As Keys) As Boolean
                Return RegisterHotKey(Handle, RegisterId, Modifiers, uVirtKey1)
            End Function
            Public Shared Function GetCode() As String
                Return "
Protected Overrides Sub WndProc(ByRef m As Message)
    If m.Msg = IMUP.WorkingArea.Window.HotKey.WM_HOTKEY Then
        Select Case m.WParam.ToInt32 'Register Id
            Case 1
                
            Case 2
                
            End Select
    End If 
MyBase.WndProc(m) 
End Sub
"
            End Function
            Public Function UnRegisterHotKeyL(RegisterId As Integer, Handle As IntPtr, Modifiers As Short, uVirtKey1 As Keys) As Boolean
                UnregisterHotKey(Handle.ToInt32, uVirtKey1)
                Return True
            End Function
        End Class
        Public Class SelfImprovePermission
            ''' <summary>
            ''' 构造自提取函数
            ''' </summary>
            ''' <remarks>
            ''' 将会重新启动进程
            ''' </remarks>
            ''' <param name="Application">Application</param>
            Public Sub New(ApplicationN As Application)
                Dim startexe = ApplicationN.ExecutablePath
                Dim identity As System.Security.Principal.WindowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent()
                Dim principal = New System.Security.Principal.WindowsPrincipal(identity)
                Dim res = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator)
                Dim startInfo = New ProcessStartInfo()
                startInfo.FileName = startexe
                startInfo.UseShellExecute = True
                startInfo.Verb = "runas"
                startInfo.Arguments = Nothing
                Process.Start(startInfo)
            End Sub
        End Class
        Public Class CreateProcessAsAdministrator
            Public Sub New()

            End Sub
            ''' <summary>
            ''' 创建进程
            ''' </summary>
            ''' <param name="startexe">启动文件</param>
            ''' <param name="strPara">启动参数</param>
            Public Sub Create(startexe As String, Optional strPara As String = Nothing)
                Dim identity As System.Security.Principal.WindowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent()
                Dim principal = New System.Security.Principal.WindowsPrincipal(identity)
                Dim res = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator)
                Dim startInfo = New ProcessStartInfo()
                startInfo.FileName = startexe
                startInfo.UseShellExecute = True
                startInfo.Verb = "runas"
                startInfo.Arguments = strPara
                Process.Start(startInfo)
            End Sub
        End Class
        Public Class KeyEvent
            Private Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Long, ByVal dwExtraInfo As Long)
            Private Declare Function MapVirtualKey Lib "user32" Alias "MapVirtualKeyA" (ByVal wCode As Long, ByVal wMapType As Long) As Long
            Public Shared Sub KeyDown(VisualKey As Long)
                keybd_event(VisualKey, MapVirtualKey(VisualKey, 0), 0, 0)
            End Sub
            Public Shared Sub KeyUp()
                keybd_event(VisualKeys.Keyup, MapVirtualKey(VisualKeys.Keyup, 0), 0, 0)
            End Sub
            Public Shared Sub KeyPress(VisualKey As Long)
                keybd_event(VisualKey, MapVirtualKey(VisualKey, 0), 0, 0)
                keybd_event(VisualKeys.Keyup, MapVirtualKey(VisualKeys.Keyup, 0), 0, 0)
            End Sub
            Public Structure VisualKeys
                Public Const Add As Long = &H6B
                Public Const Apps As Long = &H5D
                Public Const Back As Long = &H8
                Public Const Capital As Long = &H14
                Public Const Cancel As Long = &H3
                Public Const Control As Long = &H11
                Public Const Decimal_ As Long = &H6E
                Public Const Delete As Long = &H2E
                Public Const Divide As Long = &H6F
                Public Const Down As Long = &H28
                Public Const End_ As Long = &H23
                Public Const Escape As Long = &H1B
                Public Const F1 As Long = &H70
                Public Const F10 As Long = &H79
                Public Const F11 As Long = &H7A
                Public Const F12 As Long = &H7B
                Public Const F2 As Long = &H71
                Public Const F3 As Long = &H72
                Public Const F4 As Long = &H73
                Public Const F5 As Long = &H74
                Public Const F6 As Long = &H75
                Public Const F7 As Long = &H76
                Public Const F8 As Long = &H77
                Public Const F9 As Long = &H78
                Public Const Home As Long = &H24
                Public Const Insert As Long = &H2D
                Public Const Lcontrol As Long = &HA2
                Public Const Left As Long = &H25
                Public Const Lmenu As Long = &HA4
                Public Const Lshift As Long = &HA0
                Public Const Lwin As Long = &H5B
                Public Const Menu As Long = &H12
                Public Const Multiply As Long = &H6A
                Public Const Next_ As Long = &H22
                Public Const Numlock As Long = &H90
                Public Const Numpad0 As Long = &H60
                Public Const Numpad1 As Long = &H61
                Public Const Numpad2 As Long = &H62
                Public Const Numpad3 As Long = &H63
                Public Const Numpad4 As Long = &H64
                Public Const Numpad5 As Long = &H65
                Public Const Numpad6 As Long = &H66
                Public Const Numpad7 As Long = &H67
                Public Const Numpad8 As Long = &H68
                Public Const Numpad9 As Long = &H69
                Public Const Pause As Long = &H13
                Public Const Print As Long = &H2A
                Public Const Prior As Long = &H21
                Public Const Rcontrol As Long = &HA3
                Public Const Return_ As Long = &HD
                Public Const Right As Long = &H27
                Public Const Rmenu As Long = &HA5
                Public Const Rshift As Long = &HA1
                Public Const Rwin As Long = &H5C
                Public Const Scroll As Long = &H91
                Public Const Separator As Long = &H6C
                Public Const Sshif As Long = &H10
                Public Const Sleep As Long = &H5F
                Public Const Snapshot As Long = &H2C
                Public Const Space As Long = &H20
                Public Const Subtract As Long = &H6D
                Public Const Tab As Long = &H9
                Public Const Up As Long = &H26
                Public Const OEM_1 As Long = &HBA
                Public Const OEM_2 As Long = &HBF
                Public Const OEM_3 As Long = &HC0
                Public Const OEM_4 As Long = &HDB
                Public Const OEM_5 As Long = &HDC
                Public Const OEM_6 As Long = &HDD
                Public Const OEM_7 As Long = &HDE
                Public Const OEM_Comma As Long = &HBC
                Public Const OEM_Minus As Long = &HBD
                Public Const OEM_Period As Long = &HBE
                Public Const OEM_Plus As Long = &HBB

                Public Const Keyup = &H2
                Public Const Extendedkey = &H1
            End Structure
        End Class
        Public Class Mouse
            Public Declare Sub mouse_event Lib "user32" (ByVal dwFlags As Long, ByVal dx As Long, ByVal dy As Long, ByVal dwData As Long, ByVal dwExtraInfo As Long)

            Public Enum MouseEventFlags As UInteger
                ABSOLUTE = &H8000
                LEFTDOWN = &H2
                LEFTUP = &H4
                MIDDLEDOWN = &H20
                MIDDLEUP = &H40
                MOVE = &H1
                RIGHTDOWN = &H8
                RIGHTUP = &H10
                XDOWN = &H80
                XUP = &H100
                WHEEL = &H800
                HWHEEL = &H1000
            End Enum

            Public Shared Sub Click(ByVal Mode As Short, Optional Amod As Short = Nothing)
                If Amod = Nothing Then
                    mouse_event(Mode, 0, 0, 0, 0)
                Else
                    mouse_event(MouseEventFlags.LEFTDOWN Or MouseEventFlags.LEFTUP, 0, 0, 0, 0)
                End If
            End Sub
            Public Shared Function Move(ByVal Optional px_x As Integer = 0, ByVal Optional px_y As Integer = 0)
                Cursor.Position = New Point(Cursor.Position.X + px_x, Cursor.Position.Y + px_y)
                Return True
            End Function
            'Friend Function Main()
            '    Return "INSIDE-LIB IS NOT A CONSOLE COMMAND"
            'End Function
        End Class
        Public Class MenuBar
            Private Declare Function GetSystemMenu Lib "User32" (ByVal hwnd As Integer, ByVal bRevert As Long) As Integer

            Private Declare Function RemoveMenu Lib "User32" (ByVal hMenu As Integer, ByVal nPosition As Integer, ByVal wFlags As Integer) As Integer

            Private Declare Function DrawMenuBar Lib "User32" (ByVal hwnd As Integer) As Integer

            Private Declare Function GetMenuItemCount Lib "User32" (ByVal hMenu As Integer) As Integer

            Public Shared Function GetDisableAltF4Code() As String
                Return "Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
    If keyData = (Keys.Alt Or Keys.F4) Then
        Return True
    Else
        Return MyBase.ProcessCmdKey(msg, keyData)
    End If
End Function"
            End Function

            Private Const MF_BYPOSITION = &H400&

            Private Const MF_DISABLED = &H2&

            Private Shared Sub DisableCloseButton(ByVal wnd As Form)

                On Error Resume Next

                Dim hMenu As Integer, nCount As Integer

                hMenu = GetSystemMenu(wnd.Handle.ToInt32, 0)

                nCount = GetMenuItemCount(hMenu)

                Call RemoveMenu(hMenu, nCount - 1, MF_BYPOSITION Or MF_DISABLED)

                DrawMenuBar(wnd.Handle.ToInt32)

            End Sub
            Public Shared Function DiableFormCloseButton(wnd As Form) As Integer
                Try
                    DisableCloseButton(wnd)
                    Return 1
                Catch ex As Exception
                    Return ex.HResult
                End Try
            End Function
        End Class
    End Module
    Public Module Cryptography
        Public Function GetFileHash(ByVal path As String)
            Dim hash = Security.Cryptography.SHA1.Create()
            Dim stream = New IO.FileStream(path, IO.FileMode.Open)
            Dim hashByte() As Byte = hash.ComputeHash(stream)
            stream.Close()
            Return BitConverter.ToString(hashByte).Replace("-", "")
        End Function
        Public Function GetSha256HashByString(ByVal word As String, ByVal Optional toUpper As Boolean = True)
            Try
                Dim SHA256CSP = New System.Security.Cryptography.SHA256CryptoServiceProvider()

                Dim bytValue() As Byte = System.Text.Encoding.UTF8.GetBytes(word)
                Dim bytHash() As Byte = SHA256CSP.ComputeHash(bytValue)
                SHA256CSP.Clear()
                Dim sHash As String = ""
                Dim sTemp As String = ""
                For counter = 0 To bytHash.Count() - 1
                    Dim i As Long = bytHash(counter) / 16
                    If (i > 9) Then
                        sTemp = (ChrW(i - 10 + &H41)).ToString()
                    Else
                        sTemp = (ChrW(i + &H30)).ToString()
                    End If
                    i = bytHash(counter) Mod 16
                    If (i > 9) Then
                        sTemp += (ChrW(i - 10 + &H41)).ToString()
                    Else
                        sTemp += (ChrW(i + &H30)).ToString()
                    End If
                    sHash += sTemp
                Next
                If toUpper Then
                    sHash.ToLower()
                End If
                Return sHash
            Catch Ex As Exception
                Throw New Exception(Ex.Message)
            End Try
        End Function

    End Module
    Public Module Paths
        Public Function UserProfile() As String
            Dim filePath As String = Environment.ExpandEnvironmentVariables("%USERPROFILE%")
            Dim d As IO.DirectoryInfo = New IO.DirectoryInfo(filePath)
            Dim d1 = d.GetDirectories()
            Return d.FullName
        End Function
        Public Function Temp() As String
            Dim filePath As String = Environment.ExpandEnvironmentVariables("%TEMP%")
            Dim d As IO.DirectoryInfo = New IO.DirectoryInfo(filePath)
            Dim d1 = d.GetDirectories()
            Return d.FullName
        End Function
        Public Function AppData() As String
            Dim filePath As String = Environment.ExpandEnvironmentVariables("%APPDATA%")
            Dim d As IO.DirectoryInfo = New IO.DirectoryInfo(filePath)
            Dim d1 = d.GetDirectories()
            Return d.FullName
        End Function
    End Module
    Module SystemPath
        Public Function WinDir() As String
            Dim filePath As String = Environment.ExpandEnvironmentVariables("%WINDIR%")
            Dim d As IO.DirectoryInfo = New IO.DirectoryInfo(filePath)
            Dim d1 = d.GetDirectories()
            Return d.FullName
        End Function
        Public Function RandomNum() As Integer
            Dim filePath As String = Environment.ExpandEnvironmentVariables("%RANDOM%")
            Dim d As IO.DirectoryInfo = New IO.DirectoryInfo(filePath)
            Dim d1 = d.GetDirectories()
            Return CInt(d.FullName)
        End Function
    End Module
End Namespace
'Namespace Trash___
'    Namespace Experiment
'        Namespace Data
'            Module Instruction
'                Public Const MakerName As String =
'                     "I Am System32"
'                Public Const Version As String =
'                    "1.0.0"
'            End Module
'        End Namespace
'        Namespace Codes
'            Namespace Experimental
'                Module Test
'                    Public Sub Main_()
'                        MsgBox(Classes.Null.Null)
'                    End Sub
'                End Module
'            End Namespace
'            Namespace Completed
'                Module JustNothing

'                End Module
'            End Namespace
'        End Namespace
'        Namespace Classes
'            Class Null
'                Public Const Null = Nothing
'            End Class
'        End Namespace
'        Namespace Structures
'            Structure Empty

'            End Structure
'        End Namespace
'    End Namespace
'    Public Class ECPT
'        Private bz As Int32
'        Const null As Object = Nothing
'        Public Const n As String = vbCrLf
'        Private msswrkey As String = "WRONG PRIVATE KEY"
'        Private mssnullstr As String = "NULL STRING"
'        Public PrivateKeyValue As String
'        Public MainString As String
'        Private ReturnValue As String

'        Public Function Run(ByVal Str As String, ByVal PTK As String) As String
'            PrivateKeyValue = PTK
'            MainString = Str
'            Main()
'            Return ReturnValue
'        End Function

'        Private Function CAZ(ByVal PRT As String, ByVal TH As Double) As String

'            CAZ = CStr(CInt(AscW(PRT.Substring(CInt(TH) - 1, 1))))

'        End Function

'        Private Function AZ(ByVal PRT As String, ByVal TH As Double) As String
'            bz = bz + 1
'            On Error Resume Next
'            AZ = CStr(AscW(PRT.Substring(CInt(TH - 1), 1)))
'        End Function

'        Private Function SS(ByVal Text As String, ByVal TH As ULong) As String

'            SS = Text.Substring(CInt(TH - 1), CInt(TH))

'        End Function

'        Private Function PrivateKey(ByVal PrivateKeyText As String) As String

'            'On Error Resume Next
'            PrivateKey = vbNullString
'            For i = 1 To 32
'                PrivateKey = PrivateKey & AZ(PrivateKeyText, 1)
'                PrivateKeyText = PrivateKeyText.Remove(0, 1)
'            Next

'        End Function

'        Private Function Encrypt(ByVal Text As String) As String
'            Dim RAM As Double
'            Dim RAM2 As Double
'            Encrypt = CStr(CInt(AZ(PrivateKey(PrivateKeyValue), 1)) * 3 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 2)) _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 3)) _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 4)) * 2 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 5)) * 3 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 6)) _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 7)) _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 8)) * 2 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 9)) * 0 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 10)) * 4 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 11)) _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 12)) _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 13)) _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 14)) _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 15)) * 2 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 16)) * -3 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 17)) * 0 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 18)) * 5 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 19)) _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 20)) _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 21)) * 0 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 22)) * -2 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 23)) _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 24)) _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 25)) _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 26)) * 2 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 27)) * 0 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 28)) * 0 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 29)) * 3 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 30)) * 2 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 31)) * 1 _
'            - CInt(AZ(PrivateKey(PrivateKeyValue), 32)) _
'            + 2000)
'            RAM = CDbl(Encrypt)
'            Encrypt = vbNullString
'            For i = 1 To Len(Text)
'                RAM2 = CDbl(0 - CInt(CAZ(Text, i)) - RAM + 35000)
'                Encrypt = Encrypt & ChrW(CInt(RAM2))
'            Next
'        End Function


'        Private Sub Main()
'            If Len(PrivateKeyValue) = 32 Then
'                Dim STR As String
'                STR = MainString
'                If STR <> "" Then
'                    ReturnValue = Encrypt(MainString)
'                Else
'                    ReturnValue = mssnullstr
'                End If
'            Else
'                ReturnValue = msswrkey
'            End If
'        End Sub



'    End Class
'    Class UAC
'        Public Const SE_DEBUG_NAME = "SeDebugPrivilege"
'        Public Const SE_SHUTDOWN_NAME = "SeShutdownPrivilege"
'        Public Const SE_PRIVILEGE_ENABLED = &H2
'        Public Const TOKEN_ADJUST_PRIVILEGES = &H20
'        Public Const TOKEN_QUERY = &H8
'        Public Const ANYSIZE_ARRAY = 1
'        Public Structure Luid
'            Public lowpart As Long
'            Public highpart As Long
'        End Structure
'        Public Structure LUID_AND_ATTRIBUTES
'            Public pLuid As Luid
'            Public Attributes As Long
'        End Structure
'        Public Structure TOKEN_PRIVILEGES
'            Public PrivilegeCount As Long
'            Public Privileges() As LUID_AND_ATTRIBUTES
'        End Structure
'        Public Structure FILETIME ' 8 Bytes
'            Public dwLowDateTime As Long
'            Public dwHighDateTime As Long
'        End Structure
'        Declare Function GetCurrentProcess Lib "Kernel32" () As Long
'        Declare Function CloseHandle Lib "kernel32" Alias "CloseHandle" (ByVal hObject As Long) As Long
'        Declare Function OpenProcessToken Lib "advapi32.dll" (ByVal ProcessHandle As Long, ByVal DesiredAccess As Long, TokenHandle As Long) As Long
'        Declare Function SetPrivilege Lib "kernel32" Alias "SetPrivilege" (ByVal hObject As Long) As Long
'        Public Function getPrivileges(hhToken As Long, ByVal sPrivilegeName As String) As Boolean
'            'Dim hProcessID As Long ' Handle to your sample
'            ' process you are going to
'            ' terminate.
'            Dim hProcess As Long ' Handle to your current process
'            ' (Term02.exe).
'            Dim hToken As Long ' Handle to your process token.
'            'Dim lPrivilege As Long ' Privilege to enable/disable
'            'Dim iPrivilegeflag As Boolean ' Flag whether to enable/disable
'            ' the privilege of concern.
'            Dim lResult As Long ' Result call of various APIs. getPrivileges = False
'            'hProcessID = ApplicationPID ' get our current process handle
'            hProcess = GetCurrentProcess
'            lResult = OpenProcessToken(hProcess, TOKEN_ADJUST_PRIVILEGES Or
'        TOKEN_QUERY, hToken)
'            If lResult = 0 Then
'                CloseHandle(hToken)
'                getPrivileges = False
'                Exit Function
'            End If ' lResult = SetPrivilege(hToken, SE_DEBUG_NAME, True)

'            'lResult = SetPrivilege(hToken, sPrivilegeName, True)
'            If (lResult = False) Then
'                CloseHandle(hToken)
'                getPrivileges = False
'                Exit Function
'            End If
'            getPrivileges = True
'            hhToken = hToken
'        End Function
'    End Class
'    Public Class ApplicationLocker
'        Private LText
'        Private Sub KillProcess(ByVal LText As String)
'            Dim boo As Boolean
'            Dim proc() As Process
'            Dim tmp As String
'            tmp = LText
'            If Process.GetProcessesByName(tmp).Length > 0 Then
'                proc = Process.GetProcessesByName(tmp)
'                For i = 0 To proc.Length - 1
'                    proc(i).Kill()
'                    boo = True
'                Next
'            End If
'            If Not boo Then
'                MsgBox("Process not found",, "")
'            End If
'        End Sub
'        Public Function UnLock(ByVal LText As String) As Integer
'            Try
'                Microsoft.Win32.Registry.LocalMachine.DeleteSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & LText)
'                Return 1
'            Catch ex As Exception
'                Return ex.HResult
'            End Try
'        End Function
'        Private Function Lock(ByVal LText As String) As Integer
'            Try
'                Dim rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE")
'                rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\Microsoft\")
'                rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\Microsoft\Windows NT")
'                rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\")
'                rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & LText)
'                rk.SetValue("Debugger", """" & "" & "MESSEGE.EXE"" This application is blocked!")
'                rk.Close()
'                Return 1
'            Catch ex As Exception
'                Return ex.HResult
'            End Try
'        End Function
'        Friend Function Main(ByVal Command As String, LText As String)
'            If Command = "Lock" Then
'                Lock(LText)
'                Return 1
'            ElseIf Command = "Unlock" Then
'                UnLock(LText)
'                Return 1
'            Else
'                Return 0
'            End If
'        End Function
'    End Class
'    'Public Class Form1
'    'End Class
'    'Public Module Main
'    '    Sub Main()
'    '        Dim Form1 As New Form1
'    '        'Form1.Label1.Text = Command()
'    '        'Form1.ShowDialog()
'    '        Dim Cmd(666) As String
'    '        Dim l As Short = 0
'    '        Dim p As Short = 0
'    '        For Each i In Command()
'    '            l += 1

'    '            If Not i = " " Then
'    '                Cmd(p) &= i
'    '            Else
'    '                p += 1
'    '            End If
'    '        Next
'    '        Dim MainCommand = Cmd(0)
'    '        Dim SubCommand(666) As String
'    '        Dim StringCommand(666) As String
'    '        l = 0
'    '        p = 0
'    '        Dim o As Short = 0
'    '        For Each i In Cmd
'    '            If Not l = 0 Then
'    '                'Form1.ShowDialog()
'    '                'Form1.Label1.Text = i
'    '                If i = "" Then
'    '                    Exit For
'    '                End If
'    '                If i.Substring(0, 1) = "-" Or i.Substring(0, 1) = "/" Then
'    '                    Dim t As Short
'    '                    For Each s In i
'    '                        t += 1
'    '                        If Not t = 1 Then
'    '                            SubCommand(p) &= s
'    '                        End If
'    '                    Next
'    '                    p += 1
'    '                ElseIf i.Substring(0, 1) = """" Or i.Substring(0, 1) = "'" Then
'    '                    Dim t As Short = 0
'    '                    For Each s In i
'    '                        t += 1
'    '                        If Not t = 1 Then
'    '                            If s = """" Or s = "'" Then
'    '                                Exit For
'    '                            End If
'    '                            StringCommand(p) &= s
'    '                        End If
'    '                    Next
'    '                    o += 1
'    '                End If
'    '            End If
'    '            l += 1
'    '        Next
'    '        'Form1.Label1.Text = MainCommand & vbCrLf
'    '        For Each i In SubCommand
'    '            'Form1.Label1.Text &= i & " "
'    '        Next
'    '        'Form1.ShowDialog()
'    '        Dim Result = WorkingArea.Library.DoCommand(MainCommand, SubCommand, StringCommand)
'    '        Console.WriteLine(Result)
'    '        If Result = Nothing Then Console.ReadKey()
'    '        End
'    '    End Sub
'    'End Module
'    'Friend Function DoCommand(ByVal MainCommand As String, Optional Subcommand() _
'    'As String = Nothing, Optional Stringcommand() As String = Nothing) As Object
'    '    Dim ReturnValue As String = Nothing
'    '    If MainCommand = "ECPT" Then
'    '        'Dim ECPT As ECPT = Nothing
'    '        Dim ECPT As New ECPT
'    '        ReturnValue = ECPT.Run(Stringcommand(0), Stringcommand(1))
'    '    ElseIf MainCommand = "Mouse" Then
'    '        Dim mouse As New Mouse
'    '        ReturnValue = mouse.Main
'    '    End If
'    '    Return ReturnValue
'    'End Function
'End Namespace

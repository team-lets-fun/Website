I Am Universal Platform结构


Namespace WorkingArea
    Public Module Windows
        Public Function GetWindowHwnd(ByVal WindowClassVsWindowText As String) As Long
    End Module
    Public Module Window
        Public Class ProcessProtect
            Public ReadOnly Property GetProtectionState As Boolean 
            Public Sub AntiKill()
            Public Sub AllowKill()
            Public Sub New()
                RtlAdjustPrivilege(20, 1, 0, b)
        End Class
        Public Class RtlAdjustPrivilege
            Public Enum Privilege
            Public Sub New(ByVal Privilege As Integer,ByVal Switch As Boolean, ByVal OnlyCurrentThread As Boolean, ByRef OldPrivilege As Boolean)
        End Class
        Public Class Window
            Public Shared Function SetTopMost(a As IntPtr) As Integer
            Public Shared Function SetNormal(a As IntPtr) As Integer
        End Class
        Public Class HotKey
            Public Shared Function UnregisterHotKeyL(Handle As IntPtr, id As Integer)
            Public Shared Function RegisterHotKeyL(Handle As IntPtr, RegisterId As Integer, Modifiers As Short, uVirtKey1 As Keys) As Boolean
            Public Shared Function GetCode() As String
            Public Function UnRegisterHotKeyL(RegisterId As Integer, Handle As IntPtr, Modifiers As Short, uVirtKey1 As Keys) As Boolean
        End Class
        Public Class SelfImprovePermission
            Public Sub New(ExecutablePath As String)
        End Class
        Public Class CreateProcessAsAdministrator
            Public Sub Create(startexe As String, Optional strPara As String = Nothing)
        End Class
        Public Class KeyEvent
            Public Shared Sub KeyDown(VisualKey As Long)
            Public Shared Sub KeyUp()
            Public Shared Sub KeyPress(VisualKey As Long)
            Public Structure VisualKeys
        End Class
    End Module
    Public Module Cryptography
        Public Function GetFileHash(ByVal path As String)
        Public Function GetSha256HashByString(ByVal word As String, ByVal Optional toUpper As Boolean = True)
    End Module
        Public Class Mouse
            Public Declare Sub mouse_event Lib "user32" (ByVal dwFlags As Long, ByVal dx As Long, ByVal dy As Long, ByVal dwData As Long, ByVal dwExtraInfo As Long)
            Public Enum MouseEventFlags As UInteger
            Public Sub Click(ByVal Mode As Short, Optional Amod As Short = Nothing)
            Public Shared Function Move(ByVal Optional px_x As Integer = 0, ByVal Optional px_y As Integer = 0)
        End Class
        Public Class MenuBar
            Public Shared Function DiableFormCloseButton(wnd As Form) As Integer
        End Class
    Module Paths
        Public Function UserProfile() As String
        Public Function Temp() As String
        Public Function AppData() As String
    End Module
    Module SystemPath
        Public Function WinDir() As String
        Public Function RandomNum() As Integer
    End Module
End Namespace
VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "ZMIN VB Demo"
   ClientHeight    =   4350
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5265
   LinkTopic       =   "Form1"
   ScaleHeight     =   4350
   ScaleWidth      =   5265
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command1 
      Caption         =   "Print"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   612
      Left            =   1920
      TabIndex        =   1
      Top             =   2280
      Width           =   1212
   End
   Begin VB.Label Label1 
      Caption         =   "Before run the demo, please read the manual!"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   10.5
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   648
      Left            =   360
      TabIndex        =   0
      Top             =   840
      Width           =   4620
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Private Declare Function ZM_Getinfo Lib "ZMWIN.dll" () As Integer
Private Declare Function ZM_SetPrintSpeed Lib "ZMWIN.dll" (ByVal speed As Integer) As Long
Private Declare Function ZM_SetDirection Lib "ZMWIN.dll" (ByVal direct As Byte) As Long ' String->Byte 130621
Private Declare Function ZM_SetDarkness Lib "ZMWIN.dll" (ByVal darkness As Integer) As Long
Private Declare Function ZM_ClearBuffer Lib "ZMWIN.dll" () As Long
Private Declare Function ZM_DrawBarcode Lib "ZMWIN.dll" (ByVal px As Long, ByVal py As Long, ByVal pdirec As Long, ByVal typee As String, ByVal NarrowWidth As Long, ByVal pHorizontal As Long, ByVal pVertical As Long, ByVal ptext As Byte, ByVal pstr As String) As Long
Private Declare Function OpenPort Lib "ZMWIN.dll" (ByVal OP As String) As Long
Private Declare Sub ClosePort Lib "ZMWIN.dll" ()
Private Declare Function ZM_DrawTextTrueTypeW Lib "ZMWIN.dll" (ByVal x As Long, ByVal y As Long, ByVal FHeight As Long, ByVal FWidth As Long, ByVal FType As String, ByVal Fspin As Long, ByVal FWeight As Long, ByVal FItalic As Long, ByVal FUnline As Long, ByVal FStrikeOut As Long, ByVal id_name As String, ByVal data As String) As Long
Private Declare Function ZM_PcxGraphicsDel Lib "ZMWIN.dll" (ByVal pid As String) As Long
Private Declare Function ZM_PcxGraphicsDownload Lib "ZMWIN.dll" (ByVal pcxname As String, ByVal pcxpath As String) As Long
Private Declare Function ZM_PrintLabel Lib "ZMWIN.dll" (ByVal number As Integer, ByVal cpnumber As Integer) As Long
Private Declare Function ZM_PrintPCX Lib "ZMWIN.dll" (ByVal px As Integer, ByVal py As Integer, ByVal filename As String) As Long
Private Declare Function ZM_DrawPcxGraphics Lib "ZMWIN.dll" (ByVal x As Long, ByVal y As Long, ByVal gname As String) As Long
Private Declare Function ZM_DrawLineOr Lib "ZMWIN.dll" (ByVal px As Integer, ByVal py As Integer, ByVal plength As Integer, ByVal pH As Integer) As Long
Private Declare Function GetErrState Lib "ZMWIN.dll" () As Integer
Private Declare Function ZM_SetLabelHeight Lib "ZMWIN.dll" (ByVal lheight As Integer, ByVal gapH As Integer) As Integer
Private Declare Function ZM_SetLabelWidth Lib "ZMWIN.dll" (ByVal lwidth As Integer) As Integer
Private Declare Function SetPCComPort Lib "ZMWIN.dll" (ByVal BaudRate As Integer, ByVal HandShake As Boolean) As Integer
Private Declare Function ZM_DrawText Lib "ZMWIN.dll" (ByVal px As Integer, ByVal py As Integer, ByVal pdirec As Integer, ByVal pFont As Integer, ByVal pHorizontal As Integer, ByVal pVertical As Integer, ByVal ptext As Byte, ByVal pstr As String) As Long
Private Declare Function ZM_DrawBar2D_Pdf417 Lib "ZMWIN.dll" (ByVal x As Integer, ByVal y As Integer, ByVal w As Integer, ByVal v As Integer, ByVal s As Integer, ByVal c As Integer, ByVal px As Integer, ByVal py As Integer, ByVal r As Integer, ByVal l As Integer, ByVal t As Integer, ByVal o As Integer, ByVal LRTSTR As String) As Long
Private Declare Function ZM_DrawBar2D_QR Lib "ZMWIN.dll" (ByVal x As Integer, ByVal y As Integer, ByVal w As Integer, ByVal v As Integer, ByVal o As Integer, ByVal r As Integer, ByVal m As Integer, ByVal g As Integer, ByVal s As Integer, ByVal LRTSTR As String) As Long
Private Declare Function ZM_DrawRectangle Lib "ZMWIN.dll" (ByVal px As Long, ByVal py As Long, ByVal thickness As Integer, ByVal pEx As Long, ByVal pEy As Long) As Long




Private Sub Command1_Click()

OpenPort "ZMIN X1"              '打开打印机

ZM_ClearBuffer                   '清空打印机缓存

ZM_SetDirection 84               '设置打印方向. 84：反向，66：正向

ZM_SetDarkness 10                '设置打印机打印温度;
ZM_SetPrintSpeed 4               '设置打印机打印速度;
ZM_SetLabelHeight 600, 24        '设置标签的高度和间隙大小；
ZM_SetLabelWidth 800             '设置标签的宽度


'画矩形
ZM_DrawRectangle 50, 10, 3, 460, 340
'画横线
ZM_DrawLineOr 60, 108, 402, 3


'打印PCX图片,更多方式请查看动态链接库文档
ZM_PcxGraphicsDel "PCX"
ZM_PcxGraphicsDownload "PCX", "pic.pcx"
ZM_DrawPcxGraphics 80, 30, "PCX"

'打印一个128 Auto条码;
ZM_DrawBarcode 80, 208, 0, "1", 2, 2, 50, 66, "123456789"

'打印PDF417码
ZM_DrawBar2D_Pdf417 80, 300, 400, 300, 0, 0, 3, 7, 10, 2, 0, 0, "123456789"

'打印QR码
ZM_DrawBar2D_QR 360, 30, 180, 180, 0, 3, 2, 0, 0, "ZMIN Electronics Co., Ltd."


'打印内置字体点阵文字
ZM_DrawText 80, 168, 0, 3, 1, 1, 78, "Internal Font"

'打印WINDWOS系统TrueType Font文字;
ZM_DrawTextTrueTypeW 80, 120, 40, 0, "Arial", 1, 400, 0, 0, 0, "A1", "TrueType Font"


'打印WINDWOS系统TrueType Font文字（旋转90度);
ZM_DrawTextTrueTypeW 420, 102, 22, 0, "Arial", 2, 400, 0, 0, 0, "A2", "www.zmin.com.cn"

'开始打印
ZM_PrintLabel 1, 1

ClosePort
End Sub

Option Strict Off
Option Explicit On
Friend Class Form1
	Inherits System.Windows.Forms.Form
	Private Declare Function ZM_Getinfo Lib "ZMWIN.dll" () As Short
	Private Declare Function ZM_SetPrintSpeed Lib "ZMWIN.dll" (ByVal speed As Short) As Integer
	Private Declare Function ZM_SetDirection Lib "ZMWIN.dll" (ByVal direct As Byte) As Integer ' String->Byte 130621
	Private Declare Function ZM_SetDarkness Lib "ZMWIN.dll" (ByVal darkness As Short) As Integer
	Private Declare Function ZM_ClearBuffer Lib "ZMWIN.dll" () As Integer
	Private Declare Function ZM_DrawBarcode Lib "ZMWIN.dll" (ByVal px As Integer, ByVal py As Integer, ByVal pdirec As Integer, ByVal typee As String, ByVal NarrowWidth As Integer, ByVal pHorizontal As Integer, ByVal pVertical As Integer, ByVal ptext As Byte, ByVal pstr As String) As Integer
	Private Declare Function OpenPort Lib "ZMWIN.dll" (ByVal OP As String) As Integer
	Private Declare Sub ClosePort Lib "ZMWIN.dll" ()
	Private Declare Function ZM_DrawTextTrueTypeW Lib "ZMWIN.dll" (ByVal x As Integer, ByVal y As Integer, ByVal FHeight As Integer, ByVal FWidth As Integer, ByVal FType As String, ByVal Fspin As Integer, ByVal FWeight As Integer, ByVal FItalic As Integer, ByVal FUnline As Integer, ByVal FStrikeOut As Integer, ByVal id_name As String, ByVal data As String) As Integer
	Private Declare Function ZM_PcxGraphicsDel Lib "ZMWIN.dll" (ByVal pid As String) As Integer
	Private Declare Function ZM_PcxGraphicsDownload Lib "ZMWIN.dll" (ByVal pcxname As String, ByVal pcxpath As String) As Integer
	Private Declare Function ZM_PrintLabel Lib "ZMWIN.dll" (ByVal number As Short, ByVal cpnumber As Short) As Integer
	Private Declare Function ZM_PrintPCX Lib "ZMWIN.dll" (ByVal px As Short, ByVal py As Short, ByVal filename As String) As Integer
	Private Declare Function ZM_DrawPcxGraphics Lib "ZMWIN.dll" (ByVal x As Integer, ByVal y As Integer, ByVal gname As String) As Integer
	Private Declare Function ZM_DrawLineOr Lib "ZMWIN.dll" (ByVal px As Short, ByVal py As Short, ByVal plength As Short, ByVal pH As Short) As Integer
	Private Declare Function GetErrState Lib "ZMWIN.dll" () As Short
	Private Declare Function ZM_SetLabelHeight Lib "ZMWIN.dll" (ByVal lheight As Short, ByVal gapH As Short) As Short
	Private Declare Function ZM_SetLabelWidth Lib "ZMWIN.dll" (ByVal lwidth As Short) As Short
	Private Declare Function SetPCComPort Lib "ZMWIN.dll" (ByVal BaudRate As Short, ByVal HandShake As Boolean) As Short
	Private Declare Function ZM_DrawText Lib "ZMWIN.dll" (ByVal px As Short, ByVal py As Short, ByVal pdirec As Short, ByVal pFont As Short, ByVal pHorizontal As Short, ByVal pVertical As Short, ByVal ptext As Byte, ByVal pstr As String) As Integer
	Private Declare Function ZM_DrawBar2D_Pdf417 Lib "ZMWIN.dll" (ByVal x As Short, ByVal y As Short, ByVal w As Short, ByVal v As Short, ByVal s As Short, ByVal c As Short, ByVal px As Short, ByVal py As Short, ByVal r As Short, ByVal l As Short, ByVal t As Short, ByVal o As Short, ByVal LRTSTR As String) As Integer
	Private Declare Function ZM_DrawBar2D_QR Lib "ZMWIN.dll" (ByVal x As Short, ByVal y As Short, ByVal w As Short, ByVal v As Short, ByVal o As Short, ByVal r As Short, ByVal m As Short, ByVal g As Short, ByVal s As Short, ByVal LRTSTR As String) As Integer
	Private Declare Function ZM_DrawRectangle Lib "ZMWIN.dll" (ByVal px As Integer, ByVal py As Integer, ByVal thickness As Short, ByVal pEx As Integer, ByVal pEy As Integer) As Integer
	
	
	
	
	Private Sub Command1_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command1.Click
		
        OpenPort("ZMIN X1") '打开打印机
		
		ZM_ClearBuffer() '清空打印机缓存
		
        ZM_SetDirection(84) '设置打印方向. 84：反向，66：正向
		
        ZM_SetDarkness(10) '设置打印机打印温度;
        ZM_SetPrintSpeed(4) '设置打印机打印速度;
        ZM_SetLabelHeight(600, 24) '设置标签的高度和间隙大小；
        ZM_SetLabelWidth(800) '设置标签的宽度
		
		
		'画矩形
		ZM_DrawRectangle(50, 10, 3, 460, 340)
		'画横线
		ZM_DrawLineOr(60, 108, 402, 3)
		
		
		'打印PCX图片,更多方式请查看动态链接库文档
        ZM_PcxGraphicsDel("PCX")
        ZM_PcxGraphicsDownload("PCX", "pic.pcx")
        ZM_DrawPcxGraphics(80, 30, "PCX")
		
		'打印一个128 Auto条码;
		ZM_DrawBarcode(80, 208, 0, "1", 2, 2, 50, 66, "123456789")
		
		'打印PDF417码
		ZM_DrawBar2D_Pdf417(80, 300, 400, 300, 0, 0, 3, 7, 10, 2, 0, 0, "123456789")
		
		'打印QR码
		ZM_DrawBar2D_QR(360, 30, 180, 180, 0, 3, 2, 0, 0, "ZMIN Electronics Co., Ltd.")
		
		
		'打印内置字体点阵文字
		ZM_DrawText(80, 168, 0, 3, 1, 1, 78, "Internal Font")
		
		'打印WINDWOS系统TrueType Font文字;
		ZM_DrawTextTrueTypeW(80, 120, 40, 0, "Arial", 1, 400, 0, 0, 0, "A1", "TrueType Font")
		
		
		'打印WINDWOS系统TrueType Font文字（旋转90度);
        ZM_DrawTextTrueTypeW(420, 102, 22, 0, "Arial", 2, 400, 0, 0, 0, "A2", "www.zmin.com.cn")
		
		'开始打印
		ZM_PrintLabel(1, 1)
		
		ClosePort()
	End Sub
End Class
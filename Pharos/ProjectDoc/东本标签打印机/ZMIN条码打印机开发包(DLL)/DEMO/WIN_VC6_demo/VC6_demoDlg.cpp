// VC6_demoDlg.cpp : implementation file
//

#include "stdafx.h"
#include "VC6_demo.h"
#include "VC6_demoDlg.h"
#include "Printer.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	//{{AFX_DATA(CAboutDlg)
	enum { IDD = IDD_ABOUTBOX };
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAboutDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	//{{AFX_MSG(CAboutDlg)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
	//{{AFX_DATA_INIT(CAboutDlg)
	//}}AFX_DATA_INIT
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutDlg)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	//{{AFX_MSG_MAP(CAboutDlg)
		// No message handlers
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CVC6_demoDlg dialog

CVC6_demoDlg::CVC6_demoDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CVC6_demoDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CVC6_demoDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CVC6_demoDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CVC6_demoDlg)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CVC6_demoDlg, CDialog)
	//{{AFX_MSG_MAP(CVC6_demoDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDOK, OnPrint)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CVC6_demoDlg message handlers

BOOL CVC6_demoDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	
	// TODO: Add extra initialization here
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CVC6_demoDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CVC6_demoDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CVC6_demoDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

void CVC6_demoDlg::OnPrint() 
{
	// TODO: Add your control notification handler code here
	HINSTANCE gt1 = NULL;
	
	gt1=LoadLibrary("ZMWIN.dll");
	OpenPort = (fOpenPort)GetProcAddress(gt1,"OpenPort");
    ClosePort = (fClosePort)GetProcAddress(gt1,"ClosePort");
	ZM_DrawBarcode = (fZM_DrawBarcode)GetProcAddress(gt1,"ZM_DrawBarcode");
    ZM_SetLabelHeight = (fZM_SetLabelHeight)GetProcAddress(gt1,"ZM_SetLabelHeight");
    ZM_PrintLabel = (fZM_PrintLabel)GetProcAddress(gt1,"ZM_PrintLabel");
    ZM_SetDarkness = (fZM_SetDarkness)GetProcAddress(gt1,"ZM_SetDarkness");
    ZM_SetPrintSpeed = (fZM_SetPrintSpeed)GetProcAddress(gt1,"ZM_SetPrintSpeed");
	ZM_SetLabelWidth = (fZM_SetLabelWidth)GetProcAddress(gt1,"ZM_SetLabelWidth");
    ZM_ClearBuffer = (fZM_ClearBuffer)GetProcAddress(gt1,"ZM_ClearBuffer");
	ZM_DrawTextTrueTypeW = (fZM_DrawTextTrueTypeW)GetProcAddress(gt1,"ZM_DrawTextTrueTypeW");
	ZM_DrawLineOr = (fZM_DrawLineOr)GetProcAddress(gt1,"ZM_DrawLineOr");
	ZM_DrawText = (fZM_DrawText)GetProcAddress(gt1,"ZM_DrawText");
	ZM_DrawBar2D_QR = (fZM_DrawBar2D_QR)GetProcAddress(gt1,"ZM_DrawBar2D_QR");
	ZM_DrawBar2D_Pdf417 = (fZM_DrawBar2D_Pdf417)GetProcAddress(gt1,"ZM_DrawBar2D_Pdf417");
	ZM_PcxGraphicsDel  = (fZM_PcxGraphicsDel)GetProcAddress(gt1,"ZM_PcxGraphicsDel");
	ZM_PcxGraphicsDownload = (fZM_PcxGraphicsDownload)GetProcAddress(gt1,"ZM_PcxGraphicsDownload");
	ZM_DrawPcxGraphics = (fZM_DrawPcxGraphics)GetProcAddress(gt1,"ZM_DrawPcxGraphics");
	ZM_DrawRectangle  = (fZM_DrawRectangle)GetProcAddress(gt1,"ZM_DrawRectangle");
	
	int i1;
	int errorcode = 0;

    //打开打印机
	OpenPort("ZMIN X1i");
	for (i1=0;i1<1;i1++)
	 {
		errorcode=ZM_ClearBuffer();  	             //清空打印机缓存
		if (errorcode!=0)  {break;}
		errorcode = ZM_SetLabelHeight (600,24);      // 设置标签的高度和行间隙大小；
		if(errorcode != 0) {break;}
		errorcode = ZM_SetLabelWidth (800);          // 设置标签的宽度；
		if(errorcode != 0) {break;}
        errorcode = ZM_SetDarkness( 10 );            // 设置打印机打印温度;
		if(errorcode != 0) {break;}
        errorcode = ZM_SetPrintSpeed( 5 );           // 设置打印机打印速度;

    	// 画矩形
        errorcode = ZM_DrawRectangle(50,10,3,460,340);
		if(errorcode != 0) {break;}

        // 画表格分割线
        errorcode = ZM_DrawLineOr(60,108,402,3);
		if(errorcode != 0) {break;}

        // 打印PCX图片 方式一
        errorcode = ZM_PcxGraphicsDel("PCX");
		if(errorcode != 0) {break;}
        errorcode = ZM_PcxGraphicsDownload("PCX","pic.pcx");
		if(errorcode != 0) {break;}
        errorcode = ZM_DrawPcxGraphics(80,30,"PCX");
		if(errorcode != 0) {break;}

        // 打印一个128 Auto条码;
        errorcode = ZM_DrawBarcode(80, 208, 0, "1", 2, 2, 50, 'B', "123456789");
		if(errorcode != 0) {break;}

        // 打印PDF417码
        errorcode = ZM_DrawBar2D_Pdf417(80,300,400,300,0,0,3,7,10,2,0,0,"123456789");//PDF417码
		if(errorcode != 0) {break;}

        // 打印QR码
        errorcode = ZM_DrawBar2D_QR(360,30,180,180,0,3,2,0,0, "ZMIN Electronics Co., Ltd.");
		if(errorcode != 0) {break;}

		// 打印内置字体点阵文字
        errorcode = ZM_DrawText(80, 168, 0, 3, 1, 1, 'N', "Internal Font");
		if(errorcode != 0) {break;}

        // 打印WINDWOS系统TrueType Font文字;
        errorcode = ZM_DrawTextTrueTypeW (80, 120, 40, 0, "Arial", 1, 400, 0, 0, 0, "A1", "TrueType Font");
		if(errorcode != 0) {break;}

        
        // 打印WINDWOS系统TrueType Font文字（旋转90度);
        errorcode = ZM_DrawTextTrueTypeW(420, 102, 22, 0, "Arial", 2, 400, 0, 0, 0, "A2", "www.ZMIN.com.cn");
		if(errorcode != 0) {break;}

        // 开始打印
		errorcode=ZM_PrintLabel(1,1);
		if (errorcode!=0)  {break;}	
	}
    
	//关闭打印机
    ClosePort();
	
    
	FreeLibrary(gt1);
 
}

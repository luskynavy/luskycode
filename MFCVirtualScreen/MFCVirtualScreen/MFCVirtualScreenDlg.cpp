
// MFCVirtualScreenDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "MFCVirtualScreen.h"
#include "MFCVirtualScreenDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CMFCVirtualScreenDlg dialog



CMFCVirtualScreenDlg::CMFCVirtualScreenDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_MFCVIRTUALSCREEN_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CMFCVirtualScreenDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT1, Infos);
}

BEGIN_MESSAGE_MAP(CMFCVirtualScreenDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON1, &CMFCVirtualScreenDlg::OnBnClickedButton1)
END_MESSAGE_MAP()


// CMFCVirtualScreenDlg message handlers

BOOL CMFCVirtualScreenDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CMFCVirtualScreenDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

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
		CDialogEx::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CMFCVirtualScreenDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

//Get real window rectangle without border but with window bar
RECT CMFCVirtualScreenDlg::getInnerWindowScreenRectangle()
{
	RECT client;
	RECT output;
	// client coordinates are (0, 0) in the top left corner
	// of the window inside the border.
	GetClientRect(&client);
	//GetWindowRect(&client);
	POINT point;
	point.x = 0;
	point.y = 0;
	int cyCaption = GetSystemMetrics(SM_CYCAPTION);
	// screen coordinates are virtual screen coordinates with (0, 0)
	// in the top left corner of the primary display.
	ClientToScreen(&point);
	output.left = point.x;
	//remove window bar size
	output.top = point.y - cyCaption;
	output.right = point.x + client.right;
	output.bottom = point.y + client.bottom;
	return output;
}


void CMFCVirtualScreenDlg::OnBnClickedButton1()
{
	//Virtual screen size
	int sm_cxvirtualscreen = GetSystemMetrics(SM_CXVIRTUALSCREEN);
	int sm_cyvirtualscreen = GetSystemMetrics(SM_CYVIRTUALSCREEN);

	//top left coordinate, not always (0, 0)
	//(0, 0) is at top left of main monitor
	int sm_xvirtualscreen = GetSystemMetrics(SM_XVIRTUALSCREEN);
	int sm_yvirtualscreen = GetSystemMetrics(SM_YVIRTUALSCREEN);

	//differents values
	int cxSizeFrame = GetSystemMetrics(SM_CXSIZEFRAME);
	int cySizeFrame = GetSystemMetrics(SM_CXSIZEFRAME);
	int cyCaption = GetSystemMetrics(SM_CYCAPTION); //window title bar size ?
	int cySize = GetSystemMetrics(SM_CYSIZE);
	int cyBorder = GetSystemMetrics(SM_CYBORDER);

	//Get window position
	RECT rect;
	GetWindowRect(&rect);
	//ClientToScreen(&rect);

	//Get real window position without window border
	//that might be out of screen in fullscreen
	rect = getInnerWindowScreenRectangle();

	//test values
	/*rect.left = 100;
	rect.top = 3000;
	rect.right = 200;
	rect.bottom = 3200;*/

	CString rectStatus;

	//HMONITOR monitor = MonitorFromRect(&rect, MONITOR_DEFAULTTONEAREST);

	//Get monitor with largest area for rect or null
	HMONITOR monitor = MonitorFromRect(&rect, MONITOR_DEFAULTTONULL);
	if (monitor != NULL)
	{
		MONITORINFOEX info;
		info.cbSize = sizeof(info);

		GetMonitorInfo(monitor, &info);
		RECT* testedRect;
		//full monitor size
		//testedRect = &info.rcMonitor;
		//work area of monitor (screen without taskbar)
		testedRect = &info.rcWork;

		//Check if rect is inside work area of monitor
		if (rect.left >= testedRect->left &&
			rect.top >= testedRect->top &&
			rect.right <= testedRect->right &&
			rect.bottom <= testedRect->bottom)
		{
			rectStatus = L"fully inside one screen";
		}
		else
		{
			rectStatus = L"not fully inside one screen";
		}
	}
	else
	{
		rectStatus = L"not on any screen";
	}

	CString str;
	str.Format(L"SM_CXVIRTUALSCREEN %d\r\nSM_CYVIRTUALSCREEN %d\r\n"
		"sm_xvirtualscreen %d\r\nsm_yvirtualscreen %d\r\n"
		"left %d\r\ntop %d\r\n"
		"right %d\r\nbottom %d\r\n"
		"%s",
		sm_cxvirtualscreen, sm_cyvirtualscreen,
		sm_xvirtualscreen, sm_yvirtualscreen,
		rect.left, rect.top,
		rect.right, rect.bottom,
		rectStatus);
	Infos.SetWindowText(str);
}

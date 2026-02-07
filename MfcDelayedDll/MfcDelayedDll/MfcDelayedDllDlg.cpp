
// MfcDelayedDllDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "MfcDelayedDllDlg.h"
#include "afxdialogex.h"

// How to add the ability to load a delayed dll and unload a delayed dll
// Add DllDelayed.lib to Linker > Input > Additional Dependencies
// Add this include delayimp.h and Add DllDelayed.dll to Linker > Input > Delayed Load Dlls for unload
// Then unload is done by calling __FUnloadDelayLoadedDLL2("DllDelayed.dll")
#include <delayimp.h>
#include "DllDelayed.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CAboutDlg dialog used for App About

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ABOUTBOX };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

// Implementation
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(IDD_ABOUTBOX)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CMfcDelayedDllDlg dialog



CMfcDelayedDllDlg::CMfcDelayedDllDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_MFCDELAYEDDLL_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	mCppDll = new CppDll();
}

void CMfcDelayedDllDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CMfcDelayedDllDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_UNLOAD, &CMfcDelayedDllDlg::OnBnClickedButtonUnload)
	ON_BN_CLICKED(IDC_BUTTON_GET, &CMfcDelayedDllDlg::OnBnClickedButtonGet)
	ON_BN_CLICKED(IDC_BUTTON_INCREMENT_COUNTER, &CMfcDelayedDllDlg::OnBnClickedButtonIncrementCounter)
END_MESSAGE_MAP()


// CMfcDelayedDllDlg message handlers

BOOL CMfcDelayedDllDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != nullptr)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
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

void CMfcDelayedDllDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CMfcDelayedDllDlg::OnPaint()
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
HCURSOR CMfcDelayedDllDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CMfcDelayedDllDlg::OnBnClickedButtonGet()
{
	int val = get_value();

	wchar_t wstr[256];
	swprintf_s(wstr, L"From C function: %d", val);

	CWnd* label = GetDlgItem(IDC_STATIC);
	label->SetWindowText(wstr);

	if (!mCppDll)
	{
		mCppDll = new CppDll();
	}

	val = mCppDll->Return43();
	swprintf_s(wstr, L"From C++ class function: %d", val);

	label = GetDlgItem(IDC_STATIC1);
	label->SetWindowText(wstr);

	val = mCppDll->Counter();
	swprintf_s(wstr, L"From C++ class Counter: %d", val);

	label = GetDlgItem(IDC_STATIC2);
	label->SetWindowText(wstr);
}


void CMfcDelayedDllDlg::OnBnClickedButtonUnload()
{
	BOOL TestReturn;
	if (mCppDll)
	{
		delete mCppDll;
		mCppDll = nullptr;
	}

	TestReturn = __FUnloadDelayLoadedDLL2("DllDelayed.dll");

	CWnd* label = GetDlgItem(IDC_STATIC);
	CWnd* label1 = GetDlgItem(IDC_STATIC1);
	CWnd* label2 = GetDlgItem(IDC_STATIC2);
	if (TestReturn)
	{
		label->SetWindowText(L"DLL was unloaded");
		label1->SetWindowText(L"DLL was unloaded");
		label2->SetWindowText(L"DLL was unloaded");
	}
	else
	{
		label->SetWindowText(L"DLL was not unloaded");
		label1->SetWindowText(L"DLL was not unloaded");
		label2->SetWindowText(L"DLL was not unloaded");
	}
}

void CMfcDelayedDllDlg::OnBnClickedButtonIncrementCounter()
{
	if (!mCppDll)
	{
		mCppDll = new CppDll();
	}

	int val = mCppDll->IncrementCounter();

	wchar_t wstr[256];
	swprintf_s(wstr, L"From C++ class Counter: %d", val);

	CWnd* label = GetDlgItem(IDC_STATIC2);
	label->SetWindowText(wstr);
}

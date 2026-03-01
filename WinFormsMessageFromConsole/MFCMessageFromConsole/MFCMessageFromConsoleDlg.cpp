
// MFCMessageFromConsoleDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "MFCMessageFromConsole.h"
#include "MFCMessageFromConsoleDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif
#include <string>


// CMFCMessageFromConsoleDlg dialog



CMFCMessageFromConsoleDlg::CMFCMessageFromConsoleDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_MFCMESSAGEFROMCONSOLE_DIALOG, pParent)

{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	mListenSocket = INVALID_SOCKET;
	memset(buffer, 0, sizeof(buffer));
}

void CMFCMessageFromConsoleDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CMFCMessageFromConsoleDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_TIMER()
	ON_WM_CLOSE()
END_MESSAGE_MAP()


// CMFCMessageFromConsoleDlg message handlers

BOOL CMFCMessageFromConsoleDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here

	SetTimer(1, 1000, NULL);

	mListenSocket = Listen(33000, 10);

	CWnd* edit = GetDlgItem(IDC_EDIT1);
	edit->SetWindowText(L"Waiting connection");

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CMFCMessageFromConsoleDlg::OnPaint()
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
HCURSOR CMFCMessageFromConsoleDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

// Listen for incoming connections on the specified port and return the listening socket
SOCKET CMFCMessageFromConsoleDlg::Listen(unsigned short port, int queue_size)
{
	// Initialize Winsock
	WSADATA WSAData;
	int status = WSAStartup(MAKEWORD(2, 0), &WSAData);

	if (status != 0)
		return INVALID_SOCKET;

	int	namelen;
	struct sockaddr_in sin = { AF_INET };

	mListenSocket = INVALID_SOCKET;
	// Create a TCP socket
#ifdef WIN32
	if ((mListenSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP)) == INVALID_SOCKET)
#else
	if ((_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP)) < 0)
#endif
	{
		mListenSocket = INVALID_SOCKET;
		return INVALID_SOCKET;
	}

	// Set the Non Blocking Flag
	unsigned long bNonBlocking = 1;
	ioctlsocket(mListenSocket, FIONBIO, &bNonBlocking);

	// Bind to the specified port
	sin.sin_port = htons(port);
	if (bind(mListenSocket, (struct sockaddr*)&sin, sizeof(sin)) < 0)
	{
		closesocket(mListenSocket);
		mListenSocket = INVALID_SOCKET;
		return INVALID_SOCKET;
	}

	namelen = sizeof(sin);
	if (getsockname(mListenSocket, (struct sockaddr*)&sin, &namelen) < 0)
	{
		closesocket(mListenSocket);
		mListenSocket = INVALID_SOCKET;
		return INVALID_SOCKET;
	}

	// Start listening for incoming connections
	if (listen(mListenSocket, queue_size) < 0)
	{
		closesocket(mListenSocket);
		mListenSocket = INVALID_SOCKET;
		return INVALID_SOCKET;
	}

	return (mListenSocket);
}

// Check if the listening socket is readable (i.e., if there are incoming connections)
SocketResult CMFCMessageFromConsoleDlg::IsReadable() const
{
	int				err;
	fd_set			read_set;
	struct timeval	tv;

	tv.tv_sec = 0;
	tv.tv_usec = 0;
	FD_ZERO(&read_set);

	FD_SET(mListenSocket, &read_set);

	err = select((int)mListenSocket + 1, (struct fd_set*)&read_set, 0, (struct fd_set*)0, &tv);

#if defined(WIN32)
	if (err == SOCKET_ERROR)
		return SocketResult::FAILED;
#else
	if (err < 0) return
		SocketResult::FAILED;
#endif

	if (FD_ISSET(mListenSocket, &read_set))
		return SocketResult::READABLE;
	return SocketResult::NO_EVENT;
}

// Handle the WM_TIMER message to update the label and check for incoming connections
void CMFCMessageFromConsoleDlg::OnTimer(UINT_PTR nIDEvent)
{
	CDialogEx::OnTimer(nIDEvent);

	// Update the label with the current time
	std::wstring ws;
	time_t now = time(0);
	struct tm tstruct;
	localtime_s(&tstruct, &now);
	ws = L"MFC Timer: " + std::to_wstring(tstruct.tm_hour) + L":" + std::to_wstring(tstruct.tm_min) + L":" + std::to_wstring(tstruct.tm_sec);

	CWnd* label = GetDlgItem(IDC_STATIC);
	label->SetWindowText(ws.c_str());

	// Check if there are incoming connections and handle them
	if (IsReadable() == SocketResult::READABLE)
	{
		SOCKET clientSocket = accept(mListenSocket, NULL, NULL);
		int sizeRecv = recv(clientSocket, buffer, sizeof(buffer) - 1, 0);

		int lastError = 0;

		if (sizeRecv == SOCKET_ERROR)
			lastError = WSAGetLastError();
		else if (sizeRecv > 0)
		{
			buffer[sizeRecv] = '\0';

			// Update the label with the received message and the current time
			std::wstring bufferws(std::begin(buffer), std::end(buffer));

			CWnd* edit = GetDlgItem(IDC_EDIT1);
			ws = std::to_wstring(tstruct.tm_hour) + L":" + std::to_wstring(tstruct.tm_min) + L":" + std::to_wstring(tstruct.tm_sec) + L" Received: " + bufferws;
			edit->SetWindowText(ws.c_str());
		}
	}

	// Eat spurious WM_TIMER messages
	MSG msg;
	while (::PeekMessage(&msg, m_hWnd, WM_TIMER, WM_TIMER, PM_REMOVE));
}

void CMFCMessageFromConsoleDlg::OnClose()
{
	// Cleanup
	closesocket(mListenSocket);
	WSACleanup();

	CDialogEx::OnClose();
}

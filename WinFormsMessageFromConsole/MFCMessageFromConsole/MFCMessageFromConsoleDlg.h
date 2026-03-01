
// MFCMessageFromConsoleDlg.h : header file
//

#pragma once

enum SocketResult
{
	FAILED = -1,
	READABLE = 0,
	NO_EVENT = 1
};


// CMFCMessageFromConsoleDlg dialog
class CMFCMessageFromConsoleDlg : public CDialogEx
{
// Construction
public:
	CMFCMessageFromConsoleDlg(CWnd* pParent = nullptr);	// standard constructor

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_MFCMESSAGEFROMCONSOLE_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	SOCKET Listen(unsigned short port, int queue_size);
	SocketResult IsReadable() const;

	SOCKET mListenSocket;
	char buffer[8192];


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnClose();
};

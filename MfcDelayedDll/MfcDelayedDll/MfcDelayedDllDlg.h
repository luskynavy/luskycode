
// MfcDelayedDllDlg.h : header file
//

#pragma once

#include "MfcDelayedDll.h"
#include "DllDelayed.h"


// CMfcDelayedDllDlg dialog
class CMfcDelayedDllDlg : public CDialogEx
{
// Construction
public:
	CMfcDelayedDllDlg(CWnd* pParent = nullptr);	// standard constructor

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_MFCDELAYEDDLL_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	CppDll* mCppDll = nullptr;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButton1();
	afx_msg void OnBnClickedButton2();
	afx_msg void OnBnClickedButtonUnload();
	afx_msg void OnBnClickedButtonGet();
	afx_msg void OnBnClickedButtonIncrementCounter();
};

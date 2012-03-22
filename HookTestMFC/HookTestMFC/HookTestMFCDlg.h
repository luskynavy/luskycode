
// HookTestMFCDlg.h : header file
//

#pragma once
#include "explorer1.h"
#include "hook.h"


// CHookTestMFCDlg dialog
class CHookTestMFCDlg : public CDialog
{
// Construction
public:
	CHookTestMFCDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_HOOKTESTMFC_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	CExplorer1 CWebBrowser1;
	Hook hook;
};

; CLW file contains information for the MFC ClassWizard

[General Info]
Version=1
LastClass=CVC6_demoDlg
LastTemplate=CDialog
NewFileInclude1=#include "stdafx.h"
NewFileInclude2=#include "VC6_demo.h"

ClassCount=4
Class1=CVC6_demoApp
Class2=CVC6_demoDlg
Class3=CAboutDlg

ResourceCount=3
Resource1=IDD_ABOUTBOX
Resource2=IDR_MAINFRAME
Resource3=IDD_VC6_DEMO_DIALOG

[CLS:CVC6_demoApp]
Type=0
HeaderFile=VC6_demo.h
ImplementationFile=VC6_demo.cpp
Filter=N

[CLS:CVC6_demoDlg]
Type=0
HeaderFile=VC6_demoDlg.h
ImplementationFile=VC6_demoDlg.cpp
Filter=D
BaseClass=CDialog
VirtualFilter=dWC

[CLS:CAboutDlg]
Type=0
HeaderFile=VC6_demoDlg.h
ImplementationFile=VC6_demoDlg.cpp
Filter=D

[DLG:IDD_ABOUTBOX]
Type=1
Class=CAboutDlg
ControlCount=4
Control1=IDC_STATIC,static,1342177283
Control2=IDC_STATIC,static,1342308480
Control3=IDC_STATIC,static,1342308352
Control4=IDOK,button,1342373889

[DLG:IDD_VC6_DEMO_DIALOG]
Type=1
Class=CVC6_demoDlg
ControlCount=2
Control1=IDOK,button,1342242817
Control2=IDC_STATIC,static,1342308352


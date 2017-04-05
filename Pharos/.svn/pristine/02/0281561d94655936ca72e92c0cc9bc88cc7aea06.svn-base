//DLL函数声明头文件
#ifndef _PPRINTER_H
#define _PPRINTER_H
//DLL Function Declaration start
typedef int (__stdcall *fOpenPort)(LPCTSTR printername);
typedef int (__stdcall *fZM_SetDarkness)(unsigned  int id);
typedef int (__stdcall *fZM_SetPrintSpeed)(unsigned int px);
typedef int (__stdcall *fClosePort)(void);
typedef int (__stdcall *fZM_PrintLabel)(unsigned int number,
		                                 unsigned int cpnumber);
typedef int (__stdcall *fZM_DrawBarcode)(unsigned  int px,
		                        unsigned int  py,
								unsigned int  pdirec,
								LPTSTR        pCode,
								unsigned int  pHorizontal,
								unsigned int  pVertical,
								unsigned int pbright,
								char ptext,
								LPTSTR pstr);
typedef int (__stdcall *fZM_DrawTextTrueTypeW)
		                            (int x,int y,int FHeight,
									int FWidth,LPCTSTR FType,
                                    int Fspin,int FWeight,
									BOOL FItalic,BOOL FUnline,
                                    BOOL FStrikeOut,
									LPCTSTR id_name,
									LPCTSTR data);
typedef int (__stdcall *fZM_SetLabelHeight)
		                (unsigned int lheight, unsigned int gapH);
typedef int (__stdcall *fZM_SetLabelWidth)(unsigned int lwidth);
typedef int (__stdcall *fZM_ClearBuffer)();
typedef int (__stdcall *fZM_DrawRectangle)(unsigned int    px,  unsigned int    py,   
                                            unsigned int   thickness,  unsigned int   pEx, 
                                            unsigned int   pEy); 
typedef int (__stdcall *fZM_PcxGraphicsDel) (LPTSTR pid);
typedef int (__stdcall *fZM_PcxGraphicsDownload) (char*  pcxname, char* pcxpath);
typedef int (__stdcall *fZM_DrawPcxGraphics)(unsigned int  px, unsigned int  py, LPTSTR  gname);
typedef int (__stdcall *fZM_DrawBar2D_Pdf417)(unsigned int x, unsigned int  y,
								      unsigned int w, unsigned int v,
								      unsigned int s, unsigned int c,
									  unsigned int px, unsigned int  py,
									  unsigned int r, unsigned int l,
								      unsigned int t, unsigned int o,					   
								      LPTSTR pstr);
typedef int (__stdcall *fZM_DrawBar2D_QR)( unsigned int x,
								   unsigned int y,
								   unsigned int w, 
								   unsigned int v,
								   unsigned int o, 
								   unsigned int r,
								   unsigned int m, 
								   unsigned int g,
								   unsigned int s,
								   LPTSTR pstr);
typedef int (_stdcall *fZM_DrawLineOr)(unsigned int px,unsigned int py,unsigned int plength,unsigned int pH);
typedef int (_stdcall *fZM_DrawText)(unsigned  int px,unsigned int  py,
							  unsigned int  pdirec,unsigned int pFont,
							  unsigned int  pHorizontal,
							  unsigned int  pVertical,
							  char ptext,LPTSTR pstr);

fOpenPort OpenPort = NULL;
fZM_SetDarkness ZM_SetDarkness = NULL;
fZM_DrawTextTrueTypeW ZM_DrawTextTrueTypeW = NULL;
fZM_SetPrintSpeed ZM_SetPrintSpeed = NULL;
fClosePort ClosePort = NULL;
fZM_PrintLabel ZM_PrintLabel = NULL;
fZM_DrawBarcode ZM_DrawBarcode = NULL;
fZM_SetLabelHeight ZM_SetLabelHeight = NULL;
fZM_SetLabelWidth ZM_SetLabelWidth = NULL;
fZM_ClearBuffer ZM_ClearBuffer = NULL;
fZM_DrawRectangle ZM_DrawRectangle = NULL;
fZM_PcxGraphicsDel ZM_PcxGraphicsDel = NULL;
fZM_PcxGraphicsDownload ZM_PcxGraphicsDownload = NULL;
fZM_DrawPcxGraphics ZM_DrawPcxGraphics = NULL;
fZM_DrawBar2D_Pdf417 ZM_DrawBar2D_Pdf417 = NULL;
fZM_DrawBar2D_QR ZM_DrawBar2D_QR = NULL;
fZM_DrawLineOr ZM_DrawLineOr =NULL;
fZM_DrawText ZM_DrawText =NULL;

#endif
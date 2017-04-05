unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TForm1 = class(TForm)
    Button1: TButton;
    Label1: TLabel;
    procedure Button1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}
        function  ZM_SetDarkness ( darkness:integer):integer;stdcall;external 'ZMWIN.dll';
        function  ZM_SetPrintSpeed ( speed:integer):integer;stdcall;external 'ZMWIN.dll';
        function  OpenPort    ( selection:pchar):integer;stdcall;external 'ZMWIN.dll';
        function  ZM_PrintLabel    ( number,cpnumber:Integer):integer;stdcall;external 'ZMWIN.dll';
        function  ZM_DrawBarcode  ( px,py,pdirec:integer;typee:pchar;NarrowWidth,pHorizontal,pVertical:integer;ptext:char;pstr:pchar):integer;stdcall;external 'ZMWIN.dll';
        Procedure ZM_ClearBuffer  ();stdcall;external 'ZMWIN.dll';
        Procedure ClosePort     ();stdcall;external 'ZMWIN.dll';
        function  ZM_DrawTextTrueTypeW ( x,y,FHeight,FWidth:integer;FType:pchar;Fspin,FWeight,FItalic,FUnline,FStrikeOut:integer;id_name,data:pchar):integer;stdcall;external 'ZMWIN.dll';
        Function GetErrState ():Integer;stdcall;external 'ZMWIN.dll';
        function ZM_PcxGraphicsDel (pic:pchar):integer;stdcall;external 'ZMWIN.dll';
        function ZM_SetLabelHeight(lheight,gapH:Integer):integer;stdcall;external 'ZMWIN.dll';
        function ZM_SetLabelWidth(lwidth:Integer):integer;stdcall;external'ZMWIN.dll';
        function ZM_DrawText(px,py,pdirec,pFont,pHorizontal,pvertical:integer;ptext:char;pstr:Pchar):integer;stdcall;external 'ZMWIN.dll';
        function SetPCComPort(BaudRate:Integer;HandShake:Boolean):integer;stdcall;external'ZMWIN.dll';
        function ZM_PcxGraphicsDownload(pcxname,pcxpath:pchar):integer;stdcall;external'ZMWIN.dll';
        function ZM_DrawPcxGraphics(x,y:Integer;gname:pchar):integer;stdcall;external'ZMWIN.dll';
        function ZM_DrawBar2D_Pdf417(x,y,w,v,s,c,px,py,r,l,t,o:Integer;LPTSTR:pchar):integer;stdcall;external'ZMWIN.dll';
        function ZM_DrawRectangle(px, py,thickness,pEx,pEy:Integer):integer;stdcall;external'ZMWIN.dll';
        function ZM_DrawBar2D_QR(x,y,w,v,o,r,m, g,s:Integer;pstr:pchar):integer;stdcall;external'ZMWIN.dll';
        function  ZM_DrawLineOr( px,py,plength,pH:integer):integer;stdcall;external 'ZMWIN.dll';
        function ZM_PrintPCX(x,y:Integer;filename:pchar):integer;stdcall;external'ZMWIN.dll';
 procedure TForm1.Button1Click(Sender: TObject);
begin
        OpenPort('ZMIN X1i');             //打开打印机;
        ZM_ClearBuffer();                 //清空打印机缓存;
        ZM_SetDarkness(10);              //设置打印机打印温度;
        ZM_SetPrintSpeed(4);             //设置打印机打印速度;
        ZM_SetLabelHeight(600,24);       //设置标签的高度和间隙大小；
        ZM_SetLabelWidth(800);           //设置标签的宽度；


        // 画矩形
        ZM_DrawRectangle(50,10,3,460,340);

         // 画表格分割线
        ZM_DrawLineOr(60,108,402,3);

        // 打印PCX图形 
        ZM_PcxGraphicsDel(pchar('PCX'));
        ZM_PcxGraphicsDownload(pchar('PCX'),pchar('pic.pcx'));
        ZM_DrawPcxGraphics(80,30,pchar('PCX'));

        // 打印一个128 Auto条码;
        ZM_DrawBarcode(80, 208, 0, '1', 2, 2, 50, 'B', pchar('123456789'));

        // 打印PDF417码
        ZM_DrawBar2D_Pdf417(80,300,400,300,0,0,3,7,10,2,0,0,pchar('123456789'));

        // 打印QR码
        ZM_DrawBar2D_QR(360, 30, 180, 180, 0, 3, 2, 0,0, pchar('ZMIN Electronics Co., Ltd.'));

        // 打印内置字体点阵文字
        ZM_DrawText(80, 168, 0, 3, 1, 1, 'N', pchar('Internal Font'));

        // 打印WINDWOS系统TrueType Font文字;
        ZM_DrawTextTrueTypeW (80, 120, 40, 0, pchar('Arial'), 1, 400, 0, 0, 0, pchar('A1'), pchar('TrueType Font'));

        // 打印WINDWOS系统TrueType Font文字（旋转90度);
        ZM_DrawTextTrueTypeW(420, 102, 22, 0, pchar('Arial'), 2, 400, 0, 0, 0, pchar('A2'), pchar('www.zmin.com.cn'));

        //开始打印
        ZM_PrintLabel(1, 1);

        //关闭打印机
        ClosePort();
end;

end.



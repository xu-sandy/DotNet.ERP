program WIN_Delphi_demo;

uses
  Forms,
  Unit1 in '..\WIN_Delphi7_demo\Unit1.pas' {Form1};

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TForm1, Form1);
  Application.Run;
end.

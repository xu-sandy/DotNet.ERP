@echo off  
reg query "HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" /v %~dp0Pharos.POS.ClientService.exe
IF errorlevel == 0 (  
reg delete "HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" /v %~dp0Pharos.POS.ClientService.exe /f  
reg add "HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" /v %~dp0Pharos.POS.ClientService.exe -t  REG_SZ /d "~ RUNASADMIN"  
) else (  
reg add "HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" /v %~dp0Pharos.POS.ClientService.exe -t  REG_SZ /d "~ RUNASADMIN"  
) 
%~dp0Pharos.POS.ClientService.exe -u
pause
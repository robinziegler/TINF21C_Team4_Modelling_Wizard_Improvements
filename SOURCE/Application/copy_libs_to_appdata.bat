@echo off
cls
md %AppData%\MURR
md %AppData%\MURR\Templates
md %AppData%\MURR\x64
md %AppData%\MURR\x86

copy Templates\*.* %AppData%\MURR\Templates
copy packages\SevenZipSharp.Interop.19.0.2\build\x86\*.* %AppData%\MURR\x86
copy packages\SevenZipSharp.Interop.19.0.2\build\x64\*.* %AppData%\MURR\x64
cls
echo.
echo.
echo.
echo "All libs copied to system!"
echo.
echo.
echo.
pause

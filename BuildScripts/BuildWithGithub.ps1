#Build from repozotory sourse

$lastLocation = (Get-Location).Path

$srcPath = Join-Path  $lastLocation "ExtSrc"


If (Test-Path $srcPath){
	Remove-Item $srcPath -Force -Recurse
}
New-Item $srcPath -type directory

Write-Host "Cloning repozitory  MassiveDynamicProxyGenerator master" -ForegroundColor Green
cd $srcPath
#&git clone "https://github.com/harrison314/MassiveDynamicProxyGenerator" 2>&1

start-process -FilePath git -ArgumentList ("clone", "-b master https://github.com/harrison314/MassiveDynamicProxyGenerator") -Wait -NoNewWindow

cd  $lastLocation

.\Build.ps1 -SrcRelativePath "ExtSrc\MassiveDynamicProxyGenerator\src"


# Build script

Param(
  [Parameter(Mandatory=$False)]
  [string]$SrcRelativePath = "../src/",
  [switch]$RunTests 
)

$msBuild = Join-Path ${env:ProgramFiles(x86)} "MSBuild\14.0\Bin\amd64\MSBuild.exe"
$lastLocation = (Get-Location).Path

$srcPath = Join-Path  $lastLocation $SrcRelativePath
$toolPath = Join-Path $lastLocation "Tools"
$outPath = Join-Path $lastLocation "BuildOutput"

# ----------------------------------------------------------------------------------------------------#

If (Test-Path $outPath){
	Remove-Item $outPath -Force -Recurse
}
New-Item $outPath -type directory

If (-Not (Test-Path $toolPath)){
	New-Item $toolPath -type directory
}

$nugetExe = (Join-path $toolPath "nuget.exe")
If (-Not (Test-Path $nugetExe)){
	Write-Host "Download Nuget.exe" -ForegroundColor Green
	Invoke-WebRequest -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -OutFile $nugetExe
}

Write-Host "Restore Nuget packages..." -ForegroundColor Green
$slnPath = Join-Path $srcPath "MassiveDynamicProxyGenerator.sln"

cd $srcPath
foreach($p in (Get-ChildItem -Path $srcPath -Filter "*.csproj" -Recurse | Where-Object { $_.Attributes -ne "Directory"}))
{
	&$nugetExe restore "$($p.FullName)" -SolutionDirectory "$srcPath"
}

cd $lastLocation

Write-Host "Restore Nuget Core packages..." -ForegroundColor Green
foreach($p in (Get-ChildItem -Path $srcPath -Filter "project.json" -Recurse | Where-Object { $_.Attributes -ne "Directory"}))
{
	$workDir = [System.IO.Path]::GetDirectoryName($p.FullName)
	cd $workDir
	&dotnet restore
}

cd $lastLocation


$slnPath =Join-Path $srcPath "MassiveDynamicProxyGenerator.NuGet/MassiveDynamicProxyGenerator.NuGet.csproj"

& "$msBuild" "$slnPath" "/t:Clean" "/p:Configuration=Release"
& "$msBuild" "$slnPath" "/t:Build" "/p:Configuration=Release" "/p:RestorePackages=true"

Copy-Item (Join-Path ${srcPath} "MassiveDynamicProxyGenerator.NuGet\bin\Release\MassiveDynamicProxyGenerator.*.nupkg") $outPath

If($RunTests){
	Write-Host "Start .Net Core tests" -ForegroundColor Green

	$testCoreFolder = Join-Path $srcPath "MassiveDynamicProxyGenerator.Tests.NetStandard"
    cd $testCoreFolder

	&dotnet restore
	&dotnet test

	cd $lastLocation

   # TODO: test other frameworks
}

# Build script

# TODO: add --version-suffix

Param(
  [Parameter(Mandatory=$False)]
  [string]$SrcRelativePath = "../src/",
  [switch]$RunTests 
)

#$msBuild = Join-Path ${env:ProgramFiles(x86)} "MSBuild\14.0\Bin\amd64\MSBuild.exe"
$lastLocation = (Get-Location).Path

$srcPath = Join-Path  $lastLocation $SrcRelativePath
$outPath = Join-Path $lastLocation "BuildOutput"

# ----------------------------------------------------------------------------------------------------#

If (Test-Path $outPath){
	Remove-Item $outPath -Force -Recurse
}
New-Item $outPath -type directory


cd $lastLocation

Write-Host "Restore Nuget Core packages..." -ForegroundColor Green
foreach($p in (Get-ChildItem -Path $srcPath -Filter "project.json" -Recurse | Where-Object { $_.Attributes -ne "Directory"}))
{
	$workDir = [System.IO.Path]::GetDirectoryName($p.FullName)
	cd $workDir
	&dotnet restore
}

cd $lastLocation


$slnPath = Join-Path $srcPath "MassiveDynamicProxyGenerator"

cd $slnPath
& dotnet restore
& dotnet pack -c Release -o "$outPath"

$projSimpleInjectorPath = Join-Path $srcPath "MassiveDynamicProxyGenerator.SimpleInjector"

cd $projSimpleInjectorPath
& dotnet restore
& dotnet pack -c Release -o "$outPath"

$projSimpleInjectorPath = Join-Path $srcPath "MassiveDynamicProxyGenerator.Microsoft.DependencyInjection"

cd $projSimpleInjectorPath
& dotnet restore
& dotnet pack -c Release -o "$outPath"

cd $lastLocation


If($RunTests){
	Write-Host "Start .Net Core tests" -ForegroundColor Green

	$testCoreFolder = Join-Path $srcPath "MassiveDynamicProxyGenerator.Tests.NetStandard"
    cd $testCoreFolder

	&dotnet restore
	&dotnet test

	$testCoreFolder = Join-Path $srcPath "MassiveDynamicProxyGenerator.SimpleInjector.Tests.NetCore"

	cd $testCoreFolder

	&dotnet restore
	&dotnet test

    $testCoreFolder = Join-Path $srcPath "MassiveDynamicProxyGenerator.DependencyInjection.Test"
    cd $testCoreFolder

	&dotnet restore
	&dotnet test

	cd $lastLocation

   # TODO: test other frameworks
}

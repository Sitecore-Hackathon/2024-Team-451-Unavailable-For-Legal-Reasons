$ErrorActionPreference = "STOP"

# validate build tools
if ($null -eq (Get-Command "nuget.exe" -ErrorAction SilentlyContinue))
{
    throw "nuget.exe not found in '`$env:PATH'"
}

$msbuildPath = "C:\Program Files\Microsoft Visual Studio\2022\*\MSBuild\Current\Bin\amd64\MSBuild.exe"
$msbuildExe = (Get-Item $msbuildPath).FullName

if ($null -eq $msbuildExe)
{
    throw "msbuild.exe not found in '$msbuildPath'"
}

if ($null -eq (Get-Command "dotnet.exe" -ErrorAction SilentlyContinue))
{
    throw "dotnet.exe not found in '`$env:PATH'"
}

$env:BUILD_CONFIGURATION = "Debug"

# start build and publish
nuget restore ./team451.sln -LockedMode

& $msbuildExe ./team451.sln `
    -p:Configuration=$env:BUILD_CONFIGURATION `
    -p:DeployOnBuild=True `
    -p:PublishProfile=Local `
    -p:CollectWebConfigsToTransform=False `
    -p:TransformWebConfigEnabled=False `
    -p:AutoParameterizationWebConfigConnectionStrings=False `
    -m -v:m -noLogo

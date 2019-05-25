$location = Get-Location
$currentDirectory = $location.Path

Write-Host "Currect working directory: $currentDirectory"

$nupkg = Get-ChildItem -Path $currentDirectory -Filter *.nupkg -Recurse | Select-Object -First 1

if($nupkg -eq $null)
{
    Throw "No NuGet Package could be found in the current directory"
}

Write-Host "Package Name: $($nupkg.Name)"
$nupkg.Name -match '^(.*?)\.((?:\.?[0-9]+){3,}(?:[-a-z]+)?)\.nupkg$'

$VersionName = $Matches[2]
$IsPreview = $VersionName -match '-pre$'
$ReleaseDisplayName = $VersionName

if($env:IS_PREVIEW -eq $null)
{
    Write-Output ("##vso[task.setvariable variable=IS_PREVIEW;]$IsPreview")
}

if($IsPreview -eq $true)
{
    $ReleaseDisplayName = "$VersionName - Preview"
}

Write-Host "Version Name" $VersionName
Write-Host "Release Display Name: $ReleaseDisplayName"

Write-Output ("##vso[task.setvariable variable=VersionName;]$VersionName")
Write-Output ("##vso[task.setvariable variable=ReleaseDisplayName;]$ReleaseDisplayName")

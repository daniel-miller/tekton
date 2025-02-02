$Now            = (Get-Date)
$NowUtc         = $Now.ToUniversalTime()

$MajorNumber    = "10"
$MinorNumber    = ($NowUtc.ToString("yyyy")) -as [int]
$BuildNumber    = ($NowUtc.ToString("MM") + $NowUtc.ToString("dd")) -as [int]
$RevisionNumber = ($NowUtc.ToString("HH") + $NowUtc.ToString("mm")) -as [int]
$PackageVersion = "$MajorNumber.$MinorNumber.$BuildNumber.$RevisionNumber"


Write-Host "`nStep 1: Starting build for Tek.Terminal version $PackageVersion`n" -ForegroundColor Blue

  $PublishFolder = ".\Releases\Publish\Tek.Terminal"
  $ReleasePath   = ".\Releases\Tek.Terminal.$PackageVersion.zip"

  if (Test-Path -Path $PublishFolder) { Remove-Item -Path $PublishFolder\* -Recurse }

  dotnet restore ..\src\terminal\Tek.Terminal\Tek.Terminal.csproj
  dotnet publish ..\src\terminal\Tek.Terminal\Tek.Terminal.csproj -c Release -o $PublishFolder --self-contained -r win-x64 /p:Version=$PackageVersion

  Compress-Archive -Path $PublishFolder\* -DestinationPath $ReleasePath
  Remove-Item -Path $PublishFolder -Recurse

  $elapsedTime = $(get-date) - $Now
  Write-Host "`nStep 1: Completed build for Tek.Terminal version $PackageVersion (elapsed time = $($elapsedTime.ToString("mm\:ss")))" -ForegroundColor Blue


  Write-Host "`nStep 2: Starting build for Tek.Api version $PackageVersion`n" -ForegroundColor Blue

  $PublishFolder = ".\Releases\Publish\Tek.Api"
  $ReleasePath   = ".\Releases\Tek.Api.$PackageVersion.zip"

  if (Test-Path -Path $PublishFolder) { Remove-Item -Path $PublishFolder\* -Recurse }

  dotnet restore ..\src\api\Tek.Api\Tek.Api.csproj
  dotnet publish ..\src\api\Tek.Api\Tek.Api.csproj -c Release -o $PublishFolder --self-contained -r win-x64 /p:Version=$PackageVersion

  Compress-Archive -Path $PublishFolder\* -DestinationPath $ReleasePath
  Remove-Item -Path $PublishFolder -Recurse

  $elapsedTime = $(get-date) - $Now
  Write-Host "`nStep 2: Completed build for Tek.Api version $PackageVersion (elapsed time = $($elapsedTime.ToString("mm\:ss")))" -ForegroundColor Blue
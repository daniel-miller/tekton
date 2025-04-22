$Now            = (Get-Date)
$NowUtc         = $Now.ToUniversalTime()

$MajorNumber    = "1"
$MinorNumber    = ($NowUtc.ToString("yyyy")) -as [int]
$BuildNumber    = ($NowUtc.ToString("MM") + $NowUtc.ToString("dd")) -as [int]
$RevisionNumber = ($NowUtc.ToString("HH") + $NowUtc.ToString("mm")) -as [int]
$PackageVersion = "$MajorNumber.$MinorNumber.$BuildNumber.$RevisionNumber"


Write-Host "`nStep 1: Starting build for Tek.Terminal version $PackageVersion`n" -ForegroundColor Blue

  $PublishFolder = ".\releases\Publish\Tek.Terminal"
  $ReleasePath   = ".\releases\Tek.Terminal.$PackageVersion.zip"

  if (Test-Path -Path $PublishFolder) { Remove-Item -Path $PublishFolder\* -Recurse }

  dotnet restore ..\src\terminal\Tek.Terminal\Tek.Terminal.csproj
  dotnet publish ..\src\terminal\Tek.Terminal\Tek.Terminal.csproj -c Release -o $PublishFolder --self-contained -r win-x64 /p:Version=$PackageVersion

  Compress-Archive -Path $PublishFolder\* -DestinationPath $ReleasePath
  Remove-Item -Path $PublishFolder -Recurse

  $elapsedTime = $(get-date) - $Now
  Write-Host "`nStep 1: Completed build for Tek.Terminal version $PackageVersion (elapsed time = $($elapsedTime.ToString("mm\:ss")))" -ForegroundColor Blue


Write-Host "`nStep 2: Starting build for Tek.Api version $PackageVersion`n" -ForegroundColor Blue

  $PublishFolder = ".\releases\Publish\Tek.Api"
  $ReleasePath   = ".\releases\Tek.Api.$PackageVersion.zip"

  if (Test-Path -Path $PublishFolder) { Remove-Item -Path $PublishFolder\* -Recurse }

  dotnet restore ..\src\api\Tek.Api\Tek.Api.csproj
  dotnet publish ..\src\api\Tek.Api\Tek.Api.csproj -c Release -o $PublishFolder --self-contained -r win-x64 /p:Version=$PackageVersion

  Compress-Archive -Path $PublishFolder\* -DestinationPath $ReleasePath
  Remove-Item -Path $PublishFolder -Recurse

  $elapsedTime = $(get-date) - $Now
  Write-Host "`nStep 2: Completed build for Tek.Api version $PackageVersion (elapsed time = $($elapsedTime.ToString("mm\:ss")))" -ForegroundColor Blue


Write-Host "`nStep 3: Starting build for plugin Apis`n" -ForegroundColor Cyan

  $PublishFolder = ".\releases\Publish\Tek.Integration.PreMailer.Api"
  $ReleasePath   = ".\releases\Tek.Integration.PreMailer.Api.$PackageVersion.zip"

  if (Test-Path -Path $PublishFolder) { Remove-Item -Path $PublishFolder\* -Recurse }

  dotnet restore ..\src\plugin\Tek.Integration.PreMailer.Api\Tek.Integration.PreMailer.Api.csproj
  dotnet publish ..\src\plugin\Tek.Integration.PreMailer.Api\Tek.Integration.PreMailer.Api.csproj -c Release -o $PublishFolder --self-contained -r win-x64 /p:Version=$PackageVersion

  Compress-Archive -Path $PublishFolder\* -DestinationPath $ReleasePath
  Remove-Item -Path $PublishFolder -Recurse

  $PublishFolder = ".\releases\Publish\Tek.Integration.Scorm.Api"
  $ReleasePath   = ".\releases\Tek.Integration.Scorm.Api.$PackageVersion.zip"

  if (Test-Path -Path $PublishFolder) { Remove-Item -Path $PublishFolder\* -Recurse }

  dotnet restore ..\src\plugin\Tek.Integration.Scorm.Api\Tek.Integration.Scorm.Api.csproj
  dotnet publish ..\src\plugin\Tek.Integration.Scorm.Api\Tek.Integration.Scorm.Api.csproj -c Release -o $PublishFolder --self-contained -r win-x64 /p:Version=$PackageVersion

  Compress-Archive -Path $PublishFolder\* -DestinationPath $ReleasePath
  Remove-Item -Path $PublishFolder -Recurse

  $elapsedTime = $(get-date) - $Now
  Write-Host "`nStep 3: Completed build for plugin Apis (elapsed time = $($elapsedTime.ToString("mm\:ss")))" -ForegroundColor Cyan


  Write-Host "`nStep 4: Building documentation`n" -ForegroundColor Blue

  $SiteFolder = ".\_site"
  if (Test-Path -Path $SiteFolder) { Remove-Item -Path $SiteFolder\* -Recurse }

  $ApiFolder = ".\api"
  if (Test-Path -Path $ApiFolder) { Remove-Item -Path $ApiFolder\* -Recurse }

  docfx docfx.json --logLevel Verbose

  $ReleasePath   = ".\releases\Tek.Docs.$PackageVersion.zip"

  $PublishFolder = ".\releases\Tek.Docs"
  if (Test-Path -Path $PublishFolder) { Remove-Item -Path $PublishFolder\* -Recurse }

  Copy-Item $SiteFolder $PublishFolder -Recurse
  Remove-Item -Path $SiteFolder -Recurse
  Remove-Item -Path $ApiFolder -Recurse

  Compress-Archive -Path $PublishFolder\* -DestinationPath $ReleasePath
  
  Remove-Item -Path $PublishFolder -Recurse

  $elapsedTime = $(get-date) - $Now
  Write-Host "`nStep 4: Completed build for Tek.Docs (elapsed time = $($elapsedTime.ToString("mm\:ss")))" -ForegroundColor Blue
  
  
exit


Write-Host "`nStep 5: Uploading packages to Octopus...`n" -ForegroundColor Red

  $OctoServer = "https://miller.octopus.app"
  $OctoKey    = Get-Content -Path ..\config\octopus-api-key.txt -TotalCount 1

  Octo push --server=$OctoServer --apiKey=$OctoKey --replace-existing --package=Releases\Tek.Api.$PackageVersion.zip
  Octo push --server=$OctoServer --apiKey=$OctoKey --replace-existing --package=Releases\Tek.Terminal.$PackageVersion.zip

  Octo push --server=$OctoServer --apiKey=$OctoKey --replace-existing --package=Releases\Tek.Docs.$PackageVersion.zip

  Octo push --server=$OctoServer --apiKey=$OctoKey --replace-existing --package=Releases\Tek.Integration.PreMailer.Api.$PackageVersion.zip
  Octo push --server=$OctoServer --apiKey=$OctoKey --replace-existing --package=Releases\Tek.Integration.Scorm.Api.$PackageVersion.zip
 
  $elapsedTime = $(get-date) - $Now
  Write-Host "`nStep 5: Completed upload. Elapsed time = $($ElapsedTime.ToString("mm\:ss"))`n" -ForegroundColor Red
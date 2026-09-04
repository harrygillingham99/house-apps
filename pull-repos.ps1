param(
    [switch]$SkipGitFolderRemoval
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location $ScriptDir

$repos = @(
    @{ Name = "HouseAPI"; Url = "https://github.com/harrygillingham99/HouseAPI.git" },
    @{ Name = "HouseDashboard"; Url = "https://github.com/harrygillingham99/HouseDashboard.git" }
)

foreach ($repo in $repos) {
    $targetPath = Join-Path $ScriptDir $repo.Name
    $gitDir = Join-Path $targetPath ".git"

    if (Test-Path $gitDir) {
        Write-Host "Updating $($repo.Name)..." -ForegroundColor Cyan
        git -C $targetPath pull
    } else {
        if (Test-Path $targetPath) {
            Remove-Item -Recurse -Force $targetPath
        }
        Write-Host "Cloning $($repo.Name)..." -ForegroundColor Cyan
        git clone $repo.Url $targetPath
    }

    if (-not $SkipGitFolderRemoval) {
        if (Test-Path $gitDir) {
            Write-Host "Removing .git directory from $($repo.Name) so it can be tracked directly in parent repo..." -ForegroundColor DarkGray
            Remove-Item -Recurse -Force $gitDir
        }
    }
}

Write-Host "All repositories synchronized." -ForegroundColor Green

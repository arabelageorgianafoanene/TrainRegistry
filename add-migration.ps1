param(
    [Parameter(Mandatory=$true)]
    [string]$name
)

# Load environment variables from .env file
Get-Content .env | ForEach-Object {
    if ($_ -match "^([^=]+)=(.+)$") {
        [System.Environment]::SetEnvironmentVariable($matches[1].Trim(), $matches[2].Trim(), "Process")
    }
}

# Add the migration
dotnet ef migrations add $name --project TrainRegistry.Infrastructure --startup-project TrainRegistry.API
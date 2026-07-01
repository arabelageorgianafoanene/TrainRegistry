# Load environment variables from .env file
Get-Content .env | ForEach-Object {
    if ($_ -match "^([^=]+)=(.+)$") {
        [System.Environment]::SetEnvironmentVariable($matches[1].Trim(), $matches[2].Trim(), "Process")
    }
}

# Run the migration
dotnet ef database update --project TrainRegistry.Infrastructure --startup-project TrainRegistry.API
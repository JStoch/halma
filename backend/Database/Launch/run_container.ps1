﻿cd ..

$containerName = "mssql-server"

# Check if the container exists
if (docker ps -a --format "{{.Names}}" | Select-String -Pattern "^$containerName$" -Quiet) {
    # Container exists, so remove it
    docker rm -f $containerName
    Write-Output "Container $containerName removed."
}
else {
    Write-Output "Container $containerName does not exist."
}


# Create docker network if not exists
if (-not (docker network ls | Select-String -Pattern "halma-network")) {
    echo "Creatig halma-network...";
    docker network create halma-network
}

# Build MSSQL database setup image
docker build -t mssql-setup -f ./Dockerfile .

# Run MSSQL database setup container -p 1433:1433
docker run -d --name mssql-server --hostname mssql-server  --network halma-network -v mssql_data:/var/opt/mssql mssql-setup
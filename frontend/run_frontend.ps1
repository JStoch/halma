$containerName = "halma-frontend"

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
docker build -t halma-frontend-setup -f ./Dockerfile .

# Run MSSQL database setup container 
docker run -d --name halma-frontend --network halma-network --hostname halma-frontend -p 8082:80  halma-frontend-setup
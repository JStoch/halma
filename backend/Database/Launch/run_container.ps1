cd ..

# Create docker network if not exists

if (-not (docker network ls | Select-String -Pattern "halma-network")) {
    echo "Creatig halma-network...";
    docker network create halma-network
}

# Build MSSQL database setup image
docker build -t mssql-setup -f ./Dockerfile .

# Run MSSQL database setup container 
docker run -d --name mssql-server --hostname mssql-server -p 1433:1433 --network halma-network -v mssql_data:/var/opt/mssql mssql-setup
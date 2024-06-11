#!/bin/bash
cd ..
# Create docker network if not exists

docker network ls|grep halma-netwotk > /dev/null || docker network create halma-network

container_name="mssql-server"

# Check if the container exists
if docker ps -a --format '{{.Names}}' | grep -q "^$container_name$"; then
    # Container exists, so remove it
    docker rm -f $container_name
    echo "Container $container_name removed."
else
    echo "Container $container_name does not exist."
fi


chmod +x ./database-scripts/*.sql
# Build MSSQL database setup image
docker build -t mssql-setup -f ./Dockerfile .

# Run MSSQL database setup container
docker run -d --name mssql-server -p 1433:1433 --network halma-network -v mssql_data:/var/opt/mssql mssql-setup
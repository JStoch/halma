#!/bin/bash
cd ..
# Create docker network if not exists

docker network ls|grep halma-netwotk > /dev/null || docker network create halma-network


chmod +x ./database-scripts/*.sql
# Build MSSQL database setup image
docker build -t mssql-setup -f ./Dockerfile .

# Run MSSQL database setup container
docker run -d --name mssql-server -p 1433:1433 --network halma-network -v mssql_data:/var/opt/mssql mssql-setup
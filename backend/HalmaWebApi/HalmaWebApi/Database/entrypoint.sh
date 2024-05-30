#!/bin/bash

# Change to the SQL scripts directory
cd /docker-entrypoint-initdb.d/

# Iterate over each SQL script in the directory and execute it becouse docker wont run it manually 
# or im too stupid to configure it properly :(
for script in *.sql; do
    echo "Executing $script..."
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -i "$script"
done
# Use the official SQL Server 2019 image from Docker Hub
FROM mcr.microsoft.com/mssql/server:2019-latest

# Set environment variables for SQL Server
ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=P@ssw0rdSql
ENV HOST_HOSTNAME=HALMA-MSSQL

# Copy the SQL scripts for database setup
COPY ./database-scripts /docker-entrypoint-initdb.d

# Ensure that mssql user had proper permissions
USER root
RUN chown -R mssql:root /docker-entrypoint-initdb.d && \
    chmod -R 755 /docker-entrypoint-initdb.d

# Copy the entrypoint shell script into the container
COPY entrypoint.sh /entrypoint.sh

# Set the entrypoint script as executable
RUN chmod +x /entrypoint.sh

# Expose the SQL Server port
EXPOSE 1433

# Add a volume to persist data
VOLUME /var/opt/mssql

ENTRYPOINT ["/opt/mssql/bin/sqlservr", "/entrypoint.sh"]
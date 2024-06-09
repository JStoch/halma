# Database Configuration Information

-  (Work in progress) Due to connectivity problem try creating new network: 

	```code
	$ docker network create halma-network
	```


- In case of 'entrpoint.sh' modification locally on Windows machine make sure to fix formating for UNIX shell, before launching the container.

	Exemple:

	```code
	$ dos2unix ./entrypoint.sh
	```

- In case that 'entrypoint.sh' still does not execute automaticly run it manually inside container's shell:

	```code
	$ cd / 
	$ ./entrypoint.sh
	```
- Container uses volume 'mssql-data' to preserve data, so 'entrypoint.sh' should be run only on first image build. 
  Delete volume if any changes require that:
	
	```code
	$ docker volume rm mssql_data
	```
- Connection string to database should look like this (add them in appsettings.json):
	```code
	string connString = "Server=tcp:mssql-server,1433;Database=<DB_NAME>;User ID=<USER_NAME>;Password=<USER_PASSWORD>;<add other params if nesseccery>"
	```
- Manage database content via EF Migrations by NuGet PM Console:
	```code
	add-migration init -Context HalmaDbContext //to add migation
	update-database -Context HalmaDbContext // to update database with latest migration
	Update-Database -Migration:0 -Context HalmaDbContext //to rollback all changes
	```
- Sooo EF decided to stop working for normal migrations done by 'Update-Database' command. Here is another solution to perform migrations:
	```code
	Add-Migration -v init -Context HalmaDbContext
	Script-Migration -v -Context HalmaDbContext -o ./backend/Database/database-scripts/migration.sql
    Script-Migration -From <PreviousMigration> -To <LastMigration> -Context HalmaDbContext -o... //changing db content based on migrations
	```
	Adjust the names of migration and context accordingly, and then finally run migration.sql manualy on the desired database :)
	The '-v' flag is useful to see what's going on under the hood.

- Clearing databases to reload data (if needed) is done by executing:
  .\Purging\purge.sql

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

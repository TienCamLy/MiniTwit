## Docker Commands
Replace all <...>, with correct names/ids
- Build Docker container (from current directory; docker file):
    ```bash
    docker built -t <your_id>/<container> .
    ```
- Run Docker container:
    ```bash
    docker run --name <name> -p 8080:8080 <your_id>/<container>
    docker run --name webserver -p 8080:8080 id/myserver
    ```
- Stop/Start Container:
    ```bash
    docker stop <container_name>
    docker start <container_name>
    ```
- Docker Compose (composes multiple built containers, defined in `docker-compose.yml`)
    ```bash
    docker compose up
    ```

- Check accessible Docker containers:
    ```bash
    docker images
    ```
- Check running Docker containers: 
    ```bash
    docker ps -a
    ```
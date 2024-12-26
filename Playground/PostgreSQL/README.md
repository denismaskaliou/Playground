# Setting Up PostgreSQL with Docker

This guide explains how to set up and run a PostgreSQL container using Docker.

## Prerequisites

- [Docker](https://www.docker.com/products/docker-desktop) installed on your machine.

## Steps

### 1. Pull the PostgreSQL Docker Image

Download the official PostgreSQL image from Docker Hub:
```bash
docker pull postgres
```

### 2. Run a PostgreSQL Container

Use the following command to create and start a PostgreSQL container:
```bash
docker run --name postgres-container -e POSTGRES_USER=user -e POSTGRES_PASSWORD=1234 -e POSTGRES_DB=Playground -p 5432:5432 -d postgres
```

**Explanation:**
- `--name postgres-container`: Assigns a name to your container.
- `-e POSTGRES_USER=your_username`: Sets the username for PostgreSQL.
- `-e POSTGRES_PASSWORD=your_password`: Sets the password for PostgreSQL.
- `-e POSTGRES_DB=your_database`: Creates a database with the specified name.
- `-p 5432:5432`: Maps the container's PostgreSQL port to your host machine.
- `-d postgres`: Runs the container in detached mode.

### 3. Verify the Container is Running

Check the running containers:
```bash
docker ps
```

You should see your PostgreSQL container in the list.

### 4. Connect to PostgreSQL

#### Using `psql`
You can connect to the database using the `psql` command-line tool:
```bash
psql -h localhost -p 5432 -U your_username -d your_database
```
You will be prompted to enter the password.

#### Using a GUI Tool
You can use GUI tools like [pgAdmin](https://www.pgadmin.org/) or DBeaver to connect to the PostgreSQL database. Use the following connection details:
- **Host:** `localhost`
- **Port:** `5432`
- **Username:** `your_username`
- **Password:** `your_password`
- **Database:** `your_database`

### 5. Persist Data Using Volumes (Optional)

To persist data beyond the lifecycle of the container, you can mount a volume:
```bash
docker run --name postgres-container -e POSTGRES_USER=your_username -e POSTGRES_PASSWORD=your_password -e POSTGRES_DB=your_database -p 5432:5432 -v postgres-data:/var/lib/postgresql/data -d postgres
```

**Explanation:**
- `-v postgres-data:/var/lib/postgresql/data`: Mounts a Docker volume to store PostgreSQL data.

### 6. Stop and Remove the Container (Optional)

#### Stop the container:
```bash
docker stop postgres-container
```

#### Remove the container:
```bash
docker rm postgres-container
```

#### Remove the volume (if used):
```bash
docker volume rm postgres-data
```

## Additional Resources

- [Official PostgreSQL Docker Image](https://hub.docker.com/_/postgres)
- [Docker Documentation](https://docs.docker.com/)
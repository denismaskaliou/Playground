# RabbitMQ in Docker

This guide provides simple steps to run MongoDB in a Docker container and connect to it.

---

## Usage

Follow the steps below to set up and use MongoDB in Docker.

### Running the MongoDB Container

Run the following command to start a MongoDB container with a root username and password:

```bash
docker run -d --name mongodb \
  -p 27017:27017 \
  -e MONGO_INITDB_ROOT_USERNAME=admin \
  -e MONGO_INITDB_ROOT_PASSWORD=1234 \
  mongo
```

### Connecting to MongoDB

You can connect to MongoDB using the MongoDB shell from inside the container. Run:
```bash
docker exec -it mongodb mongosh -u admin -p secret
```
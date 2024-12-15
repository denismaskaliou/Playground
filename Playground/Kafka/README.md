# Running Kafka in Docker

This guide provides step-by-step instructions to run Apache Kafka in a Docker environment.

## Prerequisites

Before starting, ensure you have the following installed on your machine:

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Steps to Run Kafka in Docker

### 1. Create a `docker-compose.yml` File

Create a `docker-compose.yml` file with the following content:

```yaml
version: '3.8'
services:
  zookeeper:
    image: confluentinc/cp-zookeeper:latest
    container_name: zookeeper
    ports:
      - "2181:2181"
    environment:
      ZOOKEEPER_CLIENT_PORT: 2181
      ZOOKEEPER_TICK_TIME: 2000

  kafka:
    image: confluentinc/cp-kafka:latest
    container_name: kafka
    ports:
      - "9092:9092"
    environment:
      KAFKA_BROKER_ID: 1
      KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
      KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://localhost:9092
      KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1
    depends_on:
      - zookeeper
```

### 2. Start the Kafka and Zookeeper Containers

Run the following command to start the containers:

```bash
docker-compose up -d
```

This command will download the required images (if not already available) and start the Zookeeper and Kafka services in detached mode.

### 3. Verify the Setup

To check if the containers are running, use:

```bash
docker ps
```

You should see both the `zookeeper` and `kafka` containers listed as running.

### 4. Access Kafka

You can now interact with Kafka using a Kafka client. Here are some common commands:

#### List Topics
```bash
docker exec kafka kafka-topics --list --bootstrap-server localhost:9092
```

#### Create a Topic
```bash
docker exec kafka kafka-topics --create \
  --topic test-topic \
  --bootstrap-server localhost:9092 \
  --partitions 1 \
  --replication-factor 1
```

#### Send Messages to a Topic
```bash
docker exec -it kafka kafka-console-producer \
  --topic test-topic \
  --bootstrap-server localhost:9092
```
Type your messages, pressing `Enter` after each message. Use `Ctrl+C` to exit.

#### Consume Messages from a Topic
```bash
docker exec -it kafka kafka-console-consumer \
  --topic test-topic \
  --bootstrap-server localhost:9092 \
  --from-beginning
```

### 5. Stop the Containers

To stop and remove the containers, use:

```bash
docker-compose down
```

## Notes

- The configuration in this guide is suitable for local development and testing. For production setups, consider additional configurations for security, performance, and scalability.
- Use [Docker volumes](https://docs.docker.com/storage/volumes/) to persist Kafka data between container restarts.

For further details, visit the [Kafka Documentation](https://kafka.apache.org/documentation/).
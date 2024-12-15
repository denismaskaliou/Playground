# RabbitMQ in Docker

This guide provides simple steps to run RabbitMQ in a Docker container and access its management interface.

---

## Usage

Follow the steps below to set up and use RabbitMQ in Docker.

### Running the RabbitMQ Container

Run the following command to start a RabbitMQ container with the management interface enabled:

```bash
docker run -d --name rabbitmq \
  -p 5672:5672 \
  -p 15672:15672 \
  rabbitmq:management

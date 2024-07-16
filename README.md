# RPSSL
Web Api service for rock-paper-scissors-spock-lizzard game


## Requirements

- .NET 8 SDK (for local debugging)
- PostgreSQL
- Docker

## Installation

1. **Pull PostgreSQL Image:**

   Open your Docker terminal and pull the PostgreSQL image with the following command:

   ```sh
   docker pull postgres

2. **Modify the Connection String:**

   If you are not using the Dockerfile provided and prefer to run PostgreSQL and the .NET application separately, replace the connection string in your application settings with your own.

   The connection string should look like this:
   Host=localhost;Port=5432;Username=admin;Password=SuperSecret2024!;Database=rpssl.gameservice
   
## Usage

To run the application using Docker, follow these steps:

1. **Build the Docker image**:

docker build -t rpssl_image .

2. **Run the Docker container:**

docker run -d -p 8080:8080 -p 8081:8081 -p 5432:5432 rpssl_image 

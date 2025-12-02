# Java Spring Boot Application

A Spring Boot REST API demonstrating CI/CD patterns with Java and Maven.

## Features

- Spring Boot 3.2.1
- Java 17
- Maven build system
- JUnit 5 tests with MockMvc
- JaCoCo code coverage
- Checkstyle for code quality
- Spring Boot Actuator for health checks
- Book management REST API

## Prerequisites

- Java 17 or higher
- Maven 3.8 or higher

## Building

```bash
# Clean and build
mvn clean install

# Build without tests
mvn clean install -DskipTests

# Package as JAR
mvn package
```

## Running

```bash
# Run with Maven
mvn spring-boot:run

# Run the JAR
java -jar target/java-app-1.0.0.jar
```

## Testing

```bash
# Run all tests
mvn test

# Run tests with coverage
mvn test jacoco:report

# View coverage report
open target/site/jacoco/index.html
```

## Code Quality

```bash
# Run Checkstyle
mvn checkstyle:check

# Run all quality checks
mvn verify
```

## API Endpoints

- `GET /actuator/health` - Health check endpoint
- `GET /api/books` - Get all books
- `GET /api/books/{id}` - Get book by ID
- `POST /api/books` - Create a new book
- `DELETE /api/books/{id}` - Delete a book

## Example Requests

### Get all books
```bash
curl http://localhost:8080/api/books
```

### Get a specific book
```bash
curl http://localhost:8080/api/books/1
```

### Create a book
```bash
curl -X POST http://localhost:8080/api/books \
  -H "Content-Type: application/json" \
  -d '{"title":"New Book","author":"John Doe","year":2024}'
```

### Delete a book
```bash
curl -X DELETE http://localhost:8080/api/books/1
```

## Configuration

Application configuration is in `src/main/resources/application.properties`:

- Server port: 8080
- Health endpoint: `/actuator/health`

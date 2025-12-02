# Python Flask Microservice

A simple Flask microservice demonstrating CI/CD patterns with Python.

## Features

- Flask web framework
- RESTful API for product management
- pytest for testing with coverage
- flake8 and pylint for linting
- black for code formatting
- Health check endpoint

## Prerequisites

- Python 3.11 or higher
- pip

## Installation

```bash
# Create virtual environment
python -m venv venv

# Activate virtual environment
# On macOS/Linux:
source venv/bin/activate
# On Windows:
# venv\Scripts\activate

# Install dependencies
pip install -r requirements.txt
pip install -r requirements-dev.txt
```

## Development

```bash
# Run the application
python -m flask --app app run

# Or using gunicorn
gunicorn app:app

# Run tests
pytest

# Run tests with coverage
pytest --cov=app --cov-report=html

# Lint code
flake8 app tests
pylint app tests

# Format code
black app tests
```

## API Endpoints

- `GET /health` - Health check endpoint
- `GET /api/products` - Get all products
- `GET /api/products/<id>` - Get product by ID
- `POST /api/products` - Create a new product

## Testing

Run the test suite:

```bash
pytest
```

With coverage report:

```bash
pytest --cov=app --cov-report=html
```

Coverage reports are generated in the `htmlcov/` directory.

## Building

Build a wheel distribution:

```bash
pip install build
python -m build
```

## Environment Variables

- `PORT` - Server port (default: 5000)
- `DEBUG` - Enable debug mode (default: False)

## Running in Production

```bash
gunicorn app:app --bind 0.0.0.0:5000 --workers 4
```

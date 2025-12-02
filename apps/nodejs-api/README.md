# Node.js/TypeScript API

A simple Express.js REST API built with TypeScript demonstrating CI/CD patterns.

## Features

- TypeScript for type safety
- Express.js for API framework
- Jest for testing with coverage
- ESLint for code quality
- User management endpoints

## Prerequisites

- Node.js 20.x or higher
- npm 9.x or higher

## Installation

```bash
npm install
```

## Development

```bash
# Run in development mode
npm run dev

# Build the project
npm run build

# Run tests
npm test

# Run tests in watch mode
npm run test:watch

# Lint code
npm run lint

# Fix linting issues
npm run lint:fix
```

## API Endpoints

- `GET /health` - Health check endpoint
- `GET /api/users` - Get all users
- `GET /api/users/:id` - Get user by ID
- `POST /api/users` - Create a new user

## Testing

The project includes comprehensive unit tests with Jest. Run tests with:

```bash
npm test
```

Coverage reports are generated in the `coverage/` directory.

## Building

Build the TypeScript code:

```bash
npm run build
```

The compiled JavaScript will be in the `dist/` directory.

## Running in Production

```bash
npm run build
npm start
```

import express, { Request, Response } from 'express';
import { config } from 'dotenv';

// Load environment variables
config();

const app = express();
const port = process.env.PORT || 3000;

// Middleware
app.use(express.json());

// Health check endpoint
app.get('/health', (_req: Request, res: Response) => {
  res.status(200).json({
    status: 'healthy',
    timestamp: new Date().toISOString(),
    uptime: process.uptime(),
    version: process.env.npm_package_version || '1.0.0'
  });
});

// API routes
app.get('/api/users', (_req: Request, res: Response) => {
  res.json({
    users: [
      { id: 1, name: 'Alice Johnson', email: 'alice@example.com' },
      { id: 2, name: 'Bob Smith', email: 'bob@example.com' },
      { id: 3, name: 'Charlie Brown', email: 'charlie@example.com' }
    ]
  });
});

app.get('/api/users/:id', (req: Request, res: Response) => {
  const userId = parseInt(req.params.id);
  const users = [
    { id: 1, name: 'Alice Johnson', email: 'alice@example.com' },
    { id: 2, name: 'Bob Smith', email: 'bob@example.com' },
    { id: 3, name: 'Charlie Brown', email: 'charlie@example.com' }
  ];
  
  const user = users.find(u => u.id === userId);
  
  if (user) {
    res.json(user);
  } else {
    res.status(404).json({ error: 'User not found' });
  }
});

app.post('/api/users', (req: Request, res: Response) => {
  const { name, email } = req.body;
  
  if (!name || !email) {
    res.status(400).json({ error: 'Name and email are required' });
    return;
  }
  
  const newUser = {
    id: Date.now(),
    name,
    email
  };
  
  res.status(201).json(newUser);
});

// 404 handler
app.use((_req: Request, res: Response) => {
  res.status(404).json({ error: 'Not found' });
});

// Start server only if not in test mode
if (process.env.NODE_ENV !== 'test') {
  app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
  });
}

export default app;

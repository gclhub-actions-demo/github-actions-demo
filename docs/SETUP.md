# Setup Guide

This guide walks you through setting up the GitHub Actions demo repository, including prerequisites, local development environment, and GitHub configuration.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Local Development Setup](#local-development-setup)
- [GitHub Configuration](#github-configuration)
- [Testing Workflows Locally](#testing-workflows-locally)
- [Troubleshooting](#troubleshooting)

## Prerequisites

### Required Tools

#### For All Stacks

- **Git** 2.30+
  ```bash
  git --version
  ```

- **GitHub CLI** (optional but recommended)
  ```bash
  # Install
  brew install gh  # macOS
  # or download from https://cli.github.com/
  
  # Authenticate
  gh auth login
  ```

#### For Node.js Development

- **Node.js** 20.x LTS
  ```bash
  # Install via nvm (recommended)
  curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.0/install.sh | bash
  nvm install 20
  nvm use 20
  
  # Verify
  node --version  # Should be v20.x.x
  npm --version   # Should be 10.x.x
  ```

#### For Python Development

- **Python** 3.11+
  ```bash
  # macOS with Homebrew
  brew install python@3.11
  
  # Verify
  python3 --version  # Should be 3.11.x
  pip3 --version
  ```

- **virtualenv** (recommended)
  ```bash
  pip3 install virtualenv
  ```

#### For Java Development

- **Java JDK** 17
  ```bash
  # macOS with Homebrew
  brew install openjdk@17
  
  # Add to PATH (add to ~/.zshrc or ~/.bash_profile)
  export PATH="/opt/homebrew/opt/openjdk@17/bin:$PATH"
  
  # Verify
  java -version   # Should be 17.x.x
  javac -version
  ```

- **Maven** 3.9+
  ```bash
  brew install maven
  mvn --version
  ```

#### For .NET Development

- **.NET SDK** 8.0
  ```bash
  # Download from https://dotnet.microsoft.com/download
  # Or use Homebrew
  brew install --cask dotnet-sdk
  
  # Verify
  dotnet --version  # Should be 8.0.x
  ```

### Optional Tools

- **act** - Run GitHub Actions locally
  ```bash
  brew install act
  # or
  curl https://raw.githubusercontent.com/nektos/act/master/install.sh | sudo bash
  ```

- **Docker** - Required for act
  ```bash
  # Download from https://docker.com
  # Or use Homebrew
  brew install --cask docker
  ```

## Local Development Setup

### 1. Clone the Repository

```bash
# Clone via HTTPS
git clone https://github.com/your-org/github-actions-demo.git

# Or via SSH
git clone git@github.com:your-org/github-actions-demo.git

cd github-actions-demo
```

### 2. Set Up Each Application

#### Node.js API

```bash
cd apps/nodejs-api

# Install dependencies
npm install

# Run linting
npm run lint

# Build TypeScript
npm run build

# Run tests
npm test

# Run tests with coverage
npm run test:coverage

# Start development server
npm run dev
```

**Verify Setup**:
```bash
# Should compile without errors
npm run build

# Should pass all tests
npm test

# Should start server on port 3000
npm run dev
# Open http://localhost:3000/health
```

#### Python Service

```bash
cd apps/python-service

# Create virtual environment
python3 -m venv venv

# Activate virtual environment
source venv/bin/activate  # macOS/Linux
# Or on Windows:
# venv\Scripts\activate

# Install dependencies
pip install -r requirements.txt

# Run linting
flake8 src tests
pylint src tests
black --check src tests

# Run tests
pytest

# Run tests with coverage
pytest --cov=src tests/

# Start development server
flask run
```

**Verify Setup**:
```bash
# Should pass linting
flake8 src tests

# Should pass all tests
pytest

# Should start server on port 5000
flask run
# Open http://localhost:5000/health
```

#### Java Application

```bash
cd apps/java-app

# Clean and compile
mvn clean compile

# Run linting
mvn checkstyle:check

# Run tests
mvn test

# Run tests with coverage
mvn jacoco:prepare-agent test jacoco:report

# Package application
mvn package

# Run application
mvn spring-boot:run
```

**Verify Setup**:
```bash
# Should compile without errors
mvn compile

# Should pass checkstyle
mvn checkstyle:check

# Should pass all tests
mvn test

# Should start server on port 8080
mvn spring-boot:run
# Open http://localhost:8080/health
```

#### .NET API

```bash
cd apps/dotnet-api

# Restore dependencies
dotnet restore

# Check formatting
dotnet format --verify-no-changes

# Build
dotnet build

# Run tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run application
dotnet run --project DotnetApi/DotnetApi.csproj
```

**Verify Setup**:
```bash
# Should restore successfully
dotnet restore

# Should build without errors
dotnet build

# Should pass all tests
dotnet test

# Should start server on port 5000
dotnet run --project DotnetApi/DotnetApi.csproj
# Open http://localhost:5000/health
```

## GitHub Configuration

### 1. Repository Settings

#### Enable GitHub Actions

1. Go to your repository on GitHub
2. Navigate to **Settings** → **Actions** → **General**
3. Under "Actions permissions", select:
   - ✅ "Allow all actions and reusable workflows"
   - Or configure based on [MARKETPLACE_RESTRICTIONS.md](MARKETPLACE_RESTRICTIONS.md)

#### Configure Branch Protection

1. Go to **Settings** → **Branches**
2. Add rule for `main` branch:
   - ✅ Require a pull request before merging
   - ✅ Require status checks to pass before merging
   - Add required status checks:
     - `nodejs-lint`
     - `nodejs-build`
     - `nodejs-test`
     - (Repeat for other stacks)
   - ✅ Require conversation resolution before merging
   - ✅ Do not allow bypassing the above settings

### 2. Secrets Configuration

#### Repository Secrets

1. Go to **Settings** → **Secrets and variables** → **Actions**
2. Click **New repository secret**
3. Add the following secrets:

```plaintext
# Example secrets (replace with your actual values)

NPM_TOKEN              # For publishing to npm
PYPI_TOKEN             # For publishing to PyPI
MAVEN_GPG_PASSPHRASE   # For signing Maven artifacts
NUGET_API_KEY          # For publishing to NuGet

# Cloud credentials (if needed)
AWS_ACCESS_KEY_ID
AWS_SECRET_ACCESS_KEY
AZURE_CREDENTIALS
GCP_SERVICE_ACCOUNT_KEY

# Third-party services
SLACK_WEBHOOK_URL      # For notifications
CODECOV_TOKEN          # For coverage reporting
SONAR_TOKEN            # For SonarCloud
```

#### How to Add Secrets

```bash
# Using GitHub CLI
gh secret set NPM_TOKEN --body "your-token-here"

# Or manually through UI:
# Settings → Secrets and variables → Actions → New repository secret
```

### 3. Variables Configuration

#### Repository Variables

1. Go to **Settings** → **Secrets and variables** → **Actions** → **Variables**
2. Click **New repository variable**
3. Add the following variables:

```plaintext
# Build configuration
NODE_VERSION=20
PYTHON_VERSION=3.11
JAVA_VERSION=17
DOTNET_VERSION=8.0

# Feature flags
ENABLE_COVERAGE_REPORTS=true
ENABLE_SECURITY_SCANNING=true
ENABLE_PERFORMANCE_TESTS=false

# Deployment
DEV_URL=https://dev.example.com
STAGING_URL=https://staging.example.com
PROD_URL=https://example.com
```

### 4. Environment Configuration

Create three environments with different protection rules:

#### Development Environment

1. Go to **Settings** → **Environments**
2. Click **New environment**, name it `development`
3. Configuration:
   - ❌ No required reviewers
   - ❌ No wait timer
   - Deployment branches: `Any branch`

4. Add environment secrets:
   ```plaintext
   DB_CONNECTION_STRING=dev-database-url
   API_KEY=dev-api-key
   ```

#### Staging Environment

1. Create environment named `staging`
2. Configuration:
   - ✅ Required reviewers: 1 reviewer
   - ❌ No wait timer
   - Deployment branches: `Selected branches` → `main`, `develop`

3. Add environment secrets:
   ```plaintext
   DB_CONNECTION_STRING=staging-database-url
   API_KEY=staging-api-key
   ```

#### Production Environment

1. Create environment named `production`
2. Configuration:
   - ✅ Required reviewers: 2 reviewers
   - ✅ Wait timer: 5 minutes
   - Deployment branches: `Selected branches` → `main` only
   - ✅ Prevent self-review

3. Add environment secrets:
   ```plaintext
   DB_CONNECTION_STRING=prod-database-url
   API_KEY=prod-api-key
   ```

**Diagram**:
```
Development         Staging            Production
-----------        ---------          ------------
Auto-deploy   →    1 Approval   →    2 Approvals
No wait            No wait           5 min wait
Any branch         main/develop      main only
```

### 5. Organization-Level Configuration

#### Required Workflows (Organization Rulesets)

See [REQUIRED_WORKFLOWS.md](REQUIRED_WORKFLOWS.md) for detailed instructions.

**Quick Setup**:
1. Go to **Organization** → **Settings** → **Repository rulesets**
2. Click **New ruleset** → **New repository ruleset**
3. Configure required workflows:
   - Add workflow: `.github/workflows/security-scan.yml`
   - Add workflow: `.github/workflows/pr-validation.yml`

#### Marketplace Action Restrictions

See [MARKETPLACE_RESTRICTIONS.md](MARKETPLACE_RESTRICTIONS.md) for detailed guidance.

**Quick Setup**:
1. Go to **Organization** → **Settings** → **Actions** → **General**
2. Under "Policies", select "Allow select actions and reusable workflows"
3. Add allowed actions:
   ```plaintext
   actions/checkout@*
   actions/setup-node@*
   actions/setup-python@*
   actions/setup-java@*
   actions/setup-dotnet@*
   actions/cache@*
   actions/upload-artifact@*
   actions/download-artifact@*
   github/codeql-action/*
   ```

## Testing Workflows Locally

### Using `act`

[act](https://github.com/nektos/act) allows you to test GitHub Actions locally using Docker.

#### Installation

```bash
# macOS
brew install act

# Linux
curl https://raw.githubusercontent.com/nektos/act/master/install.sh | sudo bash

# Windows (with Chocolatey)
choco install act-cli
```

#### Basic Usage

```bash
# List available workflows
act -l

# Run default event (push)
act

# Run specific workflow
act -W .github/workflows/nodejs-ci.yml

# Run specific job
act -W .github/workflows/nodejs-ci.yml -j nodejs-build

# Run with specific event
act pull_request

# Run with custom event payload
act -e .github/workflows/test-event.json
```

#### Passing Secrets to act

Create `.secrets` file (add to `.gitignore`):
```plaintext
NPM_TOKEN=your-token
GITHUB_TOKEN=your-github-token
```

Run with secrets:
```bash
act --secret-file .secrets
```

#### Common Issues with act

1. **Large Docker images**: First run downloads ~10GB images
   ```bash
   # Use smaller image
   act -P ubuntu-latest=catthehacker/ubuntu:act-latest
   ```

2. **Missing GitHub context**: Some actions need GitHub token
   ```bash
   act --secret GITHUB_TOKEN=your_token
   ```

3. **Unsupported features**: Not all Actions features work locally
   - Environments (partially supported)
   - OIDC token requests
   - Some GitHub API calls

### Testing Individual Components

#### Test Composite Actions

```bash
# Create test workflow
cat > .github/workflows/test-composite.yml << 'EOF'
name: Test Composite Action
on: workflow_dispatch

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: ./.github/actions/setup-build-env
        with:
          stack: nodejs
          node-version: '20'
      - run: node --version
EOF

# Run with act
act workflow_dispatch -W .github/workflows/test-composite.yml
```

#### Test Reusable Workflows

Create a caller workflow:
```yaml
name: Test Reusable
on: workflow_dispatch

jobs:
  test:
    uses: ./.github/workflows/reusable/security-scan.yml
    with:
      stack: nodejs
      app-path: apps/nodejs-api
```

### Validating Workflow Syntax

```bash
# Install actionlint
brew install actionlint

# Lint all workflows
actionlint .github/workflows/*.yml

# Lint specific workflow
actionlint .github/workflows/nodejs-ci.yml
```

## Troubleshooting

### Common Issues

#### Issue: "Workflow file is invalid"

**Symptoms**: Workflow doesn't appear in Actions tab

**Solutions**:
1. Check YAML syntax:
   ```bash
   # Use yamllint
   brew install yamllint
   yamllint .github/workflows/nodejs-ci.yml
   ```

2. Validate with actionlint:
   ```bash
   actionlint .github/workflows/nodejs-ci.yml
   ```

3. Check for tabs (use spaces only):
   ```bash
   grep -P '\t' .github/workflows/*.yml
   ```

#### Issue: "Unable to resolve action"

**Symptoms**: "Can't find 'action.yml', 'action.yaml' or 'Dockerfile'"

**Solutions**:
1. Check action path is correct:
   ```yaml
   # Correct
   - uses: ./.github/actions/setup-build-env
   
   # Wrong (missing ./)
   - uses: .github/actions/setup-build-env
   ```

2. Ensure `action.yml` exists in the action directory

3. For external actions, verify they exist on GitHub

#### Issue: "Unable to find reusable workflow"

**Symptoms**: Reusable workflow reference shows error

**Solutions**:
1. Check path is correct:
   ```yaml
   # Correct
   uses: ./.github/workflows/reusable/security-scan.yml
   
   # Wrong
   uses: .github/workflows/reusable/security-scan.yml
   ```

2. Ensure workflow has `workflow_call` trigger:
   ```yaml
   on:
     workflow_call:
       inputs:
         # ...
   ```

3. Check that workflow is on the same branch

#### Issue: Secrets not available in workflow

**Symptoms**: Workflow fails with authentication errors

**Solutions**:
1. Check secret name matches exactly (case-sensitive):
   ```yaml
   # Correct
   ${{ secrets.NPM_TOKEN }}
   
   # Wrong
   ${{ secrets.npm_token }}
   ```

2. Verify secret is set in correct scope:
   - Repository secrets available to all workflows
   - Environment secrets only in environment jobs

3. Check branch protection doesn't block secret access

#### Issue: Path filters not triggering workflow

**Symptoms**: Workflow doesn't run when expected files change

**Solutions**:
1. Verify paths are correct:
   ```yaml
   paths:
     - 'apps/nodejs-api/**'  # Correct
     - 'apps/nodejs-api/*'   # Only matches direct children
   ```

2. Check if path is in `.gitignore` (ignored files don't trigger)

3. Test path patterns:
   ```bash
   # List files matching pattern
   git ls-files 'apps/nodejs-api/**'
   ```

#### Issue: Matrix builds failing inconsistently

**Symptoms**: Some matrix jobs pass, others fail

**Solutions**:
1. Check for race conditions in parallel jobs

2. Verify matrix variables are used correctly:
   ```yaml
   strategy:
     matrix:
       node-version: [18, 20, 22]
   steps:
     - uses: actions/setup-node@v4
       with:
         node-version: ${{ matrix.node-version }}
   ```

3. Use `fail-fast: false` to see all failures:
   ```yaml
   strategy:
     fail-fast: false
     matrix:
       # ...
   ```

### Getting Help

1. **Check workflow run logs**:
   - Go to Actions tab
   - Click on failed workflow
   - Expand failing step
   - Check "View raw logs"

2. **Enable debug logging**:
   - Add secret `ACTIONS_STEP_DEBUG` with value `true`
   - Re-run workflow
   - See detailed logs

3. **Use job summaries**:
   ```yaml
   - name: Debug info
     run: |
       echo "### Debug Information" >> $GITHUB_STEP_SUMMARY
       echo "Runner OS: ${{ runner.os }}" >> $GITHUB_STEP_SUMMARY
       echo "Event: ${{ github.event_name }}" >> $GITHUB_STEP_SUMMARY
   ```

4. **Check GitHub Status**:
   - Visit https://www.githubstatus.com/
   - Verify Actions service is operational

### Additional Resources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Workflow Syntax Reference](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions)
- [GitHub Community Forum](https://github.community/)
- [act Documentation](https://github.com/nektos/act)

---

**Next Steps**: After completing setup, see [WORKFLOWS.md](WORKFLOWS.md) for detailed workflow documentation.

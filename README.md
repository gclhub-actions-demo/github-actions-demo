# GitHub Actions Multi-Stack Demo

A comprehensive demonstration repository showcasing GitHub Actions best practices for multi-technology CI/CD pipelines, featuring Node.js, Python, Java, and .NET applications.

## 🎯 Purpose

This repository demonstrates:

- **Multi-technology stack support** with both monolithic and separate workflow patterns
- **GitHub Actions secrets, variables, and environments** with protection rules
- **Reusable workflows and composite actions** for DRY principles
- **Required workflows in organization rulesets** for governance
- **Marketplace actions restriction policies** for security
- **CI best practices** for testing, linting, and artifact creation

> **Note**: This demo focuses on **CI (Continuous Integration)** including testing and artifact creation. CD (Continuous Deployment) is not covered.

## 📋 Table of Contents

- [Repository Structure](#repository-structure)
- [Technology Stacks](#technology-stacks)
- [Workflow Strategies](#workflow-strategies)
- [Quick Start](#quick-start)
- [Documentation](#documentation)
- [Key Features](#key-features)

## 🗂️ Repository Structure

```
github-actions-demo/
├── apps/                          # Sample applications
│   ├── nodejs-api/               # Node.js/TypeScript REST API
│   ├── python-service/           # Python/Flask microservice
│   ├── java-app/                 # Java/Spring Boot application
│   └── dotnet-api/               # .NET/ASP.NET Core Web API
├── .github/
│   ├── actions/                  # Composite actions (reusable steps)
│   │   ├── setup-build-env/     # Multi-stack environment setup
│   │   ├── quality-checks/      # Unified linting
│   │   └── package-artifacts/   # Standardized artifact packaging
│   └── workflows/
│       ├── nodejs-ci.yml        # Node.js-specific CI
│       ├── python-ci.yml        # Python-specific CI
│       ├── java-ci.yml          # Java-specific CI
│       ├── dotnet-ci.yml        # .NET-specific CI
│       ├── monolithic-ci.yml    # All stacks in one workflow
│       ├── environment-demo.yml # Environment progression demo
│       ├── pr-validation.yml    # PR quality checks
│       ├── release.yml          # Release automation
│       ├── dependency-update.yml # Dependency checking
│       └── reusable/            # Reusable workflows
│           ├── security-scan.yml
│           ├── build-test.yml
│           └── release-validation.yml
└── docs/                         # Documentation
    ├── REQUIRED_WORKFLOWS.md     # Organization rulesets guide
    ├── MARKETPLACE_RESTRICTIONS.md # Action security policies
    ├── SETUP.md                  # Getting started guide
    ├── WORKFLOWS.md              # Workflow deep dive
    ├── SECURITY.md               # Security considerations
    └── ARCHITECTURE.md           # Design decisions
```

## 🛠️ Technology Stacks

### Node.js API (`apps/nodejs-api`)

- **Runtime**: Node.js 20.x
- **Language**: TypeScript
- **Framework**: Express.js
- **Testing**: Jest with coverage
- **Linting**: ESLint
- **Build**: TypeScript compiler
- **Package Manager**: npm

**Key Features**:
- REST API with health check endpoint
- User management endpoints
- TypeScript strict mode
- Comprehensive test coverage

### Python Service (`apps/python-service`)

- **Runtime**: Python 3.11
- **Framework**: Flask
- **Testing**: pytest with pytest-cov
- **Linting**: flake8, pylint, black
- **Build**: python -m build
- **Package Manager**: pip

**Key Features**:
- Flask microservice
- Product management API
- Type hints with mypy
- Code formatting with black

### Java Application (`apps/java-app`)

- **Runtime**: Java 17
- **Framework**: Spring Boot 3.2.1
- **Testing**: JUnit 5 with JaCoCo
- **Linting**: Checkstyle
- **Build Tool**: Maven 3.9+
- **Package Manager**: Maven Central

**Key Features**:
- Spring Boot REST API
- Book management endpoints
- Spring Boot Actuator
- Comprehensive MockMvc tests

### .NET API (`apps/dotnet-api`)

- **Runtime**: .NET 8.0
- **Framework**: ASP.NET Core
- **Testing**: xUnit with coverlet
- **Linting**: dotnet format
- **Build Tool**: dotnet CLI
- **Package Manager**: NuGet

**Key Features**:
- Minimal API pattern
- Task management endpoints
- Swagger/OpenAPI documentation
- Integration testing with WebApplicationFactory

## 🔄 Workflow Strategies

This repository demonstrates two primary workflow patterns:

### Strategy 1: Stack-Specific Workflows (Recommended for Most Cases)

**Files**: `nodejs-ci.yml`, `python-ci.yml`, `java-ci.yml`, `dotnet-ci.yml`

**Approach**: Separate workflow file for each technology stack

**Pros**:
- ✅ Clear separation of concerns
- ✅ Independent execution (faster for single-stack changes)
- ✅ Technology-specific optimizations
- ✅ Easier to understand and maintain
- ✅ Path filters prevent unnecessary runs

**Cons**:
- ❌ More workflow files to manage
- ❌ Potential duplication across files
- ❌ Changes needed in multiple places

**Use When**:
- Teams are organized by technology
- Different release cycles per stack
- Stack-specific requirements differ significantly
- Want maximum CI speed

**Example**:
```yaml
# .github/workflows/nodejs-ci.yml
on:
  push:
    paths:
      - 'apps/nodejs-api/**'
      - '.github/workflows/nodejs-ci.yml'
      
jobs:
  nodejs-build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: ./.github/actions/setup-build-env
        with:
          stack: nodejs
```

### Strategy 2: Monolithic Multi-Stack (Good for Uniformity)

**File**: `monolithic-ci.yml`

**Approach**: Single workflow with matrix strategy for all stacks

**Pros**:
- ✅ Single source of truth
- ✅ Consistent patterns across all stacks
- ✅ Easy to add new stacks (just update matrix)
- ✅ Centralized changes

**Cons**:
- ❌ Runs all stacks even if only one changed (without path filters)
- ❌ More complex workflow file
- ❌ Harder to customize per stack
- ❌ All-or-nothing approach

**Use When**:
- Need absolute consistency across stacks
- Small repository with infrequent commits
- Unified release process for all stacks
- Simplified governance/compliance needs

**Example**:
```yaml
# .github/workflows/monolithic-ci.yml
jobs:
  build-and-test:
    strategy:
      matrix:
        stack:
          - name: nodejs
            path: apps/nodejs-api
          - name: python
            path: apps/python-service
          - name: java
            path: apps/java-app
          - name: dotnet
            path: apps/dotnet-api
```

### Which Strategy Should You Choose?

| Criteria | Stack-Specific | Monolithic |
|----------|---------------|------------|
| **Team Structure** | Teams by technology | Cross-functional teams |
| **Release Cadence** | Different per stack | Unified releases |
| **Repository Size** | Large monorepo | Small/medium repo |
| **CI Speed Priority** | High (independent runs) | Medium (all run together) |
| **Maintenance** | More files, less duplication | Fewer files, more complexity |
| **Recommendation** | ✅ **Most cases** | Specific uniformity needs |

## 🚀 Quick Start

### Prerequisites

- Git
- Node.js 20.x (for Node.js app)
- Python 3.11 (for Python app)
- Java 17 (for Java app)
- .NET 8.0 SDK (for .NET app)
- GitHub account with Actions enabled

### Local Development

#### 1. Clone the Repository

```bash
git clone https://github.com/your-org/github-actions-demo.git
cd github-actions-demo
```

#### 2. Choose a Stack to Work With

**Node.js**:
```bash
cd apps/nodejs-api
npm install
npm run lint
npm run build
npm test
```

**Python**:
```bash
cd apps/python-service
python -m venv venv
source venv/bin/activate  # On Windows: venv\Scripts\activate
pip install -r requirements.txt
pytest
```

**Java**:
```bash
cd apps/java-app
mvn clean install
mvn test
```

**.NET**:
```bash
cd apps/dotnet-api
dotnet restore
dotnet build
dotnet test
```

### Testing Workflows Locally

Use [act](https://github.com/nektos/act) to test workflows locally:

```bash
# Install act
brew install act  # macOS
# or
curl https://raw.githubusercontent.com/nektos/act/master/install.sh | sudo bash

# Run a workflow
act push -W .github/workflows/nodejs-ci.yml

# Test with specific event
act pull_request -W .github/workflows/pr-validation.yml
```

### Enabling in Your Organization

See [docs/SETUP.md](docs/SETUP.md) for detailed setup instructions including:
- Configuring secrets and variables
- Setting up environments with protection rules
- Enabling required workflows
- Configuring marketplace restrictions

## 📚 Documentation

- **[SETUP.md](docs/SETUP.md)** - Prerequisites, local development setup, configuration
- **[WORKFLOWS.md](docs/WORKFLOWS.md)** - Detailed workflow documentation and usage
- **[REQUIRED_WORKFLOWS.md](docs/REQUIRED_WORKFLOWS.md)** - Organization rulesets and required workflows
- **[MARKETPLACE_RESTRICTIONS.md](docs/MARKETPLACE_RESTRICTIONS.md)** - Action security policies
- **[SECURITY.md](docs/SECURITY.md)** - Security best practices and guidelines
- **[ARCHITECTURE.md](docs/ARCHITECTURE.md)** - Architectural decisions and rationale

## ✨ Key Features

### 1. Composite Actions

Reusable steps packaged as local actions:

- **`setup-build-env`**: Multi-stack environment setup with intelligent caching
- **`quality-checks`**: Unified linting across all stacks
- **`package-artifacts`**: Standardized artifact packaging with manifests

**Benefits**: DRY principle, consistent behavior, easier testing

### 2. Reusable Workflows

Job-level reuse with callable workflows:

- **`security-scan.yml`**: Dependency scanning and vulnerability detection
- **`build-test.yml`**: Parameterized build and test execution
- **`release-validation.yml`**: Pre-release quality gates

**Benefits**: Centralized logic, cross-repository sharing, inputs/outputs

### 3. Environment Management

Progressive deployment with protection rules:

```
Development → Staging → Production
    ↓            ↓          ↓
  Auto       Approval   Approval +
             Required   Time Delay
```

**Features**:
- Environment-specific secrets
- Manual approval gates
- Wait timers for soak testing
- Deployment protection rules

### 4. Advanced Workflow Patterns

**PR Validation**:
- Title format checking
- Description completeness
- PR size analysis
- Auto-labeling
- Automated comments

**Release Automation**:
- Semantic version validation
- Changelog generation
- Cross-stack coordination
- Artifact verification

**Dependency Management**:
- Scheduled dependency checks
- Security vulnerability scanning
- Automated update PRs

### 5. Path-Based Triggers

Optimize CI by running only affected workflows:

```yaml
on:
  push:
    paths:
      - 'apps/nodejs-api/**'
      - '.github/workflows/nodejs-ci.yml'
      - '.github/actions/**'
```

**Benefits**: Faster CI, reduced costs, better resource usage

### 6. Matrix Strategies

Test across multiple dimensions:

```yaml
strategy:
  matrix:
    node-version: [18, 20, 22]
    os: [ubuntu-latest, windows-latest, macos-latest]
```

**Benefits**: Comprehensive testing, parallel execution, version validation

## 🔐 Security Features

- **Secret scanning**: Automated secret detection
- **Dependency scanning**: Known vulnerability detection (Dependabot)
- **Action pinning**: SHA-based action references
- **Marketplace restrictions**: Controlled third-party action usage
- **CODEOWNERS**: Required reviews for sensitive files
- **Branch protection**: Required status checks before merge

## 📊 Workflow Status

<!-- Add your workflow status badges here -->

```markdown
[![Node.js CI](https://github.com/your-org/github-actions-demo/workflows/Node.js%20CI/badge.svg)](https://github.com/your-org/github-actions-demo/actions/workflows/nodejs-ci.yml)
[![Python CI](https://github.com/your-org/github-actions-demo/workflows/Python%20CI/badge.svg)](https://github.com/your-org/github-actions-demo/actions/workflows/python-ci.yml)
[![Java CI](https://github.com/your-org/github-actions-demo/workflows/Java%20CI/badge.svg)](https://github.com/your-org/github-actions-demo/actions/workflows/java-ci.yml)
[![.NET CI](https://github.com/your-org/github-actions-demo/workflows/.NET%20CI/badge.svg)](https://github.com/your-org/github-actions-demo/actions/workflows/dotnet-ci.yml)
```

## 🤝 Contributing

This is a demo repository, but contributions to improve examples are welcome:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/improvement`)
3. Make your changes
4. Run relevant tests
5. Commit your changes (`git commit -am 'Add improvement'`)
6. Push to the branch (`git push origin feature/improvement`)
7. Create a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙋 FAQ

### Q: Can I use this in production?

A: This is a demo repository. You should adapt patterns to your specific needs, security requirements, and scale.

### Q: Which workflow strategy should I choose?

A: For most cases, **stack-specific workflows** are recommended. They provide better CI speed and clearer separation. Use the monolithic approach only if you need absolute uniformity across all stacks.

### Q: How do I add a new technology stack?

A: 
1. Create app in `apps/<stack-name>/`
2. Create workflow in `.github/workflows/<stack-name>-ci.yml`
3. Update composite actions if needed (they auto-detect stack)
4. Update monolithic workflow matrix if using it
5. Document in README

### Q: What about CD (Continuous Deployment)?

A: This demo focuses on CI. For CD examples, see GitHub's official documentation on deployment environments and the `environment-demo.yml` workflow for basic patterns.

### Q: How do I handle secrets in local testing?

A: Use a `.env` file (gitignored) or `act` secrets file. See [docs/SETUP.md](docs/SETUP.md) for details.

## 📞 Support

For issues or questions:
- Open a [GitHub Issue](https://github.com/your-org/github-actions-demo/issues)
- Check [GitHub Actions Documentation](https://docs.github.com/actions)
- Review [docs/](docs/) for detailed guides

## 🎓 Learning Resources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Workflow Syntax Reference](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions)
- [GitHub Actions Marketplace](https://github.com/marketplace?type=actions)
- [Awesome GitHub Actions](https://github.com/sdras/awesome-actions)

---

**Built with ❤️ to demonstrate GitHub Actions best practices**

# Required Workflows in Organization Rulesets

This document explains how to set up and use required workflows at the organization level using GitHub's repository rulesets feature.

## Table of Contents

- [Overview](#overview)
- [What are Required Workflows?](#what-are-required-workflows)
- [Benefits](#benefits)
- [Setup Instructions](#setup-instructions)
- [Example Required Workflows](#example-required-workflows)
- [Best Practices](#best-practices)

## Overview

Required workflows allow organizations to enforce specific GitHub Actions workflows across multiple repositories. These workflows run automatically and cannot be bypassed or disabled at the repository level, ensuring consistent security, compliance, and quality standards.

## What are Required Workflows?

Required workflows are GitHub Actions workflows that:

1. **Run automatically** on specified events (e.g., pull requests, pushes)
2. **Cannot be disabled** by repository maintainers
3. **Must pass** before code can be merged (when configured as required status checks)
4. **Are centrally managed** at the organization level
5. **Apply to multiple repositories** based on ruleset targeting

## Benefits

### Security
- **Mandatory security scans**: Ensure all code is scanned for vulnerabilities
- **Secret detection**: Prevent secrets from being committed
- **Dependency scanning**: Identify vulnerable dependencies

### Compliance
- **License checking**: Verify license compliance
- **Code signing**: Enforce commit signature verification
- **Audit trails**: Maintain compliance documentation

### Quality
- **Consistent standards**: Apply the same quality checks across all repos
- **Required documentation**: Ensure README, LICENSE, and other files exist
- **Branch protection**: Enforce naming conventions and protection rules

## Setup Instructions

### Step 1: Create a Workflows Repository

First, create a dedicated repository to store your required workflows:

\`\`\`bash
# Example repository structure
organization-workflows/
├── .github/
│   └── workflows/
│       ├── required-security.yml
│       ├── required-compliance.yml
│       └── required-quality.yml
└── README.md
\`\`\`

### Step 2: Create Required Workflow Files

Create your required workflow files in the `.github/workflows/` directory.

**Example: `required-security.yml`**

\`\`\`yaml
name: Required Security Scan

on:
  pull_request:
    branches: [main, master, develop]
  push:
    branches: [main, master, develop]

permissions:
  contents: read
  security-events: write

jobs:
  security-scan:
    name: Security Scanning
    runs-on: ubuntu-latest
    
    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Run CodeQL Analysis
        uses: github/codeql-action/init@v3
        with:
          languages: javascript, python, java, csharp

      - name: Perform CodeQL Analysis
        uses: github/codeql-action/analyze@v3

      - name: Secret Scanning
        run: |
          echo "Scanning for secrets..."
          # Add secret scanning logic here

      - name: Dependency Check
        run: |
          echo "Checking dependencies for vulnerabilities..."
          # Add dependency scanning logic here
\`\`\`

### Step 3: Configure Organization Rulesets

1. **Navigate to Organization Settings**
   - Go to your organization on GitHub
   - Click "Settings" in the top navigation
   - Select "Rulesets" under "Code, planning, and automation"

2. **Create New Ruleset**
   - Click "New ruleset" → "New branch ruleset"
   - Give it a descriptive name (e.g., "Required Security Workflows")

3. **Configure Target Repositories**
   - Choose "All repositories" or
   - Select specific repositories with patterns (e.g., `*-api`, `frontend-*`)

4. **Add Branch Targeting**
   - Target: `main`, `master`, `develop`, or use patterns

5. **Add Required Workflows**
   - Scroll to "Required workflows" section
   - Click "Add workflow"
   - Select your workflows repository
   - Choose the workflow file (e.g., `.github/workflows/required-security.yml`)
   - Repeat for each required workflow

6. **Set Enforcement Level**
   - **Active**: Workflows are enforced immediately
   - **Evaluate**: Test mode - logs violations without blocking

7. **Save Ruleset**
   - Review your configuration
   - Click "Create" to activate the ruleset

### Step 4: Configure as Required Status Checks

To prevent merging without passing required workflows:

1. Go to repository "Settings" → "Branches"
2. Edit branch protection rule for your target branch
3. Enable "Require status checks to pass before merging"
4. Search for and select the required workflow job names
5. Save changes

## Example Required Workflows

### 1. Security Baseline Workflow

**File**: `.github/workflows/required-security.yml`

**Purpose**: Mandatory security checks for all repositories

**Features**:
- CodeQL static analysis
- Secret scanning
- Dependency vulnerability scanning
- License compliance checks

**When it runs**:
- On all pull requests
- On pushes to main/master branches

**Cannot be bypassed by**:
- Repository administrators
- Branch protection bypasses
- Force pushes

### 2. Compliance and Audit Workflow

**File**: `.github/workflows/required-compliance.yml`

**Purpose**: Enforce organizational compliance policies

**Features**:
- Commit signature verification
- Required file checks (LICENSE, SECURITY.md, CODE_OF_CONDUCT.md)
- PR description template validation
- Branch naming convention enforcement

**When it runs**:
- On all pull requests
- On tag creation

### 3. Quality Gates Workflow

**File**: `.github/workflows/required-quality.yml`

**Purpose**: Maintain code quality standards

**Features**:
- Linting checks
- Test coverage validation
- Documentation completeness
- Breaking change detection

**When it runs**:
- On all pull requests
- Before release creation

## Best Practices

### Workflow Design

1. **Keep workflows focused**
   - Each workflow should have a single, clear purpose
   - Avoid combining unrelated checks in one workflow

2. **Make workflows fast**
   - Use caching for dependencies
   - Run jobs in parallel when possible
   - Set reasonable timeouts (15-30 minutes max)

3. **Provide clear feedback**
   - Use descriptive job and step names
   - Include helpful error messages
   - Generate job summaries for results

4. **Handle failures gracefully**
   - Use `continue-on-error` for non-critical checks
   - Provide remediation guidance in failure messages

### Repository Targeting

1. **Start small**
   - Begin with a few pilot repositories
   - Use "Evaluate" mode initially
   - Gather feedback before rolling out broadly

2. **Use meaningful patterns**
   - Group similar repositories
   - Example: `*-api` for all API repositories
   - Example: `frontend-*` for all frontend projects

3. **Document exceptions**
   - Maintain a list of repositories exempt from certain rules
   - Document the business justification
   - Review exceptions quarterly

### Versioning and Updates

1. **Pin action versions**
   - Use SHA commits instead of tags for security
   - Example: `actions/checkout@8e5e7e5ab8b370d6c329ec480221332ada57f0ab`

2. **Test changes first**
   - Create a test repository
   - Verify workflow changes don't break existing repos
   - Use pull requests in the workflows repository

3. **Communicate changes**
   - Announce upcoming changes to developers
   - Provide migration guides if needed
   - Give advance notice (1-2 weeks minimum)

### Monitoring and Maintenance

1. **Track workflow performance**
   - Monitor execution times
   - Watch for failure patterns
   - Optimize slow workflows

2. **Review failures regularly**
   - Investigate recurring failures
   - Update workflows based on feedback
   - Fix broken checks promptly

3. **Keep workflows updated**
   - Update actions to latest versions quarterly
   - Review and update security scans
   - Add new checks as needed

## Common Use Cases

### Use Case 1: Enforce Security Scanning

**Scenario**: Require all code to pass security scans before merge

**Implementation**:
- Create required security workflow
- Apply to all repositories
- Include CodeQL, dependency scanning, secret detection
- Make workflow job a required status check

### Use Case 2: Ensure Documentation Standards

**Scenario**: Every repository must have required documentation files

**Implementation**:
- Create required compliance workflow
- Check for README.md, LICENSE, SECURITY.md, CONTRIBUTING.md
- Validate documentation completeness
- Fail if required files are missing or empty

### Use Case 3: Validate Pull Request Quality

**Scenario**: PRs must meet quality standards before review

**Implementation**:
- Create required quality workflow
- Check PR title/description format
- Validate code changes meet linting standards
- Ensure tests are included for new features
- Verify documentation updates accompany code changes

## Troubleshooting

### Workflow Not Running

**Problem**: Required workflow doesn't trigger

**Solutions**:
- Verify ruleset is set to "Active" not "Evaluate"
- Check repository matches targeting pattern
- Ensure branch matches targeting rules
- Review workflow trigger conditions

### Blocking Legitimate Merges

**Problem**: Valid PRs are blocked by required workflows

**Solutions**:
- Review workflow logs for specific failures
- Update workflow to handle edge cases
- Temporarily switch to "Evaluate" mode if needed
- Document and communicate any manual bypass process

### Performance Issues

**Problem**: Workflows take too long to run

**Solutions**:
- Implement dependency caching
- Run independent jobs in parallel
- Reduce unnecessary steps
- Set appropriate timeouts
- Consider splitting into multiple smaller workflows

## Resources

- [GitHub Docs: About required workflows](https://docs.github.com/en/actions/using-workflows/required-workflows)
- [GitHub Docs: Repository rulesets](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/about-rulesets)
- [GitHub Actions: Security hardening](https://docs.github.com/en/actions/security-guides/security-hardening-for-github-actions)

## Example Organization Setup

\`\`\`
organization: acme-corp
│
├── repositories/
│   ├── acme-workflows/           # Required workflows repository
│   │   └── .github/workflows/
│   │       ├── required-security.yml
│   │       ├── required-compliance.yml
│   │       └── required-quality.yml
│   │
│   ├── product-api/               # Subject to required workflows
│   ├── web-frontend/              # Subject to required workflows
│   └── mobile-app/                # Subject to required workflows
│
└── rulesets/
    ├── "Security Requirements"    # Applies to all repos
    ├── "API Standards"            # Applies to *-api repos
    └── "Frontend Standards"       # Applies to *-frontend repos
\`\`\`

## Conclusion

Required workflows are a powerful tool for maintaining security, compliance, and quality standards across an organization. By following these guidelines and best practices, you can ensure consistent enforcement of organizational policies while maintaining developer productivity.

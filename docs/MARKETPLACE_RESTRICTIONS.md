# GitHub Actions Marketplace Restrictions

This document explains how to configure and manage GitHub Actions marketplace restrictions to control which third-party actions can be used in your organization's workflows.

## Table of Contents

- [Overview](#overview)
- [Why Restrict Marketplace Actions?](#why-restrict-marketplace-actions)
- [Policy Tiers](#policy-tiers)
- [Setup Instructions](#setup-instructions)
- [Best Practices](#best-practices)
- [Migration Guide](#migration-guide)

## Overview

GitHub Actions marketplace restrictions allow organizations to control which actions from the GitHub Marketplace can be used in workflows. This is a critical security control that helps prevent supply chain attacks and ensures only trusted actions are executed in your CI/CD pipelines.

## Why Restrict Marketplace Actions?

### Security Risks of Unrestricted Actions

1. **Supply Chain Attacks**
   - Malicious code in third-party actions
   - Compromised action repositories
   - Backdoors in popular actions

2. **Secret Exposure**
   - Actions can access workflow secrets
   - Potential for secret exfiltration
   - Unintended credential leakage

3. **Code Execution**
   - Actions run with repository permissions
   - Can modify code and commits
   - May access organization resources

4. **Compliance Violations**
   - Unknown data processing
   - Unclear licensing terms
   - Regulatory compliance issues

### Benefits of Restrictions

- **Reduced attack surface**: Limit potential entry points
- **Controlled dependencies**: Know exactly what code runs
- **Audit trail**: Track which actions are approved
- **Compliance**: Meet regulatory requirements
- **Stability**: Prevent breaking changes from upstream actions

## Policy Tiers

GitHub provides four levels of marketplace action restrictions, from most permissive to most restrictive:

### Tier 1: Allow All Actions ⚠️ NOT RECOMMENDED

**Configuration**: All actions are allowed

**Risk Level**: 🔴 High

**When to Use**:
- Proof-of-concept projects
- Temporary development environments
- Very small teams with high trust

**Pros**:
- Maximum flexibility
- Quick prototyping
- Easy onboarding

**Cons**:
- Highest security risk
- No visibility into action usage
- Potential compliance violations
- Supply chain attack exposure

**Example Use Case**:
\`\`\`yaml
# No restrictions - any action can be used
on: [push]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: random-user/untrusted-action@v1
      - uses: another-user/unknown-action@main
\`\`\`

### Tier 2: Allow Verified Creators ⚠️ MODERATE RISK

**Configuration**: Only GitHub-verified creators allowed

**Risk Level**: 🟡 Medium

**When to Use**:
- Growing teams
- Balance between security and flexibility
- Organizations building marketplace action expertise

**Pros**:
- More security than allowing all
- Still offers good flexibility
- Verified creators undergo some vetting
- Many popular actions included

**Cons**:
- Verification doesn't guarantee security
- Can still have vulnerabilities
- No version control enforcement
- Limited audit capability

**Who is Verified?**:
- GitHub (official actions)
- Microsoft
- Actions organization
- Other verified publishers

**Example Workflow**:
\`\`\`yaml
on: [push]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      # ✅ Allowed - GitHub is verified
      - uses: actions/checkout@v4
      
      # ✅ Allowed - actions org is verified
      - uses: actions/setup-node@v4
      
      # ❌ Blocked - unverified creator
      - uses: some-user/custom-action@v1
\`\`\`

### Tier 3: Allow Specific Actions ✅ RECOMMENDED

**Configuration**: Explicit allowlist of approved actions

**Risk Level**: 🟢 Low

**When to Use**:
- Enterprise environments
- Security-conscious organizations
- Regulated industries
- Production workloads

**Pros**:
- Maximum control over dependencies
- Clear audit trail
- Can enforce version pinning
- Known security posture
- Compliance friendly

**Cons**:
- Requires ongoing maintenance
- Initial setup effort
- Developer friction (requires approvals)
- May slow adoption of new tools

**Example Allowlist**:
\`\`\`
# Organization Settings > Actions > General > Policies

Allowed actions and reusable workflows:
✅ Allow actions created by GitHub
✅ Allow actions by Marketplace verified creators
✅ Allow specified actions and reusable workflows:

actions/checkout@*
actions/setup-node@*
actions/setup-python@*
actions/setup-java@*
actions/setup-dotnet@*
actions/cache@*
actions/upload-artifact@*
actions/download-artifact@*
actions/github-script@*
github/codeql-action/*
docker/login-action@*
docker/build-push-action@*
azure/login@*
azure/webapps-deploy@*
hashicorp/setup-terraform@*
\`\`\`

**Example Workflow**:
\`\`\`yaml
on: [push]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      # ✅ Allowed - on allowlist
      - uses: actions/checkout@v4
      
      # ✅ Allowed - wildcard match
      - uses: actions/setup-node@v3
      
      # ❌ Blocked - not on allowlist
      - uses: some-org/unapproved-action@v1
\`\`\`

### Tier 4: Allow Local Actions Only 🔒 MAXIMUM SECURITY

**Configuration**: Only actions within organization/repository

**Risk Level**: 🟢 Very Low

**When to Use**:
- Maximum security requirements
- Air-gapped environments
- Highly regulated industries
- Zero-trust architectures

**Pros**:
- Complete control over code
- No external dependencies
- Maximum security
- Perfect audit trail
- No supply chain risk

**Cons**:
- Requires building everything in-house
- High maintenance burden
- Slow feature development
- Reinventing the wheel
- Large engineering investment

**Example Workflow**:
\`\`\`yaml
on: [push]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      # ✅ Allowed - local composite action
      - uses: ./.github/actions/setup-environment
      
      # ✅ Allowed - action in same org
      - uses: myorg/shared-actions/.github/actions/build@main
      
      # ❌ Blocked - external action (even GitHub's)
      - uses: actions/checkout@v4
\`\`\`

## Setup Instructions

### Organization-Level Configuration

1. **Navigate to Organization Settings**
   \`\`\`
   GitHub.com → Your Organization → Settings → Actions → General
   \`\`\`

2. **Scroll to "Policies"**
   - Find "Fork pull request workflows from outside collaborators"
   - And "Permissions" section

3. **Configure "Allowed actions and reusable workflows"**
   
   **Option A: Allow all actions** (Not recommended)
   - Select "Allow all actions and reusable workflows"

   **Option B: Allow verified creators**
   - Select "Allow GitHub-owned actions and actions from verified creators"

   **Option C: Allow specific actions** (Recommended)
   - Select "Allow select actions and reusable workflows"
   - Add your allowlist (see examples below)

4. **Save Changes**

### Repository-Level Configuration

Organizations can also allow individual repositories to set their own policies:

1. **Enable repository override**
   - In organization settings, check "Allow repository administrators to configure additional permissions"

2. **Configure at repository level**
   - Go to Repository → Settings → Actions → General
   - Configure allowed actions (cannot be more permissive than org level)

### Creating an Allowlist

#### Wildcard Patterns

\`\`\`
# Allow all versions of specific actions
actions/checkout@*
actions/setup-node@*

# Allow all actions from an organization
docker/*
azure/*

# Allow specific action with specific version
actions/checkout@v4
actions/checkout@8e5e7e5ab8b370d6c329ec480221332ada57f0ab
\`\`\`

#### Recommended Starter Allowlist

\`\`\`
# GitHub Core Actions
actions/checkout@*
actions/cache@*
actions/upload-artifact@*
actions/download-artifact@*
actions/github-script@*

# Language Setup
actions/setup-node@*
actions/setup-python@*
actions/setup-java@*
actions/setup-dotnet@*
actions/setup-go@*

# Security
github/codeql-action/*
github/super-linter@*

# Container/Cloud (if needed)
docker/login-action@*
docker/build-push-action@*
azure/login@*
aws-actions/configure-aws-credentials@*

# Testing/Quality
codecov/codecov-action@*
\`\`\`

## Best Practices

### 1. Pin Actions to SHA Commits

**Why?**: Tags and branches can be changed maliciously

**Bad**:
\`\`\`yaml
- uses: actions/checkout@v4          # Tag can be moved
- uses: actions/checkout@main        # Branch can be updated
\`\`\`

**Good**:
\`\`\`yaml
- uses: actions/checkout@8e5e7e5ab8b370d6c329ec480221332ada57f0ab  # SHA
\`\`\`

**Hybrid Approach** (readable + secure):
\`\`\`yaml
# v4.1.1
- uses: actions/checkout@8e5e7e5ab8b370d6c329ec480221332ada57f0ab
\`\`\`

### 2. Establish an Action Approval Process

Create a formal process for adding actions to the allowlist:

1. **Request**: Developer submits action for approval
2. **Security Review**: Security team evaluates the action
   - Check action source code
   - Review permissions required
   - Assess maintainer reputation
   - Verify license compatibility
3. **Documentation**: Document why action was approved
4. **Add to Allowlist**: Update organization settings
5. **Communicate**: Notify teams of new approved action

### 3. Regular Allowlist Review

- **Quarterly reviews**: Check if actions are still needed
- **Version updates**: Ensure using latest secure versions
- **Deprecation**: Remove unused actions
- **Audit**: Review action usage across repositories

### 4. Create Internal Action Library

Build a repository of approved, tested composite actions:

\`\`\`
organization/shared-actions/
├── .github/actions/
│   ├── build-node/
│   │   └── action.yml
│   ├── build-python/
│   │   └── action.yml
│   ├── security-scan/
│   │   └── action.yml
│   └── deploy/
│       └── action.yml
\`\`\`

Benefits:
- Centralized maintenance
- Consistent behavior
- Easier to secure
- Reduced dependencies

### 5. Use Dependabot for Action Updates

Enable Dependabot to keep actions up to date:

\`\`\`.github/dependabot.yml
version: 2
updates:
  - package-ecosystem: "github-actions"
    directory: "/"
    schedule:
      interval: "weekly"
    labels:
      - "dependencies"
      - "github-actions"
\`\`\`

### 6. Monitor Action Usage

Track which actions are used across your organization:

\`\`\`bash
# Script to audit action usage
gh api --paginate \\
  '/orgs/YOUR_ORG/repos' \\
  --jq '.[] | .name' | while read repo; do
    echo "Checking $repo..."
    gh api "/repos/YOUR_ORG/$repo/actions/workflows" \\
      --jq '.workflows[] | .path'
  done
\`\`\`

## Migration Guide

### Transitioning from Allow All to Allowlist

#### Phase 1: Discovery (Week 1-2)

1. **Audit current usage**
   \`\`\`bash
   # Find all actions used in workflows
   find .github/workflows -name "*.yml" -o -name "*.yaml" | \\
     xargs grep "uses:" | \\
     sed 's/.*uses: //' | \\
     sort -u > current-actions.txt
   \`\`\`

2. **Categorize actions**
   - Essential (required for builds)
   - Nice-to-have (convenience)
   - Questionable (need review)

#### Phase 2: Initial Allowlist (Week 3)

1. **Create baseline allowlist**
   - Include all GitHub official actions
   - Add essential third-party actions
   - Document decisions

2. **Set to Evaluate mode** (if available)
   - Test allowlist without blocking
   - Identify missing actions

#### Phase 3: Enforcement (Week 4)

1. **Enable allowlist**
   - Activate restrictions
   - Monitor for issues

2. **Provide support**
   - Create documentation
   - Establish approval process
   - Support developer questions

#### Phase 4: Optimization (Ongoing)

1. **Replace with local actions**
   - Build composite actions for common patterns
   - Reduce third-party dependencies

2. **Regular reviews**
   - Quarterly allowlist audit
   - Remove unused actions
   - Update to latest versions

### Example Migration Timeline

\`\`\`
Week 1-2: Discovery & Analysis
  ├── Audit all workflows
  ├── List all used actions
  ├── Categorize by criticality
  └── Security review of current actions

Week 3: Build Allowlist
  ├── Create initial allowlist
  ├── Document approved actions
  ├── Set up approval process
  └── Communicate to teams

Week 4: Enable Restrictions
  ├── Activate allowlist policy
  ├── Monitor for issues
  ├── Quick-approve blocking cases
  └── Collect feedback

Week 5+: Optimize
  ├── Build internal actions library
  ├── Reduce external dependencies
  ├── Quarterly reviews
  └── Continuous improvement
\`\`\`

## Common Patterns

### Pattern 1: Core GitHub Actions Only

**Use Case**: Maximum trust, minimal dependencies

\`\`\`
actions/checkout@*
actions/cache@*
actions/upload-artifact@*
actions/download-artifact@*
actions/setup-node@*
actions/setup-python@*
actions/setup-java@*
github/codeql-action/*
\`\`\`

### Pattern 2: Cloud Deployment

**Use Case**: AWS/Azure deployments

\`\`\`
# Core
actions/checkout@*
actions/cache@*

# AWS
aws-actions/configure-aws-credentials@*
aws-actions/amazon-ecr-login@*

# Azure
azure/login@*
azure/webapps-deploy@*
azure/functions-action@*

# Docker
docker/login-action@*
docker/build-push-action@*
\`\`\`

### Pattern 3: Security-First

**Use Case**: High security requirements

\`\`\`
# GitHub official only
actions/*
github/*

# Pre-approved, pinned to SHA
# v4.1.1
actions/checkout@8e5e7e5ab8b370d6c329ec480221332ada57f0ab

# Internal actions only
myorg/shared-actions/*
\`\`\`

## Troubleshooting

### Action Blocked - What to Do?

1. **Check if truly needed**
   - Can you use an approved action instead?
   - Can you use a local composite action?

2. **Submit approval request**
   - Document why action is needed
   - Link to action source code
   - Provide security justification

3. **Temporary alternatives**
   - Use bash/script steps instead
   - Create local composite action
   - Use approved action with manual steps

### Keeping Workflows Running During Transition

Use feature flags in workflows:

\`\`\`yaml
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      
      # Fallback for blocked action
      - name: Setup (approved action)
        if: vars.USE_APPROVED_ACTIONS == 'true'
        uses: ./.github/actions/local-setup
      
      - name: Setup (legacy)
        if: vars.USE_APPROVED_ACTIONS != 'true'
        uses: third-party/blocked-action@v1
\`\`\`

## Resources

- [GitHub Docs: Managing GitHub Actions settings for a repository](https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/enabling-features-for-your-repository/managing-github-actions-settings-for-a-repository)
- [GitHub Docs: Allowing select actions to run](https://docs.github.com/en/organizations/managing-organization-settings/disabling-or-limiting-github-actions-for-your-organization#allowing-select-actions-to-run)
- [Security hardening for GitHub Actions](https://docs.github.com/en/actions/security-guides/security-hardening-for-github-actions)

## Conclusion

Implementing GitHub Actions marketplace restrictions is a critical security practice for organizations. Start with the recommended allowlist approach, build your internal action library, and continuously refine your policies based on security requirements and developer needs.

Remember: **Security and developer experience must be balanced**. Work with your teams to find the right level of restriction that maintains security while enabling productivity.

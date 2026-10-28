# Centrally Required PR Workflows

The [required-workflows repository](https://github.com/gclhub-actions-demo/required-workflows)
is the proposed source of mandatory PR checks for the demo application repositories.
An organization branch ruleset will require the central workflow for PRs targeting
`main`. Repository-local workflow files and starter templates are not the enforcement
mechanism.

**Rollout status:** the integration is proposed through unmerged PRs. Publishing
these files does not activate an organization ruleset. An authorized organization
administrator must review and approve the central revision and settings before
enforcement is enabled.

## Repository roles

| Repository | Role |
|---|---|
| `github-actions-demo` | Four-stack application monorepo; governed |
| `java-app-demo` | Java application; governed |
| `python-app` | Python application; governed |
| `required-workflows` | Centrally owned workflow, policy, validators, and rollout configuration |
| `.github` | Starter workflows and organization metadata; not an application |

The central repository must be public to serve these public application repositories.
The initial central CODEOWNER is `@gclhub`. Only authorized central maintainers should
have write access. App repository administrators must not receive write access to
central policy merely because they administer an app.

## What is required

The central policy assigns this repository four projects:

| Project ID | Path | Mandatory profile |
|---|---|---|
| `java` | `apps/java-app` | Java 17, Maven, Checkstyle, build/tests, JaCoCo reports |
| `node` | `apps/nodejs-api` | Node.js, npm lockfile install, ESLint, TypeScript, Jest reports |
| `python` | `apps/python-service` | Python 3.11, dependency install, flake8, pytest reports, package build |
| `dotnet` | `apps/dotnet-api` | .NET 8, restore, format, Release build, tests and coverage |

All four configured projects run on every applicable PR, including documentation-only
changes. Removing a manifest or changing the app configuration cannot silently remove
a project from central policy. Missing projects, malformed configuration, and missing
or empty required test evidence fail.

The Java application's existing package-level 70% line-coverage minimum remains in
effect. The Node application retains its 70% minimum for branches, functions, lines,
and statements. No new Python or .NET coverage percentage is introduced by this
integration. Reports must describe actual execution; a fixed coverage number is not
evidence.

## Application configuration

[`.github/app-ci.json`](../.github/app-ci.json) declares the expected projects and
optional additive extensions:

```json
{
  "version": 1,
  "projects": [
    {"id": "java", "path": "apps/java-app"},
    {"id": "node", "path": "apps/nodejs-api"},
    {"id": "python", "path": "apps/python-service"},
    {"id": "dotnet", "path": "apps/dotnet-api"}
  ],
  "extensions": {
    "preflight": ".github/ci/preflight.sh",
    "postflight": ".github/ci/postflight.sh"
  }
}
```

The central schema rejects unknown properties. Project IDs and paths must match
central policy; the application cannot choose weaker profiles, override test commands,
change runners, lower coverage requirements, or disable required stages.

## Developer extensions

Developers may edit the optional repository-relative shell scripts. This repository
demonstrates documentation checks in [preflight](../.github/ci/preflight.sh) and
tracked-file hygiene in [postflight](../.github/ci/postflight.sh).

Each phase runs on a separate fresh hosted runner under central orchestration.
Preflight runs before the mandatory baseline; postflight runs after it passes.
Extensions receive no organization secrets, deployment credentials, or write-capable
token. They do not share their modified checkout, caches, or executables with the
mandatory jobs. A declared script that is missing, fails, or times out blocks the gate.
An undeclared optional phase is not required.

This is deliberately not arbitrary same-runner step injection. A hook cannot replace
the mandatory commands or modify their environment. If an application needs generated
inputs in its mandatory build, request a centrally designed capability rather than
expecting a preflight workspace to be transferred.

Try the examples locally from the repository root:

```bash
bash .github/ci/preflight.sh
bash .github/ci/postflight.sh
```

## Organization enforcement

The proposed organization ruleset:

- Targets only the three application repositories and `refs/heads/main`.
- Requires PRs and the identified workflow from the central repository.
- Has an empty bypass list, including no repository-admin or automation bypass.
- Blocks force pushes and deletion of `main`.

The workflow runs on PR opening, new commits, and reopening. GitHub ignores workflow
event filters such as `branches`, `paths`, and `types` for ruleset workflows; **the
ruleset** provides the branch scope. Local `push`, `workflow_dispatch`, or
similarly named check results are not substitutes for the required central workflow.

Required checks must all succeed. The central final gate explicitly considers failed,
cancelled, skipped, and missing mandatory results rather than reporting success just
because a summary job ran. Changing or deleting a local workflow cannot satisfy the
central workflow requirement.

Organization owners and authorized central governance administrators remain trusted:
they can change the governing rules or central source. This is not a promise that
organization owners cannot change policy. Similarly, a central build can execute
mandatory commands but cannot prove that application-owned tests and build definitions
are honest; code review remains necessary.

## Staged rollout and maintenance

1. Review the central framework PR and application integration PRs. Leave them unmerged
   until the designated human reviewer is ready.
2. Select an approved immutable central commit; the initial central owner is `@gclhub`.
   Protect central `main` with required review; CODEOWNERS alone is not protection.
3. Have an appropriately authorized administrator inspect existing organization rules
   and Actions policies, then review the proposed settings from the central repository.
4. After the required code is merged, explicitly approve Evaluate-mode deployment.
   Exercise real PRs, then separately approve Active enforcement.

Do not silently remove protections or add an app-admin bypass to fix a broken pipeline.
Fix central code through a PR. Any emergency policy change requires an authorized
administrator and an auditable decision.

New rules do not automatically run on PRs that were already open. Push a commit,
update the branch, or close and reopen the PR. Missing/cancelled runs must not be
treated as passing. Merge queues are not enabled by this rollout; if introduced,
the required workflow must handle `merge_group` before queue enforcement is activated.

## Existing demo workflows

The local stack-specific, reusable, security, release, and environment workflows remain
examples or optional CI. They are not the central enforcement source. In particular,
the older reusable build example's configurable `test-command` and illustrative
coverage output must not be used as mandatory evidence.

CodeQL `init` and `analyze` actions cannot run inside a ruleset workflow, including
through nested reusable workflows. CodeQL enforcement would instead use a separately
configured code-scanning setup and its merge-protection rule. It is not activated here.

## References

- [Available rules for rulesets](https://docs.github.com/en/enterprise-cloud@latest/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/available-rules-for-rulesets)
- [Troubleshooting ruleset workflows](https://docs.github.com/en/enterprise-cloud@latest/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/troubleshooting-rules#troubleshooting-ruleset-workflows)
- [Central framework and rollout instructions](https://github.com/gclhub-actions-demo/required-workflows)
- [Organization starter workflows](https://github.com/gclhub-actions-demo/.github/tree/main/workflow-templates)

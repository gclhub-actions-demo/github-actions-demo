#!/usr/bin/env bash
set -euo pipefail

for project in apps/java-app apps/nodejs-api apps/python-service apps/dotnet-api; do
  if [[ ! -s "$project/README.md" ]]; then
    printf 'Missing application documentation: %s/README.md\n' "$project" >&2
    exit 1
  fi
done
printf 'Application documentation preflight passed.\n'

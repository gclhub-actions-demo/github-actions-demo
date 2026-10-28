#!/usr/bin/env bash
set -euo pipefail

tracked_binaries=$(git ls-files -- '*.pyc' '*.class' '*.dll')
if [[ -n "$tracked_binaries" ]]; then
  printf 'Generated binaries must not be tracked in the application repository.\n' >&2
  exit 1
fi
printf 'Tracked-file hygiene postflight passed.\n'

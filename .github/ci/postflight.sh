#!/usr/bin/env bash
set -euo pipefail

if git ls-files -- '*.pyc' '*.class' '*.dll' | grep -q .; then
  printf 'Generated binaries must not be tracked in the application repository.\n' >&2
  exit 1
fi
printf 'Tracked-file hygiene postflight passed.\n'

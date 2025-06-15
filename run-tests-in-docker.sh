#!/bin/bash
set -e

echo "##########################################"
echo "Running unit tests"
echo "##########################################"
docker compose -f compose-unit-tests.yml build
docker compose -f compose-unit-tests.yml up \
  --force-recreate \
  --remove-orphans \
  --abort-on-container-exit \
  --exit-code-from unit-tests

echo "##########################################"
echo "Running integration tests"
echo "##########################################"
docker compose -f compose-integration-tests.yml build
docker compose -f compose-integration-tests.yml -f compose-infra.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from integration-tests

echo "##########################################"
echo "Running acceptance tests"
echo "##########################################"
docker compose -f compose-acceptance-tests.yml build
docker compose -f compose-acceptance-tests.yml -f compose-web.yml -f compose-infra.yml up \
  --force-recreate \
  --remove-orphans \
  --no-log-prefix \
  --abort-on-container-exit \
  --exit-code-from acceptance-tests

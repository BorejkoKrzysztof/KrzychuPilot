#!/bin/sh
set -e

DB_HOST="${DB_HOST:-krzychupilot-db}"
DB_PORT="${DB_PORT:-1433}"

echo "Waiting for database at $DB_HOST:$DB_PORT..."
while ! nc -z "$DB_HOST" "$DB_PORT"; do
  echo "Database is unavailable - sleeping"
  sleep 2
done

echo "Database is up - starting API"
exec dotnet KrzychuPilot.API.dll

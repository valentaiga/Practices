# Fluent Migration

## Goals
- [x] Create a small and easy-to-run project for db migrations


## Project setup (for your needs)
1. Add nuget packages like in project (`FluentMigrator.Runner.Postgres` can be replaced with similar packages)
2. Configure fluent migration services to DI (see method `MigrationRunner.CreateServices`)
3. Project can be lib/console application depending on your needs:
  However, console application is easier to run with CI on your project updates 
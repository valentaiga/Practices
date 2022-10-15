# PostgreSQL Repo

## Goals
- [x] Get repo with entities to work with

## Environment setup
If you need a local database follow instruction below:
Pull&Run PostgreSQL image in docker: `docker run --name postgres-db -e POSTGRES_PASSWORD=admin -p 5432:5432 -d postgres`
    
## Project setup
1. Add lib to project
2. Configure services to DI using `ConfigurePostgres(settings => settings.ConnectionString = "something"")`
3. Use `PosgresRepository.cs` for your needs
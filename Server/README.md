# MTCG

## :package: Requirements

PostgreSQL Database

Tested with `Postgres 12` under `WSL2`.

> Note: A connection configuration needs to be added under `Server/Resources/dbConfig.json` (a template can be found in the `Resources` folder). If no configuration is found, default values will be used.

## 🛠 Build 

```
dotnet build --configuration Release
```

## 🚴‍♂️Run

Run MTCG server (port defaults to `8080`)

```
dotnet run --project ./Server/Server.csproj
```

## 🧪 Test

Run unit tests

```
dotnet test
```

For the integration CURL and/or [Postman](https://www.postman.com/) is needed.

The curl-batch scripts can be found under `MTCG-Test/Integration/curl`.

The test collections for Postman can be found under `MTCG-Test/Integration/postman`.

For both cases two integration test-suites can be found, one for the basic requirements and one that tests bonus features.

> Note: After every integration test run, the database needs to be dropped:
>
> ```sql
> DROP DATABASE mtcg;
> ```




# SWE2-Tour-Planner

## :package: Requirements

* PostgreSQL Database     
  Tested with `Postgres 12` under `WSL2`.     

* Filled out configuration     
  The file `config.template.json` is a template file for the configuration. In order to take effect rename the file to `config.json`.         
  Following parameters can be tweaked:

  | Name                      | Function                                                     |
  | ------------------------- | ------------------------------------------------------------ |
  | `client:base-url`         | URL of the server which the client communicates to           |
  | `client:logger-config`    | Filename of the client logging configuration, leave untouched if not sure |
  | `server:port`             | Port on which the server listens to clients                  |
  | `server:db`               | Postgres database config consisting of the fields `user`, `password`, `ip` and `port` |
  | `server:logger-config`    | Filename of the server logging configuration, leave untouched if not sure |
  | `server:mapquest-api-key` | Your private key to the MapQuest API, find more about [here](https://developer.mapquest.com/plan_purchase/steps/business_edition/business_edition_free/register) |
  | `server:routes-path`      | Path in which the route information images are stored        |
  | `server:exports-path`     | Path in which the tour export pdfs are stored                |

## 🛠 Build 

```
dotnet build --configuration Release
```

Pre-build artefacts can be found [here](https://github.com/kurbaniec/SWE2-Tour-Planner/releases)

## 🚀 Run

Run Tour-Planner server

```
dotnet run --project .\Server\Server\Server.csproj
```

Run Tour-Planner server

```
dotnet run --project .\Client\Client\Client.csproj
```

## 🧪 Test

Run unit tests

```
dotnet test
```

---

## 🧾 Protocol 

### App Architecture

layers etc

### Used Libraries



### Implemented Design Patterns

* MVVM
* Mediator Pattern

### Unit Test Design



### Unique Feature



### Bonus Feature

REST-server

### Time Tracking

Estimated time spent: ~ 65 - 75 h

### Link to git

https://github.com/kurbaniec/SWE2-Tour-Planner

### Encountered Problems

#### Responsive Design

WrapPanel with two Grids (that stretch to infinite)

Force Wrapping via `WidthConverter`

#### Scrolling

Logs are Listviews with an internal Scrollviewer by default. This Scrollviewer can be removed.

See: https://stackoverflow.com/a/11451793/12347616

Also DataGrids have a scroller...

#### DataGrid

Bind one item to DataGrid? Solution: ItemSourceConverter, returns a plain list!.


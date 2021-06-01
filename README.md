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

Tour-Planner is based on a Client-Server architecture where the server does all the heavy lifting like data management and persistence and the client is a simple thin-client that communicates with the server through REST-calls. This makes it possible to add or swap out client applications without touching the core business rules in the server.

Both server and client feature internally a layered architecture:

#### Server

* Services Layer     
  The server does not have a view or anything similar. Its utmost functionality is to provide access to the offered tour-planner services. This is done through REST-endpoints which are defined internally in the `Controllers` package in the `TourController` class. Each valid request is passed to the Business Layer and returned via a corresponding HTTP-Response. 
* Business Layer       
  The Business Layer implements the core functionality and encapsulates all relevant logic.  It is defined in the `BL` package and consists of the `TourPlannerServer` class which performs the functionality through mostly internal calls to the Data Access Layer.
* Data Access Layer      
  The server has three interfaces in the `DAL` package for different data access methods:
  * `IDataManagement`    
    Interface used for CRUD operations for the Tour models. Concrete implementation is the `PostgresDb` class.
  * `IMapApi`     
    Interface used to interact with a Map API. Concrete implementation is the `MapQuestApi` class.
  * `IExportHanlder`    
    Interface that describes a way to create a printable document. Concrete implementation is the `PdfExportHandler` class.

#### Client

* Presentation Layer    
  In comparison to the server the client has a typical GUI which the user can interact with. The Views are  defined in the `Views` package, corresponding ViewModels are found in the `ViewModels` package. Events from the Views typically trigger a Command in the ViewModels which calls the Business Layer when core functionality is affected.

* Business Layer    
  The Business Layer gives clients access to core functionality which in the current implementation means that the client asks the server to perform it and return the response back to the client. It is defined in the `Logic.BL` package in the `TourPlannerClient` class.

* Data Access Layer       
  The client has three interfaces in the `Logic.DAL` package for different data access methods:

  * `ITourApi`    
    Interface that describes methods to interact with the server. Concrete implementation is the `TourApi` class.
  * `IImportExportHandler`     
    Interfaces that defines serialization & deserialization of Tours with files. Concrete implementation is the `DataHandler` class.
  * `IFilter`   
    Interfaces that defines a way to filter objects. Concrete implementation is the `GeneralFilter` class.

  

Each project uses the same definition for the `Tour` data model which is found in its own Solution called `Model`.  

### Used Libraries

List of some import libraries used throughout the project:

* `Webservice_Lib`    
  REST-server & Dependency Injection used in the server, based on last semesters Webservice project which can be used now as a [library](https://github.com/kurbaniec/WebServiceLib)
* `Npgsql`     
  Postgres Database Access in the server
* `Microsoft.Extensions.Logging` with `Log4Net `     
  Used for Logging in the server & client
* `Microsoft.Extensions.DependencyInjection `    
  Dependency Injection used in the client

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


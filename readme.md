# Loqr Api
This is a configurable ASP.NET api backed off of an synchronous database.

#### Version 0.1

Requires c# 8.0

### Run Project
```bash
npm install -g dotnet
dotnet run
```

## Usage
With version one, all interaction with the database is done via URL requests.

### Get all
<sub>One request per ip per two seconds</sub>
```c#
$"https://{hostname}/api/Loqr"
```

<sub>All requests below: one request per ip per one second</sub>
### Get by id:
```c#
$"https://{hostname}/api/Loqr/{id}"
```
In version one, id is a string type.

### Post:
```c#
$"https://{hostname}/api/Loqr/post/{id}/{payload}"
```
Example payload: col_name1=col_val1&col_name2=col_val2

### Edit:
```c#
$"https://{hostname}/api/Loqr/edit/{id}/{payload}"
```
Example payload: col_name_to_update=col_val_to_update

### Delete:
```c#
$"https://{hostname}/api/Loqr/delete/{id}"
```

### Add column to db
```c#
$"https://{hostname}/api/Loqr/db_config/{payload}"
```
Example payload: col=col_name

### Configuration
```c#
$"https://{hostname}/api/Loqr/db_config"
```

## Sqlite Implementation
Why? Why not!
C#'s sqlite is synchronous ONLY and does not offer any async overloads.
I still wanted to utilize AspNetCore.Mvc's async type, so I've used SemaphoreSlim to lock thread workers
to one, to ensure we don't corrupt the sqlite.

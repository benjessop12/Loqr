# Loqr Api
This is a configurable ASP.NET api backed off of an synchronous database and custom database interaction wrapper.
I wrote my own db handlers for fun.

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

<sub>If a request has [Auth] next to it, you must pass authentication parameters with the payload</sub>
### Get by id
```c#
$"https://{hostname}/api/Loqr/{id}"
```

### Post [Auth]
```c#
$"https://{hostname}/api/Loqr/post/{id}/{payload}"
```
Example payload: col_name1=col_val1&col_name2=col_val2

### Edit [Auth]
```c#
$"https://{hostname}/api/Loqr/edit/{id}/{payload}"
```
Example payload: col_name_to_update=col_val_to_update

### Delete [Auth]
```c#
$"https://{hostname}/api/Loqr/delete/{id}/{payload}"
```
<sub>The payload for delete is ONLY the auth details</sub>

### Add column to db [Auth]
```c#
$"https://{hostname}/api/Loqr/db_config/{payload}"
```
Example payload: col=col_name

### Configuration
```c#
$"https://{hostname}/api/Loqr/db_config"
```

## Authentication
Upon first use, the user will need to create as user id and password to be able to use the authenticated requests above.
Example url to create user details
```c#
$"https://{hostname}/api/Admin/create_auth/{id}/{payload}"
```
Example payload: auth=password

This will store an Hash and Salt in the admin database, along side the sepcified id.

Then, to access an authenticated request, you will append the below to the payload:
auth={id}{password}

As I am writing this, only one user is allowed for the system. I'll work on being able to securely add more users further down the line.

## Sqlite Implementation
Why? Why not!
C#'s sqlite is synchronous ONLY and does not offer any async overloads.
I still wanted to utilize AspNetCore.Mvc's async type, so I've used SemaphoreSlim to lock thread workers
to one, to ensure we don't corrupt the sqlite.

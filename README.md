# BuberDinner
## Overview
This repository follows course from YouTube on channel named `Amichai Mantinband` [(link)](https://www.youtube.com/channel/UClz49zOCnzsclUJY-t62lIw). The course's name is `REST API Following CLEAN ARCHITECTURE & DDD` [(link)](https://www.youtube.com/watch?v=fhM0V2N1GpY&list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k).
I have updated the original repository so that it is more developer-friendly. I also plan to add some missing functionality.

## About applications
How does the mobile application look like?
![image](pics/mobile-app.png)  
*Pic. Mobile application UI ( © Amichai Mantinband)

At the moment we only have a REST API part in the repository.
Application is written in .NET 6.0 with C# using WebAPI technology. This solutions follows Clean Architecture principles and Domain-Driven Design principles. It contains 4 layers:
 - Domain
 - Application
 - Infrastructure
 - Api

Dinner Hosting Context

| Name | Description |Aggregate|
|--------|--------|--------|
| Menu   | A list of dishes associated to some host and dinners  |![image](pics/agr_menu.png)|
| Dinner   | An event organised by the host person. Include food and possibly some kind of action  |![image](pics/agr_dinner.png)|
| Host   |  Event location and event organiser |![image](pics/agr_host.png)|
| Guest   | Person reserving seat and attending event |![image](pics/agr_guest.png)|
| User   | Person registered in the application |![image](pics/agr_user.png)|
| Reservation | An arrangement made by guests in advance to confirm a place for the event at a particular time of day | see Dinner|
| Bill   | A statement of money owed for the supply of the event/food |![image](pics/agr_bill.png)|
| Menu Review   | Opinion voting on food  |![image](pics/agr_menu_review.png)|
| Guest Rating  | Opinion poll for guests  |see Guest|

More information about DDD and the project can be found [here](https://github.com/AlexNek/ddd-for-developers).

## Build and run
You can run the application in two ways:
1. Local application
> **Note**: This way need to have separate database running on your local machine for some requests. Go to `infrastructure` project and run command `dotnet ef database update` for database creation. As an alternative, you can use sql file added to the project.

To run this project you need to have .NET 6.0 runtime installed. Then clone (or download) this repository.\
Then open CLI and open folder with application. Then run from CLI with command: `dotnet run --project .\BuberDinner.Api\`

2. Docker

Application has `docker-compose` file for running the application with MSSQL Server database in container. \
To run the application using Docker you have to have it installed on your machine. Then close (or downlaod) this repository. \
In the main folder type the command:
```
docker-compose up -d --build
```

In both ways of running the application, you have to use migrations for creating tables to fully use the application.

After browser opening you can use swagger to test api. Don't forget to use autorization token

## Implemetation notes

At this point, all users stored in memory, so you need to register user every time after start.
No menu reading implemented now.

## Database design
![image](pics/db-host.png)
![image](pics/db-menu.png)
![image](pics/db-reservation.png)
![image](pics/db-single-tables.png)

## Changes
- Implement BuberDinner.Infrastructure.UnitTests
- Implement BuberDinner.Api.UnitTests
- Added BuberDinner.Application.UnitTests and empty test projects
- Validate email for registration and login
- Securely store user password in database
- Save the user in the database
- Added swagger entry point
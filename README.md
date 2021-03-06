# EventApp
An event and task management app for family and friends. Imagine if you could assign tasks to your kids or siblings and send them text reminders. Create events and let your loved ones know about them!

This is a small task and event management app which I am submitting to the [Dev.to Hackathon](https://dev.to/devteam/announcing-the-twilio-hackathon-on-dev-2lh8).

The app allows you to do the following:
- register new users
- verify user phone number using Twillio Api
- create tasks and events for the user
- A user can send and accept friend invites
- Send text reminders to the task assignee when a task is due.
- Send out invites or reminders to friends about your events 
- See your tasks and events as well as your friend events on a calender

**See this in action: [live demo](https://eventapplication.azurewebsites.net/authentication/login)**

## Setup Prerequisite 

- [Visual Studio 2017 +]
- [Node 10.16.3] - https://nodejs.org/en/download/ 
Verify that you are running at least Node.js version 8.x or greater and npm version 5.x or greater by running node -v and npm -v in a terminal/console window. Older versions produce errors, but newer versions are fine.
- [Angular Cli 9.0.6] - To install run the following in command line: `npm install -g @angular/cli` then verify the version running using `ng --version`
- [Typescript 3.8.3] - https://www.typescriptlang.org/ Verify that you have version 3.1 or above. You can check by running tsc -v in any terminal window.
- [docker]
- [docker compose]

## Getting Started

1. Right click on solution and select `Restore Nuget Packages`
2. Update the appsettings.Development.json file `Twilio` section with your twilio credentials.
3. Open terminal window and go to project location where the `package.json` file is, then run the following to install all the npm packages: "npm install"
4. Run `docker-compose up -d` (make sure you are in the directory which contains the docker-compose.yml file `cd eventapp`)
5. Check that your docker MySql container is up and running by connecting to it on `127.0.0.1:3306` via a client (e.g. Mysql workbench). To connect use the username `root` and password `letmein`. 
6. Run `docker exec -it eventapp_mysql bin/bash`to log into the mysql container. Then run `./setup.sh` to setup the DB. Or you can use the script in `docker/db/setup.sql` to run it through a client. You should see the eventapp database has been created with some tables when that has finished.
7. Run npm start and make sure the angular app has built and is running ok
8. Run the .net core application on visual studio
9. Go to https://localhost:44363 to start using the app!

## License

[Apache License v2](http://www.apache.org/licenses/)

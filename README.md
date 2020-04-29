# EventApp
This is a small task and event management app which I am submitting to the [Dev.to Hackathon](https://dev.to/devteam/announcing-the-twilio-hackathon-on-dev-2lh8).
The app aloows you to do the following:
- register new users
- verify user phone number using Twillio Api
- create tasks and events for the user
- A user send add and except friend invites
- send text reminders to the task assignee when a task is due and user friends about an upcoming event
- show user tasks and events on a calender

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
2. Open terminal window and go to project location where the `package.json` file is, then run the following to install all the npm packages: "npm install"
3. Run `docker-compose up -d` (make sure you are in the directory which contains the docker-compose.yml file)
4. Check that your docker MySql container is up and running by connecting to it on `127.0.0.1:3306` via a client (e.g. Mysql workbench). To connect use the username `root` and password `letmein`. 
5. Run `docker exec -it eventapp_mysql bin/bash`to log into the mysql container. Then run `./setup.sh` to setup the DB. Or you can use the script in `docker/db/setup.sql` to run it through a client. You should see the eventapp database has been created with some tables when that has finished.
5. Run npm start and make sure the angular app has built and is running ok
6. Run the .net core application on visual studio
7. Go to https://localhost:44363 to start using the app!

## License

[Apache License v2] (http://www.apache.org/licenses/)
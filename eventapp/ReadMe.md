---------------------------------
Prerequisite
---------------------------------

[Visual Studio 2017]

[Node] - https://nodejs.org/en/download/ 
Verify that you are running at least Node.js version 8.x or greater and npm version 5.x or greater by running node -v and npm -v in a terminal/console window. Older versions produce errors, but newer versions are fine.

[Angular Cli 7] - To install run the following in command line. 
npm install -g @angular/cli
verify version running: ng --version

[Typescript] - https://www.typescriptlang.org/
Verify that you have version 3.1 or above for npm global and SDK for visual studio. 
To check Global: You can check by running tsc -v in any terminal window.
To check Visual Studio 2017: Right click on project -> properties -> Typescript Build -> Typescript Version 3.1 (should be there)


--------------------------------------
Getting Started
--------------------------------------

1. Right click on solution and select "Restore Nuget Packages"
2. Open terminal windows and browse to project location where the package.json file is, then run the following to install all the npm packages:
"npm install"
3. Run the application

---------------------------------
Swagger
---------------------------------
To test swagger

1. Run applicaton
2. add "/swagger" to url and should take to you swagger api tester

--------------------------------
Angular
--------------------------------
To run over ssl
ng serve --ssl true

To test angular using karma
1. run "ng test" in a terminal where the angular.json file is located

--------------------------------
Run docker cotainers
--------------------------------
docker-compose up -d ----> to run the docker container (for mysql and for backend)
docker logs <image id> -f ---> to check the logs. To get the image id run docker image ls
docker ps ----> to see if the containers are running (there should be two)
docker-compose build ----> if you change the docker configs rebuild using this command
docker exec -it eventapp_mysql bin/bash ---> to log into the mysql container. Then run ./setup.sh to setup the DB and run any new SQL


Connect to the docker DB on localhost:3306
Connect to the dockerised backend on localhost:5001
Note: Microsoft Visual Studio runs the backend on port localhost:5000 so bear this in mind when connecting the frontend with the backend.
Currently the frontend is configured to connect to port 5000 so you need to run the backend using visual studio for things to work

----------------------
Frequent Docker Errors
----------------------
Cannot start service mysql: driver failed programming external connectivity on endpoint eventapp_mysql
(d6dcd3066ae462126faebba8b73b793e36af8927f2d85ba9a536d9ab0dad7818): Error starting userland proxy: mkdir /port/tcp:0.0.0.0:3306:tcp:172.19.0.3:
3306: input/output error

Solution: restart docker
# Remote-server-handler-gui
Remote server handler gui [via api]

# Required
- .NET 8 LTS
- node version >= 20

# Quick notes
If you would like to start the project then follow these steps:
- Install node & .net
- Go to the api folder and start the MySQL server container: docker-compose up --build -d
- After start the api: npm run start:dev
- Then open the solution in the gui folder and start the application :)
- If you want to run some tests then click to the UnitTest.cs and run tests
- When the application is shown, then you have to fill the API_URL field and enter your API: http://localhost:3000/user/sign-in
- If you would like to use your own, then you have to do the following routes
- ## /connector/upload-file POST
- ## /connector/system-health POST
- ## /connector/download-file GET
- ## /connector/remove-file POST
- ## /connector/list-files POST
- ## /user/loggedin POST
- ## /user/sign-out POST
- ## /user/sign-in POST
## In the root folder I put an exported sql file and you can use it if you want :)

# Supported operations
- File upload / remove / download
- System monitoring

# Pictures

### Administrator is not logged in
![image](https://github.com/user-attachments/assets/0ea650dc-7c86-4198-a712-622f25828b0f)


### File manager
![image](https://github.com/user-attachments/assets/2d35dbf0-a497-49b0-ae7f-632fbc681b30)



### System health & monitoring
![image](https://github.com/user-attachments/assets/3534db03-cbc6-40ed-8688-6341c57876a8)


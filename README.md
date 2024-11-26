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
- ### /connector/upload-file POST
- ### /connector/system-health POST
- ### /connector/download-file GET
- ### /connector/remove-file POST
- ### /connector/list-files POST
- ### /user/loggedin POST
- ### /user/sign-out POST
- ### /user/sign-in POST
  
### In the root folder I put an exported sql file and you can use it if you want :)

# Installation guide
## API
- Change your directory to ServerConnectorAPI
- npm install                                                  **(Install required dependencies)**
- npx prisma generate                                          **(Install Prisma ORM)**
- docker-compose up --build -d                                 **(Start mysql docker container in detached mode)**
- Open your mysql ide and upload the exported sql file in the 'SQL Dump folder'
- See .env file and fill it with your secrets
- npm run start:dev                                            **(Run the api in development mode (auto refresh after editing))**

## Web interface
- cd web-interface
- npm install
- npm run start

## GUI
- Open .sln project file :)

# Supported operations
- File upload / remove / download
- System monitoring

# [Desktop application - Pictures ]

### Administrator is not logged in
![image](https://github.com/user-attachments/assets/0ea650dc-7c86-4198-a712-622f25828b0f)


### File manager
![image](https://github.com/user-attachments/assets/2d35dbf0-a497-49b0-ae7f-632fbc681b30)



### System health & monitoring
![image](https://github.com/user-attachments/assets/3534db03-cbc6-40ed-8688-6341c57876a8)

# [Web application - Pictures ]

### Administrator is not logged in
![image](https://github.com/user-attachments/assets/a231c5c5-07fe-4743-b2dc-08dff6cd57ef)

### Home page
![image](https://github.com/user-attachments/assets/b1aa4824-a415-4b2f-a5e6-b72d7087a5b9)

### File manager
![image](https://github.com/user-attachments/assets/0a1751ef-4e75-47fe-a343-81e886820d8b)

### System health
![image](https://github.com/user-attachments/assets/e817a142-147f-47fa-a5f8-add7532e8b73)

# How to setup the WPF application?
- Open the project sln (main)
- Select multiple startup project (first must be the ChatServer and the second is the WPF itself)
- After that just run it
- Now, you can log in with your preferred account
- If you want a new gui just open up the project in a seperate visual studio and select single startup project and select wpf
- after just start it
- probably this time you will see the sign in part on the right. When I sign in pay attention to the chat channel because if you join another chat server you won't be able to chat with your first gui instance.
  the basic chat server is running on the pc's loopback address and port 2546 (but you can see it in the code)
- After connecting and signed in, you can chat with the logged in parties
- Have fun :)

# [Desktop application - WPF pictures]

### Administrator is not logged in
![image](https://github.com/user-attachments/assets/c9e1e0ff-da07-4a8d-99d7-4a30bf81faa7)

### File manager
![image](https://github.com/user-attachments/assets/1d636dc7-dfe8-4fd1-9b69-e373c424d460)

### Chat with active administrators
![image](https://github.com/user-attachments/assets/9d55646a-fc1c-422d-bf6f-9acfa1048cfc)


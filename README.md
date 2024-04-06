![Safety Toolbox Banner](https://github.com/Team-Safety-Toolbox/Safety-Toolbox/assets/98986881/f5048389-96a5-4bb9-86df-d1badfae8eed)

# Members
![Colorful Team Introduction Instagram Post](https://github.com/Team-Safety-Toolbox/Safety-Toolbox/assets/98986881/4144d86b-840d-40aa-8f43-6cd694f771aa)


## Project Idea
Our project is an open-source safety system tracking app. It will keep track of employee training and certifications, items for Toolbox Talks such as prompts, attendance sheets, and notes, and it will act as a central library of requirements and documents that can be revised and updated as needed.

## Project background/Business Opportunity
Our customer is Ralph McKay Industries. They are in need of a tool that can keep track of their training requirements. Their current process is to keep track using a combination of spreadsheets and written documents. They have looked at using other HR tools before, but they haven’t found one that would suit their needs. Their main concerns are the fact that OH+S requirements are different province by province, so they can’t just use any tool, and they want their data to remain in-house instead of on some company’s server. Our solution would solve these problems for them.

## Vlog Links
Vlog 1 - [Link here!](https://youtu.be/MCdWYfqRIfA)

Vlog 2 - [Link here!](https://youtu.be/ryfXBUEsTb4)

Vlog 3 - [Link here!](https://youtu.be/tpB5mSMG9_Y)

Demo Video - [Link here!](https://www.youtube.com/watch?v=rz2jSGdBQGE)

Commercial - [Link here!](https://youtu.be/zNVPYRBaVKM)

## Project Board
Can be found under the [Projects tab!](
https://github.com/orgs/Team-Safety-Toolbox/projects/1)

## Mentor
Craig Gelowitz Ph.D., P.Eng.

## Additional Credits
Icons used in the project come from Austin Andrews, [![License: CC0-1.0](https://licensebuttons.net/l/zero/1.0/80x15.png)](http://creativecommons.org/publicdomain/zero/1.0/) https://github.com/Templarian/WindowsIcons

# INSTALL INSTRUCTIONS
## Prerequisites
- Windows
- Microsoft SQL Server Management Studio
    - This guide assumes you know how to use SSMS or some version of SQL


## Option 1: Use the prebuilt version provided
1. Download the zipped [Release](https://github.com/Team-Safety-Toolbox/Safety-Toolbox/releases/tag/v1.0.0) folder and unzip it to the desired location
2. The executable file (Safety Toolbox.exe) inside this folder requires the dependancies that are also inside this folder, so leave everything in the folder together. You can create a shortcut to the application on your desktop, or pin it to your taskbar for easy access.
3. Skip down to the Database Setup section below to set up your database.


## Option 2: Build it yourself
1. In the folder `Safety-Toolbox\App\Safety Toolbox`, run the following command: 
>dotnet publish -f net7.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win10-x64 -p:WindowsPackageType=None -p:WindowsAppSDKSelfContained=true --self-contained
2. Once the command has finished running, the path that the build will end up in is here: `Safety-Toolbox\App\Safety Toolbox\Safety Toolbox\bin\Release\net7.0-windows10.0.19041.0\win10-x64`
3. The executable file (Safety Toolbox.exe) inside this folder requires the dependancies that are also inside this folder. 
    
4. Proceed to the Database Setup section below to set up your database.


## Database Setup
1. In SQL Server, run the setup code provided [here](https://github.com/Team-Safety-Toolbox/Safety-Toolbox/blob/main/Setup%20Files/Database%20Setup%20File.sql). This will set up the tables needed for the app to operate. 
2. Optionally, you can also run [this](https://github.com/Team-Safety-Toolbox/Safety-Toolbox/blob/main/Setup%20Files/Test%20Data.sql) demo setup file if you would like some demo values
## Running the app for the first time
1. The first time you launch the app it won't be connected to your database. Login with the username `admin` and the password `Adm1nU$er`. (This account can only be used to change the settings, it cannot access anything else.) 
2. Once logged in, go to the settings page and fill in the fields required, such as the database string and the folder paths.
3. Toggle the Signup page switch to 'on'. You're going to need to make a proper account for yourself.
4. Click the save button at the bottom of the page.
5. Restart the app, and this time click the Signup button and choose your desired username and password.
    - if you get a popup with a connection error for the database, your connection string in step 2 is likely incorrect.
6. You can now log in to the app with your new account. This account is a `read-only` account, meaning you will be unable to edit anything but you will be able to view most pages now.
    - To change your account to an editor-level account, you must do so in the backend with SQL statements
    - In the Users table, you need to change the RoleID to 1 (IT Role) or 2 (Managemant Role) depending on your needs.
        - The IT account is able to access the Settings page at any time and has editing access throughout the app.
        - The Management account also has editing access throughout the app, but cannot access the settings page.
    - All future accounts that need edit access must be handled in the backend like this
7. You're all set up! Head over to our [Demo Video](https://www.youtube.com/watch?v=rz2jSGdBQGE) or [Our User Guide](https://github.com/Team-Safety-Toolbox/Safety-Toolbox/blob/main/Documentation/Safety%20Toolbox%20User%20Guide.pdf) to learn more about how to use this app!

# NDWebApp
This project uses Microsoft Identity for user registration and login.

# Installation
Prerequisite: Have Visual Studio installed
1. Download and install XAMPP v.8.1.10 from here: https://sourceforge.net/projects/xampp/files/XAMPP%20Windows/8.1.10/xampp-windows-x64-8.1.10-0-VS16-installer.exe
2. Start xampp-control.exe and make sure PhpMyAdmin is running. Doublecheck that MariaDB version is 10.4.25
3. Open this solution in Visual Studio
4. Press Tools -> NuGet Package Manager -> Package Manager Console
5. In the PM console, type "update-database" and press enter and wait.
6. Check in PhpMyAdmin that the new database and tables have been created.
7. Attempt to build solution inside Visual Studio

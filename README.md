# Tech-test-79
A simple web application based on Programmer Test from a company in Bandung.

# Prerequisites
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [MySQL](https://www.mysql.com/downloads/)

# Usage
1. Clone or Download this repository
2. Create new database
3. Execute the SQL in ```Tech-test-79/DewaApp/Script_tables.sql``` to create the required tables
4. Copy the ```ConnectionStrings``` from the database and replace the ```default``` connection string in ```appsettings.json``` with the new one.
The `appsettings.json` file should be looks like this image.
![image](https://user-images.githubusercontent.com/48423286/189476860-cf9f3e62-6229-48f0-9bdf-af4b27052962.png)
5. Open new terminal and locate the following path
``../Tech-test-79/DewaApp/DewaApp``
and then execute this code
``dotnet run``
6. The terminal should be looks like this image
![image](https://user-images.githubusercontent.com/48423286/189477096-b2378ac0-514f-4b48-a778-677f91fc6805.png)\
Copy the IP address from the terminal example: ```http://localhost:5289```
7. Paste the IP to your browser, and then the result should be looks like this image.
![image](https://user-images.githubusercontent.com/48423286/189477132-0c422060-ba41-4b3f-9e21-961d59cdc283.png)\
The solution of each task are showed by the name of the navigation section in the top of the page.
- The ``Accounts`` is the solution of the task No.1
- The ``Transactions`` is the solution of the task No.2
- The ``TransactionsPoint`` is the solution of the task No.3
- The ``TransactionsHistory`` is the solution of the task No.4

﻿# TestAPI
Steps to launch api:
1. In appsettings.json change "DefaultConnection": "Data Source=(localdb)\\Local;Initial Catalog=testapi;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
     to your data: Data Source= <Machine name>\\<Server name>; Initial Catalog=<DataBase name>; The following lines do not need to be changed. 
2. In api folder open terminal and type: "dotnet ef database update"
3. After type: "dotnet watch run"

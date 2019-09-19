# zipcotest
ZipCo Test

# This is the Zip Co assessment.

# Architecture of the Project

  - This Project contains two main services such as Account and UserMangement Services. The project is based on microservices architecture     which was inspired from Microsoft. 
  - Both Services are running on the dockers compose via visual studio container tool. There is also library such as ApplicationCore where     all shared components or library are hosted. This project uses Event-Driven Architecture with combination of microservices. 
  - I've implemented Event Bus by using RabbitMQ and MSSQL which are also part of the solution and are hosted inthe docker as well. 
  - All docker settings are taken from the docker-compose so the sql connection if you wish need to be change there or another values.
  - I've implemented also some functional test and unit tests for both Services.
  - The project is based on ASP.NET Core 2.2 Library, using EntityFramework Core with Code-First Approach with MS SQL hosting inside too.
  
  
   

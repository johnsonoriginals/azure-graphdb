# Hackathon

This is an older .NET Core 3.1 application provided for sample purposes.

It may be deployed to Azure, or run locally. 

In an ideal world, projects are deployed to Azure using CI/CD pipelines as described in [Azure Devops](https://learn.microsoft.com/en-us/azure/devops/pipelines/architectures/devops-pipelines-baseline-architecture?view=azure-devops) documentation.

This project is written in C#. 

It is a very small API with a lightweight MVC front end using CosmosDB, a graph database as a datastore. 

An OpenAPI spec is also provided.

## Getting Started

These instructions should get you a copy of the project up and running on your local machine for development and testing purposes.
You may be able to get a temporary free acount in Azure to deploy this project to.

### Prerequisites

- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download)

### Running the application

1. Clone the repository.
2. Navigate to the project directory.
3. Run `dotnet restore` to install the dependencies.
4. Run `dotnet run` to start the application.
5. Open your browser and navigate to `https://localhost:5001`.


License: [License](MIT.md)

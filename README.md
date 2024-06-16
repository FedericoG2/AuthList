# AuthList

## Description

This project is a REST API developed in ASP.NET Core that provides functionalities for user and product management. It is designed to be secure and scalable, using JWT (JSON Web Token) authentication to protect sensitive endpoints.

## Features

#### Authentication and Authorization: 
Uses JWT for user authentication and access control to resources through roles and authorization policies.

#### User Management:
Allows registration of new users and authentication using encrypted credentials.

#### Product Management: 
Provides CRUD (Create, Read, Update, Delete) operations for products, including the ability to list, search, add, update, and delete products.

## Technologies Used:
#### ASP.NET Core:
Web development framework for building modern and scalable APIs.

#### Entity Framework Core: 
ORM (Object-Relational Mapping) used for interacting with the SQL Server database.

#### C#: 
Primary programming language used for backend development.


## Installation

```python
# Ensure you have .NET SDK installed on your machine.
# Clone this repository
git clone https://github.com/yourusername/authlist.git

# Install Dependencies
dotnet restore

# Configure the Database
Ensure you have a SQL Server instance set up and update the connection string in `appsettings.json`.

# Navigate to the project directory
cd authlist

# Build and run the application
dotnet run

```

## Demo

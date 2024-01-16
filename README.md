Welcome to the Bookshop Project!

This project is an e-shop (Bookshop) developed using .NET 6 for the backend and Angular 17 for the frontend. This project use modern and clean architecture using .NET 6 RESTful Web API with various modern libraries such as MediatR, Swagger, Microsoft.AspNetCore.Identity, System.IdentityModel.Tokens.Jwt, Serilog, FluentValidation, and more. The frontend is built using Angular 17, incorporating tools like PrimeNG, PrimeFlex, angular-fontawesome, Bootstrap(for Grid only), rxjs, Ngx-toastr, jwt-decode, and HttpInterceptor for JWT token handling.

For a quick demo, visit Bookshop Demo : https://mementocoding.be/


Swagger Documentation:

    Bookshop Swagger

Author Queries:

    Retrieve author details by ID: GET /api/author/{id}
    Browse all authors: GET /api/author

Book Queries:

    Access book details by ID: GET /api/book/{id}
    Explore the book catalog: GET /api/book
    Find books by author: GET /api/book/author/{id}
    Filter books by category: GET /api/book/category/{id}

Book Commands:

    Create a new comment for specific book with bookId: POST /api/book/add-comment-book
    Delete a comment: POST /api/book/delete-comment-book
    Update a comment: POST /api/book/update-comment-book

Category Queries:

    Retrieve category details by ID: GET /api/category/{id}
    Browse all categories: GET /api/category

Customer Commands:

    Create a new customer profile: POST /api/customer/create-customer
    Edit customer profile details: POST /api/customer/edit-profile-customer
    Change customer password: POST /api/customer/edit-password-customer

Customer Queries:

    Authenticate and obtain customer details: POST /api/customer/authenticate

Order Commands:

    Create a new user order: POST /api/order/create-user-order
    Cancel a user order: POST /api/order/cancel-user-order
    Update a user order: POST /api/order/update-user-order

Order Queries:

    Retrieve all user orders: GET /api/order/get-user-orders
    Get details of a specific user order by ID: GET /api/order/get-user-order/{id}

Search Queries:

    Find books using keywords: GET /api/search/book/{keyword}

Shopping Cart Commands:

    Create a user shopping cart: POST /api/shopcart/create-user-shopcart
    Update a user shopping cart: POST /api/shopcart/update-user-shopcart
    Reset a user shopping cart: POST /api/shopcart/reset-user-shopcart

Shopping Cart Queries:

    Retrieve user shopping cart details: GET /api/shopcart/get-user-shopcart
    Get user shopping cart details with reviews: GET /api/shopcart/get-user-shopcart-details-review


Prerequisites:

    Operating System: Ensure that you have a compatible operating system (e.g., Windows, macOS, or Linux) installed on your machine.

    Software Requirements: Make sure you have the following software installed:
        .NET 6 SDK (6.0.400)
        Node: 18.15.0
        Angular CLI: 17.0.7 (install using npm: npm install -g @angular/cli)
        Database: You'll need a database server such as SQL Server installed and running.

Installation Steps:

Clone the Repository:

    git clone https://github.com/AjrAli/Bookshop.git

Navigate to the Project Directory:

    cd Bookshop

Backend Setup:
a. Database Configuration:

    Configure your database connection string in the appsettings.json file.

b. Migrations:

    Run the database migrations to create the required tables. Refer to the file named "DBCommands.txt" in Bookshop\api and run these commands in the Package Manager Console.

c. Run the Backend:

    dotnet restore
    dotnet build
    dotnet test
    dotnet run

d. Update launchSettings.json:

Open the launchSettings.json file and add the following configuration:

    {
      "profiles": {
        "DevProfile": {
          "commandName": "Project",
          "launchBrowser": true,
          "launchUrl": "swagger",
          "environmentVariables": {
            "ASPNETCORE_ENVIRONMENT": "Development"
          },
          "applicationUrl": "https://localhost:5001"
        }
      }
    }

Frontend Setup:
a. Navigate to the Client Directory:

    cd Bookshop\app

b. Install Dependencies:

    npm ci

c. Run the Frontend:

    ng build
    ng serve

Access the Application:

    Open a web browser and navigate to http://localhost:4200 to access the Bookshop Angular app.


Note: These installation instructions are a general guideline. Depending on your specific project structure and configurations, you may need to adjust the steps accordingly.

Enjoy using the Bookshop Project!


Technologies used on the project :

    .Back-end app using :
        .Net 6.0
        ASP.NET RESTful Web API
        Serilog
        FluentValidation
        AutoMapper
        Entity Framework (EF) Core
        Microsoft.AspNetCore.Identity
        System.IdentityModel.Tokens.Jwt
        Swagger
        MediatR
        CQRS pattern
        Unit test...

    UI app using :
        Angular 17 (with Lazy Loading, incorporating standalone components for improved performance and modularization.)
        PrimeNG
        PrimeFlex
        Fontawesome
        Bootstrap(Grid only)
        Rxjs
        Ngx-toastr
        Jwt-decode + HttpInterceptor for JWT token

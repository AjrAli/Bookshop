Welcome to the Bookshop Project!

This project is an e-shop (Bookshop) developed using .NET 9 for the backend and Angular 17 for the frontend. This project use modern and clean architecture using .NET 9 RESTful Web API with various modern libraries such as MediatR, Swagger, Microsoft.AspNetCore.Identity, System.IdentityModel.Tokens.Jwt, Serilog, FluentValidation, and more. The frontend is built using Angular 17, incorporating tools like PrimeNG, PrimeFlex, angular-fontawesome, Bootstrap(for Grid only), rxjs, Ngx-toastr, jwt-decode, and HttpInterceptor for JWT token handling.

For a quick demo, visit Bookshop Demo : https://mementocoding.be/


Swagger Documentation:

    Bookshop Swagger

Author Commands

    POST /api/authors: Creates a new author resource.
    PUT /api/authors/{id}: Updates an existing author resource.
    DELETE /api/authors/{id}: Deletes an existing author resource.

Author Queries

    GET /api/authors: Retrieves a collection of authors.
    GET /api/authors/{id}: Retrieves a specific author resource.

Book Commands

    POST /api/books/{bookId}/comments: Adds a new comment to a specific book.
    DELETE /api/books/comments/{commentId}: Deletes a specific comment on a book.
    PUT /api/books/comments/{commentId}: Updates a specific comment on a book.
    POST /api/books: Creates a new book resource.
    PUT /api/books/{id}: Updates an existing book resource.
    DELETE /api/books/{id}: Deletes an existing book resource.

Book Queries

    GET /api/books: Retrieves a collection of books.
    GET /api/books/{id}: Retrieves a specific book resource.
    GET /api/books/by-author/{authorId}: Retrieves books written by a specific author.
    GET /api/books/by-category/{categoryId}: Retrieves books belonging to a specific category.

Category Commands

    POST /api/categories: Creates a new category resource.
    PUT /api/categories/{id}: Updates an existing category resource.
    DELETE /api/categories/{id}: Deletes an existing category resource.

Category Queries

    GET /api/categories: Retrieves a collection of categories.
    GET /api/categories/{id}: Retrieves a specific category resource.

Customer Commands

    POST /api/customers: Creates a new customer resource.
    PUT /api/customers: Updates an existing customer resource.
    PUT /api/customers/password: Updates a customer's password.

Customer Queries

    GET /api/customers: Retrieves a collection of customers.
    POST /api/customers/authenticate: Authenticates a customer.
    GET /api/customers/{id}: Retrieves a specific customer resource.

Order Commands

    POST /api/orders: Creates a new order resource.
    PUT /api/orders/{id}/cancel: Cancels an existing order.
    PUT /api/orders/{id}: Updates an existing order resource.

Order Queries

    GET /api/orders: Retrieves a collection of orders.
    GET /api/orders/{id}: Retrieves a specific order resource.
    GET /api/admin/orders/{id}: Retrieves a specific order resource for admin purposes.
    GET /api/admin/orders: Retrieves a collection of orders for admin purposes.

Search Queries

    GET /api/search/book?keyword={keyword}: Retrieves books based on a keyword search.

ShoppingCart Commands

    POST /api/shopcarts: Creates a new shopping cart resource.
    PUT /api/shopcarts: Updates an existing shopping cart resource.
    PUT /api/shopcarts/reset: Resets a shopping cart.

ShoppingCart Queries

    GET /api/shopcarts/current: Retrieves the current shopping cart.
    GET /api/shopcarts/current/reviews: Retrieves the current shopping cart with reviews.


Prerequisites:

    Operating System: Ensure that you have a compatible operating system (e.g., Windows, macOS, or Linux) installed on your machine.

    Software Requirements: Make sure you have the following software installed:
        .NET 9 SDK (9.0.200)
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

    cd Bookshop\ui

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
        .Net 9.0
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

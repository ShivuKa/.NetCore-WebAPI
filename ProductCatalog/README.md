# Objective
Implement online shopping purchase application with minimal features to demonstrate the following:

-	Ability to develop this exercise using .Net Core and ASP.Net Core
-	Ability to implement design patterns and other techniques to demonstrate efficient coding practices
-	Setup devops for the application
-	Expectations on tech stack to be used:
    o	.Net Core 
    o	Web API using Asp.Net Core
    o	Entity Framework
    o	Jenkins & Docker for deployment


# Stories/ User Journey:
# Product Catalog:
- User has ability to search products based on selected category (electronics, home, etc.)
- Product listing will display product details such as description & price.
- Product listing should be pre-filtered based on conditions like category, price, etc.

# Shopping Cart:
- User has option to add products to shopping cart specifying the quantity.
- User will provide details like shipping and billing address, payment details that should be stored in back end (DB/ flat file) 
- Implement few validations such as Minimum purchase quantity for certain products, Prevent expired products, or similar validations.


## Implementation Phase 1:
•	Use design patterns for efficient coding
•	Sorting in product listing has to use LINQ.
•	API calls should be authorized using Auth0
•	API endpoint for Product Catalog Service and Order Catalog Service should be accessible only to the specific user.
•	All API operations must be accessible through POSTMAN, Swagger, or equivalent tools
•	Use Data Annotation attribute for validation
•	Db Context must be replaced with Scaffolding code

## Implementation Phase 2:
•	Actions should be logged to flat files with rollover logging
•	Class members must have Unit Test implementation and adequate code coverage. Use MOQ for unit test data
•	Project should be deployed in Jenkins and Docker

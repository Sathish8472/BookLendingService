# **BookLendingService – .NET 8 Web API**

This project implements a simple Book Lending Service built with **.NET 8**, containerized with **Docker**, and prepared for deployment to **AWS ECS Fargate** using **CloudFormation**.

## **1. Overview**

The Book Lending Service exposes four core operations:

- Add a new book to the catalogue
- View all books
- Check out a book (mark as unavailable)
- Return a book (make available again)

The API uses a clean service/repository structure and returns consistent responses using a shared `ApiResponse<T>` wrapper.

## **2. Project Structure**

```
BookLendingService/
│
├── src/
│   └── BookLendingService.API/         # Main API project
│       ├── Controllers/
│       ├── Services/
│       ├── Repositories/
│       ├── Models/
│       ├── Data/
│       ├── Common/
│       ├── Dockerfile
│       ├── .dockerignore
│       └── Program.cs
└── tests/
|    └── BookLendingService.Tests/
|
└── Infrastructure/
    └── ecs-fargate.yaml                # CloudFormation template for AWS deployment
```

### Project Components
**Models/DTOs**

Book entity and API response DTOs.

**Repositories**

Abstracts data persistence (in-memory).

**Services**

Business logic for lending workflow.

**Controllers**

REST endpoints.

**Common**

Reusable API response wrapper (`ApiResponse<T>`).


## **3. Running the API Locally (without Docker)**

```
dotnet --version
```

Run the API:

```
cd src/BookLendingService.API
dotnet run
```

Open Swagger UI:
 http://localhost:5001/swagger

## 4. Unit Testing
This project includes a dedicated **xUnit** test project under the `tests/` folder:

### Frameworks Used

- **xUnit** – test framework
- **Moq** – mocking dependencies
- **FluentAssertions** – readable and expressive assertions

### Coverage Included
The test suite provides functional coverage:
- **BookServiceTests**

- Add book
- Get all books
- Checkout logic (success/failure cases)
- Return logic (success/failure cases)
- **BooksControllerTests**

- Validates HTTP responses (200/400)
- Ensures controller delegates correctly to service
- Tests behavior for all major endpoints (Add, List, Checkout, Return)

### Running Tests
From the solution root:
```
dotnet test
```

All tests run against mocked dependencies, ensuring the API logic works independently of the database and infrastructure layers.


## **5. Running with Docker**

The project uses a multi-stage Dockerfile.

### Build the image:
```
cd src/BookLendingService.API
docker build -t booklendingservice .
```

### Run the container:
```
docker run -d -p 5001:80 --name booklendingservice-container booklendingservice
```

Access Swagger inside the container:

 http://localhost:5001/swagger

Port **80** is used inside the container; port **5001** is exposed externally.


## **6. Deploying to AWS ECS Fargate (IaC)**
The CloudFormation template in:

```
Infrastructure/ecs-fargate.yaml
```

provisions:

- ECS Cluster
- ECS Task Definition
- Task Execution Role
- ECS Service
- CloudWatch Logs
- Health checks
- Basic networking via parameters

### Required Parameters:
- **ImageUrl** – ECR image URL
- **SubnetId1** / **SubnetId2** – two public subnets
- **SecurityGroupId** – SG that allows inbound **port 80**

### Example AWS CLI Deployment
```
aws cloudformation deploy \
  --template-file Infrastructure/ecs-fargate.yaml \
  --stack-name BookLendingServiceStack \
  --capabilities CAPABILITY_NAMED_IAM \
  --parameter-overrides \
      ImageUrl=<your-ecr-url> \
      SubnetId1=<subnet-id> \
      SubnetId2=<subnet-id> \
      SecurityGroupId=<sg-id>
```

Once deployed, ECS will pull the image and run the API on Fargate with a public IP.

## **7. Design Decisions**

### **1. Simple layered structure**
Clear separation between controllers, services, and repositories keeps the service maintainable and extensible.

### **2. Consistent API responses**
Every endpoint returns `ApiResponse<T>`, giving predictable structure and error information.

### **3. Docker-first approach**
The API is configured to listen on port 80 inside containers, avoiding port mismatches in ECS/Fargate.

### **4. CloudFormation for deployability**
A single YAML template demonstrates IaC discipline while keeping the setup simple and readable.

### **5. Avoiding unnecessary complexity**
Given the assignment size, patterns like CQRS, Clean Architecture, MediatR were intentionally avoided to keep the service lightweight.

## **8. Future Improvements**
If this were expanded into a real service:
- Replace in-memory DB with PostgreSQL or DynamoDB
- Add authentication/authorization (JWT)
- Add integration tests and contract tests
- Implement CI/CD (GitHub Actions → ECR → ECS)
- Add pagination, filtering, and catalogue management
- Monitoring dashboards and alerts (CloudWatch/Datadog)

## **9. Conclusion** 
This solution demonstrates how a small .NET microservice can be developed, containerized, and prepared for cloud deployment using AWS ECS Fargate.

The same structure and delivery approach scale to more complex services: clean architecture, predictable API design, and infrastructure expressed as code.

# BookLendingService – .NET 8 Web API

This project implements a simple Book Lending Service built with **.NET 8**, containerized with **Docker**, and prepared for deployment to **AWS ECS Fargate** using **CloudFormation**.  
Although the domain is intentionally small, the solution is structured and delivered the same way I would approach a real production service.


## 1. Overview

The Book Lending Service exposes four core operations:

- Add a new book to the catalogue  
- View all books  
- Check out a book (mark as unavailable)  
- Return a book (make available again)

The API follows a straightforward service/repository pattern and returns consistent responses using a common `ApiResponse<T>` wrapper.

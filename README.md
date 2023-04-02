# Project Briefing
## Microservices
I have divided my project into the following microservices:

Authentication,
Notification,
Teacher,
Course,
Each microservice has its own specific database with corresponding tables.

## Architecture
I am using event-driven architecture with RabbitMQ.

All of the microservices have applied domain-driven design (DDD) architecture, which has made it difficult to implement multitenancy in each of them. However, I have successfully implemented it in the AddCourse endpoint of the Course microservice.

## Future Work
I plan to continue working on implementing multitenancy in the remaining microservices. However, due to my academic commitments, I am currently unable to devote more time to this project.

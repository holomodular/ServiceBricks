![ServiceBricks Logo](https://github.com/holomodular/ServiceBricks/blob/main/Logo.png)  

[![NuGet version](https://badge.fury.io/nu/ServiceBricks.svg)](https://badge.fury.io/nu/ServiceBricks)
![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/holomodular-support/bdb5c7c570a7a88ffb3efb3505273e34/raw/servicebricks-codecoverage.json)
[![License: MIT](https://img.shields.io/badge/License-MIT-389DA0.svg)](https://opensource.org/licenses/MIT)

# ServiceBricks: The Foundation for Microservices

## Overview

Welcome to [ServiceBricks](https://ServiceBricks.com), your cornerstone for building a robust microservices foundation. 
ServiceBricks is a powerful platform designed to streamline the development, deployment, and maintenance of distributed systems. 
Leveraging Domain-Driven Design (DDD), Event-Driven Architecture (EDA), and a host of advanced features, ServiceBricks empowers teams to create scalable, customizable services tailored to specific business domains.

## Why ServiceBricks?

* **Architectural Excellence:** Provides the core architectural patterns, implementation, standardization, and governance for your microservices.
* **Storage Agnostic:** Exposes a storage platform-agnostic model and repository-based API, supporting both relational (SQL) and document (NoSQL) databases.
* **Seamless Integration:** Switch storage providers without impacting microservice operations.


## Major Features

* **Generics:** Extensive use of generics, allowing the compiler to generate most of the required code.
* **REST API Services:** Templated, repository-based services for quickly exposing standard CRUD methods or custom methods.
* **[ServiceQuery Integration](https://github.com/holomodular/ServiceQuery):** Supports standardized, polyglot data querying for SQL and NoSQL databases.
* **[Business Rule Engine](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/BusinessRuleEngine.md):** Polymorphic techniques to build reusable business logic.
* **[Domain-Driven Design (DDD)](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/FlowOfData.md) & [Event-Driven Architecture (EDA)](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/EventDrivenArchitecture.md):** Customize business logic for any supported object and method.
* **[Background Processing](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/BackgroundTasks.md):** Supports asynchronous processes, tasks, and rules.
* **[SQL and NoSQL Database Support](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/SupportedDatabaseEngines.md):** Works standard with Azure Data Tables, Cosmos DB, InMemory, MongoDB, Postgres, SQLite, SQL Server or other providers.
* **[Service Bus Engine](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/BroadcastsAndServiceBus.md):** Supports broadcasts of system data with InMemory and Azure Service Bus.
* **[Classic or Modern REST API Design](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/ClassicVsModernRestApi.md):** Choose between Classic or Modern modes, with various response formats.
* **[NuGet Packages](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/NuGet.md):** Quickly build new services and applications.
* **Testing Framework:** Comprehensive Xunit test framework for robust unit and integration testing.
* **Open Source:** Only three dependencies (AutoMapper, Newtonsoft.Json, and ServiceQuery), all MIT licensed.
* **AI Integration:** Training AI to build new ServiceBricks microservices. Updates coming soon!


## Getting Started with Examples

Explore our [ServiceBricks-Examples](https://github.com/holomodular/ServiceBricks-Examples) repository for practical examples on hosting and deploying your ServiceBricks foundation. From single, monolithic web applications to containerized web applications, these examples provide the building blocks to create and scale your foundations quickly.

## Documentation

Check out our [ServiceBricks-Documentation](https://github.com/holomodular/ServiceBricks-Documentation) repository for comprehensive documentation on the platform, including guides on using all components and developing your own microservices.

## Official Pre-Built Microservices

Get started quickly with our pre-built microservices:

* [ServiceBricks-Cache](https://github.com/holomodular/ServiceBricks-Cache): Generic data storage with expiration microservice.
* [ServiceBricks-Logging](https://github.com/holomodular/ServiceBricks-Logging): Service-scoped or centralized application and web request logging microservice.
* [ServiceBricks-Notification](https://github.com/holomodular/ServiceBricks-Notification): Notification and delivery of emails and SMS messages.
* [ServiceBricks-Security](https://github.com/holomodular/ServiceBricks-Security): Authentication, authorization, and application security with JWT bearer token support for multi-application deployments.


## About

I am a business executive and software architect with 25+ years professional experience. You can reach me via www.linkedin.com/in/danlogsdon or https://HoloModular.com

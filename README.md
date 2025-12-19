![ServiceBricks Logo](https://raw.githubusercontent.com/holomodular/ServiceBricks/main/Logo.png) 

[![NuGet version](https://badge.fury.io/nu/ServiceBricks.svg)](https://badge.fury.io/nu/ServiceBricks)
![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/holomodular-support/bdb5c7c570a7a88ffb3efb3505273e34/raw/servicebricks-codecoverage.json)
[![License: MIT](https://img.shields.io/badge/License-MIT-389DA0.svg)](https://opensource.org/licenses/MIT)

# ServiceBricks: The Microservices Foundation

## Overview

[ServiceBricks](https://ServiceBricks.com) is a powerful microservices platform designed to streamline the development, deployment, and maintenance of distributed systems using artificial intelligence. 
Leveraging Domain-Driven Design (DDD), Event-Driven Architecture (EDA), and a host of advanced features, ServiceBricks empowers teams to create scalable, customizable services tailored to specific business domains.

## Why ServiceBricks?

* **Artificial Intelligence:** Use our online generator to create production-grade microservices in seconds using only a single human sentence as input.
* **Advanced Architecture:** Provides the core architectural patterns, implementation, standardization, and governance for your microservices.
* **REST APIs:** Expose standardized, secure REST APIs to manage your data.
* **Storage Agnostic:** Interchangeably supports relational, document, cloud or embedded database engines
* **Seamless Integration:** Switch storage providers without impacting microservice operations and avoid vendor lock-in.


## Major Features

* **Artificial Intelligence Integration:** Use large language models to build, query and manipulate your microservice data using simple human input.
* **Generics:** Extensive use of generics, allowing the compiler to generate most of the required source code.
* **REST API Services:** Templated, repository-based services for quickly exposing standard CRUD+QPV (Query, Patch, Validate) methods or custom methods.
* **[ServiceQuery Integration](https://github.com/holomodular/ServiceQuery):** Supports standardized, polyglot, dynamic data querying for all database engines.
* **[Business Rule Engine](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/BusinessRuleEngine.md):** Polymorphic techniques to build reusable business logic.
* **[Domain-Driven Design (DDD)](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/FlowOfData.md) & [Event-Driven Architecture (EDA)](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/EventDrivenArchitecture.md):** Customize business logic for any supported object and method.
* **[Background Processing](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/BackgroundTasks.md):** Supports asynchronous processes, tasks, and rules.
* **[Relational, Document, Cloud and Embedded Database Support](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/SupportedDatabaseEngines.md):** Works standard with Azure Data Tables, Cosmos DB, InMemory, MongoDB, Postgres, SQLite, SQL Server and more.
* **[Service Bus Engine](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/BroadcastsAndServiceBus.md):** Supports broadcasts of system data with InMemory and Azure Service Bus.
* **[Classic or Modern REST API Design](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/ClassicVsModernRestApi.md):** Choose between Classic or Modern modes, with various response formats.
* **[NuGet Packages](https://github.com/holomodular/ServiceBricks-Documentation/blob/main/V1/NuGet.md):** Quickly build new services and applications.
* **Testing Framework:** Comprehensive Xunit test framework for robust unit and integration testing with thousands of tests available.
* **Open Source:** All referenced assemblies are open source and licensed under MIT or an equivalent license.


## Getting Started with Examples

Explore our [ServiceBricks-Examples](https://github.com/holomodular/ServiceBricks-Examples) repository for practical examples on hosting and deploying your ServiceBricks foundation. From single, monolithic web applications to distributed, multi-deployment, containerized web applications, these examples provide the building blocks to create and scale your own foundations quickly.

## Documentation

Check out our [ServiceBricks-Documentation](https://github.com/holomodular/ServiceBricks-Documentation) repository for comprehensive documentation on the platform, including guides on using all components and developing your own microservices.

## Official Pre-Built Microservices

Get started quickly with our pre-built microservices:

* [ServiceBricks-Cache](https://github.com/holomodular/ServiceBricks-Cache): Generic data storage microservice with a built in expiration process and a distributed semaphore for cache-level locking for multi-instance deployments.
* [ServiceBricks-Logging](https://github.com/holomodular/ServiceBricks-Logging): Service-scoped or centralized logging and a web request auditing microservice.
* [ServiceBricks-Notification](https://github.com/holomodular/ServiceBricks-Notification): Notification and delivery for emails and SMS messages.
* [ServiceBricks-Security](https://github.com/holomodular/ServiceBricks-Security): Authentication, authorization, and application security with JWT bearer token support for multi-instance deployments.
* [ServiceBricks-Work](https://github.com/holomodular/ServiceBricks-Work): Work queue microservice for reliable, event-driven background processing.

## Trademarks

“ServiceBricks”, "ServiceQuery" and “HoloModular” are trademarks of HoloModular LLC. The MIT License covers code only; it does not grant rights to use our trademarks, logos, or brand assets (including in modified or redistributed versions) without permission.

## About

ServiceBricks is owned and maintained by HoloModular LLC and authored by Danny Logsdon (Founder). Visit our websites at https://HoloModular.com, https://ServiceBricks.com or https://www.linkedin.com/in/danlogsdon to learn more.

![ServiceBricks Logo](https://github.com/holomodular/ServiceBricks/blob/main/Logo.png)  

[![NuGet version](https://badge.fury.io/nu/ServiceBricks.svg)](https://badge.fury.io/nu/ServiceBricks)
![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/holomodular-support/bdb5c7c570a7a88ffb3efb3505273e34/raw/servicebricks-codecoverage.json)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

# ServiceBricks - A Microservices Foundation

ServiceBricks is the cornerstone for building a microservices foundation. Visit http://ServiceBricks.com to learn more.

## Notes from the Author 5/13/2024

### System Status: Semi-Stable

* The latest release v1.0.6 is working well with InMemory providers. There are some issues with the notification microservice using Postgres, Sqlite and SqlServer when processing notifymessages in the background task. Fixes for these will be next week, I'm on vacation! :)

* The new website (ServiceBricks.com) is not currently working. Coming Soon!

* Start a discussion if you need any help.


## Overview

ServiceBricks provides the core architectural patterns, implementation, standardization and governance for your microservices foundation.
It exposes a storage platform agnostic model and repository-based API that works the same for SQL and NoSQL database engines, allowing you to seamlessly switch storage providers transparently to clients.

## Major Features

* Extensive use of generics allowing the compiler to generate most of the required code needed.
* Templated, repository-based, REST API services for quickly exposing standard CRUD methods or new methods you define.
* [ServiceQuery](https://github.com/holomodular/ServiceQuery) to support standardized, polyglot data querying for SQL and NoSQL database engines.
* Business Rule engine and use of polymorphic techniques to build business logic once that can be applied to any supported object.
* Domain-driven design and event-based design for customizing business logic for any supported object and method.
* Background processes, tasks and rules to support asynchronous processing.
* Service Bus engine to support broadcasts of system data, supporting InMemory and Azure ServiceBus (basic - queues) or (standard and premium - topics and subscriptions) providers.
* Supporting SQL and NoSQL database engines with standard support for the following database engines: AzureDataTables, CosmosDb, InMemory, MongoDb, Postgres, Sqlite and SqlServer.
* All packages published via NuGet to quickly build new services and applications quickly.
* Full Xunit unit test package to ensure your microservices work correctly in the platform. Automatically get over 50% unit test code coverage and over 75% integration test coverage with just a few files and lines of code.
* Completely open source containing only three packages (AutoMapper, NewtonSoft.Json, and ServiceQuery) all components used are MIT licensed with no external dependencies.

## Artificial Intelligence Integration

We are currently training AI on how to build new ServiceBricks microservices and are getting fantastic results! More updates coming on this soon!


# Getting Started with Examples

To help you get started, we have created the following repository to store all of our examples.

[ServiceBricks-Examples](https://github.com/holomodular/ServiceBricks-Examples)

This repository provide several examples on how to host and deploy your ServiceBricks foundations. 
From hosting a single, monolithic web application, to hosting several containerized web applications, these examples contain the building blocks needed to create your own foundations quickly.

# Documentation

We have created a repository specifically for documentation for ServiceBricks.

[ServiceBricks-Documentation](https://github.com/holomodular/ServiceBricks-Documentation)

This repository contains all documentation on the ServiceBricks platform, how to use all of its individual components and how to develop your own microservices.

# Official Pre-Built Microservices

We have developed several pre-built microservices to help get you started. View the following repositories for more information:

* [ServiceBricks-Cache](https://github.com/holomodular/ServiceBricks-Cache): This repository is a temporary generic data storage microservice.
* [ServiceBricks-Logging](https://github.com/holomodular/ServiceBricks-Logging): This repository is a service-scoped or centralized application and web request logging microservice.
* [ServiceBricks-Notification](https://github.com/holomodular/ServiceBricks-Notification): This repository is a notification and delivery of emails and SMS messages microservice.
* [ServiceBricks-Security](https://github.com/holomodular/ServiceBricks-Security): This repository is an authentication, authorization and application security microservice supporting JWT token membership for all ServiceBricks microservices.

# About

I am a business executive and software architect with 25+ years professional experience. You can reach me via www.linkedin.com/in/danlogsdon or https://HoloModular.com

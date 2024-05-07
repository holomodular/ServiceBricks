![ServiceBricks Logo](https://github.com/holomodular/ServiceBricks/blob/main/Logo.png)  

[![NuGet version](https://badge.fury.io/nu/ServiceBricks.svg)](https://badge.fury.io/nu/ServiceBricks)
![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/holomodular-support/bdb5c7c570a7a88ffb3efb3505273e34/raw/servicebricks-codecoverage.json)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

# ServiceBricks - A Microservices Foundation

ServiceBricks is the cornerstone for building a microservices foundation. Visit http://ServiceBricks.com to learn more.

## Quick Notes from the Author

### System Status: Unstable

Examples have been uploaded but need some massaging to work. I need about a week to fix the below problems before the examples will be ready. 

* All source code has now been uploaded with all examples! WooHoo!

* All solutions are builing, running unit tests (for InMemory providers) and updating unit test badges for each repo readme. 

* The new website is currently not working yet but the old one is. Check out http://ServiceBrick.com for the time being for documentation only.

* Documentation will be on going, I'm looking at templates to use now.

* Start a discussion if you need any help.

### Current Issues

* Cache Microservice
  
SqlServer migrations broken, have to delete tables then re-run again.

* Logging Microservice

SqlServer migrations broken, have to delete tables then re-run again. WebRequest messages automapper errors, don't turn on in config yet.

* Notification Microservice

DomainQueueProcess service throwing errors, not processing notifymessages.

* Security Microservice

Member NuGet package fix for authentication (fixed code in example).



## Overview

ServiceBricks provides the core architectural patterns, implementation, standardization and governance for your microservices foundation.
It exposes a storage platform agnostic model and repository-based API that works the same for SQL and NoSQL database engines.

## Major Features

* Extensive use of generics allowing the compiler to generate most of the required code needed.
* Templated, repository-based, REST API services for quickly exposing standard CRUD methods or new methods you define.
* [ServiceQuery](https://github.com/holomodular/ServiceQuery) to support standardized, polyglot data querying for SQL and NoSQL database engines.
* Business Rule engine and use of polymorphic techniques to build business logic once that can be applied to any supported object.
* Domain-driven design and event-based design for customizing business logic for any supported object and method.
* Background processes, tasks and rules to support asynchronous processing.
* Service Bus engine to support publication and subscription of system data, supporting Azure ServiceBus (basic) or (standard and premium - topics and subscriptions) out of the box.
* Supporting most SQL and NoSQL database engines with standard support for the following database engines out of the box: AzureDataTables, CosmosDb, InMemory, MongoDb, MySql, Postgres, Sqlite and SqlServer.
* All packages published via NuGet to quickly build new services and applications quickly.
* Full Xunit unit test package, ServiceBricks.Xunit, to ensure your objects work correctly in the framework. Automatically get over 50% unit test code coverage and over 75% integration test coverage with just a few files and lines of code.


## Artificial Intelligence Integration

We are currently training AI on how to build new ServiceBricks microservices and are getting fantastic results! More updates coming on this soon!


# Getting Started

To help you get started, we have created the following repository to store all of our examples: NOW AVAILABLE!

[ServiceBricks Examples](https://github.com/holomodular/ServiceBricks-Examples)

# Official Pre-Built Microservices

We have developed several pre-built microservices to help get you started. View the following repositories for more information:

* [Cache Microservice](https://github.com/holomodular/ServiceBricks-Cache)

This repository is a temporary generic data storage microservice.

* [Logging Microservice](https://github.com/holomodular/ServiceBricks-Logging)

This repository is a service-scoped or centralized application and web request logging microservice.

* [Notification Microservice](https://github.com/holomodular/ServiceBricks-Notification)

This repository is a notification and delivery of emails and SMS messages microservice.

* [Security Microservice](https://github.com/holomodular/ServiceBricks-Security)

This repository is an authentication, authorization and application security microservice supporting JWT token membership for all ServiceBricks microservices.

# About

I am a business executive and software architect with 25+ years professional experience. You can reach me via www.linkedin.com/in/danlogsdon or https://HoloModular.com

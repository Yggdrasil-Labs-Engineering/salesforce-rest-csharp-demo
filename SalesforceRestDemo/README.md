# Salesforce REST C# Demo

Minimal C# console application demonstrating authenticated integration with the Salesforce **REST** **API**.

## Purpose

This project was created as a focused proof-of-concept for:

- Salesforce **REST** **API** integration
- C# **HTTP** client usage
- Bearer-token authentication
- **JSON** response parsing
- Positive and negative integration validation
- Clear **PASS** / **FAIL** reporting

The goal is to keep the implementation intentionally small and easy to explain.

## Current Capabilities

### Positive Validation

The application queries Salesforce Account records using:

```text **GET** /services/data/v65.0/query/?q=**SELECT**+Id,Name+**FROM**+Account+**LIMIT**+5

The application validates that:
- Salesforce returns a successful **HTTP** response
- **JSON** is returned
- the response contains Account records
Example:
Running positive Salesforce **REST** **API** test...
**PASS**: Salesforce **REST** **API** returned Account records.
Records returned: 5

### Negative Validation

The application can also execute a deliberate invalid request: **GET** /services/data/v65.0/sobjects/DefinitelyNotARealObject

The test passes when Salesforce rejects the request with the expected **HTTP** failure. Example: Running negative Salesforce **REST** **API** test... **PASS**: Salesforce rejected the invalid request with **HTTP** **404**.

Running the Demo The application requires two environment variables: SALESFORCE_INSTANCE_URL SALESFORCE_ACCESS_TOKEN

Example PowerShell setup: $env:SALESFORCE_INSTANCE_URL = *[https://your-instance.my.salesforce.com*](https://your-instance.my.salesforce.com*) $env:SALESFORCE_ACCESS_TOKEN = *YOUR_ACCESS_TOKEN"

Do not store Salesforce access tokens in source code or commit them to Git.

### Positive Test

dotnet run

### Negative Test

dotnet run -- --negative

Technology
- C#
- .**NET**
- Salesforce **REST** **API**
- Salesforce **CLI**
- HttpClient
- System.Text.Json
### Project Structure
salesforce-rest-csharp-demo/
└── SalesforceRestDemo/
    ├── Program.cs
    └── SalesforceRestDemo.csproj

Authentication
Authentication is handled outside the application.
The Salesforce **CLI** is used to authenticate to a Salesforce Developer Edition org, and the resulting runtime values are supplied to the C# application using environment variables.
No Salesforce credentials or access tokens are stored in the repository.
### Current Scope
This demo intentionally focuses on a small integration path:
C# Console Application
        ↓
Salesforce **REST** **API**
        ↓
**JSON** Response
        ↓
Validation
        ↓
**PASS** / **FAIL**

### Known Limitations

This is a demonstration project and does not currently include:
- token refresh handling
- retry logic
- production secrets management
- extensive exception handling
- dependency injection
- automated unit tests
- multiple Salesforce object types
These would be appropriate additions for a production implementation.
### Next Steps
Potential future enhancements include:
- invalid authentication testing
- additional **SOQL** validation
- Salesforce record creation and update validation
- structured automated tests
- expanded error handling
- production OAuth / service-account patterns
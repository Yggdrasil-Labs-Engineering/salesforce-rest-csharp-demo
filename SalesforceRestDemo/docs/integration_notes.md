# Salesforce REST API Integration - Engineering Notes

## Purpose

This document provides implementation and handoff notes for the Salesforce REST API demonstration project.

The project is intentionally small and is designed to demonstrate:

- C# integration with the Salesforce REST API
- authenticated REST requests
- JSON response handling
- positive and negative validation
- explicit PASS / FAIL reporting
- secure handling of runtime credentials

## Current Implementation

The application is a C# console program built with .NET.

It connects to a Salesforce Developer Edition org using:

- Salesforce CLI authentication
- runtime environment variables
- HTTP Bearer authentication
- Salesforce REST API
- SOQL queries

The application does not store Salesforce usernames, passwords, or access tokens in source code.

## Architecture

```text
Salesforce CLI Authentication
        ↓
Runtime Environment Variables
        ↓
C# Console Application
        ↓
HttpClient
        ↓
Salesforce REST API
        ↓
JSON Response
        ↓
Validation Logic
        ↓
PASS / FAIL

### Authentication Model

The Salesforce **CLI** is used to authenticate the developer org. The C# application expects two environment variables: SALESFORCE_INSTANCE_URL SALESFORCE_ACCESS_TOKEN

Example PowerShell configuration: $env:SALESFORCE_INSTANCE_URL = *[https://your-instance.my.salesforce.com*](https://your-instance.my.salesforce.com*) $env:SALESFORCE_ACCESS_TOKEN = *YOUR_ACCESS_TOKEN"

These values exist only for the active shell session unless explicitly persisted.

### Security Note
Access tokens must not be:
- committed to Git
- stored in source code
- added to documentation
- included in screenshots
- stored in plain-text configuration files
The repository .gitignore excludes .env files as an additional safeguard.

### Positive Test Flow
The default execution path validates successful Salesforce **REST** access.
Command:
dotnet run

The application executes a **SOQL** query against the Salesforce Account object: **SELECT** Id, Name **FROM** Account **LIMIT** 5

**REST** resource: /services/data/v65.0/query/

The application validates: ## Salesforce returns a successful HTTP response. ## The response body contains valid JSON. ## The totalSize property can be read. ## At least one Account record is returned. Expected result: Running positive Salesforce **REST** **API** test... **PASS**: Salesforce **REST** **API** returned Account records. Records returned: 5

### Negative Test Flow

The application also supports an intentional negative test. Command: dotnet run -- --negative

This sends a request to an invalid Salesforce object: /services/data/v65.0/sobjects/DefinitelyNotARealObject

The expected result is an **HTTP** error response. The test is considered successful when Salesforce rejects the invalid request as expected. Example: Running negative Salesforce **REST** **API** test... **PASS**: Salesforce rejected the invalid request with **HTTP** **404**.

Why the Negative Test Passes
A negative test does not require the system itself to return a successful response.
The expected behavior is rejection.
Therefore:
Invalid request
        ↓
Salesforce rejects request
        ↓
Expected **HTTP** error returned
        ↓
Negative test **PASS**

If Salesforce unexpectedly accepted the invalid request, that would be considered a test failure. ### Code Responsibilities ### Environment Validation The application checks that both required Salesforce environment variables exist before attempting an **API** request. If either value is missing, execution stops immediately. **HTTP** Client HttpClient is responsible for sending **REST** requests to Salesforce. The access token is supplied through the standard Bearer authentication header. ### Request Selection The application uses the --negative argument to determine which resource to call. Without the argument: Valid Account query

With: --negative

the application calls an intentionally invalid resource. **JSON** Validation The successful Salesforce response is parsed with: System.Text.Json

The application reads the Salesforce totalSize property and confirms that Account records were returned.
### Current Test Coverage
The project currently demonstrates:
- successful authentication
- valid Salesforce **REST** **API** access
- **SOQL** query execution
- **JSON** parsing
- Account record validation
- invalid resource handling
- expected **HTTP** failure validation
- explicit **PASS** / **FAIL** reporting

### Known Limitations
This is a proof-of-concept and does not currently include:
- automatic token refresh
- OAuth client-credential management
- service-account authentication
- retry logic
- configurable **API** versions
- structured logging
- dependency injection
- automated unit tests
- Salesforce create/update/delete testing
- multiple Salesforce object types
These would be expected considerations for a production implementation.
Troubleshooting

### Environment Variables Not Configured
Symptom:

**FAIL**: Salesforce environment variables are not configured.

Check: $env:SALESFORCE_INSTANCE_URL $env:SALESFORCE_ACCESS_TOKEN

**HTTP** **401**
A **401** response usually indicates that:
- the access token is invalid
- the token has expired
- the token was copied incorrectly
- the token does not belong to the configured Salesforce instance
Obtain a new token from the authenticated Salesforce **CLI** session and retry.

**HTTP** **404** During Negative Test
This is expected behavior when running:
dotnet run -- --negative

The invalid Salesforce object is deliberately used to verify error handling.

### Future Enhancements
Potential extensions include:
- invalid authentication testing
- Salesforce record creation
- record update validation
- delete validation
- reusable **API** client abstraction
- configurable **SOQL** queries
- structured logging
- formal automated test project
- OAuth production authentication model

### Handoff Summary
This project demonstrates a minimal Salesforce **REST** integration using C#.
The implementation is intentionally compact so that the authentication path, **HTTP** request, **JSON** handling, and **PASS** / **FAIL** logic remain easy to understand and maintain.
The current implementation is suitable as a proof-of-concept and learning artifact, but additional resiliency and security controls would be required for production use.


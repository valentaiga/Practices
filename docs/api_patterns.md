# API design patterns
- [REST API design](#rest-api-design)
    - [HTTP Methods and Status Codes](#http-methods-and-status-codes)
    - [Friendly and Common API Endpoint Names](#friendly-and-common-api-endpoint-names)
    - [GET vs. POST API Bulk Operations](#get-vs-post-api-bulk-operations)
    - [Data cache](#data-cache)
    - [API expansion](#api-expansion)
    - [OpenAPI definition](#openapi-definition)
- [Microservices API design](#microservices-api-design)
    - []()
    - []()
    - []()


## REST API design

### HTTP Methods and Status Codes
To structure your API around the built-in methods and status codes that are already well-defined in HTTP.

### Friendly and Common API Endpoint Names
API design will be much easier to understand if these names are descriptive. 

### GET vs. POST API Bulk Operations
As you're designing RESTful APIs, you’ll want to rely on the HTTP methods and best practices to express the primary purpose of a call. 
For that reason, you don’t want to use a POST to simply retrieve data. 
Nor would you want a GET to create or remove data. You should examine your use cases to determine when to use each.  

Factors:
- **GET** requests can be cached
- **GET** requests are  (in that they can be called any number of times while guaranteeing the same outcome)
- **GET** requests should never be used when dealing with sensitive data
- **GET** requests have length restrictions
- **GET** requests should be used only to retrieve data
- **POST** requests are never cached (unless specified in the header)
- **POST** requests are NOT idempotent
- **POST** requests have no restrictions on data length

### Data cache
Caching partially eliminates unnecessary trips to the server. Returning data from local memory rather than sending a query for each new request can improve your **app’s performance**. 
GET requests are cacheable by default, however, POST requests require you to specify the cache requirements in the header.

### API expansion
Plan for Future Use Cases with API Parameters. 
There are three common types of parameters to consider for your API which can be used together to support very specific queries:
- Filtering
- Pagination
- Sorting

### OpenAPI definition
Perhaps the most common use of an OpenAPI document is to **generate API documentation**, especially an API reference. 
Since the format outlines the ways an API can be called, it contains all the information a developer needs to integrate with the API. 
Plus, some API references don’t include essential details like error codes, so OpenAPI encourages accurate documentation.

## Microservices API design

### 


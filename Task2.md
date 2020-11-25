# Q2 New Feature Development (Search)

## Inputs

### Functional requirements:

- Deliver a REST API that is able to search for an image. The REST API must pass the image search requests in these postman tests (and images). You can find postman here.

### Non Functional requirements:

- Backend
  - The backend must be built in one of the following programming languages:Java
    - Javascript
    - C++
    - C#
  - The endpoint for the search uses the GET method with query parameters per search field and current “page”.
  - The application must return 200 (HTTP Response code) in case of results found.
  - The application must return 204 (HTTP Response code) in case of results not found.
  - The application must return 500 (HTTP Response code) in case of any unexpected error.
  - All exceptions are handled safely.
  - The image information must be retrieved from AWS Elastic Search (id, description, file type, and size).
  - The response results are truncated to 20 (per “page”).
  - Unit Tests 80% coverage.
  - Integration Tests covering each one of the API response codes.
  - Use Credentials File for the AWS services authentication
    - Java
    - Javascript
    - C++
    - C#

### Out of scope

- Functional
  - Authentication
  - Searching support beyond AND clause (no OR, CONTAINS, NOT, etc)
  - Case sensitive search
  - UI/FrontEnd
- Non-Functional
  - Logging abilities

## Outputs

- Link to a zip file of the whole solution
- Product code
- Unit-test code
- Integration-test code
- Script to run the solution (backend and migration from AWS RDS to Elastic Search) from a single command.
- Readme file with precise instructions on how to execute the script including preconditions/dependencies required at the system level.
- Link to a demo video with the successful execution of the postman tests, evidence of unit and integration test coverage, and evidence of Sonarqube not reporting any errors. Use Loom if you don’t have a preferred tool.

## Grading Criteria: 

Check your work against these criteria before you submit, to make sure you’re sending us your best work.

- Zip file of the whole solution includes:
- Solution files
- There is a POST method on a controller that receives a multipart upload.
- There are IT’s ensuring that the controller responds accordingly to the speced response codes.
- There is a service wrapping up all business logic
- Code uses dependency injection
- Code uses an SDK to connect to RDS
- Any related RDS and S3 configuration is abstracted in configuration settings
- There isn’t a “transaction committed” to RDS before the S3 bucket image is uploaded
- S3 bucket keys are unique
- Read me file
- Link to a demo video includes:
- Postman Test scenarios demonstrated successfully and without visible errors
- Code coverage is shown with test coverage >= 80%
- Sonarcube not reporting any errors
- Code passes Quality Bar
- Code follows SOLID principles.  Sonarqube will flag some, but not all.
- Magic values are not used
- There are no duplicated code blocks introduced by this PR
- There are no unused references, methods or variables
- There are no raw/high-level exceptions thrown, caught or used (unless they can't be replaced by specific ones)

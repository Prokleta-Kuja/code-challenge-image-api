# Q1 New Feature Development (Upload)

## Inputs

### Functional requirements:

- Deliver a REST API that is able to upload an image.
  - The file upload must be restricted to PNG or JPEG types.
  - The image must not exceed 500KB.
  - The user uploads an image and provides a description.
  - The application displays helpful user-friendly information when an error occurs.
- The REST API must pass the image upload tests from these postman tests (and images).

### Non Functional requirements:

- The REST APIs must be built in one of the following programming languages:
  - Java
  - Javascript
  - C++
  - C#
- The endpoint for the upload uses the POST method
- The application must return 201 (HTTP Response code) in case of a successful upload.
- The application must return 400 (HTTP Response code) in case of any validation failure.
- The application must return 500 (HTTP Response code) in case of any unexpected error.
- All exceptions are handled safely.
- API must be resource-based
- API must have proper HTTP verb and status
- API must have integration tests covering all cases leading to different HTTP statuses
- Business logic should guarantee data model constraints
  - If database schema requires a uniqueness constraint on a column, then that constraint should be validated at business layer
  - If the entity class requires a field not to be blank, then that constraint should be validated at business layer
- The uploaded images must be physically stored in an AWS S3 bucket (Create a free-tier AWS account here if you don’t already have one)
- The image information (description, file type, and size, besides a primary key) must be stored in AWS RDS.
- You must handle logical consistency (if DB or S3 operation fails, ensure the remaining record is removed/rolled back).
- There is a migration to automatically set up the DB database schema (script to create database table automatically).
- Unit Tests with 80% coverage.
- Integration Tests covering each one of the API response codes.
- Use Credentials File for the AWS services authentication
  - Java
  - Javascript
  - C++
  - C#
- Adhere to Code Quality Bar:
  - Code follows SOLID principles.  Sonarqube will flag some of these, but not all.
  - Magic values are not used
  - There are no duplicated code blocks introduced by this PR
  - There are no unused references, methods or variables
  - There are no raw/high-level exceptions thrown, caught or used (unless they can't be replaced by specific ones).

### Out of scope

- Functional
  - Authentication
  - Multiple documents upload support
  - UI/FrontEnd
- Non-Functional
  - Performance concerns
  - Logging abilities

## Outputs

- Link to a zip file of the whole solution
- Product code
- Unit-test code
- Integration-test code
- Script to run the solution (including migration) from a single command. Include a Readme file with precise instructions on how to execute the script, including any preconditions/dependencies required at the system level.
- A demo video showing:
- Walkthrough of the solution
- Evidence of Unit Test and Integration test coverage
- Execution of each PostMan test individually.
- Evidence of images loaded onto S3 and Meta data in the DB
- Evidence of Sonarqube not reporting any errors.
- You may use the video recording tool of your choice, but if you don’t have a preferred tool Loom is an easy option. Create a free account, download their Chrome plugin or stand-alone app and start recording. To confirm that it is set up correctly you may want to prepare a test video and send it to yourself

## Grading Criteria: 

Check your work against these criteria before you submit, to make sure you’re sending us your best work.

- Zip file of the whole solution includes:
- Solution files
- There is a GET method on a controller that receives all args as parameters.
- There are IT’s ensuring that the controller responds accordingly to the speced response codes.
- There is a service wrapping up all business logic
- Code uses dependency injection
- Code uses an SDK to connect to ES
- Any related ES configuration is abstracted in configuration settings
- Read me file
- Link to a demo video includes:
- Postman Test scenarios individually executed without errors.
- Code coverage is shown with test coverage >= 80%
- Sonarcube not reporting any errors
- Code passes Quality Bar
- Code follows SOLID principles (Sonarqube will flag some but not all)
- Magic values are not used
- There are no duplicated code blocks introduced by this PR
- There are no unused references, methods or variables
- There are no raw/high-level exceptions thrown, caught or used (unless they can't be replaced by specific ones)

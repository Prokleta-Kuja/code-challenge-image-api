# Overview

The team you are applying for is laser-focused on writing new code.   You'll get a spec, write the code, and write the tests. That's it. No diagramming, no writing documentation, no scrum meetings. Our team is dedicated to test-driven development so our code writing includes unit, integration, and mutation tests to meet our no-nonsense expectations for code quality.  To be successful on our team you must be an expert programmer in at least one of our 4 main languages with the desire and ability to add languages and technologies to your tool set.

Our most successful team members have the ability to receive a spec, research any new technologies required and return code that delivers exactly what is written.  They are capable of quickly integrating and debugging new technologies so that we can ensure simplicity in our code, avoiding inconsistencies or redundancies.

This is your chance to demonstrate more of your development skills and your ability to learn quickly.  Research any technology you are less familiar with to deliver a complete solution and show us your commitment to high quality, maintainable code complete with unit and integration test code.

# Configuration

In `WebApi/appsettings.json`:

- update `ImageDbContext` connection string to point to accessible Microsoft Sql Server instance with create database permissions.
- update `S3ImagesBucketName` in `AWS` section with valid bucket name
- remove `ServiceURL` & `ProfilesLocation` from `AWS` section to use configuration file at default location

# Local docker dev environment setup

## AWS credentials file

Create file `.aws/credentials` with contents:

```
[development]
aws_access_key_id = a
aws_secret_access_key = b
```

## Sonarqube

Script to run scanner:

`.vscode/analyze.sh`

Command to run local docker instance (requires bridge network named dev):

`docker run -d --network dev --name sonarqube -e SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true -p 5000:9000 sonarqube:latest`

Login with username `admin` and password `admin`, then create project `image-api`

## SQL Server

Command to run local docker instance (requires bridge network named dev):

`docker run -d --network dev --name sql -p 1433:1433 -e ACCEPT_EULA=Y -e SA_PASSWORD=TmWQK6hhFGYqs6ZD mcr.microsoft.com/mssql/server`

## LocalStack

Command to run local docker instance (requires bridge network named dev):

`docker run -d --network dev --name localstack -p 4566:4566 -e "SERVICES=s3" -e "DEFAULT_REGION=eu-west-2" localstack/localstack:0.12.2`

After startup create the bucket with:

`docker run --network dev --rm -it -e AWS_ACCESS_KEY_ID=a -e AWS_SECRET_ACCESS_KEY=b amazon/aws-cli --endpoint-url=http://localstack.dev:4566 s3 mb s3://images`

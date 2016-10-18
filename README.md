# Crossover
Project developed in 48 hours, for a hiring process.

It was requested to build a Web API webservice to logging data for source applications, containing three methods:
- Register: Register a new application, generating an exclusive and randomly application ID and secret;
- Auth: Generates an access token for the provided application ID and secret;
- Log: Actually logs the provided data of the specific application;

The project had still a few more specifications:
- The authentication data (application ID and secret) must be provided in the request header, into the form of the HTTP Basic Authentication;
- It must have a restriction for the number of calls;
- The session's lifetime had to be configurable through the database;
- An MVC application consuming the WS (not finished);
- A demo video walking through the application's features;
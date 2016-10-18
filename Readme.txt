1. Install and configure
- Build MessageLogger.Services;
- Copy the files Global.asax and Web.config, and the bin directory into the disered hosting location;
- In the IIS, create an Application with a .NET 4.0 application pool, refering to the location difened at the previous step;

1.1. Updating data model
- Run script "Crossover.sql" (Felipe_Lopes\SoftwarEengineer_.NET\Source\Database\Crossover.sql), into the the "Crossover" database;

1.2. Configuring session lifetime
- The default session lifetime is 60 minutes. In order to change this value, it must update the column [session_lifetime_minutes] of the [session_configuration] table. Reference below:
USE [Crossover]
UPDATE [session_configuration] SET
	   [session_lifetime_minutes] = 90
WHERE [configuration_id] = 1

1.3. Enabling log
- On the root folder of the application hosting location, creates a directory called "Log";
- In the Web.config, uncomment the <switches> and <trace> sections, of system.diagnostics;
- Give write and read permissions for the application's application pool user, into the "Log" folder;

2. Configure SqlServer connection
- In the Web Services' Web.config file, change the "Data source", "User ID" and "Password" values, of the "Crossover" connection string;

3. Prepare source code to build properly
- There are no steps required to build;

4.Assumptions and missing requirements
- There is a slight discrepancy between the data model and the API description document. In the document, the "application_secret" is described as 32 characters long, but in script data model, this same column is defined as varchar(25). I chose to follow the data model definition;

5. Feedback
- There is no feedback to provide about the assignment itself, but you may find in the code some "TODO"s of mine, over improvements that I would have made, if I had just a little bit more time. But I do comprehend that the provided amount of time is enough to deliver this solution;

6. Unit testing
- In order to unit test the data layer, it's necessary to configure the "Crossover" connection string (just like step 2) in the MessageLogger.Test's App.config, and execute the unit tests in the following order:

Application.Create
Application.Update
Application.RetrieveAll
Application.RetrieveByID

ApplicationCall.Create
ApplicationCall.Update
ApplicationCall.RetrieveAll
ApplicationCall.RetrieveByID

Log.Create
Log.Update
Log.RetrieveAll
Log.RetrieveByID

ApplicationSession.Create
ApplicationSession.Update
ApplicationSession.RetrieveAll
ApplicationSession.RetrieveByID

SessionConfiguration.Create
SessionConfiguration.Update
SessionConfiguration.RetrieveAll
SessionConfiguration.RetrieveByID

SessionConfiguration.Delete
ApplicationSession.Delete
Log.Delete
ApplicationCall.Delete
Application.Delete
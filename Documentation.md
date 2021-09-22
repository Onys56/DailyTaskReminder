# Architecture
Nearly all methods are documented by comments in the source files so it would be needles to repeat it here. Instead you can read about the overall structure of the program and if needed look for more details in the source code.

The program is currently divided into 4 projects (not counting unit tests):
## 1. Tasks
Contains definition for all task types. The abstract Task class serves as a interface for other parts of the program. Each task provides information about its status (if it is finished and if the reminder has been sent) and about the next remind time as well as the next deadline time. Each task is also able to serialize itself into file and deserialize back.
### Task serializer
Provides function for serializing and deserializing list of tasks.
### Task validator
Is able to validate tasks, checks if all tasks have unique names and calls the Validate function of each task. Error handling is done through `TaskNotValidException` that has the error message specified so it can be displayed to the the user. 
## 2. Reminders
Provides static class that is able to serialize and deserialize reminders into a static dictionary from which they can be retrieved by name. Reflection is used in both cases to make adding additional reminders as smooth as possible. Holds definition for the `IReminder` interface that all reminders must implement.
### Adding reminders 
Simply add a class that implements `IReminder` interface. Make sure to use only string fields (not properties), these fields will be automatically serialized and deserialized.
## 3. DailyTaskReminder
The main project of the program, holds the "business logic" and HTTP server.
### Options
Contains definition for the Option class and a very simple parser. Parses the command line arguments and then starts the program with the parsed options.
### Program
Schedules reminders and task resetting after deadline. That is done using two timers that find the next soonest reminder or deadline and then wait until the time comes, then they send reminders or reset the task and again look for the soonest reminder or deadline.
### Server
HTTP (RESTful) server that currently can handle two types of requests. GET request - returns tasks and all information about them. POST request with query `name` specifying the name of the task - marks the task as finished.
## 4. Configurator
WinForms application for configuring the tasks and reminders. Mostly just on change events that update the user interface and the internal data structures with the tasks or reminders.

Again for reminders reflection is used to find all fields of a specific reminder type so that the configurator will work even if the user adds in new reminders in the source code.
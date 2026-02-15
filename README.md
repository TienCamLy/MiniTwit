# Setting Up the Repository
TODO: Add installation instructions

# Processes & Workflows
Contributions to this repository should be structured as necessary following these guidelines:
- Project Work Activities are tracked through [Trello](https://trello.com/b/a72cgcKI/devops-minitwit)
    - Trello tasks are linked to relevant GitHub Issues / Branches / PRs using the GitHub PowerUp
    - Tasks should be small enough that they may be solved within a short timeframe and individual from other tasks.
- All Work should be done on branches representing a single task. In the case of multiple closely related tasks, these may be implemented on the same branch as necessary.
- Branches shall not be merged directly to main, but peer reviewed using PRs before merging.

# Documentation of choices and issues
## Choice of Programming Language / Tech Stack
We chose to refactor to C# with [Razor Pages](https://learn.microsoft.com/en-us/aspnet/core/razor-pages).
This choice was made on the basis of picking a programming language that half of the group already knew and the other half found feasible to work with. Razor Pages was picked for the same reason, as we were intent on the familiarity for some of the group members gaining us the opportunity to move faster along with the refactoring.

We find Razor Pages to be a good choice, as it supports HTML templating similar to how Jinja is used within Flask applications. One of the main challenges was keeping track of the logged in user within a session, as each bit of code for each page was siloed in its own .cs file. We managed to keep common things common, such as Layout and database functionality, such that the page-specific code mere constituted the functionality of each page-function from the Flask application.
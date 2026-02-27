### Week 1 - Python Refactoring
* We added argument parsing support to avoid errors with the port possibly being taken by another application (like AirPlay Receiver on MacOs). `Const` provides the desired functionality by calling `python minitwit.py --port` *(Note: we changed it to `default` in week 4 to make it `python minitwit.py` instead)*
* For the remaining changes, we followed the `README_TASKS.md` guide from `session_01`
* We created `Release 1.0.0` from the above changes

### Week 2

### Week 5
* Introduced EntityFrameworkCore to handle database interactions, replacing direct SQL queries with the .db file.
* Refactored the codebase to use the onion structure, splitting the code into Core, Infrastructure and Web projects.
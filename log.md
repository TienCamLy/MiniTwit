### Week 1 – Python Refactoring (Jan 30 – Feb 5)
* We added argument parsing support to avoid errors with the port possibly being taken by another application (like AirPlay Receiver on MacOs). `Const` provides the desired functionality by calling `python minitwit.py --port` *(Note: we changed it to default in week 4 to make it `python minitwit.py` instead)*
* For the remaining changes, we followed the `README_TASKS.md` guide from `session_01`
* We created `Release 1.0.0` from the above changes
* Other: gitignore, requirements, db path and test fixes, control.sh adapted with shellcheck

### Week 2 – Refactor to C# (Feb 6 – Feb 12)
* Refactored to C# with Razor Pages, as this is a well-known combo for part of the group.
* Added compose files and Docker configs to allow containerization.
* Implemented DBContext and pages: PublicTimeline, UserTimeline, Register, Login (with password hashing), Logout, MyTimeline; follow/unfollow and posting messages; Gravatar on public timeline; user session and auth UI.
* Release delayed due to unforeseen issues with the refactoring.

### Week 3 – API Stub & Deployment (Feb 13 – Feb 19)
* Added Swagger/OpenAPI spec and generated API stub (OpenAPI Generator); integrated stub into main project and added Makefile targets.
* Implemented Minitwit API endpoints (messages, follow/unfollow, register, /latest), .env for API token, 403 on wrong token.
* Added Vagrantfile and DigitalOcean deployment (Vagrant + Makefile: deploy, provision, clean).
* Added missing parts from week 2: follow/unfollow behaviour, flash messages, follow status for authorized users only.

### Week 4 – CI/CD & Polish (Feb 20 – Feb 26)
* Added GitHub Actions workflow for deployment; documented deployment and choices in README.
* Renamed `python-version` to `legacy-version`; merged API stub into main project; db permissions and README updates.
* Made the default port work as intended; small tweaks to control.sh and progress logging; clarified log for week 1 (PR #24).

### Week 5 – Onion Architecture & EF Core (Feb 27 – Mar 5)
* Introduced EntityFrameworkCore to handle database interactions, replacing direct SQL queries with the .db file.
* Refactored the codebase to use the onion structure, splitting the code into Core, Infrastructure and Web projects.
* Added reset-database workflow; first EF migration; User/Message/Follower repositories; Identity hashing and login verification; HttpContext.SignInAsync for auth.
* Paginators on public and my timeline; FollowUser/UnfollowUser; MyTimeline shows author and followees’ messages.

### Week 6 – Monitoring & workflow (Mar 6 – Mar 12)
* Added basic monitoring with Prometheus and Grafana (compose, scrape config, datasource/dashboard provisioning, metric storage persistence).
* Metric service and metrics endpoint for Prometheus; dashboard.json for Grafana.
* Data migration from legacy user/message/follower tables into EF/Identity schema.
* Workflow deploy trigger and .env file location for deployment.

### Week 7 – DevOps & repo polish (Mar 13 – Mar 19)
* Chore: split app and monitoring layout; workflow env path for compose.
* Merged monitoring and restructure branches; added PR template.

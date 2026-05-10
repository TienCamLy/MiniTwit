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
* Migrated to PostgreSQL database from SQLite

### Week 7 – DevOps & repo polish (Mar 13 – Mar 19)
* Chore: split app and monitoring layout; workflow env path for compose.
* Merged monitoring and restructure branches; continued deployment workflow fixes (SSH/options, known_hosts, env).
* PostgreSQL migration was merged experimentally then **reverted** (#37); stayed on the existing SQLite + EF setup due to some parts not working.
* Grafana dashboards updated for clearer layout / beautification (#36).
* Added GitHub **pull request template** (#38);
* **Removed legacy Python MiniTwit** from the repo (C# app is the single source); SonarCube-driven cleanups on API models, repositories, and pages (#39 and related).
* Security / hygiene: dropped obsolete `python-version` usage and addressed prominent security findings (#39).

### Week 8 - Logging (Mar 20 - Mar 26)
* Added new droplet for running test-deployments to avoid failing on PROD.
* **Loki:** Added `monitoring/loki/loki-config.yaml` and `loki-dev-config.yaml` for Grafana Loki 3.6.x (tsdb / v13, `http_listen_address: 0.0.0.0`). Extended `monitoring/compose.yaml` with `loki`, `loki-dev`, and `prometheus-dev` (dev Prometheus on host port 9091 with `--web.listen-address=0.0.0.0:9091`; TSDB path aligned with the `/prometheus` volume).
* **Grafana:** Updated `datasources.yml` so grafana now contains an instance for prod and dev alike.
* **Promtail:** Root `compose.yaml` runs Promtail beside the app; added `promtail/promtail-config.yaml` (Docker service discovery → remote Loki), `promtail/env.example`, and ignored `promtail/.env` in `.gitignore`.
* Added limit to message length to avoid potential performance issues. 
* Created new dashboards to show the Loki loggings of PROD and DEV.
* Added Up-time metrics to monitoring dashboards
* Changed message publish date to show UTC+1

### Week 9 - Docker Swarm (Mar 27 - Apr 9)
* Added local development connecting to test database and ensured QA workflow connects to the test database as well.
* Added Docker Swarm, such it contains three replicas. The PROD droplet functions as the manager node, while the two other droplets act as worker nodes.  

### Week 10 (Apr 10 - Apr 16)
* **Grafana monitoring dashboard (PROD):** Reworked `monitoring/grafana/dashboards/dashboard.json` for a clearer layout. Where appropriate, panels now use **rates** instead of raw **totals** so traffic and counters are easier to interpret over time. Added **subsections / row groupings** so visuals are easier to scan and to separate concerns when analysing monitoring data.
* **Monitor deployment workflow:** Dropped **`--force-recreate`** (and rely on `docker compose up -d --build` without recreating all services) so Prometheus/Loki/Grafana **named volumes** are not wiped on each deploy.
* Specified versions for prometheus and grafana for project version consistency, so as to avoid sudden issues arising from updates to them.
* Added function to get message count for the MyTimeline page, and fixed paginator functionality for both MyTimeline and UserTimeline. 

### Week 11 (Apr 17 - Apr 23)
* **Monitor deployment / Grafana:** After `docker compose up -d --build`, the workflow now runs **`docker compose restart grafana`** so Grafana reloads provisioning on every deploy. Otherwise the Grafana container often stayed running when only bind-mounted dashboard JSON changed, so dashboards stored in the **`grafana-storage`** volume did not pick up repo updates.
* Removed login_success/failure metrics and their associated graph in grafana as they ceased to work, were redundant, and were primarily implemented as a basic first graph example to test grafana. 
* Implemented SonarQube's security recommendation to not expand secrets inside run blocks, instead expanding it in an environment block and referencing that in the run. 
* Changed CI/CD workflow to handle the Docker Swarm changes. 
* **Simulator `latest` counter:** Dropped the in-memory static field; the value now lives in Postgres (`SimulatorLatest`, one row, `latest_id`, EF migration). Endpoints read and update that row, so it survives restarts and stays shared when several instances talk to the same database.

### Week 12 (Apr 24 - Apr 30)
* GitHub Actions **PR validation** workflow: on pull requests to `main`, run `make test-all` (Docker build + tests).
* **`tests/` layout:** API simulator scenario moved to `tests/API_Spec/`; simulator uses **`API_TOKEN`** from `.env` / secrets instead of hardcoded credentials.
* **Selenium UI tests** under `tests/selenium/` with Dockerfile and compose; Makefile targets `test-ui-selenium`, `test-api-simulator`, `test-all`.
* Root **`compose.yaml` / Makefile** updated so app build, API simulator, and Selenium tests run consistently in CI and locally.
* Simulator and UI tests now use a remote postgres-based db `minitwit-test-db` to run validation. The database tables are truncated at the beginning of each workflow ensuring state consistency.
* Cleanup of unused images on `webserver-test` before running tests to address running out of space issues.
* Added additional debug printouts to `test-api-simulator`

### Week 13 (May 1 - May 7)
* Added `.mailmap` file to consolidate authors into persons
* Added `Docker-Scout` to the CD workflow, which scans for vulnerabilities.
* Added `Codeql` but its not a workflow file. its a setting enabled within github that automatically scan our code
* Added two new static analysis tool to `continuous-QA-deployment`, `hadolint` for testing the linting of Dockerfiles and Roslynator for analyzing the C# code. 
* **Monitor deployment / Docker Swarm**:** Migrated the workflow and the monitoring services to Docker Swarm by deploying the services to the cluster through Webserver (manager node). 
* Reviewed the idempotence of our configuration files. Applied small changes to both Dockerfiles; reviewed both Vagrantfiles but found no issues; applied changes to the Makefile; updated the readme.

### Week 14 (May 8 - May 14)
* Updated workflow action versions to be compatible with the upcoming GitHub Actions runtime upgrade to Node.js 24 on June 2nd.
* **Terraform / IaC on DigitalOcean:** Added `infrastructure/` with **dev** and **prod** root modules, **remote state** via DigitalOcean Spaces (`backend.tf` / `backend.tfvars`, `terraform init -backend-config=…`), and documented prerequisites, secrets (`do_token`, SSH key paths), and `plan` / `apply` with `dev.tfvars` / `prod.tfvars` in the README.
* **IaC Modules:** `do-ssh-key` (upload SSH public key), `do-single-droplet` (single VM + compose file artifact for provisioning), `do-docker-swarm` (Swarm leader + worker droplets and stack from compose), `do-public-ip` (floating IP on the target droplet), `do-postgres-db` (managed PostgreSQL with firewall rules so only the relevant droplet(s) can reach the DB).
* **Environments:** **Dev** — one droplet and `minitwit-test-db`. **Prod** — Swarm layout (leader `webserver`, two workers) and `minitwit-db`, with DB firewall `droplet_firewall_entries` covering leader, managers, and workers; tightened firewall configuration toward **one firewall resource with multiple allowances** instead of one per droplet where applicable.
* Small **`.gitignore`** updates for Terraform artifacts alongside the new tree.
* Terraform Import for all existing resources in digital ocean to have non-destructive swap-over and allow managing existing resources using terraform going forward.
* Fix the Continuous Deployment Terraform Apply step to have the auto-approve flag.

# ITU-MiniTwit
A course project as part of "DevOps, Software Evolution and Software Maintenance, MSc (Spring 2026)" at IT-University of Copenhagen.

## Deployment of Infrastructure using Terraform
### 0. Prerequisites
1. An account within Digital Ocean
2. A "Spaces Object Storage" S3 bucket inside Digital Ocean, to manage the backend of terraform.
3. Terraform installation.

### 1. Cloning the Repo
Start by cloning the repo by running
```
git clone https://github.com/TienCamLy/MiniTwit.git
```
Then navigate into the freshly created local clone:
```
cd MiniTwit
```

### 2. Initializing the backend
Run the following in your terminal from within the environment you would like to initialize: (`infrastructure/environments/[dev|prod]`)
```
export AWS_ACCESS_KEY_ID="<your_spaces_access_key>"
export AWS_SECRET_ACCESS_KEY="<your_spaces_secret_key>"

cd infrastructure/environments/prod
terraform init -backend-config=backend.tfvars
```
Note, that initializing the backend is the first step of any terraform process and is done initially to ensure file structures and state files exist that can then be used in other terraform commands as well as to initialize any submodules etc.

### 3. Provisioning and Planning using Terraform
Once you have created new infrastructure resources or changed existing resources you can initially "plan" and later "apply" (provision) the resources and/or updates.
Note, that in order to set up to providers some secrets are needed, which should be generated from source systems and can be provided in one of the following two ways:
1. Append a new line to the `*.tfvars` with `<var_name> = "<secret_value>"`
2. Set an environment variable named `export TF_VAR_<var_name>="<secret_value>"`

#### Secrets
- `do_token` - a PAT token generated from within Digital Ocean
- `pub_key` - Path to your public key file. Should match `pvt_key`.
- `pvt_key` - Path to your private key file. Should match `pub_key`.

#### Commands
Once the secret variables are set up, you can run the following command in your terminal from within the environment you would like to initialize: (`infrastructure/environments/[dev|prod]`)
```
terraform plan --var-file="[dev|prod].tfvars"
```
If the resource modification look as you expect, you can provision them:
```
terraform apply --var-file="[dev|prod].tfvars"
```

### Migration into Terraform from Vagrant / Click-Ops

Anything you built in the DigitalOcean UI (or only ran locally in Vagrant) needs to be imported into Terraform state files before they can be managed i IaC. You write it up in `.tf` files like everything else, then run `terraform import '<address>' '<id>'` from `infrastructure/environments/dev` or `prod` once the [backend](#initializing-the-backend) is initialized. Import only updates state — it does not generate config — so run `plan` afterwards and adjust until Terraform agrees with what actually exists (image slug vs id, SSH key material, tags, and so on) - in some cases items may state that a certain change "forces recreate", but you can get around this by defining the "ignore_changes = []" on the relevant attributes under the lifecycle sub-block. Note, that ignoring changes should only be done for things that you know should NEVER change over the entire lifecycle of a resource and only is part of initialization.

For import ids we mostly grabbed them straight from the URLs: droplet number from `/droplets/…`, database UUID from `/databases/…`, floating IP as the IPv4 string. SSH keys use their numeric id from `doctl compute ssh-key list`, not the fingerprint. A few things bit us along the way: `for_each` needs predictable keys if values are droplet ids; DigitalOcean tends to replace droplets when the `ssh_keys` attributes changes and sometimes the droplet is registered with a local `image` specific to only that droplet (instead of a more general `ubuntu` image or similar).

## DevOps Principles
The group adheres to the "*Three Ways*" characterizing DevOps (from "The DevOps Handbook") by the following:
- **Flow**:
  - To make our work visible, we make use of a visual work board by using Trello. This consists of assigning group members to small, self-contained tasks (ensuring small batch sizes) and displaying the current status of them. Every time a task needs to be done, it is added to the board first. We structure our pull requests accordingly to ensure small, atomic changes to the main branch. Our workflow is described in more detail in the [Processes and Workflows](https://github.com/TienCamLy/MiniTwit?tab=readme-ov-file#processes--workflows) section.
  - To increase visibility, we have also introduced a log (log.txt in the root path) where we write down design decisions made during development each week. A GitHub webhook has been added to our main communication channel, so that all members get notified whenever a change to the repository has been made.
  - We ensure limiting Work In Process (WIP) by, as a general rule, allowing one group member to work on one ticket at the same time. If someone wants to take on new work, they either have to finish their previous ticket or report it as needing help.
  - To improve the flow, we automate the process by using workflows. This includes setting the environment up and building/deploying the application.
  - We don’t have any hard constraints limiting our flow, save for the development pace (a constraint as close to the developers as possible) and PR acceptance (which can potentially take up to a day but is usually much faster)
- **Feedback**:
  - The workflows provide us feedback on whether the automated build succeeded or not. This happens every time a commit has been pushed to the main branch.
  - For quality control, every pull request must be reviewed by another group member.
- **Continual Learning and Experimentation**:
  - We don't blame group members for trying to solve a problem and failing; instead, we appreciate the work they did, and let other people help if needed. For this reason, we have added a "Need help" status in Trello. If anyone is stuck with a specfic problem and is unsure how to proceed, they can also write on discord; the conversation is always focused on how to proceed best instead of simply critisizing. This creates safety culture.
  - After each solved issue, a group member posts an update to the discord groupchat about what they did, and how they solved the problem. If anything is unclear, other group members may request a presentation of a new solution in-person. This enables transforming local discoveries into global improvement by openly sharing information that other group members might find relevant in their work.
  - If our process during the past week didn't seem streamlined during a Friday meeting, we introduce improvements right away. For this reason, for example, we decided to log our work in Trello. This way we improve our daily work continuously. 

## Processes & Workflows
Contributions to this repository should be structured as necessary following these guidelines:
- Project Work Activities are tracked through [Trello](https://trello.com/b/a72cgcKI/devops-minitwit)
    - Trello tasks are linked to relevant GitHub Issues / Branches / PRs using the GitHub PowerUp
    - Tasks should be small enough that they may be solved within a short timeframe and individual from other tasks.
- All Work should be done on branches representing a single task. In the case of multiple closely related tasks, these may be implemented on the same branch as necessary.
- Branches shall not be merged directly to main, but peer reviewed using PRs before merging.

### Naming Conventions
- Practical changes not related to a particular feature: *branches* - 'chore/XXX' *PRs* - 'Chore: XXX'
- Changes related to fixing bugs: *branches* - 'bug/XXX' *PRs* - 'Bug: XXX'
- Changes related to writing/running tests: *branches* - 'test/XXX' *PRs* - 'Test: XXX'
- Changes related to deployment/CICD flows: *branches* - 'deploy/XXX' *PRs* - 'Deploy: XXX'
- Changes related to Refactoring: *branches* - 'refactor/XXX' *PRs* - 'Refactor: XXX'
- Changes related to API implementation: *branches* - 'api/XXX' *PRs* - 'API: XXX'

## Documentation of choices and issues
### Choice of Programming Language / Tech Stack
We chose to refactor to C# with [Razor Pages](https://learn.microsoft.com/en-us/aspnet/core/razor-pages).
This choice was made on the basis of picking a programming language that half of the group already knew and the other half found feasible to work with. Razor Pages was picked for the same reason, as we were intent on the familiarity for some of the group members gaining us the opportunity to move faster along with the refactoring.

We find Razor Pages to be a good choice, as it supports HTML templating similar to how Jinja is used within Flask applications. One of the main challenges was keeping track of the logged in user within a session, as each bit of code for each page was siloed in its own .cs file. We managed to keep common things common, such as Layout and database functionality, such that the page-specific code mere constituted the functionality of each page-function from the Flask application.

When migrating to another database, PostgreSQL was chosen as it was an available option as a managed database on DigitalOcean, and PostgreSQL is convenient to integrate with EF Core.

### Choice of Deployment Infrastructure
We choose to use GitHub Actions to deploy our application, as it is a native functionality that does not require additional setup. For this project, we are not dependent on 100% uptimes and do not mind the 1 minute runtime of our workflow. Keeping the deployment within GitHub also means we do not have to expand our tech stack with additional tools and connections, which would otherwise increase the complexity unnecessarily for the project scope.

### Choice of logging Infrastructure
We use **Grafana Loki** with **Promtail** and visualize logs in **Grafana**, alongside our existing Prometheus setup. Loki indexes metadata (labels) rather than full log text, which keeps storage and operational cost reasonable for a course project while still supporting useful queries with **LogQL**. Promtail runs on each application droplet, discovers Docker containers via the host socket, and ships stdout/stderr logs to Loki over HTTP, while the monitoring server hosts Loki and Grafana so logs stay centralized without running heavy logging agents on the metrics box. This stack is well documented, fits naturally next to Grafana dashboards we already use, and aligns with our goal of observable deployments with minimal extra moving parts.

### Choice of Update Strategy

We choose to use **Rolling Updates** with Docker Swarm.

#### Motivation

Rolling updates allow us to deploy new versions of the MiniTwit application **without downtime** by gradually replacing running containers with updated ones. Instead of stopping the entire system containers are updated one at a time. This ensures that the service remains available throughout the deployment.

This strategy fits well with our system because:
- We are using Docker Swarm, which already supports rolling updates
- Our application uses containers making it easy to run multiple instances simultaneously
- It does not require running a second system, which keeps resources low and simplifies our setup

#### Tradeoffs

Rolling updates are simple and efficient but they come with some limitations:
- Multiple versions of the application may run at the same time during deployment
- Rollbacks are slower compared to blue-green deployment, as containers must be updated again

#### Why not Blue-Green Deployment?

An alternative strategy is blue-green deployment, which provides instant rollback and avoids running mixed versions. However, it requires maintaining two identical systems and to switch traffic between them. Given our setup and project scope, this added complexity is not worth it

### Choice of Static Analysis Tools

We use the following static analysis tools in our CI pipelines to improve code quality. None of these tools modifies code on their own; however, pull requests don't pass the quality gate if one of them issues errors.
- **Dotnet Format**, a build-in .NET SDK code formatter. We use the default rules from the SDK-provided .editorconfig files to format our C# code in the `razor-pages` folder. If a pull request doesn't follow these rules, the tools issues an error and fails the CI pipeline. `make auto-lint` can be run to fix the errors, but it's not run on its own against the project repository.
- **Roslynator**, a Roslyn-based analyzer (meaning deep understanding of C#) that detects bugs, security issues, and violations of best practices. Set to `--severity-level=warning` to limit the amount of diagnostics it produces by default. 
- **Codespell**, a general spell-checker, ensuring the right spelling of common words within the entire codebase. Fails the pipeline if it finds errors such as "teh" ==> "the".
- **Hadolint**, a Docker linter. Scans the repository for Dockerfiles, then runs the linter against each. Runs with default `failure-threshold=info`.

### Idempotence in Configuration Files

We have analyzed the Vagrantfiles, the Dockerfile and the Makefile for idempotence issues. We decided against migrating our solution to an external tool like Ansible for simplicity.Dockerfiles only required small fixes related to potential double user creation. We found no issues in the Vagrantfiles provisioning — if something is reinstalled or rebuilt but the system ends up in the same state without throwing an error, we don't consider that a problem. 

The Makefile required most effort to analyze. We defined the desired quality to be a consistent state and the absence of errors if a command runs repeatedly, provided all the prerequisites (such as *environment variables*) are met. 

We applied changes to migration recipes `db-migrate` and `db-remove-migration` as they had thrown errors if executed repeatedly. If a package is already present, `dotnet tool install` reports it but succeeds, so we left it as is. Similarly, `pip install` will reinstall, but not fail. All validation recipes can be run repeatedly with the same effect. `Vagrant up` is idempotent, as per the following documentation snippet:

>If the virtual machine already exists and you run vagrant up again, Vagrant will start the machine without running the provisioning scripts. If you modify your provisioning scripts or need to reapply them, use vagrant provision or --provision. This re-runs all provisioning scripts in your Vagrantfile to apply updates or fixes.

`Vagrant destroy` had thrown an error if executed repeatedly, so we added a guard check. `docker compose up` is designed to be idempotent, as per the following (it does nothing if nothing was changed): 

>If there are existing containers for a service, and the service’s configuration or image was changed after the container’s creation, docker compose up picks up the changes by stopping and recreating the containers (preserving mounted volumes).

`API-generate` generates the same stub each time it runs, overwriting the previous one. The container exits immediately after completion, so there is no risk of multiple leftover containers running.
# ITU-MiniTwit
A course project as part of "DevOps, Software Evolution and Software Maintenance, MSc (Spring 2026)" at IT-University of Copenhagen.

## Deployment in DigitalOcean
Deployment of the application MiniTwit was done by using Vagrant and DigitalOcean. The following steps were taken to create a VM (droplet) on DigitalOcean and deploy a Docker release to it.

### 1. Prerequisites
- Vagrant ([Download here](https://developer.hashicorp.com/vagrant/install))
- DigitalOcean plugin:`vagrant plugin install vagrant-digitalocean`
- SSH Key
- A DigitalOcean account

### 2. DigitalOcean
In DigitalOcean:
- Upload SSH public key
- Generate API token

### 3. Set Environment Variables 
Set the environment variables from DigitalOcean by:
```bash
export DIGITAL_OCEAN_TOKEN="your_api_token_on_digitalocean"
export SSH_KEY_NAME="your_ssh_key_name_on_digitalocean"
```
### 4. Creating the VM (droplet) and deploy Docker Release
Clone the repository by running:
```
git clone https://github.com/TienCamLy/MiniTwit.git
cd MiniTwit
```
Run the following command in terminal:
```
vagrant up
```
`vagrant up` creates the VM and executes the provisioning script (shell script) in the VagrantFile. 
This will build the Docker image and deploys the application using Docker Compose.

### 5. Droplet IP
The application can be accessed by:
```
http://<YOUR_DROPLET_IP>:8080
```
Our public droplet IP:
```
http://157.230.30.175:8080
```

### Shutdown
To stop the VM and destroy all resources that were created during the machine creation process,
run the following in terminal:
```
vagrant destroy
```

### Logging
To connect to the VM, navigate to the project directory, and stream the logs of the running Docker container,
run the following in termnial:
```
vagrant ssh webserver
cd /vagrant
sudo docker compose logs -f razor-pages
```

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

### Choice of Deployment Infrastructure
We choose to use GitHub Actions to deploy our application, as it is a native functionality that does not require additional setup. For this project, we are not dependent on 100% uptimes and do not mind the 1 minute runtime of our workflow. Keeping the deployment within GitHub also means we do not have to expand our tech stack with additional tools and connections, which would otherwise increase the complexity unnecessarily for the project scope.
# ITU-MiniTwit
A course project as part of "DevOps, Software Evolution and Software Maintenance, MSc (Spring 2026)" at IT-University of Copenhagen.

## Deployment in DigitalOcean
Deployment of the application MiniTwit was done by using Vagrant and DigitalOcean. The following steps were taken to create a VM (droplet) on DigitalOcean and deploy a Docker release to it.

### 1. Prerequisites
- Vagrant ([Download here](https://developer.hashicorp.com/vagrant/install))
- DigitalOcean plugin:`vagrant plugin install vagrant-digitalocean`
- SSH Key

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
### 4. Creating the VM (droplet)
Clone the repository by running:
```
git clone https://github.com/TienCamLy/MiniTwit.git
cd MiniTwit
```
Run the following command in terminal:
```
vagrant up
```
### 5. Deploy Docker Release
Run the following command in terminal:
```
docker compose up --build -d
```
### 6. Droplet IP
The application can be accessed by:
```
http://<YOUR_DROPLET_IP>
```
Our public droplet IP:
```
http://157.230.30.175:8080/
```

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
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
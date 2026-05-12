# Report

## 1. System's Perspective

<!--- A description and illustration of the: -->

<!--- Design and architecture of your ITU-MiniTwit systems. -->
### 1.1

#### 1.1.1 Choice of Final Infrastructure-as-Code Architecture
We ended up migrating to Terraform towards the end of the project as it allows easy maintenance and resource control through defined interfaces. Terraform has a thoroughly defined Documentation for Digital Ocean resources, and the migration to defining existing Vagrant deployments along with "Click-Ops" resources therefore did not have much extra overhead.

<!--- All dependencies of your ITU-MiniTwit systems on all levels of abstraction and development stages. That is, list and briefly describe all technologies and tools you applied and depend on. -->
### 1.2

<!--- Describe the current state of your systems, for example using results of static analysis and quality assessments. -->
### 1.3

## 2. Process' perspective
<!--- 
This perspective should clarify how code or other artifacts come from idea into the running system and everything that happens on the way.
In particular, the following descriptions should be included: -->

<!--- A complete description and illustration of stages and tools included in the CI/CD pipelines, including deployment and release of your systems. -->
### 2.1

<!--- How do you monitor your systems and what precisely do you monitor? -->
### 2.2

<!--- What do you log in your systems and how do you aggregate logs? -->
### 2.3

<!--- Brief description of how you security hardened your systems. -->
### 2.4

<!--- How do you handle availability and scaling in your systems? -->
### 2.5

## 3. Reflection Perspective

<!--- Describe the biggest issues, how you solved them, and which are major lessons learned with regards to: evolution and refactoring, operation, and maintenance 

of your ITU-MiniTwit systems. Link back to respective commit messages, issues, tickets, etc. to illustrate these.

Also reflect and describe what was the "DevOps" style of your work. For example, what did you do differently to previous development projects and how did it work?
-->

<!--- evolution and refactoring -->
### 3.1 Evolution and Refactoring
We discussed that it may have been useful to have defined the infrastructure in Terraform from the beginning and that it may have led us to avoid having the amount of "Click-Ops" we had during the project (setting up a managed database, modifying network rules for droplets etc.) giving better reproducibility and version history.

<!--- operation -->
### 3.2 Operation
<!--- QA Building -->
To increase robustness we added a QA deployment on Pull Requests, requiring the application to be built, pushed and tests to pass before merging it into the main branch. This allowed us to test all of our features fully before releasing them to our main application and thereby decreased the amount of bugs and operational work.
<!--- Database CPU Overload -->
In early April we started receiving warnings from the built-in resource alert system in Digital Ocean that our Database Cluster was above 90% CPU utilization. We started investigating the issue and realized that the amount of requests coming in had ramped up so much that our database could not follow along.
We chose to resize the cluster such that it had an extra virtual CPU after cost-benefit analysis concluding that the developer time it would take to improve the ORM system to send fewer requests would be too time consuming versus the cost of upgrading the database cluster.
The database was resized with no downtime.
<!--- Monitoring Droplet CPU Overload -->
Once we had fully migrated to swarm, including log shipping from all our droplets, the droplet containing the monitoring application ended up being overloaded such that our monitoring application became unreachable. This warranted an upgrade of the droplet containing the monitoring application. If we had access to spin up more droplets, we may have considered horizontal scaling instead of vertical.
The monitoring droplet was resized using terraform and therefore had minimal possible downtime. The full process took ~6 minutes:

![DigitalOcean monitoring droplet resize (duration ~6 minutes)](images/monitor_droplet_resize.png)

<!--- Grafana Restart / Volume Management -->
We struggled with the continuous deployment of our monitoring application in terms of ensuring new dashboards would appear and an unintended addition of a flag that reset the volumes for Loki and Prometheus. After realizing the issue and looking at a few different combinations of flags, we fixed the issues and accepted the loss of earlier metrics & logs.

<!--- maintenance -->
### 3.3 Maintenance

## 4. Use of Generative AI
<!---
ITU's rules on the use of generative AI apply for this report too. They are described here and in detail here. 
Please follow them. For your report that means that you have to state which generative AI tools have been used for which task(s) in your projects. 
Additionally, describe how generative AI tools have been used and briefly reflect and discuss how they supported or hindered your work process.
-->

<!--- Mie -->

<!--- Daniel -->

<!--- Mads -->

<!--- Kris -->

<!--- Patrick -->

<!--- Tien -->
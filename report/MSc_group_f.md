# Report

## 1. System's Perspective

<!--- A description and illustration of the: -->

<!--- Design and architecture of your ITU-MiniTwit systems. -->
### 1.1

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
### 2.5 Availability and Scaling

availability:
- declarative Scaling (three replicas)

scaling:
- internal load balancing (ingress routing mesh)
  - The ingress mesh acts as an internal load balancer. Swarm automatically distributes the traffic across all healthy replicas


To ensure low downtime during the transition from the standalone containers to a Docker Swarm cluster, 
the stack was deployed with a **blue-green service deployment** strategy from the start, such that the running containers were gradually replaced by the updated ones. 
This is achieved by configuring the deployment settings within the Docker Compose file to set the update order to `start-first` and 
a delay parameter that dictates how long Docker Swarm should wait after starting an updated container before terminating an old one, 
allowing the new container to initialize.

As a result, the services remain available during deployment. By default, Docker Swarm uses the rolling update strategy which terminates the old container 
before starting a new one. This is called `stop-first`. By terminating the containers first, the default strategy forces the application to experience downtime 
during the window between container termination and container initialization.

While our strategy should have ensured low downtime during the transition to Docker Swarm, the production application still experienced downtime 
due to overlooked human errors. These errors came as a result of separate Docker Swarm migration issues. 
Specifically, the Swarm Ingress routing mesh overwrote the port of the development environment on one of our DigitalOcean Droplets, 
and the Loki logs failed to display on our Grafana dashboards. During the debugging process, accidental downtime of the application was introduced 
when pulling the wrong container image due to misconfigured environment secrets, or changing the application to use another port, 
which prevented the simulator from reaching it. 

<!--- Move to reflection part? -->
To avoid these issues in the future, a solution could be to replicate the Docker Swarm infrastructure 
within an isolated development environment, so any configuration changes during the transition does not affect live production. 


## 3. Reflection Perspective

<!--- Describe the biggest issues, how you solved them, and which are major lessons learned with regards to: evolution and refactoring, operation, and maintenance 

of your ITU-MiniTwit systems. Link back to respective commit messages, issues, tickets, etc. to illustrate these.

Also reflect and describe what was the "DevOps" style of your work. For example, what did you do differently to previous development projects and how did it work?
-->

<!--- evolution and refactoring -->
### 3.1 Evolution and Refactoring

<!--- operation -->
### 3.2 Operation

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
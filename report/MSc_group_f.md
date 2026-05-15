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
<!--- Firewalls -->
The firewall for the DigitalOcean Droplets was configured to improve security. Inbound firewall rules were configured in DigitalOcean to provide an clear overview of the rules for each Droplet instead of `ufw`, 
such that the rules can be conveniently managed by each group member through the user interface. 
Inbound rules defines the traffic allowed to the Droplets on which ports and from which sources, and all other incoming traffic is blocked. In addition, Docker does not bypass DigitalOcean's firewalls. 

The production application were given firewall rules for:
- Standard internet and access ports (TCP 22, TCP 80, TCP 443)
- Docker port (TCP 2376)
- Docker Swarm infrastructure ports (TCP 2377, UDP 4789, TCP/UDP 7946)
- Application ports (TCP 8080)
- Grafana Loki log aggregation port (TCP 3100)
- Prometheus ports (TCP 9090, TCP 9095, TCP 9096, TCP 9100)

<!--- How do you handle availability and scaling in your systems? -->
### 2.5

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
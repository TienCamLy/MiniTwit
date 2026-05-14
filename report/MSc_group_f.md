# Report

## 1. System's Perspective

<!--- A description and illustration of the: -->

<!--- Design and architecture of your ITU-MiniTwit systems. -->
### 1.1

<!--- All dependencies of your ITU-MiniTwit systems on all levels of abstraction and development stages. That is, list and briefly describe all technologies and tools you applied and depend on. -->
### 1.2 Dependencies of MiniTwit

| Technology | Stage | Role |
|------------|-------|------|
| Git / GitHub | Development, CI/CD | Source control, reviews, and workflow hosting |
| Trello | Development, operations | Backlog management and work tracking |
| Discord | Development, operations | Team communication and receiving alerts (for example from GitHub & Grafana webhooks) |
| C# / .NET | Development, production | Application language and runtime |
| NuGet | Development, CI/CD | Package restore and feeds for .NET dependencies |
| ASP.NET Core (Razor Pages) | Development, production | Web UI and HTTP API |
| Entity Framework Core | Development, production | Database access and migrations |
| PostgreSQL | Development, testing, production | Primary data store (managed in DigitalOcean) |
| Docker | Development, CI/CD, production | Container images and runtime isolation |
| Docker Hub | CI/CD, production | Registry for built application images |
| Docker Compose | Development, testing | Local and test multi-container setups |
| Docker Swarm | Production | Orchestration and rolling updates |
| DigitalOcean | Infrastructure, production | Cloud VMs, managed database, and networking |
| DigitalOcean Spaces (S3-compatible) | Infrastructure, CI/CD | Object storage with an S3-compatible API (Terraform remote state backend) |
| Terraform | Infrastructure, CI/CD | Infrastructure as code for managing cloud resources |
| GitHub Actions | CI/CD | Continuous integration and deployment pipelines |
| Third-party GitHub Actions | CI/CD | Marketplace and vendor-maintained workflow steps (for example checkout, Docker login, Terraform setup, PR plan commenter, GitHub App token) |
| GitHub CLI | CI/CD | Command-line GitHub integration in workflows (for example `gh auth setup-git` for automated commits) |
| Ubuntu | CI/CD, production | Base operating system on GitHub-hosted runners and provisioned droplets |
| SSH (OpenSSH) | CI/CD, production | Remote deploy, file copy, and server access from pipelines |
| Prometheus | Monitoring | Metrics collection |
| Grafana | Monitoring | Dashboards and alerting |
| Loki | Monitoring | Centralized log storage |
| Promtail | Monitoring | Shipping container logs to Loki |
| Python | Testing (local and CI/CD) | API simulator test driver and test scripts |
| Selenium | Testing (local and CI/CD) | Browser-based UI tests (with Chrome in Docker) |
| dotnet format | Development, CI/CD | C# formatting and verifying the tree matches the formatter in CI |
| Roslynator | Development, CI/CD | C# static analysis (Roslyn-based diagnostics) |
| Codespell | Development, CI/CD | Spell checking across the repository |
| Hadolint | Development, CI/CD | Linting Dockerfiles |
| Codacy | Development, CI/CD | Hosted static analysis and pull-request quality checks |
| SonarCloud | Development, CI/CD | SonarQube-family analysis and quality gate on pull requests |
| CodeQL | Development, CI/CD | Semantic security and quality scanning (for example C# and Python) on pull requests |
| Docker Scout | CI/CD | Container image vulnerability scanning on QA builds |
| OpenAPI Generator | Development | Generating the API simulator stub from the OpenAPI description |
| Pandoc / LaTeX | CI/CD | Building the report PDF in automation |
| GNU Make | Development, CI/CD | Task automation (local and in workflows) |

#### Libraries

NuGet references come from the Razor Pages solution (`razor-pages/Web`, `razor-pages/Infrastructure`). Direct Python libraries for automated tests are listed below; the UI test lockfile also pins transitive versions (see `tests/selenium/requirements.txt`).

| Library | Context | Usage |
|---------|---------|-------|
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | Web, Infrastructure | ASP.NET Core Identity integrated with EF Core |
| Microsoft.EntityFrameworkCore.Design | Web, Infrastructure | EF Core design-time support and migrations |
| Npgsql.EntityFrameworkCore.PostgreSQL | Web, Infrastructure | EF Core provider for PostgreSQL |
| Newtonsoft.Json | Web | JSON serialization and deserialization |
| Swashbuckle.AspNetCore.Annotations | Web | OpenAPI metadata and attributes for the HTTP API |
| Swashbuckle.AspNetCore.Newtonsoft | Web | OpenAPI generation using Newtonsoft.Json |
| DotNetEnv | Web | Loading environment variables from `.env` files |
| prometheus-net.AspNetCore | Web | Prometheus metrics for ASP.NET Core |
| Microsoft.CodeAnalysis.Analyzers | Infrastructure | Build-time Roslyn analyzers |
| Microsoft.Extensions.Hosting | Infrastructure | Hosting abstractions for background-style infrastructure code |
| prometheus-net | Infrastructure | Prometheus metric registration and exposition primitives |
| Npgsql | Infrastructure | PostgreSQL data provider (ADO.NET) alongside EF |
| TimeZoneConverter | Infrastructure | Resolving time zones in infrastructure logic |
| requests | tests/API_Spec | HTTP calls from the API simulator |
| pytest | tests/selenium | Test runner for the Selenium UI suite |
| selenium | tests/selenium | WebDriver client driving the remote Chrome grid |


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
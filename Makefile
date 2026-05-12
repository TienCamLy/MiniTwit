ifneq (,$(wildcard ./.env))
    include .env
    export
endif

# Razor Pages App
app-down: # Delete all volumes
	docker compose down -v

# Razor Pages App
app-build: # Rebuilds the app without deleting volumes
	ISLOCALDEVELOPMENT=true docker compose -f compose-test.yaml up --build

app-down-build: # Delete all volumes and rebuild the app
	make app-down && \
	make app-build

# Install EF Tools
install-ef-tools:
	dotnet tool install --global dotnet-ef

# Database Migrations from root directory unless a migration with same name exists
db-migrate: 
	@if dotnet ef migrations list \
		--context MiniTwitContext \
		--project razor-pages/Infrastructure \
		--startup-project razor-pages/Web | grep -q "$(name)"; then \
		echo "Migration '$(name)' already exists. Skipping add."; \
	else \
		dotnet ef migrations add $(name) \
			--context MiniTwitContext \
			--project razor-pages/Infrastructure \
			--startup-project razor-pages/Web; \
	fi && \
	dotnet ef database update \
		--context MiniTwitContext \
		--project razor-pages/Infrastructure \
		--startup-project razor-pages/Web

# Database remove migrations from root directory if any exist
db-remove-migration:
	@if dotnet ef migrations list \
		--context MiniTwitContext \
		--project razor-pages/Infrastructure \
		--startup-project razor-pages/Web | grep -q "No migrations were found"; then \
		echo "No migrations to remove."; \
	else \
		dotnet ef migrations remove --force \
			--context MiniTwitContext \
			--project razor-pages/Infrastructure \
			--startup-project razor-pages/Web; \
	fi

# Database Migration Update from root directory
db-update:
	dotnet ef database update \
		--context MiniTwitContext \
		--project razor-pages/Infrastructure \
		--startup-project razor-pages/Web

# API Stub Build and Run
API-generate:
	docker run --rm -v "$$(pwd):/local" openapitools/openapi-generator-cli:v7.19.0 \
      generate \
      -i /local/swagger3.json \
      -g aspnetcore \
      -o /local/out/itu-minitwit-sim-stub \
      --additional-properties=buildTarget=program,aspnetCoreVersion=8.0,operationIsAsync=true,nullableReferenceTypes=true,useSwashbuckle=true

API-clean-generate:
	rm -rf out

# Deployment to Digital Ocean using Terraform
# Make sure to set the following environment variables:
# AWS_ACCESS_KEY_ID
# AWS_SECRET_ACCESS_KEY
# TF_VAR_do_token
# TF_VAR_pub_key
# TF_VAR_pvt_key
tf-init:
	cd infrastructure/environments/prod && terraform init -backend-config=backend.tfvars

tf-plan:
	cd infrastructure/environments/prod && terraform plan -var-file=prod.tfvars -out=plan.out

# It is here for completeness but we do not want to run this locally as it will modify the production environment
#tf-apply:
#	cd infrastructure/environments/prod && terraform apply -var-file=prod.tfvars

# Monitoring stack (Loki on monitoring VM; Promtail ships with root compose on app VM)
monitor-build:
	cd monitoring && docker compose up --build

# Tests
test-api-simulator: 
# requires API_TOKEN to be set in environment variable API_TOKEN
# requires TEST_GUI_IP to be set in environment variable TEST_GUI_IP
	printf "\n\nRunning API simulator tests...\n" && \
	python tests/API_Spec/wait_for_port.py --host $(TEST_GUI_IP) --port 8081 && \
	cd tests/API_Spec && \
	pip install -r requirements.txt && \
	SIM_DEBUG=1 python minitwit_simulator.py http://$(TEST_GUI_IP):8081 $(API_TOKEN) 50

test-ui-selenium: 
	printf "\n\nRunning UI selenium tests...\n" && \
	cd tests/selenium && \
	docker compose up --build --abort-on-container-exit --exit-code-from tests

spell-checker:
	printf "\n\nRunning spell checker tests...\n" && \
	pip install codespell && \
	cd razor-pages && \
	codespell

c-sharp-code-formatter:
	printf "\n\nRunning C# linting tests...\n" && \
	dotnet format --verify-no-changes razor-pages/Infrastructure/ && \
	dotnet format --verify-no-changes razor-pages/Web/ && \
	dotnet format --verify-no-changes razor-pages/Core/

hadolint-docker-linter: # runs as a docker container itself
	printf "\n\n Running Docker linter tests...\n" && \
	DOCKERFILES="$$(find . -type f \( -name 'Dockerfile' -o -name 'Dockerfile-*' -o -name '*.Dockerfile' \) -not -path './.git/*')" && \
	if [ -z "$$DOCKERFILES" ]; then \
		echo "No Dockerfiles found."; \
		exit 0; \
	fi && \
	for dockerfile in $$DOCKERFILES; do \
		echo "Linting $$dockerfile"; \
		cat "$$dockerfile" | docker run --rm -i hadolint/hadolint; \
	done

roslynator-c-sharp-analyzer: # change --severity-level to "info" to get more diagnostics
	printf "\n\nRunning C# analyzer tests...\n" && \
	dotnet tool install -g roslynator.dotnet.cli
	roslynator analyze razor-pages/Core/Core.csproj --severity-level warning
	roslynator analyze razor-pages/Infrastructure/Infrastructure.csproj --severity-level warning
	roslynator analyze razor-pages/Web/Web.csproj --severity-level warning

test-all: test-ui-selenium test-api-simulator
lint-all: spell-checker c-sharp-code-formatter roslynator-c-sharp-analyzer hadolint-docker-linter
run-all-validations: test-all lint-all

build-and-test: # requires API_TOKEN to be set in environment variable API_TOKEN
	printf "Building and testing Docker image...\n" && \
	make app-build && \
	make run-all-validations && \
	printf "\n\nStopping and removing Docker container...\n" && \
	make app-down

# Local auto-linting
auto-lint:
	dotnet format razor-pages/Infrastructure/ && \
	dotnet format razor-pages/Web/ && \
	dotnet format razor-pages/Core/

# Build report pdf (pandoc uses pdflatex; install TeX Live — pandoc .deb alone does not ship it)
install-pandoc:
	sudo apt-get update -qq && \
	sudo DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends \
		pandoc \
		texlive-latex-base \
		texlive-fonts-recommended \
		texlive-latex-recommended \
		texlive-lmodern

build-report-pdf:
	mkdir -p report/build && \
	pandoc --from=gfm --to=pdf -o report/build/MSc_group_f.pdf report/MSc_group_f.md

install-and-build-report:
	make install-pandoc && make build-report-pdf
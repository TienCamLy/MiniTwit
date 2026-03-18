ifneq (,$(wildcard ./.env))
    include .env
    export
endif

# Razor Pages App
app-build: # Rebuilds the app without deleting volumes
	docker compose up --build --detach

app-down: # Delete all volumes
	docker compose down -v

app-down-build: # Delete all volumes and rebuild the app
	make app-down && \
	make app-build

# Install EF Tools
install-ef-tools:
	dotnet tool install --global dotnet-ef

# Database Migrations from root directory
db-migrate:
	dotnet ef migrations add $(name) \
		--context MiniTwitContext \
		--project razor-pages/Infrastructure \
		--startup-project razor-pages/Web && \
	dotnet ef database update \
    	--context MiniTwitContext \
    	--project razor-pages/Infrastructure \
    	--startup-project razor-pages/Web

# Database remove migrations from root directory
db-remove-migration:
	dotnet ef migrations remove \
		--context MiniTwitContext \
		--project razor-pages/Infrastructure \
		--startup-project razor-pages/Web

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

# Deployment to Digital Ocean
deploy-digital-ocean: # requires Digital Ocean API PAT token to be set in environment variable DIGITAL_OCEAN_TOKEN
	export SSH_KEY_NAME="MacLocalKey" && \
	vagrant up

provision-digital-ocean:
	vagrant provision

clean-digital-ocean:
	vagrant destroy && \
	rm -rf .vagrant

# Monitoring Deployment to Digital Ocean
monitor-build:
	cd monitoring && docker compose up --build

# Tests
test-api-simulator: # requires SIM_API_CREDENTIALS to be set in environment variable SIM_API_CREDENTIALS
	printf "\n\nRunning API simulator tests...\n" && \
	cd API_Spec && \
	python minitwit_simulator.py http://localhost:8080 $(SIM_API_CREDENTIALS) 2000

test-spell-checker:
	printf "\n\nRunning spell checker tests...\n" && \
	pip install codespell && \
	cd razor-pages && \
	codespell

test-c-sharp-linting:
	printf "\n\nRunning C# linting tests...\n" && \
	dotnet format --verify-no-changes razor-pages/Infrastructure/ && \
	dotnet format --verify-no-changes razor-pages/Web/ && \
	dotnet format --verify-no-changes razor-pages/Core/

run-all-tests: test-api-simulator test-spell-checker test-c-sharp-linting

build-and-test: # requires SIM_API_CREDENTIALS to be set in environment variable SIM_API_CREDENTIALS
	printf "Building and testing Docker image...\n" && \
	make app-build && \
	make run-all-tests && \
	printf "\n\nStopping and removing Docker container...\n" && \
	make app-down

# Local auto-linting
auto-lint:
	dotnet format razor-pages/Infrastructure/ && \
	dotnet format razor-pages/Web/ && \
	dotnet format razor-pages/Core/

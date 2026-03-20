ifneq (,$(wildcard ./.env))
    include .env
    export
endif

# Razor Pages App
app-build: # Rebuilds the app without deleting volumes, ensuring the network is not present before hand
	-docker network rm minitwit-network && \ 
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
test-api-simulator: # requires API_TOKEN to be set in environment variable API_TOKEN
	printf "\n\nRunning API simulator tests...\n" && \
	cd tests/API_Spec && \
	python minitwit_simulator.py http://localhost:8080 $(API_TOKEN) 2000

test-ui-selenium: 
	printf "\n\nRunning UI selenium tests...\n" && \
	cd tests/selenium && \
	docker compose up --build --abort-on-container-exit --exit-code-from tests

lint-spell-checker:
	printf "\n\nRunning spell checker tests...\n" && \
	pip install codespell && \
	cd razor-pages && \
	codespell

lint-c-sharp:
	printf "\n\nRunning C# linting tests...\n" && \
	dotnet format --verify-no-changes razor-pages/Infrastructure/ && \
	dotnet format --verify-no-changes razor-pages/Web/ && \
	dotnet format --verify-no-changes razor-pages/Core/

test-all: test-api-simulator test-ui-selenium
lint-all: lint-spell-checker lint-c-sharp
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

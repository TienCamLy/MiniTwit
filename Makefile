ifneq (,$(wildcard ./.env))
    include .env
    export
endif

# Debian/Ubuntu PostgreSQL uses peer auth on the Unix socket: connections default to a DB role
# named like the OS user (e.g. runner), which does not exist. Run superuser CLI as postgres OS user.
# Prefer GITHUB_ACTIONS over CI so a root .env that sets CI= does not break GitHub-hosted jobs.
PG_SUPERUSER :=
ifneq (,$(GITHUB_ACTIONS))
    PG_SUPERUSER := sudo -u postgres
else ifneq (,$(CI))
    PG_SUPERUSER := sudo -u postgres
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

# Monitoring stack (Loki on monitoring VM; Promtail ships with root compose on app VM)
monitor-build:
	cd monitoring && docker compose up --build

# Create local postgres database for testing
install-postgres:
	@echo "macOS:  brew install postgresql@18 && brew services start postgresql@18" && \
	sudo apt update && sudo apt install -y postgresql && \
	sudo service postgresql start

# Schema grants must run connected to defaultdb (not postgres), or you grant the wrong DB's public schema.
# Requires POSTGRES_LOCAL_USER and POSTGRES_LOCAL_PASSWORD to be set in environment variables
create-postgres-database:
	$(PG_SUPERUSER) psql -d postgres -c "CREATE ROLE $(POSTGRES_LOCAL_USER) WITH LOGIN PASSWORD '$(POSTGRES_LOCAL_PASSWORD)';" && \
	$(PG_SUPERUSER) createdb -O $(POSTGRES_LOCAL_USER) defaultdb

clean-postgres-database:
	$(PG_SUPERUSER) dropdb --if-exists defaultdb && \
	$(PG_SUPERUSER) psql -d postgres -c "REVOKE ALL PRIVILEGES ON SCHEMA public FROM $(POSTGRES_LOCAL_USER);" 2>/dev/null || true && \
	$(PG_SUPERUSER) psql -d postgres -c "DROP ROLE IF EXISTS $(POSTGRES_LOCAL_USER);"

# Tests
test-api-simulator: 
# requires API_TOKEN to be set in environment variable API_TOKEN
# requires TEST_GUI_IP to be set in environment variable TEST_GUI_IP
	printf "\n\nRunning API simulator tests...\n" && \
	python tests/API_Spec/wait_for_port.py --host $(TEST_GUI_IP) --port 8081 && \
	cd tests/API_Spec && \
	pip install -r requirements.txt && \
	SIM_DEBUG=1 python minitwit_simulator.py http://$(TEST_GUI_IP):8081 $(API_TOKEN) 200

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

test-all: test-ui-selenium test-api-simulator
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

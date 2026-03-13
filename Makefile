# Razor Pages App
app-build: # Rebuilds the app without deleting volumes
	docker compose up --build --detach

app-down: # Delete all volumes
	docker compose down -v

app-down-build: # Delete all volumes and rebuild the app
	make app-down && \
	make app-build

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

# Tests
test-api-simulator:
	printf "\n\nRunning API simulator tests...\n" && \
	cd API_Spec && \
	python minitwit_simulator.py http://localhost:8080 2000

test-spell-checker:
	printf "\n\nRunning spell checker tests...\n" && \
	pip install codespell && \
	cd razor-pages && \
	codespell

run-all-tests: test-api-simulator test-spell-checker

build-and-test:
	python -m pip install --upgrade pip && \
	printf "Building and testing Docker image...\n" && \
	make app-build && \
	make run-all-tests && \
	printf "\n\nStopping and removing Docker container...\n" && \
	make app-down
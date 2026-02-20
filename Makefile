# Razor Pages App
app-build: # Rebuilds the app without deleting volumes
	docker compose up --build

app-down-build: # Delete all volumes and rebuild the app
	docker compose down -v && \
	docker compose up --build

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
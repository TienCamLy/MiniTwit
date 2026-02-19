# Razor Pages App
app-build:
	docker compose up --build

# API Stub Build and Run
API-build-stub:
	cd out/itu-minitwit-sim-stub && \
	docker build -f src/Org.OpenAPITools/Dockerfile -t org.openapitools .

API-run-stub:
	cd out/itu-minitwit-sim-stub && \
	echo "API running at: http://localhost:5001/openapi" && \
	docker run -p 5001:8080 org.openapitools

API-clean-stub:
	cd out/itu-minitwit-sim-stub && \
	docker rm -f org.openapitools

# Deployment to Digital Ocean
deploy-digital-ocean: # requires Digital Ocean API PAT token to be set in environment variable DIGITAL_OCEAN_TOKEN
	export SSH_KEY_NAME="MacLocalKey"
	vagrant up

clean-digital-ocean:
	vagrant destroy
	rm -rf .vagrant
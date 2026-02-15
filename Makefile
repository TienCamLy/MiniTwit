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

app-build:
	docker compose up --build
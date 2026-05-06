# slim is a minimal python installation
FROM python:3.12-slim 

WORKDIR /tests
COPY requirements.txt .
RUN pip install --no-cache-dir --only-binary :all: --require-hashes -r requirements.txt 

# create a user if doesn't exist, grant permissions
RUN (id appuser >/dev/null 2>&1 || useradd appuser) && chown appuser:appuser /tests
COPY --chown=root:root --chmod=755 test_itu_minitwit_ui.py .

USER appuser
CMD ["pytest", "test_itu_minitwit_ui.py"]


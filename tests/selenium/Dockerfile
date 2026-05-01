# slim is a minimal python installation
FROM python:3.12-slim 

COPY requirements.txt .
RUN pip install --only-binary :all: -r requirements.txt && useradd appuser

WORKDIR /tests
RUN chown appuser:appuser /tests
COPY --chown=root:root --chmod=755 test_itu_minitwit_ui.py .
USER appuser
CMD ["pytest", "test_itu_minitwit_ui.py"]


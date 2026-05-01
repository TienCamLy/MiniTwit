# slim is a minimal python installation
FROM python:3.12-slim 

RUN pip install --only-binary :all: pytest=9.0.3 selenium=4.43.0 && useradd appuser

WORKDIR /tests
RUN chown appuser:appuser /tests
COPY --chown=root:root --chmod=755 test_itu_minitwit_ui.py .
USER appuser
CMD ["pytest", "test_itu_minitwit_ui.py"]


# slim is a minimal python installation
FROM python:3.12-slim 

RUN pip install pytest selenium && useradd appuser

WORKDIR /tests
COPY --chown=root:root --chmod=755 test_itu_minitwit_ui.py .
USER appuser
CMD ["pytest", "test_itu_minitwit_ui.py"]


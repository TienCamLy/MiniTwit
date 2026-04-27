# slim is a minimal python installation
FROM python:3.12-slim 

RUN pip install pytest selenium && useradd appuser

WORKDIR /tests
RUN chown appuser:appuser /tests
COPY --chown=appuser:appuser --chmod=744 test_itu_minitwit_ui.py .
USER appuser
CMD ["pytest", "test_itu_minitwit_ui.py"]


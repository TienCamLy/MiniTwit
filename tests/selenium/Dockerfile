# slim is a minimal python installation
FROM python:3.12-slim 

WORKDIR /tests
COPY test_itu_minitwit_ui.py .

RUN pip install pytest selenium
CMD ["pytest", "test_itu_minitwit_ui.py"]


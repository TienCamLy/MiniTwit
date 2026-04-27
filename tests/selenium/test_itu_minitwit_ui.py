"""
OBSOLETE:
To run this test with a visible browser, the following dependencies have to be setup:

  * `pip install selenium`
  * `pip install pymongo`
  * `pip install pytest`
  * `wget https://github.com/mozilla/geckodriver/releases/download/v0.32.0/geckodriver-v0.32.0-linux64.tar.gz`
  * `tar xzvf geckodriver-v0.32.0-linux64.tar.gz`
  * After extraction, the downloaded artifact can be removed: `rm geckodriver-v0.32.0-linux64.tar.gz`

The application that it tests is the version of _ITU-MiniTwit_ that you got to know during the exercises on Docker:
https://github.com/itu-devops/flask-minitwit-mongodb/tree/Containerize (*OBS*: branch Containerize)

```bash
$ git clone https://github.com/HelgeCPH/flask-minitwit-mongodb.git
$ cd flask-minitwit-mongodb
$ git switch Containerize
```

After editing the `docker-compose.yml` file file where you replace `youruser` with your respective username, the
application can be started with `docker-compose up`.

Now, the test itself can be executed via: `pytest test_itu_minitwit_ui.py`.
"""

import os
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.chrome.options import Options

# Set in docker-compose
TEST_GUI_IP = os.getenv("TEST_GUI_IP")
SELENIUM_REMOTE_URL = os.getenv("SELENIUM_REMOTE_URL") 

def _create_driver():
    opts = Options()
    opts.add_argument("--headless=new")
    # cmd_executor instead od service for a remote driver
    return webdriver.Remote(command_executor = SELENIUM_REMOTE_URL, options = opts)

def _register_user_via_gui(driver, data):
    registration_page = f"http://{TEST_GUI_IP}:8081/register"
    driver.get(registration_page)

    input_fields = driver.find_elements(By.TAG_NAME, "input")

    for idx, str_content in enumerate(data):
        input_fields[idx].send_keys(str_content)
    input_fields[4].send_keys(Keys.RETURN)

    wait = WebDriverWait(driver, 5)
    flashes = wait.until(EC.presence_of_all_elements_located((By.CLASS_NAME, "flashes")))

    return flashes



def test_register_user_via_gui():
    """
    This is a UI test. It only interacts with the UI that is rendered in the browser and checks that visual
    responses that users observe are displayed.
    """

    with _create_driver() as driver:
        generated_msg = _register_user_via_gui(driver, ["Me", "me@some.where", "secure123", "secure123"])[0].text
        expected_msg = "You were successfully registered and can login now"
        assert generated_msg == expected_msg

    # cleanup, make test case idempotent
    # db_client = pymongo.MongoClient(DB_URL, serverSelectionTimeoutMS=5000)
    # db_client.test.user.delete_one({"username": "Me"})


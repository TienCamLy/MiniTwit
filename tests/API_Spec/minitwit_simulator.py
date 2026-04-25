"""
Call me for example like:

$ python minitwit_simulator.py "http://localhost:5001"
"""

import traceback
import csv
import sys
import json
import os
import requests
from time import sleep
from time import perf_counter
from datetime import datetime, timezone

CSV_FILENAME = "./minitwit_scenario.csv"
DEBUG_SIM = os.getenv("SIM_DEBUG", "0") == "1"


def dbg(msg):
    if DEBUG_SIM:
        ts = datetime.strftime(datetime.now(timezone.utc), "%Y-%m-%d %H:%M:%S")
        print(f"[SIM-DEBUG] {ts} {msg}", flush=True)

def get_actions():

    # read scenario .csv and parse to a list of lists
    with open(CSV_FILENAME, "r", encoding="utf-8") as f:
        reader = csv.reader(f, delimiter="\t", quotechar=None)

        # for each line in .csv
        for line in reader:
            try:
                # we know that the command string is always the fourth element
                command = line[3]

                command_id = int(line[0])
                delay = int(line[1])
                user = line[4]
                if command == "register":
                    email = line[5]
                    user_pwd = line[-1]

                    item = {
                        "latest": command_id,
                        "post_type": command,
                        "username": user,
                        "email": email,
                        "pwd": user_pwd,
                    }

                    yield item, delay

                elif command == "tweet":
                    tweet_content = line[5]
                    item = {
                        "latest": command_id,
                        "post_type": command,
                        "username": user,
                        "content": tweet_content,
                    }

                    yield item, delay

                elif command == "follow":
                    user_to_follow = line[5]
                    item = {
                        "latest": command_id,
                        "post_type": command,
                        "username": user,
                        "follow": user_to_follow,
                    }
                    yield item, delay

                elif command == "unfollow":
                    user_to_unfollow = line[5]

                    item = {
                        "latest": command_id,
                        "post_type": command,
                        "username": user,
                        "unfollow": user_to_unfollow,
                    }
                    yield item, delay

                else:
                    # This should never happen and can likely be removed to
                    # make parsing for plot generation later easier
                    print("Unknown type found: (" + command + ")")

            except Exception:
                print("========================================")
                print(traceback.format_exc())

def handle_response(command, status_code, action_id, host, good_status_codes, total_actions):
    if status_code in good_status_codes:
        return total_actions + 1
    else:
        ts_str = datetime.strftime(
            datetime.now(timezone.utc), "%Y-%m-%d %H:%M:%S"
        )
        print(
            ",".join(
                [
                    ts_str,
                    host,
                    str(action_id),
                    str(status_code),
                    command,
                ]
            )
        )
        return total_actions

class Action:
    def __init__(self, command, action_id, host, action_data, headers, timeout=(2, 5)):
        self.command = command
        self.action_id = action_id
        self.host = host    
        self.headers = headers
        self.action_data = action_data
        self.timeout = timeout
    
    def build_data(self):
        # Building parameters
        self.params = {"latest": self.action_id}
        if self.command == "msgs":
            self.params["no"] = self.action_data["no"]
        
        # Building data
        if self.command == "register":
            self.data = {
                "username": self.action_data["username"],
                "email": self.action_data["email"],
                "pwd": self.action_data["pwd"],
            }
        elif self.command == "msgs":
            self.data = {}
        elif self.command == "follow":
            self.data = {"follow": self.action_data["follow"]}
        elif self.command == "unfollow":
            self.data = {"unfollow": self.action_data["unfollow"]}
        elif self.command == "tweet":
            self.data = {"content": self.action_data["content"]}

    def execute(self):
        if self.command == "register":
            return requests.post(
                f"{self.host}/register",
                data=json.dumps(self.data),
                params=self.params,
                headers=self.headers,
                timeout=self.timeout,
            )
        elif self.command == "msgs":
            return requests.post(
                f"{self.host}/msgs",
                params=self.params,
                headers=self.headers,
                timeout=self.timeout,
            )
        elif self.command == "follow" or self.command == "unfollow":
            return requests.post(
                f"{self.host}/fllws/{self.action_data['username']}",
                data=json.dumps(self.data),
                params=self.params,
                headers=self.headers,
                timeout=self.timeout,
            )
        elif self.command == "tweet":
            return requests.post(
                f"{self.host}/msgs/{self.action_data['username']}",
                data=json.dumps(self.data),
                params=self.params,
                headers=self.headers,
                timeout=self.timeout,
            )
        else:
            raise ValueError(f"Unknown command: {self.command}")

def main(host, token, max_actions=None):
    HEADERS = {
        "Connection": "close",
        "Content-Type": "application/json",
        "Authorization": f"Basic {token}",
    }
    total_actions = 0
    for action, delay in get_actions():
        if max_actions and int(action["latest"]) >= max_actions:
            return total_actions
        try:
            # SWITCH ON TYPE
            command = action["post_type"]
            url_hint = {
                "register": f"{host}/register",
                "msgs": f"{host}/msgs",
                "follow": f"{host}/fllws/{action['username']}",
                "unfollow": f"{host}/fllws/{action['username']}",
                "tweet": f"{host}/msgs/{action['username']}",
            }.get(command, f"{host}/<unknown>")

            if command == "register":

                action_builder = Action(command, action["latest"], host, action, HEADERS)
                action_builder.build_data()
                dbg(f"START id={action['latest']} cmd={command} url={url_hint}")
                started_at = perf_counter()
                response = action_builder.execute()
                elapsed_ms = int((perf_counter() - started_at) * 1000)
                dbg(f"DONE  id={action['latest']} cmd={command} status={response.status_code} dt_ms={elapsed_ms}")

                # error handling (204 success, 400 user exists)
                # 400 user exists already but not an error to log
                total_actions = handle_response(command, response.status_code, action["latest"], host, [204, 400], total_actions)

                response.close()

            elif command == "msgs":

                action_builder = Action(command, action["latest"], host, action, HEADERS)
                action_builder.build_data()
                dbg(f"START id={action['latest']} cmd={command} url={url_hint}")
                started_at = perf_counter()
                response = action_builder.execute()
                elapsed_ms = int((perf_counter() - started_at) * 1000)
                dbg(f"DONE  id={action['latest']} cmd={command} status={response.status_code} dt_ms={elapsed_ms}")

                # error handling (200 success, 403 failure (no headers))

                # 403 bad request
                total_actions = handle_response(command, response.status_code, action["latest"], host, [200], total_actions)

                response.close()

            elif command == "follow" or command == "unfollow" or command == "tweet":

                action_builder = Action(command, action["latest"], host, action, HEADERS)
                action_builder.build_data()
                dbg(f"START id={action['latest']} cmd={command} url={url_hint}")
                started_at = perf_counter()
                response = action_builder.execute()
                elapsed_ms = int((perf_counter() - started_at) * 1000)
                dbg(f"DONE  id={action['latest']} cmd={command} status={response.status_code} dt_ms={elapsed_ms}")

                # error handling (204 success, 403 failure, 404 Not Found no user id)

                # 403 unauthorized or 404 Not Found
                total_actions = handle_response(command, response.status_code, action["latest"], host, [204], total_actions)

                response.close()

            else:
                # throw exception. Should not be hit
                ts_str = datetime.strftime(
                    datetime.now(timezone.utc), "%Y-%m-%d %H:%M:%S"
                )
                print(
                    ",".join(
                        [
                            "FATAL: Unknown message type",
                            ts_str,
                            host,
                            str(action["latest"]),
                        ]
                    )
                )

        except requests.exceptions.ConnectTimeout:
            ts_str = datetime.strftime(datetime.now(timezone.utc), "%Y-%m-%d %H:%M:%S")
            print(
                ",".join([ts_str, host, str(action["latest"]), "ConnectTimeout"])
            )
        except requests.exceptions.ReadTimeout:
            ts_str = datetime.strftime(datetime.now(timezone.utc), "%Y-%m-%d %H:%M:%S")
            print(
                ",".join([ts_str, host, str(action["latest"]), "ReadTimeout"])
            )
        except requests.exceptions.Timeout:
            ts_str = datetime.strftime(datetime.now(timezone.utc), "%Y-%m-%d %H:%M:%S")
            print(
                ",".join([ts_str, host, str(action["latest"]), "Timeout"])
            )
        except requests.exceptions.ConnectionError:
            ts_str = datetime.strftime(datetime.now(timezone.utc), "%Y-%m-%d %H:%M:%S")
            print(
                ",".join(
                    [ts_str, host, str(action["latest"]), "ConnectionError"]
                )
            )
        except Exception as e:
            print("========================================")
            print(traceback.format_exc())
            ts_str = datetime.strftime(datetime.now(timezone.utc), "%Y-%m-%d %H:%M:%S")
            print(
                ",".join(
                    [ts_str, host, str(action["latest"]), type(e).__name__]
                )
            )

        sleep(delay / (1000 * 100000))
    return total_actions


if __name__ == "__main__":
    host = sys.argv[1]
    token = sys.argv[2]
    max_actions = int(sys.argv[3]) if len(sys.argv) > 3 else None
    start_time = datetime.now()
    total_actions = main(host, token, max_actions)
    if max_actions and total_actions < max_actions * 0.95:
        print(f"Simulation failed in {datetime.now() - start_time} seconds.")
        print(f"Expected {max_actions} actions, but only ran {total_actions} actions successfully")
        sys.exit(1)
    else:
        print(f"Simulation completed in {datetime.now() - start_time} seconds.")
        print(f"Ran {total_actions} actions successfully.")
        sys.exit(0)


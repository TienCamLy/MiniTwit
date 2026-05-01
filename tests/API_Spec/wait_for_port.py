#!/usr/bin/env python3
"""Wait until a TCP port accepts connections (e.g. after docker compose up)."""

import argparse
import socket
import sys
import time


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--host", default="127.0.0.1")
    parser.add_argument("--port", type=int, default=8080)
    parser.add_argument("--timeout", type=int, default=120, help="seconds")
    args = parser.parse_args()

    deadline = time.monotonic() + args.timeout
    while time.monotonic() < deadline:
        try:
            with socket.create_connection((args.host, args.port), timeout=2):
                print(f"{args.host}:{args.port} is accepting connections.")
                return 0
        except OSError:
            time.sleep(2)

    print(
        f"Timed out after {args.timeout}s waiting for {args.host}:{args.port}.",
        file=sys.stderr,
    )
    return 1


if __name__ == "__main__":
    sys.exit(main())

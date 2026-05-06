terraform {
  required_providers {
    digitalocean = {
        source = "digitalocean/digitalocean"
        version = "~> 2.37.1"
    }
  }
}

provider "digitalocean" {
  token = var.do_token
}
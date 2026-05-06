terraform {
  required_providers {
    digitalocean = {
        source = "digitalocean/digitalocean"
        version = "~> 2.37.1"
    }
    null = {
            source = "hashicorp/null"
            version = "3.1.0"
    }
  }
}
# OBS: NOT DEPLOYED
# This is what we intiially added as a QA / DEV environment, but due to Digital Ocean's limitation on
# number of droplets being max 3, the droplet for this was taken over by the PROD environment.
# Therefore it is not possible at our current account level to spin up DEV :(
module "ssh_key_register" {
  source  = "../../modules/do-ssh-key"
  pub_key = var.pub_key
  name    = var.ssh_key_name
}

module "single-droplet" {
  source = "../../modules/do-single-droplet"

  name                 = var.droplet_name
  region               = var.region
  pvt_key              = var.pvt_key
  ssh_key_fingerprints = [module.ssh_key_register.fingerprint]
  compose_file_source  = var.droplet_file_source
}

module "public_ip" {
  source     = "../../modules/do-public-ip"
  region     = var.region
  droplet_id = module.single-droplet.droplet_id
}

module "postgres-db" {
  source = "../../modules/do-postgres-db"
  name = "minitwit-test-db"
  engine = "pg"
  engine_version = "18"
  size = "db-s-1vcpu-1gb"
  region = var.region
  node_count = 1
  droplet_firewall_entries = { dev = module.single-droplet.droplet_id }
}

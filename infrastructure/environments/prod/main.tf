module "ssh_key_register" {
  source  = "../../modules/do-ssh-key"
  pub_key = var.pub_key
  name    = var.ssh_key_name
}

module "swarm" {
  source = "../../modules/do-docker-swarm"

  region                   = var.region
  pvt_key                  = var.pvt_key
  ssh_key_fingerprints     = [module.ssh_key_register.fingerprint]
  docker_stack_file_source = var.docker_stack_file_source
}

module "public-ip" {
  source     = "../../modules/do-public-ip"
  droplet_id = module.swarm.minitwit-swarm-leader-droplet-id
}

module "postgres-db" {
  source         = "../../modules/do-postgres-db"
  name           = "minitwit-prod"
  engine         = "pg"
  engine_version = "15"
  size           = "db-s-1vcpu-1gb"
  region         = var.region
  node_count     = 1
  droplet_firewall_entries = merge(
    { leader = module.swarm.minitwit-swarm-leader-droplet-id },
    { for i, id in module.swarm.minitwit-swarm-manager-droplet-ids : "manager-${i}" => id },
    { for i, id in module.swarm.minitwit-swarm-worker-droplet-ids : "worker-${i}" => id },
  )
}
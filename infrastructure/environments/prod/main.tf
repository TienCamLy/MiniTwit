module "ssh_key_register" {
  source  = "../../modules/do-ssh-key"
  pub_key = var.pub_key
  name = var.ssh_key_name
}

module "swarm" {
  source = "../../modules/swarm"

  region                   = var.region
  pvt_key                  = var.pvt_key
  ssh_key_fingerprints     = [module.ssh_key_register.fingerprint]
  docker_stack_file_source = var.docker_stack_file_source
}

module "public-ip" {
  source = "../../modules/do-public-ip"
  droplet_id = module.swarm.minitwit-swarm-leader-droplet-id
}
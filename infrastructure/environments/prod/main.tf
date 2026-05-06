module "ssh_key_register" {
  source  = "../../modules/do-ssh-key"
  pub_key = var.pub_key
}

module "swarm" {
  source = "../../modules/swarm"

  region                   = var.region
  pvt_key                  = var.pvt_key
  ssh_key_fingerprints     = [module.ssh_key_register.fingerprint]
  docker_stack_file_source = var.docker_stack_file_source
}
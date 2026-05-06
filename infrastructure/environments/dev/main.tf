module "ssh_key_register" {
  source  = "../../modules/do-ssh-key"
  pub_key = var.pub_key
  name    = var.ssh_key_name
}

module "single-droplet" {
  source = "../../modules/single-droplet"

  name                 = var.droplet_name
  region               = var.region
  pvt_key              = var.pvt_key
  ssh_key_fingerprints = [module.ssh_key_register.fingerprint]
  file_source          = var.droplet_file_source
}

module "public_ip" {
  source     = "../../modules/do-public-ip"
  region     = var.region
  droplet_id = module.single-droplet.droplet_id
}

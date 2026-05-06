module "ssh-key-register" {
  source  = "../../modules/do-ssh-key"
  pub_key = var.pub_key
}
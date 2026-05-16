# add the ssh key
resource "digitalocean_ssh_key" "minitwit" {
  name       = var.name
  public_key = file(var.pub_key)
}
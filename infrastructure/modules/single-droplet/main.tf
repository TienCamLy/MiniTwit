# https://registry.terraform.io/providers/digitalocean/digitalocean/latest/docs/resources/droplet

resource "digitalocean_droplet" "single-droplet" {
  image  = var.image
  name   = var.name
  region = var.region
  size   = var.size
  # add public ssh key so we can access the machine
  ssh_keys = var.ssh_key_fingerprints

  # specify a ssh connection
  connection {
    user        = "root"
    host        = self.ipv4_address
    type        = "ssh"
    private_key = file(var.pvt_key)
    timeout     = var.ssh_connection_timeout
  }

  provisioner "file" {
    source      = var.file_source
    destination = "/root/Dockerfile"
  }

  provisioner "remote-exec" {
    inline = [
      # allow ports for docker swarm
      "ufw allow 2377/tcp",
      "ufw allow 7946",
      "ufw allow 4789/udp",
      # ports for apps
      "ufw allow 80",
      "ufw allow 8080",
      "ufw allow 8888",
      # SSH
      "ufw allow 22",

      # initialize docker swarm cluster
      "docker swarm init --advertise-addr ${self.ipv4_address}"
    ]
  }
}
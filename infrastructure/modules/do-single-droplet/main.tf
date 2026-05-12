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
    source      = var.compose_file_source
    destination = "/root/compose.yaml"
  }

  provisioner "remote-exec" {
    inline = [
      # ports for apps
      "ufw allow 80",
      "ufw allow 8080",
      "ufw allow 8081",
      # SSH
      "ufw allow 22",

      # start the services
      "sudo DEBIAN_FRONTEND=noninteractive apt-get install -y -qq docker.io docker-compose-v2",
      "docker compose -f /root/compose.yaml up -d --build"
    ]
  }

  # Imported (or manually keyed) droplets often diverge from state for ssh_keys; the DO provider
  # replaces the droplet if ssh_keys changes. Ignore drift so plans stay non-destructive.
  lifecycle {
    ignore_changes = [ssh_keys, image]
  }
}
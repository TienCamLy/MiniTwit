# Code obtained from exercise repo https://github.com/itu-devops/itu-minitwit-docker-swarm-teraform/blob/master/minitwit_swarm_cluster.tf
#  _                _
# | | ___  __ _  __| | ___ _ __
# | |/ _ \/ _` |/ _` |/ _ \ '__|
# | |  __/ (_| | (_| |  __/ |
# |_|\___|\__,_|\__,_|\___|_|

# create cloud vm
resource "digitalocean_droplet" "minitwit-swarm-leader" {
  image  = var.droplet_image // ubuntu-22-04-x64
  name   = var.swarm_leader_name
  region = var.region
  size   = var.droplet_size
  tags   = var.swarm_leader_tags
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
    source      = var.docker_stack_file_source
    destination = "/root/minitwit_stack.yml"
  }

  provisioner "remote-exec" {
    inline = [
      # initialize docker swarm cluster
      "sudo DEBIAN_FRONTEND=noninteractive apt-get install -y -qq docker.io",
      "docker swarm init --advertise-addr ${self.ipv4_address}"
    ]
  }

  # Imported (or manually keyed) droplets often diverge from state for ssh_keys; the DO provider
  # replaces the droplet if ssh_keys changes. Ignore drift so plans stay non-destructive.
  lifecycle {
    ignore_changes = [ssh_keys, image]
  }
}

resource "terraform_data" "swarm-worker-token" {
  depends_on = [digitalocean_droplet.minitwit-swarm-leader]

  # save the worker join token
  provisioner "local-exec" {
    command = "ssh -o 'ConnectionAttempts 3600' -o 'StrictHostKeyChecking no' root@${digitalocean_droplet.minitwit-swarm-leader.ipv4_address} -i ${var.local_exec_ssh_identity_path} 'docker swarm join-token worker -q' > temp/worker_token"
  }
}

resource "terraform_data" "swarm-manager-token" {
  depends_on = [digitalocean_droplet.minitwit-swarm-leader]
  # save the manager join token
  provisioner "local-exec" {
    command = "ssh -o 'ConnectionAttempts 3600' -o 'StrictHostKeyChecking no' root@${digitalocean_droplet.minitwit-swarm-leader.ipv4_address} -i ${var.local_exec_ssh_identity_path} 'docker swarm join-token manager -q' > temp/manager_token"
  }
}


#  _ __ ___   __ _ _ __   __ _  __ _  ___ _ __
# | '_ ` _ \ / _` | '_ \ / _` |/ _` |/ _ \ '__|
# | | | | | | (_| | | | | (_| | (_| |  __/ |
# |_| |_| |_|\__,_|_| |_|\__,_|\__, |\___|_|
#                              |___/

# create cloud vm
resource "digitalocean_droplet" "minitwit-swarm-manager" {
  # create managers after the leader
  depends_on = [terraform_data.swarm-manager-token]

  # number of vms to create
  count = var.swarm_manager_count

  image  = var.droplet_image
  name   = "${var.swarm_manager_name_prefix}-${count.index}"
  region = var.region
  size   = var.droplet_size
  tags   = var.swarm_manager_tags
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
    source      = "temp/manager_token"
    destination = "/root/manager_token"
  }

  provisioner "remote-exec" {
    inline = [
      # join swarm cluster as managers
      "sudo DEBIAN_FRONTEND=noninteractive apt-get install -y -qq docker.io",
      "docker swarm join --token $(cat manager_token) ${digitalocean_droplet.minitwit-swarm-leader.ipv4_address}"
    ]
  }

  # Imported (or manually keyed) droplets often diverge from state for ssh_keys; the DO provider
  # replaces the droplet if ssh_keys changes. Ignore drift so plans stay non-destructive.
  lifecycle {
    ignore_changes = [ssh_keys, image]
  }
}


#                     _
# __      _____  _ __| | _____ _ __
# \ \ /\ / / _ \| '__| |/ / _ \ '__|
#  \ V  V / (_) | |  |   <  __/ |
#   \_/\_/ \___/|_|  |_|\_\___|_|
#
# create cloud vm
resource "digitalocean_droplet" "minitwit-swarm-worker" {
  # create workers after the leader
  depends_on = [terraform_data.swarm-worker-token]

  # number of vms to create
  count = var.swarm_worker_count

  image  = var.droplet_image
  name   = "${var.swarm_worker_name_prefix}-${count.index}"
  region = var.region
  size   = var.droplet_size
  tags   = var.swarm_worker_tags
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
    source      = "temp/worker_token"
    destination = "/root/worker_token"
  }

  provisioner "remote-exec" {
    inline = [
      # join swarm cluster as workers
      "sudo DEBIAN_FRONTEND=noninteractive apt-get install -y -qq docker.io",
      "docker swarm join --token $(cat worker_token) ${digitalocean_droplet.minitwit-swarm-leader.ipv4_address}"
    ]
  }

  # Imported (or manually keyed) droplets often diverge from state for ssh_keys; the DO provider
  # replaces the droplet if ssh_keys changes. Ignore drift so plans stay non-destructive.
  lifecycle {
    ignore_changes = [ssh_keys, image]
  }
}

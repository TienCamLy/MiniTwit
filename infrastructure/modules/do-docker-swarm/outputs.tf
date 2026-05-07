output "minitwit-swarm-leader-ip-address" {
  value = digitalocean_droplet.minitwit-swarm-leader.ipv4_address
}

output "minitwit-swarm-manager-ip-address" {
  value = digitalocean_droplet.minitwit-swarm-manager.*.ipv4_address
}

output "minitwit-swarm-worker-ip-address" {
  value = digitalocean_droplet.minitwit-swarm-worker.*.ipv4_address
}

output "minitwit-swarm-leader-droplet-id" {
  value = digitalocean_droplet.minitwit-swarm-leader.id
}

output "minitwit-swarm-manager-droplet-ids" {
  value = digitalocean_droplet.minitwit-swarm-manager.*.id
}

output "minitwit-swarm-worker-droplet-ids" {
  value = digitalocean_droplet.minitwit-swarm-worker.*.id
}
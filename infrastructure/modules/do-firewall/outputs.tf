output "firewall_id" {
  value       = digitalocean_firewall.swarm.id
  description = "ID of the DigitalOcean Cloud Firewall managing the swarm droplets."
}

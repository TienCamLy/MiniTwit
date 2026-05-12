output "droplet_id" {
  description = "DigitalOcean droplet ID (for floating IP assignment and similar)."
  value       = digitalocean_droplet.single-droplet.id
}

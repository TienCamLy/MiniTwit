output "public_ip" {
  description = "IPv4 address of the floating IP assigned to the droplet."
  value       = digitalocean_floating_ip.public-ip.ip_address
}
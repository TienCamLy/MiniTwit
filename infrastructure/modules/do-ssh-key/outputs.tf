output "fingerprint" {
  description = "Fingerprint of the SSH key in DigitalOcean (for digitalocean_droplet.ssh_keys)."
  value       = digitalocean_ssh_key.minitwit.fingerprint
}

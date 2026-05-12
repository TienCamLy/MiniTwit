variable "name" {
  type        = string
  description = "Name shown in the DigitalOcean Cloud Firewalls UI."
}

variable "target_tags" {
  type        = set(string)
  description = "Droplet tags the firewall is attached to (firewall scope). Defaults to the swarm tags used by the do-docker-swarm module."
  default     = ["Manager", "Worker"]
}

variable "swarm_internal_tags" {
  type        = set(string)
  description = "Tags treated as 'inside the swarm' for source_tags on docker swarm control / gossip / overlay rules. Usually identical to target_tags."
  default     = ["Manager", "Worker"]
}

variable "monitoring_droplet_ids" {
  type        = set(string)
  description = "Droplet IDs allowed to scrape node_exporter on :9100. Leave empty to omit the rule entirely."
  default     = []
}

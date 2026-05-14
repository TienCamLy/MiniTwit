variable "region" {
  type        = string
  description = "DigitalOcean region slug for all swarm droplets (e.g. fra1, nyc3)."
}

variable "pvt_key" {
  type        = string
  description = "Filesystem path to the SSH private key used by provisioners to connect as root to each droplet."
  sensitive   = true
}

variable "ssh_key_fingerprints" {
  type        = list(string)
  description = "Fingerprints of SSH public keys already registered in DigitalOcean; attached to each droplet (same as digitalocean_droplet.ssh_keys)."
}

variable "droplet_image" {
  type        = string
  description = "DigitalOcean image slug for swarm nodes (Docker-on-Ubuntu appliance)."
  default     = "docker-20-04"
}

variable "droplet_size" {
  type        = string
  description = "Default droplet size slug for leader, managers, and workers. Per-worker overrides use swarm_worker_size_overrides."
  default     = "s-1vcpu-1gb"
}

variable "swarm_worker_size_overrides" {
  type        = map(string)
  description = "Optional droplet size per worker index (map keys: \"0\", \"1\", ...). Indexes not listed use droplet_size."
  default     = {}
}

variable "swarm_leader_name" {
  type        = string
  description = "DigitalOcean droplet name for the node that runs docker swarm init."
  default     = "minitwit-swarm-leader"
}

variable "swarm_leader_tags" {
  type        = list(string)
  description = "Tags applied only to the swarm leader droplet."
  default     = ["Manager"]
}

variable "swarm_manager_tags" {
  type        = list(string)
  description = "Tags applied only to the swarm manager droplets."
  default     = ["Manager"]
}

variable "swarm_worker_tags" {
  type        = list(string)
  description = "Tags applied only to the swarm worker droplets."
  default     = ["Worker"]
}

variable "swarm_manager_name_prefix" {
  type        = string
  description = "Prefix for manager droplet names; final name is \"<prefix>-<index>\"."
  default     = "minitwit-swarm-manager"
}

variable "swarm_worker_name_prefix" {
  type        = string
  description = "Prefix for worker droplet names; final name is \"<prefix>-<index>\"."
  default     = "minitwit-swarm-worker"
}

variable "swarm_manager_count" {
  type        = number
  description = "Number of manager nodes in addition to the leader (each joins with a manager token)."
}

variable "swarm_worker_count" {
  type        = number
  description = "Number of worker nodes joining the swarm with a worker token."
}

variable "docker_stack_file_source" {
  type        = string
  description = "Path to the compose/stack file uploaded to the leader (relative to this module's directory)."
}

variable "ssh_connection_timeout" {
  type        = string
  description = "Timeout for SSH connections used by file and remote-exec provisioners (Terraform duration string)."
  default     = "2m"
}

variable "do_token" {
  sensitive   = true
  type        = string
  description = "Digital Ocean PAT"
}

variable "pub_key" {
  type        = string
  description = "Path to the Public Key that should be copied into the VM"
}

variable "region" {
  type        = string
  description = "DigitalOcean region slug for droplets (e.g. fra1)."
  default     = "fra1"
}

variable "pvt_key" {
  type        = string
  description = "Path to the SSH private key used by swarm provisioners; must pair with pub_key."
  sensitive   = true
}

variable "ssh_key_name" {
  type = string
  description = "The name of the SSH key to use for the swarm cluster"
  default     = "minitwit-prod-key"
}

variable "docker_stack_file_source" {
  type        = string
  description = "Compose/stack file path relative to the swarm module directory (e.g. ../../../compose.yaml)."
  default     = "../../../compose.yaml"
}
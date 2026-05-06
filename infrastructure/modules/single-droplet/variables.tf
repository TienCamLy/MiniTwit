variable "image" {
  type        = string
  description = "DigitalOcean droplet image slug (e.g. docker-20-04)."
  default     = "docker-20-04"
}

variable "name" {
  type        = string
  description = "DigitalOcean droplet name."
}

variable "region" {
  type        = string
  description = "DigitalOcean region slug (e.g. fra1, nyc3)."
}

variable "size" {
  type        = string
  description = "Droplet size slug (vCPU/RAM plan)."
  default     = "s-1vcpu-1gb"
}

variable "ssh_key_fingerprints" {
  type        = list(string)
  description = "Fingerprints of SSH public keys registered in DigitalOcean; attached to the droplet."
}

variable "pvt_key" {
  type        = string
  description = "Filesystem path to the SSH private key used by the provisioner to connect as root."
  sensitive   = true
}

variable "ssh_connection_timeout" {
  type        = string
  description = "SSH timeout for file and remote-exec provisioners (Terraform duration string)."
  default     = "2m"
}

variable "file_source" {
  type        = string
  description = "Local path to the file uploaded by the file provisioner (relative to this module's directory unless an absolute path is used)."
}

variable "region" {
  type        = string
  description = "DigitalOcean region slug for the floating IP."
  default     = "fra1"
}

variable "droplet_id" {
  type        = string
  description = "DigitalOcean droplet ID to assign the floating IP to."
}
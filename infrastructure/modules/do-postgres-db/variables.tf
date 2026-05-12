variable "name" {
  type        = string
  description = "The name of the database cluster."
}

variable "engine" {
  type        = string
  description = "The engine of the database cluster."
  default     = "pg"
}

variable "engine_version" {
  type        = string
  description = "The version of the database engine."
  default     = "15"
}

variable "size" {
  type        = string
  description = "The size of the database cluster."
  default     = "db-s-1vcpu-1gb"
}

variable "region" {
  type        = string
  description = "The region of the database cluster."
  default     = "fra1"
}

variable "node_count" {
  type        = number
  description = "The number of nodes in the database cluster."
  default     = 1
}

variable "droplet_firewall_entries" {
  type        = map(string)
  description = "Static-key map of droplet IDs for managed-database firewall rules (keys must be known at plan time)."
}
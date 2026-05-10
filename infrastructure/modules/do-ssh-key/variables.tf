variable "pub_key" {
  type        = string
  description = "Path to the Public Key that should be copied into the VM"
}

variable "name" {
  type = string
  description = "Name that the SSH key will be stored under in digital ocean"
}
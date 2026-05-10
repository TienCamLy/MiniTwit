terraform {
  backend "s3" {
    endpoints = {
      s3 = "https://fra1.digitaloceanspaces.com"
    }

    region                      = "fra1"
    skip_credentials_validation = true
    skip_requesting_account_id  = true
    skip_metadata_api_check     = true
    skip_region_validation      = true
    skip_s3_checksum            = true
    acl                         = "private"
    force_path_style            = true
  }
}
terraform {
  backend "s3" {
    region                      = "fra1"
    skip_credentials_validation = true
    skip_metadata_api_check     = true
    skip_region_validation      = true
    acl                         = "private"
    endpoint                    = "https://fra1.digitaloceanspaces.com"
    force_path_style            = true
  }
}
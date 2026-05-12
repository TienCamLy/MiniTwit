resource "digitalocean_database_cluster" "postgres-db" {
  name       = var.name
  engine     = var.engine
  version    = var.engine_version
  size       = var.size
  region     = var.region
  node_count = var.node_count
}

resource "digitalocean_database_firewall" "droplet-firewall" {
  cluster_id = digitalocean_database_cluster.postgres-db.id

  dynamic "rule" {
    for_each = var.droplet_firewall_entries
    content {
      type  = "droplet"
      value = rule.value
    }
  }
}
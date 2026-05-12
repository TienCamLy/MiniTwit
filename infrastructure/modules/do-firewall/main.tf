# https://registry.terraform.io/providers/digitalocean/digitalocean/latest/docs/resources/firewall

# Shared cloud firewall applied to every swarm droplet via tags.
# Rules are the union of what makes sense across leader + workers; per-droplet
# specifics (e.g. Wazuh on a single worker) are intentionally left out and can
# be layered on top with a more targeted firewall if needed.
resource "digitalocean_firewall" "swarm" {
  name = var.name

  tags = var.target_tags

  # SSH
  inbound_rule {
    protocol         = "tcp"
    port_range       = "22"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }

  # HTTP / HTTPS — public entry point on the manager; harmless on workers
  # because the per-droplet ufw still gates which ports actually accept traffic.
  inbound_rule {
    protocol         = "tcp"
    port_range       = "80"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }
  inbound_rule {
    protocol         = "tcp"
    port_range       = "443"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }

  # Application ports (MiniTwit Razor pages + supporting service)
  inbound_rule {
    protocol         = "tcp"
    port_range       = "8080"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }
  inbound_rule {
    protocol         = "tcp"
    port_range       = "8081"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }

  # Prometheus
  inbound_rule {
    protocol         = "tcp"
    port_range       = "9090"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }

  # Loki HTTP (log ingestion / query)
  inbound_rule {
    protocol         = "tcp"
    port_range       = "3100"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }

  # Loki / Promtail gRPC + metrics
  inbound_rule {
    protocol         = "tcp"
    port_range       = "3101"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }
  inbound_rule {
    protocol         = "tcp"
    port_range       = "9095"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }
  inbound_rule {
    protocol         = "tcp"
    port_range       = "9096"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }

  # Wazuh agent (security log shipping)
  inbound_rule {
    protocol         = "tcp"
    port_range       = "1514"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }
  inbound_rule {
    protocol         = "udp"
    port_range       = "1514"
    source_addresses = ["0.0.0.0/0", "::/0"]
  }

  # node_exporter — only the monitoring droplet should be able to scrape.
  # When monitoring_droplet_ids is empty the rule is omitted entirely.
  dynamic "inbound_rule" {
    for_each = length(var.monitoring_droplet_ids) > 0 ? [1] : []
    content {
      protocol           = "tcp"
      port_range         = "9100"
      source_droplet_ids = var.monitoring_droplet_ids
    }
  }

  # Docker Swarm control plane — TLS docker daemon + swarm management.
  inbound_rule {
    protocol    = "tcp"
    port_range  = "2376"
    source_tags = var.swarm_internal_tags
  }
  inbound_rule {
    protocol    = "tcp"
    port_range  = "2377"
    source_tags = var.swarm_internal_tags
  }

  # Docker Swarm gossip (TCP + UDP)
  inbound_rule {
    protocol    = "tcp"
    port_range  = "7946"
    source_tags = var.swarm_internal_tags
  }
  inbound_rule {
    protocol    = "udp"
    port_range  = "7946"
    source_tags = var.swarm_internal_tags
  }

  # Docker Swarm overlay network (VXLAN)
  inbound_rule {
    protocol    = "udp"
    port_range  = "4789"
    source_tags = var.swarm_internal_tags
  }

  # Outbound — wide open, matches the current firewalls.
  outbound_rule {
    protocol              = "icmp"
    destination_addresses = ["0.0.0.0/0", "::/0"]
  }
  outbound_rule {
    protocol              = "tcp"
    port_range            = "1-65535"
    destination_addresses = ["0.0.0.0/0", "::/0"]
  }
  outbound_rule {
    protocol              = "udp"
    port_range            = "1-65535"
    destination_addresses = ["0.0.0.0/0", "::/0"]
  }
}

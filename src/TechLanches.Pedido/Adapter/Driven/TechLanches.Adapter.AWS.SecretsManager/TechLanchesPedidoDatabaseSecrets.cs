﻿namespace TechLanches.Adapter.AWS.SecretsManager
{
    public class TechLanchesPedidoDatabaseSecrets
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string Engine { get; set; }
    }
}

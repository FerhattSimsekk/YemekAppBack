﻿namespace Infrastructure.Settings;
public class MailServerSettings
{
    public string DisplayName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public bool UseSsl { get; set; }
}
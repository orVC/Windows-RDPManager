namespace RDPManager.Models;

public class RdpConnection
{
    public int Id { get; set; }
    public string ServerAddress { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    public string DisplayName => $"{ServerAddress} ({Username})";
}

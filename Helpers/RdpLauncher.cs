using System.Diagnostics;
using RDPManager.Models;

namespace RDPManager.Helpers;

public static class RdpLauncher
{
    public static void Launch(RdpConnection connection)
    {
        if (string.IsNullOrWhiteSpace(connection.ServerAddress))
            return;

        if (!string.IsNullOrEmpty(connection.Username) && !string.IsNullOrEmpty(connection.Password))
        {
            var server = connection.ServerAddress;
            if (!server.Contains(":"))
                server = $"{server}:3389";

            var proc = new ProcessStartInfo("cmdkey")
            {
                Arguments = $"/generic:TERMSRV/{connection.ServerAddress} /user:\"{connection.Username}\" /pass:\"{connection.Password}\"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
            };
            Process.Start(proc)?.WaitForExit(3000);
        }

        Process.Start("mstsc", $"/v:{connection.ServerAddress}");
    }
}

using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;
using RDPManager.Models;

namespace RDPManager.Data;

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
        Initialize();
    }

    private void Initialize()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS RdpConnections (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ServerAddress TEXT NOT NULL,
                Username TEXT NOT NULL,
                PasswordEncrypted TEXT NOT NULL,
                Notes TEXT DEFAULT ''
            )
        """;
        cmd.ExecuteNonQuery();
    }

    public List<RdpConnection> GetAll()
    {
        var list = new List<RdpConnection>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, ServerAddress, Username, PasswordEncrypted, Notes FROM RdpConnections ORDER BY ServerAddress";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new RdpConnection
            {
                Id = reader.GetInt32(0),
                ServerAddress = reader.GetString(1),
                Username = reader.GetString(2),
                Password = reader.IsDBNull(3) ? "" : Decrypt(reader.GetString(3)),
                Notes = reader.IsDBNull(4) ? "" : reader.GetString(4),
            });
        }
        return list;
    }

    public void Insert(RdpConnection conn)
    {
        using var c = new SqliteConnection(_connectionString);
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "INSERT INTO RdpConnections (ServerAddress, Username, PasswordEncrypted, Notes) VALUES (@addr, @user, @pwd, @notes)";
        cmd.Parameters.AddWithValue("@addr", conn.ServerAddress);
        cmd.Parameters.AddWithValue("@user", conn.Username);
        cmd.Parameters.AddWithValue("@pwd", Encrypt(conn.Password));
        cmd.Parameters.AddWithValue("@notes", conn.Notes ?? "");
        cmd.ExecuteNonQuery();
    }

    public void Update(RdpConnection conn)
    {
        using var c = new SqliteConnection(_connectionString);
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "UPDATE RdpConnections SET ServerAddress=@addr, Username=@user, PasswordEncrypted=@pwd, Notes=@notes WHERE Id=@id";
        cmd.Parameters.AddWithValue("@id", conn.Id);
        cmd.Parameters.AddWithValue("@addr", conn.ServerAddress);
        cmd.Parameters.AddWithValue("@user", conn.Username);
        cmd.Parameters.AddWithValue("@pwd", Encrypt(conn.Password));
        cmd.Parameters.AddWithValue("@notes", conn.Notes ?? "");
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var c = new SqliteConnection(_connectionString);
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "DELETE FROM RdpConnections WHERE Id=@id";
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    private static string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return "";
        var data = Encoding.UTF8.GetBytes(plainText);
        var encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(encrypted);
    }

    private static string Decrypt(string encryptedBase64)
    {
        if (string.IsNullOrEmpty(encryptedBase64)) return "";
        try
        {
            var data = Convert.FromBase64String(encryptedBase64);
            var decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decrypted);
        }
        catch
        {
            return "";
        }
    }
}

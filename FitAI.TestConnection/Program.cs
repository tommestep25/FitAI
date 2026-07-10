using Npgsql;

var connectionString =
    "Host=aws-1-ap-southeast-1.pooler.supabase.com;" +
    "Port=5432;" +
    "Database=postgres;" +
    "Username=postgres.haibdshlekwlbiztpapt;" +
    "Password=Luckygamer7012;" +
    "TrustServerCertificate=true;" +
    "Pooling=false;" +
    "GSS Encryption Mode=Disable;" +
    "Timeout=30;" +
    "Command Timeout=30;";
try
{
    await using var conn = new NpgsqlConnection(connectionString);
    await conn.OpenAsync();

    Console.WriteLine("Connected successfully.");

    await using var cmd = new NpgsqlCommand("select now();", conn);
    var result = await cmd.ExecuteScalarAsync();

    Console.WriteLine($"Database time: {result}");
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
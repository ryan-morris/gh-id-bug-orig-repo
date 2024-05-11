// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using Npgsql;

Console.WriteLine("Initial Vulnerable Version");

byte[] keyBytes = new byte[24];
byte[] encryptedBytes = Array.Empty<byte>();

using var aesAlg = Aes.Create();

aesAlg.Mode = CipherMode.ECB; // detected by advanced security
aesAlg.Padding = PaddingMode.Zeros;
aesAlg.Key = keyBytes.Take(24).ToArray();

byte[] result = Array.Empty<byte>();

var decryptedPart = aesAlg.DecryptEcb(encryptedBytes.AsSpan().Slice(0, encryptedBytes.Length), PaddingMode.None);
decryptedPart.CopyTo(result, 0);

var anotherOne = new NpgsqlConnection("Server=10.10.10.25,1433;Database=production;User ID=sa;Password=zcBFjQ*nARN9S9;Trusted_Connection=False;Encrypt=True;");
await anotherOne.OpenAsync();
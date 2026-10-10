using Platform.Core.Enums;
using System.Security.Cryptography;

namespace Platform.Infrastructure.Hashing
{
    public static class HashService
    {
        public static string ComputeHash(byte[] data, HashType hashType = HashType.MD5)
        {
            using HashAlgorithm hasher = hashType switch
            {
                HashType.MD5 => MD5.Create(),
                HashType.SHA256 => SHA256.Create(),
                HashType.SHA512 => SHA512.Create(),
                _ => throw new ArgumentOutOfRangeException(nameof(hashType))
            };

            byte[] hashByte = hasher.ComputeHash(data);

            return Convert.ToHexString(hashByte);
        }
    }
}

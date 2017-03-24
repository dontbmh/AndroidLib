/*
 * Hasher.cs - Computes File Hashes
 * Developed by Dan Wager
 * 05/31/2011
 * Revised 10/27/2011
 */

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AndroidLib.Classes.Util
{
    internal enum HashType
    {
        Md5,
        Sha1,
        Sha256,
        Sha384,
        Sha512
    }

    internal static class Hasher
    {
        private static readonly StringBuilder Builder = new StringBuilder();

        internal static string HashFile(string inFile, HashType algo)
        {
            byte[] hashBytes = null;

            switch (algo)
            {
                case HashType.Md5:
                    hashBytes = MD5.Create().ComputeHash(new FileStream(inFile, FileMode.Open));
                    break;
                case HashType.Sha1:
                    hashBytes = SHA1.Create().ComputeHash(new FileStream(inFile, FileMode.Open));
                    break;
                case HashType.Sha256:
                    hashBytes = SHA256.Create().ComputeHash(new FileStream(inFile, FileMode.Open));
                    break;
                case HashType.Sha384:
                    hashBytes = SHA384.Create().ComputeHash(new FileStream(inFile, FileMode.Open));
                    break;
                case HashType.Sha512:
                    hashBytes = SHA512.Create().ComputeHash(new FileStream(inFile, FileMode.Open));
                    break;
            }

            return MakeHashString(hashBytes);
        }

        internal static string HashString(string inString, HashType algo)
        {
            byte[] inStringBytes = null, hashBytes = null;

            inStringBytes = Encoding.ASCII.GetBytes(inString);

            switch (algo)
            {
                case HashType.Md5:
                    hashBytes = MD5.Create().ComputeHash(inStringBytes);
                    break;
                case HashType.Sha1:
                    hashBytes = SHA1.Create().ComputeHash(inStringBytes);
                    break;
                case HashType.Sha256:
                    hashBytes = SHA256.Create().ComputeHash(inStringBytes);
                    break;
                case HashType.Sha384:
                    hashBytes = SHA384.Create().ComputeHash(inStringBytes);
                    break;
                case HashType.Sha512:
                    hashBytes = SHA512.Create().ComputeHash(inStringBytes);
                    break;
            }

            return MakeHashString(hashBytes);
        }

        private static string MakeHashString(byte[] hash)
        {
            Builder.Remove(0, Builder.Length);

            foreach (var b in hash)
                Builder.Append(b.ToString("x2").ToLower());

            return Builder.ToString();
        }
    }
}
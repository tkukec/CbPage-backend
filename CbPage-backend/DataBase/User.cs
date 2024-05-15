using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CbPage_backend.Encoders;

namespace CbPage_backend.DataBase
{
    public class User : ITable
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] salt { get; set; }
        public string RealName { get; set; }
        public string RealLastName { get; set; }
        public string Email { get; set; }

        public User(string Username, byte[] PasswordHash, byte[] salt, string RealName, string RealLastName, string Email) {
            this.Username = Username;
            this.PasswordHash = PasswordHash;
            this.salt = salt;
            this.RealName = RealName;
            this.RealLastName = RealLastName;
            this.Email = Email;
        }
        
        public User(string Username, string RealName, string RealLastName, string Email) {
            this.Username = Username;
            this.PasswordHash = PasswordHash;
            this.salt = salt;
            this.RealName = RealName;
            this.RealLastName = RealLastName;
            this.Email = Email;
        }

        public void PushToDatabase()
        {
            string fileName = "./users/" + Username;
            byte[][] bytesArray =
            {
                ByteEncoder.Encode(Username),
                ByteEncoder.Encode(PasswordHash),
                ByteEncoder.Encode(salt),
                ByteEncoder.Encode(RealName),
                ByteEncoder.Encode(RealLastName),
                ByteEncoder.Encode(Email)
            };
            using (FileStream file = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
                foreach (byte[] bytes in bytesArray)
                    file.Write(bytes);
        }

        public static User? PullFromDatabase(string username)
        {
            string fileName = "./users/" + username;
            if (!File.Exists(fileName))
                return null;
            byte[] bytes = File.ReadAllBytes(fileName);

            string Username;
            byte[] PasswordHash;
            byte[] salt;
            string RealName;
            string RealLastName;
            string Email;

            int offset;
            Span<byte> bytesSpan = new Span<byte>(bytes, 0, bytes.Length);
            (Username, offset) = ByteDecoder.DecodeString(bytesSpan);
            bytesSpan = new Span<byte>(bytes, offset, bytes.Length-offset);
            (PasswordHash, offset) = ByteDecoder.DecodeArray(bytesSpan);
            bytesSpan = new Span<byte>(bytes, offset, bytes.Length-offset);
            (salt, offset) = ByteDecoder.DecodeArray(bytesSpan);
            bytesSpan = new Span<byte>(bytes, offset, bytes.Length-offset);
            (RealName, offset) = ByteDecoder.DecodeString(bytesSpan);
            bytesSpan = new Span<byte>(bytes, offset, bytes.Length-offset);
            (RealLastName, offset) = ByteDecoder.DecodeString(bytesSpan);
            bytesSpan = new Span<byte>(bytes, offset, bytes.Length-offset);
            (Email, offset) = ByteDecoder.DecodeString(bytesSpan);

            return new User(Username, PasswordHash, salt, RealName, RealLastName, Email);
        }

        public void GenerateSalt()
        {
            byte[][] bytesArray =
            {
                Encoding.UTF8.GetBytes(Username),
                Encoding.UTF8.GetBytes(RealName),
                Encoding.UTF8.GetBytes(RealLastName),
                Encoding.UTF8.GetBytes(Email),
                BitConverter.GetBytes(DateTime.Now.Ticks),
                new byte[128]
            };
            new Random().NextBytes(bytesArray.Last());
            byte[] bytes = (byte[])bytesArray.SelectMany(x => x);
            using (SHA512 sha512 = SHA512.Create())
                bytes = sha512.ComputeHash(bytes);
            salt = bytes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CbPage_backend.Encoders;

namespace CbPage_backend.DataBase
{
    public class Comment : ITable
    {
        public string CommentText { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public Status Status { get; set; }

        public Comment(string CommentText, DateTime Date, User User, Status Status)
        {
            this.CommentText = CommentText;
            this.Date = Date;
            this.User = User;
            this.Status = Status;
        }

        public void PushToDatabase()
        {
            string fileName = "./comments/" + Date.Ticks + User.Username;
            byte[][] bytesArray =
            {
                ByteEncoder.Encode(CommentText),
                ByteEncoder.Encode(Date.Ticks),
                ByteEncoder.Encode(User.Username),
                ByteEncoder.Encode((int)Status)
            };
            using (FileStream file = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
                foreach (byte[] bytes in bytesArray)
                    file.Write(bytes);
        }

        public static Comment? PullFromDatabase(string commentId)
        {
            string fileName = "./users/" + commentId;
            if (!File.Exists(fileName))
                return null;
            byte[] bytes = File.ReadAllBytes(fileName);

            string CommentText;
            long timestamp;
            string username;
            int status;

            int offset;
            Span<byte> bytesSpan = new Span<byte>(bytes, 0, bytes.Length);
            (CommentText, offset) = ByteDecoder.DecodeString(bytesSpan);
            bytesSpan = new Span<byte>(bytes, offset, bytes.Length-offset);
            (timestamp, offset) = ByteDecoder.DecodeLong(bytesSpan);
            bytesSpan = new Span<byte>(bytes, offset, bytes.Length-offset);
            (username, offset) = ByteDecoder.DecodeString(bytesSpan);
            bytesSpan = new Span<byte>(bytes, offset, bytes.Length-offset);
            (status, offset) = ByteDecoder.DecodeInt(bytesSpan);

            return new Comment(
                CommentText,
                new DateTime(timestamp),
                User.PullFromDatabase(username),
                (Status)status
            );
        }
    }
}

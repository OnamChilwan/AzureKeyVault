namespace KeyVault.Client.Models
{
    using System;

    public class Token
    {
        private readonly string token;

        public Token(string token)
        {
            this.token = token;
        }

        public static Token Create()
        {
            //var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Guid.NewGuid()}{DateTime.Now}"));
            return new Token($"{Guid.NewGuid()}");
        }

        public override string ToString()
        {
            return this.token;
        }
    }
}
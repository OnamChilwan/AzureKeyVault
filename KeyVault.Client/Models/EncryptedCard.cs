namespace KeyVault.Client.Models
{
    public class EncryptedCard
    {
        public byte[] CardNumber { get; set; }

        public byte[] EndDate { get; set; }

        public byte[] NameOnCard { get; set; }
    }
}
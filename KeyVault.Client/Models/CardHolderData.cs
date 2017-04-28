namespace KeyVault.Client.Models
{
    using System;

    [Serializable]
    public class CardHolderData
    {
        public string CardNumber { get; set; }

        public string EndDate { get; set; }

        public string NameOnCard { get; set; }
    }
}
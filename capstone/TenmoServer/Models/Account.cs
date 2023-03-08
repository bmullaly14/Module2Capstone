namespace TenmoServer.Models
{
    public class Account
    {
        //PROPERTIES
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; } = 1000;

        //CONSTRUCTORS
        public Account() { } // NEED a blank constructor for deserialization! :)
        public Account(int userId)
        {
            UserId = userId;
        }
    }
}

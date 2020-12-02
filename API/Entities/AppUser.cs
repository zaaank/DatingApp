namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        //public,protected,private -> protected can also be used by inherited class
        public string UserName { get; set; }

        public byte[] PasswordHash {get;set;}

        public byte[] PasswordSalt { get; set; }

        
    }
} 
using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        //public,protected,private -> protected can also be used by inherited class
        public string Username { get; set; }
        public string PhotoUrl { get; set; }
        
        public int Age { get; set; }

        public string KnownAs { get; set; }
        public DateTime Created { get; set; } 

        public DateTime LastActive { get; set; }
        public string gender { get; set; } 
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
    }
}
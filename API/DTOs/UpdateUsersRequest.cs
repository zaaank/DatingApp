using System.Collections.Generic;

namespace API.DTOs
{
    public class UpdateUsersRequest
    {
        public List<MemberDto> usersTable {get; set;}
    }
}
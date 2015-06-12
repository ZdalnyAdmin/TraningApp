using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.Models.DTO
{
    public class OrganizationDto : CommonDto
    {
        public int AssignedUser { get; set; }
        public int BlockedUser { get; set; }
        public int DeleteUser { get; set; }
        public int InvationUser { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public decimal UsedSpaceDisk { get; set; }
    }
}

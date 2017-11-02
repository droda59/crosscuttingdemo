using System;
using CrossCutting.Common.Models;

namespace CrossCutting.Web.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public Employer Employer { get; set; }
    }
}
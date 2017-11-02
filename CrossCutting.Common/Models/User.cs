using System;

namespace CrossCutting.Common.Models
{
    public class User : Document
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public Employer Employer { get; set; }

        public override string ToString()
        {
            var id = this.Id;
            if (id == Guid.Empty)
            {
                return "New user";
            }

            return $"{id}: {this.FirstName} {this.LastName}";
        }
    }
}
using System;

namespace Web.Models.Group
{
    public class Model
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int NumberOfMembers { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public State State { get; set; }

        public Model()
        {

        }
    }
}
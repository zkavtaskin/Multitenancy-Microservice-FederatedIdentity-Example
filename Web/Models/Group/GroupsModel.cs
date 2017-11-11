using System.Collections.Generic;

namespace Web.Models.Group
{
    public class GroupsModel 
    {
        public GroupsModel()
        {
            this.Groups = new List<Model>();
        }

        public List<Model> Groups { get; set; }
    }
}
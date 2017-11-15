namespace Web.Models.Dashboard
{
    public abstract class Model 
    {
        public string TenantFriendlyName { get; }

        public Model(string tenantFriendlyName)
        {
            this.TenantFriendlyName = tenantFriendlyName;
        }
    }
}
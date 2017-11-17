namespace Web.Middleware
{
    public interface ITenantNameExtractor
    {
        bool CanExtract();
        string GetName();
    }
}

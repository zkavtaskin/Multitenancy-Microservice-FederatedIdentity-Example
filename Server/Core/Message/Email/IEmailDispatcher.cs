
namespace Server.Core.Message.Email
{
    public interface IEmailDispatcher
    {
        void Dispatch(Email message);
    }
}

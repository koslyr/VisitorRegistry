
namespace VisReg.Srv.Interfaces
{
    public interface ISrvAuthentication
    {
        bool VerifyHashedPassword(string HashedPasswordSalt, string ProvidedPassword);
    }
}

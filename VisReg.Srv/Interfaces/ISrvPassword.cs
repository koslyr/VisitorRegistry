using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisReg.Srv.Interfaces
{
    public interface ISrvPassword
    {
        string[] GetHashPasswordAndSalt(string UserPassword);
    }
}

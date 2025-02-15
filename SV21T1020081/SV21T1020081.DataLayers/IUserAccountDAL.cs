using SV21T1020081.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020081.DataLayers
{
    public interface IUserAccountDAL
    {
        UserAccount? Authorize(string username,string password);

        bool ChangePassword(string username,string password);
    }
}

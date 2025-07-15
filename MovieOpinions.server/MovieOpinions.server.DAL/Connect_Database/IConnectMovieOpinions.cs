using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.DAL.Connect_Database
{
    public interface IConnectMovieOpinions
    {
        string Host { get; }

        string User { get; }

        string Password { get; }

        string Port { get; }

        string DataBaseName { get; }

        string ConnectMovieOpinionsDataBase();
    }
}

using Evolent.Models.Shared;

namespace Evolent.Database.Database
{
    public class DefaultDatabase : BaseDatabase, Contracts.IDefaultDatabase
    {
        override public string connString
        {
            get { return EvolentAppSettings.DefaultDbConnectionString; }
        }
    }
}

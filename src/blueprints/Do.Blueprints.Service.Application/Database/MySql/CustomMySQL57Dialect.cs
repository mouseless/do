using NHibernate.Dialect;
using System.Data;

namespace Do.Database.MySql;

public class CustomMySQL57Dialect : MySQL57Dialect
{
    public CustomMySQL57Dialect() =>
        RegisterColumnType(DbType.String, 1023, "VARCHAR($l)");
}

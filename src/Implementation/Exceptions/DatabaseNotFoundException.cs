namespace Implementation.Exceptions
{
    /// <summary>
    /// Fires then the wrong database name was used or data base was not found
    /// </summary>
    public class DatabaseNotFoundException : EtlException
    {
        public DatabaseNotFoundException(string dataBaseName)
            : base(typeof(DatabaseNotFoundException),  string.Format("Database `{0}` was not found", dataBaseName))
        {
        }
    }


    /// <summary>
    /// Fires then the the data consitancey is critically broken in mongodb
    /// </summary>
    public class DataBaseConsitancyIsBrokenException : EtlException
    {
        public DataBaseConsitancyIsBrokenException(string entityName, string propertyName)
            : base(typeof(DataBaseConsitancyIsBrokenException), string.Format(" In entity `{0}` data consistancy is broken. Property: {1}", entityName, propertyName))
        {
        }
    }
}
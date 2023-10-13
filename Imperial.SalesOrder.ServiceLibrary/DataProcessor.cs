using Imperial.SalesOrder.Data;
using Imperial.SalesOrder.DataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imperial.SalesOrder.ServiceLibrary
{
    public class DataProcessor
    {
        public static List<GenericLookup> GetLookupsByType(string lookupTypeCode)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("LookupTypeCode", lookupTypeCode)
            };

            using (Repository repo = new Repository())
            {
                return repo.GetRecords<GenericLookup>("usp_GetLookupsByType", sqlParameters);
            }
        }
        public static GenericLookup GetLookUpValue(int valueId)
        {
            List<SqlParameter> lsParms = new List<SqlParameter>()
            {
                new SqlParameter("ValueId",valueId)
            };

            using (Repository repo = new Repository())
            {
                return repo.GetRecord<GenericLookup>("usp_GetLookUpValue", lsParms);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Imperial.SalesOrder.Data
{
    public static class Helper
    {
        public static T ToType<T>(this DataRow row) where T : new()
        {
            T val = new T();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(val);
            foreach (PropertyDescriptor item in properties)
            {
                if (row.Table.Columns.IndexOf(item.Name) < 0)
                {
                    continue;
                }

                if (!item.PropertyType.IsEnum)
                {
                    if (row[item.Name].GetType() == item.PropertyType)
                    {
                        item.SetValue(val, row[item.Name]);
                    }
                }
                else
                {
                    item.SetValue(val, Enum.Parse(item.PropertyType, row[item.Name].ToString()));
                }
            }

            return val;
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyDescriptor item in properties)
            {
                dataTable.Columns.Add(item.Name, Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType);
            }

            foreach (T datum in data)
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (PropertyDescriptor item2 in properties)
                {
                    if (item2.PropertyType == typeof(DateTime))
                    {
                        DateTime dateTime = Convert.ToDateTime(item2.GetValue(datum));
                        if (dateTime.CompareTo(new DateTime(1, 1, 1)) == 0)
                        {
                            dataRow[item2.Name] = DBNull.Value;
                        }
                        else
                        {
                            dataRow[item2.Name] = dateTime;
                        }
                    }
                    else
                    {
                        dataRow[item2.Name] = item2.GetValue(datum) ?? DBNull.Value;
                    }
                }

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }
}

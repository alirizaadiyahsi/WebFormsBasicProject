using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace WebFormsProject.Data
{
    public static class AdoExtensions
    {
        // Null olabilecek parametreler için DBNull kullanılmalı, bunun kontolünü bu metod yapıyor
        // http://www.codeproject.com/Tips/310792/Handling-null-values-in-SqlParameter-parameter-obj
        /// <summary>
        /// Null olan parametreler için default değer ataması yapar.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="parameterName">Parametre ismi</param>
        /// <param name="value">Paremetre değeri</param>
        /// <param name="nullValue">Null ise, girilecek değer</param>
        /// <returns></returns>
        public static SqlParameter AddWithValue(this SqlParameterCollection target, string parameterName, object value, object nullValue)
        {
            if (value == null)
            {
                return target.AddWithValue(parameterName, nullValue ?? DBNull.Value);
            }
            return target.AddWithValue(parameterName, value);
        }

        public static string GetNullableString(this DbDataReader reader, string columnName)
        {
            object value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }
            return value.ToString();
        }

        public static string GetString(this DbDataReader reader, string columnName)
        {
            object value = reader[columnName];
            return value.ToString();
        }

        public static T GetNullable<T>(this DbDataReader reader, string columnName)
        {
            object value = reader[columnName];
            if (value == DBNull.Value)
            {
                return default(T);
            }
            return Get<T>(reader, columnName);
        }

        public static T Get<T>(this DbDataReader reader, string columnName)
        {
            try
            {
                return (T)reader[columnName];
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException("Specified cast is not valid, field: " + columnName, ex);
            }
        }

        public static string GetNullableString(this DataRow dr, string columnName)
        {
            object value = dr[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }
            return value.ToString();
        }

        public static T? GetNullableStruct<T>(this DataRow dr, string columnName) where T : struct, IConvertible
        {
            var value = dr[columnName];
            if (value == DBNull.Value)
            {
                return (T?)null;
            }
            if (typeof(T) == typeof(DateTime))
            {
                return (T)Convert.ChangeType(dr.GetDate(columnName), typeof(T));
            }
            if (typeof(T) == typeof(bool))
            {
                //Depending on the db engine it can come as a boxed boolean or a boxed int
                //Watch out for the ints!
                var stringValue = Convert.ToString(value);
                if (stringValue == "0")
                {
                    return (T)Convert.ChangeType(false, typeof(T));
                }
                else if (stringValue == "1")
                {
                    return (T)Convert.ChangeType(true, typeof(T));
                }
            }
            return (T)value;
        }

        public static string GetString(this DataRow dr, string columnName)
        {
            return dr.GetNullable<string>(columnName);
        }

        public static T GetNullable<T>(this DataRow dr, string columnName)
        {
            object value = dr[columnName];
            if (value == DBNull.Value)
            {
                return default(T);
            }
            return Get<T>(dr, columnName);
        }

        /// <summary>
        /// Gets the date in UTC Kind
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static DateTime GetDate(this DataRow dr, string columnName)
        {
            DateTime date = (DateTime)dr[columnName];
            return DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }

        public static T Get<T>(this DataRow dr, string columnName)
        {
            try
            {
                if (dr[columnName] == DBNull.Value)
                {
                    throw new NoNullAllowedException("Column " + columnName + " has a null value.");
                }
                Type type = typeof(T);
                if (type == typeof(DateTime))
                {
                    throw new ArgumentException("Date time not supported.");
                }
                else if (type == typeof(Guid))
                {
                    return (T)(object)new Guid(dr[columnName].ToString());
                }
                else if (type.IsEnum)
                {
                    return (T)Enum.Parse(type, dr[columnName].ToString());
                }

                return (T)dr[columnName];
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException("Specified cast is not valid, field: " + columnName + ", Type: " + typeof(T).FullName, ex);
            }
        }

        /// <summary>
        /// Safely opens the connection, executes and closes the connection
        /// </summary>
        /// <param name="comm"></param>
        /// <returns>The number of rows affected.</returns>
        public static int SafeExecuteNonQuery(this DbCommand comm)
        {
            int rowsAffected = 0;
            try
            {
                comm.Connection.Open();
                rowsAffected = comm.ExecuteNonQuery();
            }
            catch (Exception ex) { }
            finally
            {
                comm.Connection.Close();
            }
            return rowsAffected;
        }

        /// <summary>
        /// Safely opens the connection, executes and closes the connection
        /// </summary>
        /// <param name="comm"></param>
        /// <returns>The number of rows affected.</returns>
        public static int SafeExecuteScalar(this DbCommand comm)
        {
            int id = 0;
            try
            {
                comm.Connection.Open();
                id = Int32.Parse(comm.ExecuteScalar().ToString());
            }
            catch (Exception ex) { }
            finally
            {
                comm.Connection.Close();
            }
            return id;
        }
    }
}

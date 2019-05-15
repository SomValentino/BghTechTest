using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGTechTest.Web.API.Helpers
{
    public class CsvSerializer : IDataSerializer
    {
        public string CsvData { get; private set; }

        private string GetHeaders<T>() where T : class, new()
        {
            var propertyNames = GetProperties<T>().Select(a => a.Name);
            return string.Join(",", propertyNames);
        }

        private PropertyInfo[] GetProperties<T>() where T : class, new()
        {
            Type dataType = typeof(T);
            return dataType.GetProperties(BindingFlags.Public | BindingFlags.Instance
                                                                  | BindingFlags.GetProperty | BindingFlags.SetProperty);
        }
        public async Task Serialize<T>(Stream stream, IEnumerable<T> data) where T : class, new()
        {
            try
            {
                var properties = GetProperties<T>();
                var values = new List<string>();
                StringBuilder sb = new StringBuilder();
                
                foreach (var item in data)
                {
                    values.Clear();
                    foreach (var property in properties)
                    {
                        var rawValue = property.GetValue(item);
                        var value = rawValue == null ? string.Empty :
                            rawValue is DateTime ? ((DateTime) rawValue).ToString("yyyy/MM/dd") :
                            rawValue.ToString();
                        values.Add(value);
                    }
                    sb.AppendLine(string.Join(",", values));
                }

                CsvData = sb.ToString();
                using (var sw = new StreamWriter(stream))
                {
                    await sw.WriteLineAsync(sb.ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> Deserialize<T>(Stream stream) where T : class, new()
        {
            string[] headers = GetHeaders<T>().Split(',');
            string[] rows;

            try
            {
                using (var sr = new StreamReader(stream))
                {
                    rows = (await sr.ReadToEndAsync())
                        .Trim().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                }
            }
            catch (Exception ex)
            { 
                throw new InvalidDataException(ex.Message,ex);
            }
            
            var dataExtract = new List<T>();
            for (int i =0; i<rows.Length;i++)
            {
                var parts = rows[i].Split(',');
                var datum = new T();
                for (int j = 0; j < parts.Length; j++)
                {
                    var dataValue = parts[j];
                    var columnName = headers[j];

                    var prop = GetProperties<T>().FirstOrDefault(a => a.Name == columnName);
          
                    var dataConverter = TypeDescriptor.GetConverter(prop.PropertyType);
                    var cvalue = dataConverter.ConvertFrom(dataValue);
                    prop.SetValue(datum,cvalue);
                }
                dataExtract.Add(datum);
            }

            return dataExtract;
        }
    }
}

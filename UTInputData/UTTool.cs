using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UTInputData
{
    public class CBOTool <T>
    {
        public static void TestGetProperty(object obj )
        {
            Type _objType = typeof(T);
            foreach (PropertyInfo objProperty in _objType.GetProperties())
            {

                var propertyvalye = objProperty.GetValue(obj);

            }
        }
        public static T CreateObjectFromType()
        {
            Dictionary<string, PropertyInfo> objProperties = GetPropertyInfo(typeof(T));

            try
            {
                T objObject = (T)Activator.CreateInstance(typeof(T));

                foreach (PropertyInfo _PropertyInfo in objProperties.Values)
                {

                    if (_PropertyInfo.CanWrite)
                    {
                        switch (_PropertyInfo.PropertyType.FullName)
                        {

                            case "System.String":
                                _PropertyInfo.SetValue(objObject, "");
                                break;

                            case "System.Boolean":
                                _PropertyInfo.SetValue(objObject, false);
                                break;

                            case "System.Decimal":
                                _PropertyInfo.SetValue(objObject, decimal.MinValue);
                                break;

                            case "System.Int16":
                                _PropertyInfo.SetValue(objObject, Int16.MinValue);
                                break;

                            case "System.Int32":
                                _PropertyInfo.SetValue(objObject, Int32.MinValue);
                                break;

                            case "System.Int64":
                                _PropertyInfo.SetValue(objObject, Int64.MinValue);
                                break;

                            case "System.DateTime":
                                _PropertyInfo.SetValue(objObject, DateTime.MinValue);
                                break;

                            case "System.Double":
                                _PropertyInfo.SetValue(objObject, double.MinValue);
                                break;

                            default:
                                // try explicit conversion
                                _PropertyInfo.SetValue(objObject, null);
                                break;
                        }
                    }
                }

                return objObject;
            }
            catch (Exception ex)
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
        }

        private static Dictionary<string, PropertyInfo> GetPropertyInfo(Type objType)
        {
            //Hashtable hashProperties = new Hashtable();
            Dictionary<string, PropertyInfo> dicProperties = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo objProperty in objType.GetProperties())
            {
                dicProperties.Add(objProperty.Name.ToUpper(), objProperty);
            }
            return dicProperties;
        }
    }
}

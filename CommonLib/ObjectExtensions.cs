using System.Reflection;

namespace CommonLib
{
    public static class ObjectExtensions
    {
        public static void TrimStringProperty(this object obj)
        {
            if (obj == null) return;

            var props = obj.GetType().GetProperties();
            if (props != null)
            {
                for (int i = 0; i < props.Length; i++)
                {
                    var prop = props[i];
                    if (prop.PropertyType == typeof(string) && prop.CanWrite)
                    {
                        string value = (string)prop.GetValue(obj, null);
                        if (!string.IsNullOrEmpty(value))
                        {
                            prop.SetValue(obj, value.Trim(), null);
                        }
                    }
                }
            }
                  
        }

        public static object GetProperty(this object obj, string propertyName, bool ignoreCase = false)
        {
            if (obj == null) return null;
            if (ignoreCase)
            {
                return obj.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase);
            }
            return obj.GetType().GetProperty(propertyName);
        }

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName)) return null;
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            if (propertyInfo == null || !propertyInfo.CanRead) return null;
            return propertyInfo.GetValue(obj);
        }

        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName)) return;
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            if (propertyInfo == null || !propertyInfo.CanWrite) return;
            propertyInfo.SetValue(obj, value);
        }
    }
}
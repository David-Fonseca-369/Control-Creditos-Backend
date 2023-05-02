using System.Reflection;

namespace Sistema_Control_Seguimiento_Backend.DTOs
{
    public static class DTOExtensions
    {
        public static void TrimTextProperties<T>(this T dto) where T : class
        {
            PropertyInfo[] properties = dto.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    if (property.GetValue(dto) is string value)
                    {
                        //Limpio los que son de tipo string
                        property.SetValue(dto, value.Trim());
                    }
                }
            }
        }
    }
}

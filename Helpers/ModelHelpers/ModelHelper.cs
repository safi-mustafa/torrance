using Newtonsoft.Json;

public static class ModelHelper
{
    public static T CreateShallowCopy<T>(this T original)
    {
        try
        {
            // Serialize the object to JSON
            string json = JsonConvert.SerializeObject(original);

            // Deserialize the JSON back into a new instance
            T copy = JsonConvert.DeserializeObject<T>(json);

            return copy;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}

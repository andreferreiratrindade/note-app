using Newtonsoft.Json;

namespace NoteApp.helpers;

public class EncodeAndDecodeObjects
{
    public string ConvertToString(object someObject)
    {
       return Base64Encode(JsonConvert.SerializeObject(someObject));
    }
 
    private static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}
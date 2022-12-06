using System.Security.Cryptography;
using System.Text;

namespace PersonGenLib.Utils;

public class RandomHash
{
    public static string GenMD5()
    {
        var md5 = MD5.Create();
        return BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(DateTime.Now.ToString()))).Replace("-", "").ToLower();
    }
}
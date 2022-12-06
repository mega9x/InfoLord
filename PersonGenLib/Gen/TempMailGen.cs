using System.Globalization;
using System.Security.Cryptography;
using PersonGenLib.Const;
using PersonGenLib.Utils;

namespace PersonGenLib.Gen;

public class TempMailGen
{
    public static string GetChacuoMail()
    {
        return RandomHash.GenMD5() + TempMail.CHACUO_DOMAIN;
    }
}
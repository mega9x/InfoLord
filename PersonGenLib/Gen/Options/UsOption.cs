using PersonGenLib.Gen.Interface;

namespace PersonGenLib.Gen.Options;

public class UsOption : IOption
{
    public string GetCountry()
    {
        return "US";
    }
}
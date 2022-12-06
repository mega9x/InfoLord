using System.Text;

namespace PersonGenLib.Gen;

public class PhoneGen
{
    public static string USPhone()
    {
        Random rand = new Random();
        StringBuilder telNo = new StringBuilder(12);
        int number;
        for (int i = 0; i < 3; i++)
        {
            number = rand.Next(0, 8);
            telNo = telNo.Append(number.ToString());
        }
        number = rand.Next(0, 743); 
        telNo = telNo.Append($"{number:D3}");
        number = rand.Next(0, 10000);   
        telNo = telNo.Append($"{number:D4}");
        return telNo.ToString();                  
    }
}
// See https://aka.ms/new-console-template for more information

using System.Text;
using PersonGenLib.Gen;
using PersonGenLib.Gen.Interface;
using PersonGenLib.Gen.Options;
using PersonGenLib.Models;

var path = "./result";

Console.WriteLine("请输入生成数量");

var num = int.Parse(Console.ReadLine() ?? string.Empty);
if (num <= 0)
{
    num = 10;
}

List<PersonModel> list = new();
IGenerator gen = new Generator(new UsOption());
for (var i = 1; i <= num; i++)
{
    await gen.GenPerson();
    while (true)
    {
        try
        {
            await gen.GenPerson();
            PersonModel p = gen.GetPersonModel();
            if (p is null) continue;
            if (!(p.Age < 18 && p.Age > 25))
            {
                list.Add(p);
                Thread.Sleep(3000);
                break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}

foreach (var v in list)
{
    SaveToFile(v);
}

Console.WriteLine("软件根目录下的 result 文件夹内检查已生成的信息");

void SaveToFile(PersonModel personModel)
{
    var filePath = Path.Combine(path, personModel.FullName + ".txt");
    
    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }
    if (!File.Exists(filePath))
    {
        using (File.Create(filePath)) {}
    }
    StringBuilder sb = new();
    sb.Append("[基本信息]" + Environment.NewLine)
        .Append("全名: " + personModel.FullName + Environment.NewLine)
        .Append("性别: " + personModel.Gender.ToString() + Environment.NewLine)
        .Append("地址: " + personModel.Address + Environment.NewLine)
        .Append("生日: " + personModel.Birthday + Environment.NewLine)
        .Append("电话: " + personModel.Phone + Environment.NewLine)
        .Append("昵称: " + personModel.NickName + Environment.NewLine)
        .Append("SSN: " + personModel.Ssn + Environment.NewLine)
        .Append("[付款信息]" + Environment.NewLine)
        .Append("银行卡类型: " + personModel.CardBrand + Environment.NewLine)
        .Append("银行卡号: " + personModel.CardNum + Environment.NewLine)
        .Append("过期时间: " + personModel.CardExpires + Environment.NewLine)
        .Append("CVV: " + personModel.Cvv)
        .Append("[邮箱]" + Environment.NewLine)
        .Append("临时邮箱: " + personModel.TempMail);
    File.WriteAllText(filePath, sb.ToString());
}

using PersonGenLib.Enums;

namespace PersonGenLib.Models;

public class PersonModel
{
    public string Country { get; set; } = "";
    public string FullName { get; set; } = "";
    public Gender Gender { get; set; } = Gender.Female;
    public string Birthday { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";
    public string Ssn { get; set; } = "";
    public string NickName { get; set; } = "";
    public string TempMail { get; set; } = "";
    public string CardNum { get; set; } = "";
    public string Cvv { get; set; } = "";
    public string CardExpires { get; set; } = "";
    public string CardBrand { get; set; } = "";
    public int Age { get; set; } = 0;
}
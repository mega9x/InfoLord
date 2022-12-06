using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text.Json.Nodes;
using System.Xml;
using AngleSharp.Html.Parser;
using Newtonsoft.Json.Linq;
using PersonGenLib.Const;
using PersonGenLib.Enums;
using PersonGenLib.Gen.Interface;
using PersonGenLib.Models;

namespace PersonGenLib.Gen;

public class Generator : IGenerator
{
    private string _csrfmiddlewaretoken = "";
    public IOption Option { get; set; }
    public string Country { get; private set; }
    private PersonModel _personModel;

    public Generator(IOption option)
    {
        this.Option = option;
        this.Country = Option.GetCountry();
    }
    public PersonModel GetPersonModel()
    {
        return _personModel;
    }
    public async Task<IGenerator> GenPerson()
    {
        await GenFromDevTools();
        return this;
    }
    private async Task GenFromDevTools()
    {
        var uri = Const.RestfulApiUri.API_DEV_TOOLS;
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 Edg/107.0.1418.62");
        using var response = await httpClient.GetAsync(uri);
        var responseBody = await response.Content.ReadAsStringAsync();
        var parser = new HtmlParser();
        var document = parser.ParseDocument(responseBody);
        var input = document.QuerySelector("#fake_name_generator")?.QuerySelector("input")?.GetAttribute("value");
        if (input is null)
        {
            throw new Exception("csf is NULL");
        }
        this._csrfmiddlewaretoken = input;
        var country = "";
        if (Country.Contains("US"))
        {
            country = "en_US";
        }
        List<KeyValuePair<string, string>> postBody = new()
        {
            new KeyValuePair<string, string>("csrfmiddlewaretoken", _csrfmiddlewaretoken),
            new KeyValuePair<string, string>("locale_provider", country),
            new KeyValuePair<string, string>("gender", "random")
        };
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        HttpContent content = new FormUrlEncodedContent(postBody);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        var postUri = RestfulApiUri.API_DEV_TOOLS_POST;
        using var result= await httpClient.PostAsync(postUri, content);
        try
        {
            await handleResult(result);
        }
        catch(Exception e)
        {
            this._personModel = null;
            throw e;
        }

    }
    private async Task handleResult(HttpResponseMessage msg)
    {
        Console.WriteLine(await msg.Content.ReadAsStringAsync());
        var rootJson = JObject.Parse(await msg.Content.ReadAsStringAsync());
        var person = new PersonModel();
        var profile = rootJson.GetValue("profile");
        if (profile is null) throw new Exception("profile is null");
        var otherDetials = rootJson.GetValue("other_details");
        if (otherDetials is null) throw new Exception("detials is null");
        var profileObj = (JObject) profile;
        var otherDetialsObj = (JObject) otherDetials;
        person.FullName = profileObj.GetValue("name")!.ToString();
        person.Address = profileObj.GetValue("address")!.ToString();
        if (profileObj.GetValue("sex")!.ToString().Contains("M"))
        {
            person.Gender = Gender.Male;
        }
        else
        {
            person.Gender = Gender.Female;
        }
        person.Birthday = profileObj.GetValue("birthdate")!.ToString();
        person.Country = "US";
        person.Ssn = profileObj.GetValue("ssn")!.ToString();
        person.NickName = profileObj.GetValue("username")!.ToString();
        person.Phone = PhoneGen.USPhone();
        person.Age = DateTime.Today.Year - int.Parse(person.Birthday.Split("-")[0]);
        person.CardNum = otherDetialsObj.GetValue("card_number")!.ToString();
        person.Cvv = otherDetialsObj.GetValue("cvv")!.ToString();
        person.CardExpires = otherDetialsObj.GetValue("expiry_date")!.ToString();
        person.TempMail = TempMailGen.GetChacuoMail();
        person.CardBrand = otherDetialsObj.GetValue("card_brand")!.ToString();
        this._personModel = person;
    }
}
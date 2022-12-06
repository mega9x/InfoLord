using PersonGenLib.Models;

namespace PersonGenLib.Gen.Interface;

public interface IGenerator
{
    PersonModel GetPersonModel();
    Task<IGenerator> GenPerson();
}
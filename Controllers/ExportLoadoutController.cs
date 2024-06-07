using lib.remnant2.saves.Model;
using lib.remnant2.saves.Model.Properties;
using lib.remnant2.saves.Navigation;
using Microsoft.AspNetCore.Mvc;
using WebSaves.Model;

namespace WebSaves.Controllers;

[Produces("application/json")]
[Route("[controller]")]
[ApiController]
public class ExportLoadoutController : Controller
{
    [HttpPost]
    public IActionResult Process([FromForm] FormData data)
    {
        var authorizationToken = Environment.GetEnvironmentVariable("AUTHORIZATION_TOKEN");
        if (data.authToken != authorizationToken)
        {
            return BadRequest();
        }

        using MemoryStream memoryStream = new();
        data.saveFile.OpenReadStream().CopyTo(memoryStream);
        SaveFile sf = SaveFile.Read(memoryStream.ToArray());
        List<Character> result = [];
        Navigator navigator = new(sf);
        var profileCharacters = navigator.GetObjects("SavedCharacter");
        foreach (var profileCharacter in profileCharacters)
        {
            Character character = new();
            result.Add(character);
            Property profileLoadoutRecords = navigator.GetProperty("LoadoutRecords", profileCharacter);
            if (profileLoadoutRecords != null)
            {
                character.Loadouts = [];
                List<Property> loadoutEntries = navigator.GetProperties("Entries", profileLoadoutRecords);
                foreach (var loadoutEntry in loadoutEntries)
                {
                    List<LoadoutRecord> loadout = [];
                    character.Loadouts.Add(loadout);
                    ArrayStructProperty asp = loadoutEntry.Get<ArrayStructProperty>();
                    foreach (object aspItem in asp.Items)
                    {
                        PropertyBag pb = (PropertyBag)aspItem!;
                        loadout.Add(new()
                        {
                            Id = pb["ItemClass"].Get<string>(),
                            Level = pb["Level"].Get<int>(),
                            Type = pb["Slot"].Get<ObjectProperty>().ClassName!
                        });
                    }
                }
            }
        }
        return new JsonResult(result);
    }
}

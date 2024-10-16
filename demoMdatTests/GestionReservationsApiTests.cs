using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDAT;
using Moq;
using DemoAPI.SV.API.Tests.Utils;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using DemoAPI.SV.Tests.Utils;
using DemoAPI;
namespace demoMdat.Tests;

[TestClass()]
public class GestionReservationsApiTests : TestsBase
{
    [TestInitialize]
    public void TestInitialize()
    {
        Factory = new DemoWebApplicationFactory();
    }

    [TestMethod()]
    [MarkdownTest("~/Tests/{method}.md")]
    public async Task DemoWeatherForecast(Dictionary<string, Expected> expected)
    {
        var client = Factory.CreateClient();

        // Executer le traitement et vérifier la sortie du traitement et les assertions
        if (expected.TryGetValue("expectedRetour", out var expectedRetour))
        {
            _ = await Verify.Assert(async () => await client.GetAndDeserialize<List<WeatherForecast>>($"/weatherforecast/"), expectedRetour);
        }
        else
        {
            throw new Exception("Wooo ! Il faut au minimum avoir un Expected dans le dictionnaire nommé 'expectedRetour'.");
        }
    }
}
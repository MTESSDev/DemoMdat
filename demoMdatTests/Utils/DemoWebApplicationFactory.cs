using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace DemoAPI.SV.API.Tests.Utils;

internal class DemoWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests");

        IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(AppSettings)
                .Build();

        builder.UseConfiguration(configuration);

        // Configurez HTTPS en spécifiant le port HTTPS et en activant la redirection HTTP vers HTTPS.
        builder.UseSetting("https_port", "5001"); // Remplacez 5001 par le port HTTPS souhaité.
        builder.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Loopback, 5001, listenOptions =>
            {
                listenOptions.UseHttps();
            });
        });
    }

    public bool EstResolutionContexteExecution { get; set; }
    public DateTime? DateHeureExecutionAsynchrone { get; set; }

    /// <summary>
    /// Mock du fichier appSettings.json pour les tests
    /// </summary>
    public Dictionary<string, string?> AppSettings { get; set; } =
         new()
                {
                    { "Logging:LogLevel:Default", "Information" },
                    { "GCO:Securite:Actif", "true" },
                    { "CAC:CAC.Asynchrone:modeDeconnecte", "false" },
                    { "CAC:CAC.AccesProfil:source", "http://" },
                    { "CAC:CAC.AccesProfil:type", "web" },
                    { "CAC:CAC.AccesProfil:delaisExpirationCache", "0" },
                    { "CAC:CAC.AccesProfil:cheminFichierCacheProfil", "C:\\700\\DonneesTechnique\\Profils\\ResetCacheProfil.txt" },
                    { "GCO:UrlPresentation", "unSiteGCO" },
                    { "GCO:ModeDemo", "false" },
                    { "GCO:Palier", "TS" },
                    { "GCO:MessageAccueil", "test" }
                };

    /// <summary>
    /// Mock du profil CAC pour les tests
    /// </summary>
    public Dictionary<string, string> ProfilCAC { get; set; } =
         new Dictionary<string, string>()
                {
                    { "IDR_DATE", "2023-10-12" },
                    { "IDR_PHASE", "1" },
                    { "IDR_ENVIRONNEMENT", "UNITAIRE" },
                    { "GCO_CONN_BDNET_01", "" },
                    { "GCO_CONN_MODEBD_01", "MEMOIRE" },
                    { "GCO_COMM_MODEMOCK_01", "3" },
                    { "GCO_FICH_DEPOTGABARIT_01", Path.GetTempPath() }
                };
}
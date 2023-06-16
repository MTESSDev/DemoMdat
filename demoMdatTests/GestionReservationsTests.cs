using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDAT;
using Moq;

namespace demoMdat.Tests;

[TestClass()]
public class GestionReservationsTests
{
    [TestMethod()]
    [MarkdownTest("~/Tests/{method}.md")]
    public async Task ObtenirReservationParIdTest(ObjectOrException<TableSQLReservation> mockAppelTableSQLReservation,
                                                  Expected expected)
    {
        // Mock de l'accesseur BD
        var mockAccesBD = new Mock<IAccesBD>();

        // Simulation du retour de l'appel à la BD (TableSQLReservation) avec un id spécifique
        mockAccesBD.Setup(setup => setup.ObtenirAsync<TableSQLReservation>(It.IsAny<int>()))
                   .ReturnsOrThrowsAsync(mockAppelTableSQLReservation);

        // Instancier la classe
        var gestionReservations = new GestionReservations(mockAccesBD.Object);

        // Executer le traitement et vérifier la sortie du traitement et les assertions
        _ = await Verify.Assert(async () => await gestionReservations.ObtenirReservationParId(It.IsAny<int>()), expected);
    }
}
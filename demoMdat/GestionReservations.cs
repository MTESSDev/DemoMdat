namespace demoMdat;

public class GestionReservations
{
    private readonly IAccesBD _accesBD;

    public GestionReservations(IAccesBD accesBD)
    {
        _accesBD = accesBD;
    }

    public async Task<Reservation> ObtenirReservationParId(int id)
    {
        var reservationBD = await _accesBD.ObtenirAsync<TableSQLReservation>(id);

        int nbNuitees = int.Parse((reservationBD.DateFin - reservationBD.DateDebut).TotalDays.ToString());

        var reservation = new Reservation
        {
            Id = reservationBD.Id,
            NumeroLot = reservationBD.NumeroLot,
            DateDebut = reservationBD.DateDebut,
            DateFin = reservationBD.DateFin,
            NombreNuitees = nbNuitees,
            NombreNuiteesHautTarif = ObtenirNombreNuiteesHautTarifPlageDate(
                                                DateOnly.FromDateTime(reservationBD.DateDebut),
                                                DateOnly.FromDateTime(reservationBD.DateFin))
        };

        return reservation;
    }

    private int ObtenirNombreNuiteesHautTarifPlageDate(DateOnly debut, DateOnly fin)
    {
        int prixHauteSaison = 0;

        for (int i = debut.DayNumber; i < fin.DayNumber; i++)
        {
            var date = DateOnly.FromDayNumber(i);

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                prixHauteSaison++;
                continue;
            }
            else if (date.Month == 7 || date.Month == 12)
            {
                prixHauteSaison++;
                continue;
            }
        }
        return prixHauteSaison;
    }
}
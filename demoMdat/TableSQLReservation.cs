namespace demoMdat
{
    public record TableSQLReservation
    {
        public int Id { get; set; }
        public string NumeroLot { get; set; } = default!;
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
    }
}
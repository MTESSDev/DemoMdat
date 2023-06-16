namespace demoMdat
{
    public record Reservation
    {
        public int Id { get; set; }
        public string NumeroLot { get; set; } = default!;
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public int NombreNuitees { get; set; }
        public int NombreNuiteesHautTarif { get; set; }
    }
}
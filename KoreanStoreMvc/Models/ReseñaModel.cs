namespace KoreanStoreMvc.Models
{
    public class ReseñaModel
    {
        public int Id { get; set; }
        public int Id_user { get; set; }
        public int Id_producto { get; set; }
        public int Calificacion { get; set; }
        public string? Comentario { get; set; }
        public DateTime Fecha { get; set; }
        public string? NombreUsuario { get; set; }

        // Propiedades calculadas (no vienen de la API, se llenan en el cliente)
        public double PromedioCalificacion { get; set; }
        public int TotalResenas { get; set; }
    }
}
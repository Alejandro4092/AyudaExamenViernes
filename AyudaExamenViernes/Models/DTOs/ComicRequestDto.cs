namespace AyudaExamenViernes.Models.DTOs
{
    public class ComicRequestDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public IFormFile Imagen { get; set; }
    }
}
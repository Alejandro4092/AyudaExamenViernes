using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AyudaExamenViernes.Models
{
    [Table("Comics")]
    public class Comic
    {
        [Key]
        [Column("IdComic")]
        public int IdComic { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Descripcion")]
        public string Descripcion { get; set; }

        [Column("Imagen")]
        public string Imagen { get; set; }
    }
}
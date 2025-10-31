using SQLite;
using System.ComponentModel.DataAnnotations;

namespace MiAgendaUTN.Models
{
    [Table("tareas")]
    public class Tarea
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Titulo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        [NotNull]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaVencimiento { get; set; }

        public string Categoria { get; set; } = "General";

        public bool Completada { get; set; } = false;

        public DateTime? FechaCompletacion { get; set; }
    }
}
using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MiAgendaUTN_Cristian__Raul.Models
{
    [Table("tareas")]
    public class Tarea : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        [NotNull]
        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        public string Categoria { get; set; } // "Trabajo", "Personal", "Estudio"

        private bool _completada;
        public bool Completada
        {
            get => _completada;
            set
            {
                if (_completada != value)
                {
                    _completada = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MiAgendaUTN_Cristian__Raul.Models;
namespace MiAgendaUTN_Cristian__Raul.Services
{
    public class TareaViewModel : INotifyPropertyChanged
    {
        private readonly IDataStore _dataStore;
        private ObservableCollection<Tarea> _tareas;
        private Tarea _tareaSeleccionada;
        private string _filtroCategoria = "Todas";
        private bool _soloCompletadas = false;

        public ObservableCollection<Tarea> Tareas
        {
            get => _tareas;
            set
            {
                if (_tareas != value)
                {
                    _tareas = value;
                    OnPropertyChanged();
                }
            }
        }

        public Tarea TareaSeleccionada
        {
            get => _tareaSeleccionada;
            set
            {
                if (_tareaSeleccionada != value)
                {
                    _tareaSeleccionada = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FiltroCategoria
        {
            get => _filtroCategoria;
            set
            {
                if (_filtroCategoria != value)
                {
                    _filtroCategoria = value;
                    OnPropertyChanged();
                    _ = CargarTareasAsync();
                }
            }
        }

        public bool SoloCompletadas
        {
            get => _soloCompletadas;
            set
            {
                if (_soloCompletadas != value)
                {
                    _soloCompletadas = value;
                    OnPropertyChanged();
                    _ = CargarTareasAsync();
                }
            }
        }

        public ICommand CargarTareasCommand { get; }
        public ICommand AgregarTareaCommand { get; }
        public ICommand ActualizarTareaCommand { get; }
        public ICommand EliminarTareaCommand { get; }

        public TareaViewModel(IDataStore dataStore)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            Tareas = new ObservableCollection<Tarea>();

            CargarTareasCommand = new Command(async () => await CargarTareasAsync());
            AgregarTareaCommand = new Command<Tarea>(async (tarea) => await AgregarTareaAsync(tarea));
            ActualizarTareaCommand = new Command<Tarea>(async (tarea) => await ActualizarTareaAsync(tarea));
            EliminarTareaCommand = new Command<int>(async (id) => await EliminarTareaAsync(id));

            _ = CargarTareasAsync();
        }

        public async Task CargarTareasAsync()
        {
            try
            {
                List<Tarea> tareas;

                if (FiltroCategoria == "Todas")
                    tareas = await _dataStore.GetTareasFiltradas(SoloCompletadas);
                else
                    tareas = await _dataStore.GetTareasByCategoria(FiltroCategoria);

                Tareas.Clear();
                foreach (var tarea in tareas.OrderBy(t => t.FechaVencimiento))
                    Tareas.Add(tarea);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"✗ Error al cargar tareas: {ex.Message}");
            }
        }

        public async Task AgregarTareaAsync(Tarea tarea)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tarea.Titulo))
                    throw new ArgumentException("El título es obligatorio");

                int result = await _dataStore.AddTareaAsync(tarea);
                if (result > 0)
                {
                    Tareas.Add(tarea);
                    System.Diagnostics.Debug.WriteLine($"✓ Tarea agregada: {tarea.Titulo}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"✗ Error al agregar tarea: {ex.Message}");
            }
        }

        public async Task ActualizarTareaAsync(Tarea tarea)
        {
            try
            {
                int result = await _dataStore.UpdateTareaAsync(tarea);
                if (result > 0)
                    System.Diagnostics.Debug.WriteLine($"✓ Tarea actualizada: {tarea.Titulo}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"✗ Error al actualizar tarea: {ex.Message}");
            }
        }

        public async Task EliminarTareaAsync(int id)
        {
            try
            {
                var tarea = Tareas.FirstOrDefault(t => t.Id == id);
                if (tarea != null)
                {
                    int result = await _dataStore.DeleteTareaAsync(id);
                    if (result > 0)
                    {
                        Tareas.Remove(tarea);
                        System.Diagnostics.Debug.WriteLine($"✓ Tarea eliminada: {tarea.Titulo}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"✗ Error al eliminar tarea: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

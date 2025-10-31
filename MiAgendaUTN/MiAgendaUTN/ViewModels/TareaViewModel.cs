using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiAgendaUTN.Data;
using MiAgendaUTN.Models;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;

namespace MiAgendaUTN.ViewModels
{
    public partial class TareaViewModel : ObservableObject
    {
        private readonly TareaRepository _repository;

        [ObservableProperty]
        private ObservableCollection<Tarea> tareas = new();

        [ObservableProperty]
        private string titulo = string.Empty;

        [ObservableProperty]
        private string descripcion = string.Empty;

        [ObservableProperty]
        private DateTime fechaVencimiento = DateTime.Now.AddDays(1);

        [ObservableProperty]
        private string categoria = "General";

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string mensajeError = string.Empty;

        [ObservableProperty]
        private string filtroEstado = "todas"; 

        [ObservableProperty]
        private string filtroCategoria = "todas";

        public TareaViewModel()
        {
            _repository = new TareaRepository();
        }

       
        [RelayCommand]
        public async Task CargarTareas()
        {
            try
            {
                IsLoading = true;
                MensajeError = string.Empty;
                var tareasList = await _repository.ObtenerTodasLasTareas();
                Tareas.Clear();
                foreach (var tarea in tareasList)
                {
                    Tareas.Add(tarea);
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al cargar tareas: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        
        [RelayCommand]
        public async Task CrearTarea()
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(Titulo))
                {
                    MensajeError = "El título es requerido";
                    return;
                }

                if (Titulo.Length < 3)
                {
                    MensajeError = "El título debe tener al menos 3 caracteres";
                    return;
                }

                if (FechaVencimiento < DateTime.Now)
                {
                    MensajeError = "La fecha de vencimiento no puede ser en el pasado";
                    return;
                }

                IsLoading = true;
                MensajeError = string.Empty;

                var nuevaTarea = new Tarea
                {
                    Titulo = Titulo,
                    Descripcion = Descripcion,
                    FechaVencimiento = FechaVencimiento,
                    Categoria = Categoria,
                    Completada = false
                };

                await _repository.CrearTarea(nuevaTarea);
                Tareas.Add(nuevaTarea);

                
                Titulo = string.Empty;
                Descripcion = string.Empty;
                Categoria = "General";
                FechaVencimiento = DateTime.Now.AddDays(1);
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al crear tarea: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        
        [RelayCommand]
        public async Task ActualizarTarea(Tarea tarea)
        {
            try
            {
                IsLoading = true;
                await _repository.ActualizarTarea(tarea);
                MensajeError = string.Empty;
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al actualizar: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        
        [RelayCommand]
        public async Task MarcarCompletada(Tarea tarea)
        {
            try
            {
                tarea.Completada = !tarea.Completada;
                tarea.FechaCompletacion = tarea.Completada ? DateTime.Now : null;
                await _repository.ActualizarTarea(tarea);
                MensajeError = string.Empty;
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al completar: {ex.Message}";
            }
        }

        
        [RelayCommand]
        public async Task EliminarTarea(Tarea tarea)
        {
            try
            {
                IsLoading = true;
                await _repository.EliminarTarea(tarea.Id);
                Tareas.Remove(tarea);
                MensajeError = string.Empty;
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al eliminar: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        
        [RelayCommand]
        public async Task AplicarFiltros()
        {
            try
            {
                IsLoading = true;
                MensajeError = string.Empty;
                var tareasList = await _repository.ObtenerTodasLasTareas();

                
                if (FiltroEstado == "completadas")
                    tareasList = tareasList.Where(t => t.Completada).ToList();
                else if (FiltroEstado == "pendientes")
                    tareasList = tareasList.Where(t => !t.Completada).ToList();

                
                if (FiltroCategoria != "todas")
                    tareasList = tareasList.Where(t => t.Categoria == FiltroCategoria).ToList();

                Tareas.Clear();
                foreach (var tarea in tareasList.OrderBy(t => t.FechaVencimiento))
                {
                    Tareas.Add(tarea);
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error al filtrar: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
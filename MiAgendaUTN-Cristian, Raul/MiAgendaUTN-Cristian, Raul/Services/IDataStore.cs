using MiAgendaUTN_Cristian__Raul.Models;

namespace MiAgendaUTN_Cristian__Raul.Services
{
    public interface IDataStore
    {
        Task<int> AddTareaAsync(Tarea tarea);
        Task<int> UpdateTareaAsync(Tarea tarea);
        Task<int> DeleteTareaAsync(int id);
        Task<Tarea> GetTareaAsync(int id);
        Task<List<Tarea>> GetAllTareasAsync();
        Task<List<Tarea>> GetTareasByCategoria(string categoria);
        Task<List<Tarea>> GetTareasFiltradas(bool soloCompletadas = false);
    }
}

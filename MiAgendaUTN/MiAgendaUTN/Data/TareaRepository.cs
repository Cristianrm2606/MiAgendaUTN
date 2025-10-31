using SQLite;
using MiAgendaUTN.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiAgendaUTN.Data
{
    public class TareaRepository
    {
        private SQLiteAsyncConnection _connection;
        private const string DbFileName = "miagenda.db3";

        public TareaRepository()
        {
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            if (_connection == null)
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, DbFileName);
                _connection = new SQLiteAsyncConnection(dbPath);
                await _connection.CreateTableAsync<Tarea>();
            }
        }

        
        public async Task<int> CrearTarea(Tarea tarea)
        {
            if (_connection == null) await Task.Delay(500);
            return await _connection.InsertAsync(tarea);
        }

        
        public async Task<List<Tarea>> ObtenerTodasLasTareas()
        {
            if (_connection == null) await Task.Delay(500);
            return await _connection.Table<Tarea>().ToListAsync();
        }

        
        public async Task<Tarea> ObtenerTareaPorId(int id)
        {
            if (_connection == null) await Task.Delay(500);
            return await _connection.Table<Tarea>().Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        
        public async Task<int> ActualizarTarea(Tarea tarea)
        {
            if (_connection == null) await Task.Delay(500);
            return await _connection.UpdateAsync(tarea);
        }

        
        public async Task<int> EliminarTarea(int id)
        {
            if (_connection == null) await Task.Delay(500);
            return await _connection.DeleteAsync<Tarea>(id);
        }

        
        public async Task<List<Tarea>> ObtenerTareasPorCategoria(string categoria)
        {
            if (_connection == null) await Task.Delay(500);
            return await _connection.Table<Tarea>()
                .Where(t => t.Categoria == categoria)
                .ToListAsync();
        }

       
        public async Task<List<Tarea>> ObtenerTareasPorEstado(bool completada)
        {
            if (_connection == null) await Task.Delay(500);
            return await _connection.Table<Tarea>()
                .Where(t => t.Completada == completada)
                .ToListAsync();
        }
    }
}
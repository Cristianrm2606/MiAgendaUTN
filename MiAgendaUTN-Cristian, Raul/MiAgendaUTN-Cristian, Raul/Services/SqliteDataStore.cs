using SQLite;
using MiAgendaUTN_Cristian__Raul.Models;
using System.Diagnostics;

namespace MiAgendaUTN_Cristian__Raul.Services
{
    public class SqliteDataStore : IDataStore
    {
        private SQLiteAsyncConnection _database;
        private const string DB_NAME = "MiAgendaUTN.db3";

        public SqliteDataStore()
        {
            InitializeDatabase();
        }

        private async void InitializeDatabase()
        {
            try
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, DB_NAME);
                _database = new SQLiteAsyncConnection(dbPath);
                await _database.CreateTableAsync<Tarea>();
                Debug.WriteLine($"✓ Base de datos iniciada en: {dbPath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error al inicializar BD: {ex.Message}");
            }
        }

        public async Task<int> AddTareaAsync(Tarea tarea)
        {
            try
            {
                tarea.FechaCreacion = DateTime.Now;
                return await _database.InsertAsync(tarea);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error al añadir tarea: {ex.Message}");
                return -1;
            }
        }

        public async Task<int> UpdateTareaAsync(Tarea tarea)
        {
            try
            {
                return await _database.UpdateAsync(tarea);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error al actualizar tarea: {ex.Message}");
                return -1;
            }
        }

        public async Task<int> DeleteTareaAsync(int id)
        {
            try
            {
                return await _database.DeleteAsync<Tarea>(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error al eliminar tarea: {ex.Message}");
                return -1;
            }
        }

        public async Task<Tarea> GetTareaAsync(int id)
        {
            try
            {
                return await _database.GetAsync<Tarea>(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error al obtener tarea: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Tarea>> GetAllTareasAsync()
        {
            try
            {
                return await _database.Table<Tarea>().ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error al obtener todas las tareas: {ex.Message}");
                return new List<Tarea>();
            }
        }

        public async Task<List<Tarea>> GetTareasByCategoria(string categoria)
        {
            try
            {
                return await _database.Table<Tarea>()
                    .Where(t => t.Categoria == categoria)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error al filtrar por categoría: {ex.Message}");
                return new List<Tarea>();
            }
        }

        public async Task<List<Tarea>> GetTareasFiltradas(bool soloCompletadas = false)
        {
            try
            {
                var query = _database.Table<Tarea>();
                if (soloCompletadas)
                    query = query.Where(t => t.Completada);
                return await query.OrderBy(t => t.FechaVencimiento).ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"✗ Error al filtrar tareas: {ex.Message}");
                return new List<Tarea>();
            }
        }
    }
}
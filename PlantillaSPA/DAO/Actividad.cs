using System.Threading.Tasks;

namespace PlantillaSpa.DAO
{
    public partial class Actividad
    {
        private static string ClaveActividad(int FK_Usuario) => $"SPA:{FK_Usuario}";

        public enum Estado
        {
            Inicio,
            EditarLista
        }

        public Estado Actual { get; set; }
        public int FK_Usuario { get; set; }

        /// <summary>
        /// Recuperamos de Redis la actividad almacenada del usuario
        /// </summary>
        /// <param name="FK_Usuario"></param>
        /// <returns></returns>
        public static async Task<Actividad> Get(int FK_Usuario, string cnRedis)
        {
            Cache cache = new Cache(cnRedis);
            return await cache.Get<Actividad>(ClaveActividad(FK_Usuario));
        }

        /// <summary>
        /// Almacenamos en Redis el objeto Actividad con su valor actual
        /// </summary>
        /// <returns></returns>
        public async Task Persistir(string cnRedis)
        {
            Cache cache = new Cache(cnRedis);
            await cache.Set<Actividad>(this, ClaveActividad(FK_Usuario));
        }
    }
}

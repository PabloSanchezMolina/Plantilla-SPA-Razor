using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantillaSpa.DAO
{
    public class Cache
    {
        private readonly string _cnRedis;
        private static Lazy<ConnectionMultiplexer> lazyConnection = null;

        private ConnectionMultiplexer Conexion()
        {
            if (lazyConnection == null)
            {
                lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                {
                    return ConnectionMultiplexer.Connect(_cnRedis);
                });
            }
            return lazyConnection.Value;
        }

        public Cache(string cnRedis)
        {
            _cnRedis = cnRedis;
        }

        public async Task Set<T>(T valor, string clave, TimeSpan? ttl = null)
        {
            await Conexion().GetDatabase().StringSetAsync(clave, JsonConvert.SerializeObject(valor), ttl);
        }

        public async Task<T> Get<T>(string clave)
        {
            T resultado;

            string valor = await Conexion().GetDatabase().StringGetAsync(clave);
            if (string.IsNullOrEmpty(valor))
            {
                resultado = default(T);
            }
            else
            {
                JsonSerializer serializer = new JsonSerializer();
                resultado = (T)serializer.Deserialize(new JTokenReader(JsonConvert.DeserializeObject<JObject>(valor)), typeof(T));
            }

            return resultado;
        }
    }
}

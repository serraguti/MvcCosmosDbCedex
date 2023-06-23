using Microsoft.Azure.Cosmos;

namespace MvcCosmosDbCedex.Services
{
    public class ServiceCosmosDb
    {
        //SE TRABAJA CON CONTAINERS Y NOSOTROS
        //ADEMAS VAMOS A CREAR LOS CONTENEDORES POR LO 
        //QUE TENDREMOS TAMBIEN EL CLIENTE DE COSMOS
        private CosmosClient client;
        public Container containerCosmos;
        private IConfiguration configuration;

        public ServiceCosmosDb(CosmosClient client
            , Container container
            , IConfiguration configuration)
        {
            this.configuration = configuration;
            this.client = client;
            this.containerCosmos = container;
        }

        //TENDREMOS UN METODO PARA CREAR LA BASE DE DATOS
        //Y DENTRO UN CONTAINER
        public async Task CreateDatabaseAsync()
        {
            //AL CREAR EL CONTENEDOR, PODEMOS INDICAR
            //QUE PRIMARY UTILIZARA (id)
            //COSMOS UTILIZA ALGO LLAMADO Partition Key
            //UN PARTITION KEY NOS SIRVE PARA AGRUPAR ELEMENTOS
            //DE UN MISMO TIPO POR CATEGORIA
            //POR EJEMPLO, CIUDAD, PAIS, EMPRESAS
            string databaseName =
                configuration.GetValue<string>("CochesCosmosDb:Database");
            string containerName =
                configuration.GetValue<string>("CochesCosmosDb:Container");
            ContainerProperties properties = 
                new ContainerProperties(containerName, "/id");
            //ITEMS CONTAINERS
            await this.client.CreateDatabaseIfNotExistsAsync
                ("bbddvehiculos");
            //UN CONTENEDOR DENTRO DE ITEMS CONTAINERS
            await this.client.GetDatabase(databaseName)
                .CreateContainerIfNotExistsAsync(properties);
        }

        public async Task CreateVehiculoAsync(Vehiculo car)
        {
            //AL CREAR UN OBJETO, DEBEMOS INDICAR SU VALOR Y
            //TAMBIEN SU PARTITION KEY
            await this.containerCosmos.CreateItemAsync<Vehiculo>
                (car, new PartitionKey(car.IdVehiculo));
        }

        public async Task<List<Vehiculo>> GetVehiculosAsync()
        {
            //LOS DATOS SE RECUPERAN MEDIANTE Iterator
            //NECESITAMOS RECORRER LOS ITEMS MIENTRAS QUE EXISTAN
            var query =
                this.containerCosmos.GetItemQueryIterator<Vehiculo>();
            List<Vehiculo> coches = new List<Vehiculo>();
            while (query.HasMoreResults)
            {
                var results = await query.ReadNextAsync();
                coches.AddRange(results);
            }
            return coches;
        }

        public async Task<Vehiculo> FindVehiculoAsync(int id)
        {
            ItemResponse<Vehiculo> response =
                await this.containerCosmos.ReadItemAsync<Vehiculo>
                (id.ToString(), new PartitionKey(id));
            return response;
        }

        public async Task DeleteVehiculo(int id)
        {
            await this.containerCosmos.DeleteItemAsync<Vehiculo>
                (id.ToString(), new PartitionKey(id));
        }

        public async Task UpdateVehiculo(Vehiculo car)
        {
            await this.containerCosmos.UpsertItemAsync<Vehiculo>
                (car, new PartitionKey(car.IdVehiculo));
        }
    }
}

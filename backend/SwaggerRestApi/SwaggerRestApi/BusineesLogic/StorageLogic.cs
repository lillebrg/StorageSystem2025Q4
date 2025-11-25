using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO;
using SwaggerRestApi.Models.DTO.Rack;
using SwaggerRestApi.Models.DTO.Storage;

namespace SwaggerRestApi.BusineesLogic
{
    public class StorageLogic
    {
        private readonly StorageDBAccess _storagedbaccess;

        public StorageLogic(StorageDBAccess storageDBAccess)
        {
            _storagedbaccess = storageDBAccess;
        }

        /// <summary>
        /// Gets a storage with a list of racks
        /// </summary>
        /// <param name="id">The id of the storage to be returned</param>
        /// <returns>storage with a list of racks</returns>
        public async Task<ActionResult<StorageGetWithRacks>> GetStorage(int id)
        {
            var storage = await _storagedbaccess.GetStorage(id);

            if (storage == null) { return new NotFoundObjectResult(new { message = "Could not find storage" }); }

            StorageGetWithRacks result = new StorageGetWithRacks
            {
                name = storage.Name,
                id = storage.Id,
                racks = new List<RackFromStorage>()
            };

            foreach (var item in storage.Racks)
            {
                RackFromStorage rack = new RackFromStorage
                {
                    id = item.Id,
                    rack_no = item.RackNo
                };
                result.racks.Add(rack);
            }

            return new OkObjectResult(result);
        }

        /// <summary>
        /// Gets all storages
        /// </summary>
        /// <returns>A list of storages</returns>
        public async Task<ActionResult<List<StorageGet>>> GetAllStorages()
        {
            var storages = await _storagedbaccess.GetAllStorages();

            List<StorageGet> result = new List<StorageGet>();

            foreach (var item in storages)
            {
                StorageGet storage = new StorageGet
                {
                    id = item.Id,
                    name = item.Name
                };
                result.Add(storage);
            }

            return new OkObjectResult(result);
        }

        /// <summary>
        /// Creates a new storage
        /// </summary>
        /// <param name="storageCreate">Contains a name of the storage to be created</param>
        /// <returns>An int with the id if the storage that was created</returns>
        public async Task<ActionResult<CreateReturnInt>> CreateStorage(StorageCreate storageCreate)
        {
            bool exist = await _storagedbaccess.CheckForExistingStorage(storageCreate.name);

            if (exist) { return new BadRequestObjectResult(new { message = "Storage already exists" }); }

            Storage storage = new Storage
            {
                Name = storageCreate.name,
                Racks = new List<Rack>()
            };

            CreateReturnInt result = new CreateReturnInt();

            result.id = await _storagedbaccess.CreateStorage(storage);

            return new OkObjectResult(result);
        }

        /// <summary>
        /// Updates a storage
        /// </summary>
        /// <param name="storageUpdate">Contains a name</param>
        /// <param name="id">The id of the object to be updated</param>
        /// <returns>True</returns>
        public async Task<ActionResult> UpdateStorage(StorageUpdate storageUpdate, int id)
        {
            var storage = await _storagedbaccess.GetStorage(id);

            if (storage == null) { return new NotFoundObjectResult(new { message = "Could not find storage" }); }

            storage.Name = storageUpdate.name;

            await _storagedbaccess.UpdateStorage(storage);

            return new OkObjectResult(true);
        }

        /// <summary>
        /// Deletes a storage
        /// </summary>
        /// <param name="id">The id of the storage to be deleted</param>
        /// <returns>True</returns>
        public async Task<ActionResult> DeleteRack(int id)
        {
            var rack = await _storagedbaccess.GetStorage(id);

            if (rack == null) { return new NotFoundObjectResult(new { message = "Could not find storage" }); }

            await _storagedbaccess.DeleteStorage(rack);

            return new OkObjectResult(true);
        }
    }
}

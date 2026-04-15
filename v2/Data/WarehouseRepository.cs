using System.Collections.Generic;
using MaliyeHesaplama.v2.Models;

namespace MaliyeHesaplama.v2.Data
{
    public class WarehouseRepository
    {
        private readonly MiniOrm _orm;

        public WarehouseRepository()
        {
            _orm = new MiniOrm();
        }

        public int Save(Dictionary<string, object> data)
        {
            return _orm.Save("Warehouse", data);
        }

        public IEnumerable<Warehouse> GetAll()
        {
            return _orm.GetAll<Warehouse>("Warehouse");
        }

        public IEnumerable<Warehouse> GetActive()
        {
            return _orm.GetAll<Warehouse>("Warehouse").Where(x => x.IsActive);
        }

        public Warehouse GetById(int id)
        {
            return _orm.GetById<Warehouse>("Warehouse", id, "Id");
        }

        public void Delete(int id)
        {
            _orm.ExecuteRaw($"DELETE FROM Warehouse WHERE Id = {id}");
        }
    }
}
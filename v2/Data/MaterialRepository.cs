using System.Collections.Generic;
using MaliyeHesaplama.v2.Models;

namespace MaliyeHesaplama.v2.Data
{
    public class MaterialRepository
    {
        private readonly MiniOrm _orm;

        public MaterialRepository()
        {
            _orm = new MiniOrm();
        }

        public int Save(Dictionary<string, object> data)
        {
            return _orm.Save("MaterialMaster", data);
        }

        public IEnumerable<MaterialMaster> GetAll()
        {
            return _orm.GetAll<MaterialMaster>("MaterialMaster");
        }

        public MaterialMaster GetById(int id)
        {
            return _orm.GetById<MaterialMaster>("MaterialMaster", id, "Id");
        }

        public void Delete(int id)
        {
            _orm.ExecuteRaw($"DELETE FROM MaterialMaster WHERE Id = {id}");
        }
    }
}
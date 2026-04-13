using System.Collections.Generic;

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

        public IEnumerable<dynamic> GetAll()
        {
            return _orm.GetAll<dynamic>("MaterialMaster");
        }

        public dynamic GetById(int id)
        {
            return _orm.GetById<dynamic>("MaterialMaster", id, "Id");
        }

        public void Delete(int id)
        {
            _orm.ExecuteRaw($"DELETE FROM MaterialMaster WHERE Id = {id}");
        }
    }
}
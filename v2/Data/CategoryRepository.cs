using System.Collections.Generic;
using MaliyeHesaplama.v2.Models;

namespace MaliyeHesaplama.v2.Data
{
    public class CategoryRepository
    {
        private readonly MiniOrm _orm;

        public CategoryRepository()
        {
            _orm = new MiniOrm();
        }

        public int Save(Dictionary<string, object> data)
        {
            return _orm.Save("Category", data);
        }

        public IEnumerable<Category> GetAll()
        {
            return _orm.GetAll<Category>("Category");
        }

        public IEnumerable<Category> GetActive()
        {
            return _orm.GetAll<Category>("Category").Where(x => x.IsActive);
        }

        public Category GetById(int id)
        {
            return _orm.GetById<Category>("Category", id, "Id");
        }

        public void Delete(int id)
        {
            _orm.ExecuteRaw($"DELETE FROM Category WHERE Id = {id}");
        }
    }
}
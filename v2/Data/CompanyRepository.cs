using System.Collections.Generic;
using System.Linq;
using MaliyeHesaplama.models;

namespace MaliyeHesaplama.v2.Data
{
    public class CompanyRepository
    {
        private readonly MiniOrm _orm;

        public CompanyRepository()
        {
            _orm = new MiniOrm();
        }

        public IEnumerable<Company> GetAll()
        {
            return _orm.GetAll<Company>("Company");
        }

        public IEnumerable<Company> GetAllActive()
        {
            return _orm.GetAll<Company>("Company").Where(x => x.IsOwner == false);
        }

        public Company GetById(int id)
        {
            return _orm.GetById<Company>("Company", id, "Id");
        }
    }
}
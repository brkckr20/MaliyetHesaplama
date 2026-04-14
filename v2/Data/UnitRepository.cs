using System.Collections.Generic;
using MaliyeHesaplama.v2.Models;

namespace MaliyeHesaplama.v2.Data
{
    public class UnitRepository
    {
        private readonly MiniOrm _orm;

        public UnitRepository()
        {
            _orm = new MiniOrm();
        }

        public IEnumerable<Unit> GetAll()
        {
            return _orm.GetAll<Unit>("Unit");
        }

        public IEnumerable<Unit> GetActive()
        {
            return _orm.GetAll<Unit>("Unit").Where(x => x.IsActive);
        }
    }
}
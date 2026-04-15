using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<MaterialMasterDto> GetAllWithDetails()
        {
            var sql = @"
                SELECT 
                    m.Id, m.Code, m.Name, m.Type, m.CategoryId, m.UnitId, m.Barcode,
                    m.VatRate, m.MinStock, m.MaxStock, m.IsActive,
                    CASE m.Type 
                        WHEN 1 THEN 'Ham Madde'
                        WHEN 2 THEN 'Yarı Mamul'
                        WHEN 3 THEN 'Mamul'
                        WHEN 4 THEN 'Sarf Malzeme'
                    END as TypeName,
                    COALESCE(c.Name, '') as CategoryName,
                    COALESCE(u.Name, '') as UnitName
                FROM MaterialMaster m
                LEFT JOIN Category c ON m.CategoryId = c.Id
                LEFT JOIN Unit u ON m.UnitId = u.Id";

            return _orm.QueryRaw<MaterialMasterDto>(sql);
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
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework
{
    class PitmanManager
    {
        public static Person GetRecordById(Int64 id)
        {
            using (PitmanContext context = new PitmanContext())
            {
                return context.Persons.FirstOrDefault(o => o.Id == id);
            }
        }

        public static List<Person> GetAllRecords()
        {
            using (PitmanContext context = new PitmanContext())
            {
                return context.Persons.OrderByDescending(o => o.Id).ToList();
            }
        }

        public static bool AddOrUpdate(Person model)
        {
            using (PitmanContext context = new PitmanContext())
            {
                if (model.Id > 0)
                {
                    context.Persons.Attach(model);
                    context.Entry(model).State = EntityState.Modified;
                    return context.SaveChanges() > 0;
                }
                else
                {
                    context.Persons.Add(model);
                    return context.SaveChanges() > 0;
                }
            }
        }

        public static bool Delete(Int64 id)
        {
            using (PitmanContext context = new PitmanContext())
            {
                Person entity = context.Persons.FirstOrDefault(o => o.Id == id);
                if (entity != null)
                {
                    context.Entry(entity).State = EntityState.Deleted;
                    return context.SaveChanges() > 0;
                }
            }
            return false;
        }
    }
}

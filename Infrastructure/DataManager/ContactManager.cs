using ASIAssignment.Entities;
using ASIAssignment.Entities.Context;
using ASIAssignment.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASIAssignment.Infrastructure.DataManager
{
    public class ContactManager : IDataRepository<Contact>
    {
        readonly DBEntities _dbContext;
        public ContactManager(DBEntities context)
        {
            _dbContext = context;
        }
        public IEnumerable<Contact> GetAll()
        {
            return _dbContext.Contact.ToList();
        }
        public Contact Get(long id)
        {
            return _dbContext.Contact.FirstOrDefault(e => e.Id == id);
        }
        public void Add(Contact entity)
        {
            _dbContext.Contact.Add(entity);
            _dbContext.SaveChanges();
        }
        public void Update(Contact model, Contact entity)
        {
            model.Name = entity.Name;
            model.BirthDate = entity.BirthDate;
            _dbContext.SaveChanges();
        }
        public void Delete(Contact obj)
        {
            _dbContext.Contact.Remove(obj);
            _dbContext.SaveChanges();
        }
    }
}

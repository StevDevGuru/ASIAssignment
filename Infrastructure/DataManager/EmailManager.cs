using ASIAssignment.Entities;
using ASIAssignment.Entities.Context;
using ASIAssignment.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASIAssignment.Infrastructure.DataManager
{
    public class EmailManager : IDataRepository<Email>
    {
        readonly DBEntities _dbContext;
        public EmailManager(DBEntities context)
        {
            _dbContext = context;
        }
        public IEnumerable<Email> GetAll()
        {
            return _dbContext.Email.ToList();
        }
        public Email Get(long id)
        {
            return _dbContext.Email
                  .FirstOrDefault(e => e.Id == id);
        }
        public void Add(Email entity)
        {
            _dbContext.Email.Add(entity);
            _dbContext.SaveChanges();
        }
        public void Update(Email model, Email entity)
        {
            model.IsPrimary = entity.IsPrimary;
            model.Address = entity.Address;
            model.ContactId = entity.ContactId;
            _dbContext.SaveChanges();
        }
        public void Delete(Email obj)
        {
            _dbContext.Email.Remove(obj);
            _dbContext.SaveChanges();
        }
    }
}

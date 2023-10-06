using ASIAssignment.Entities;
using ASIAssignment.Entities.Context;
using ASIAssignment.Infrastructure.Interfaces;
using ASIAssignment.Infrastructure.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASIAssignment.Controllers
{
    [Route("api/Contact")]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly IDataRepository<Contact> _dataRepository;
        private readonly IDataRepository<Email> _dataRepositoryEmails;
        readonly DBEntities _dbContext;
        public ContactController(IDataRepository<Contact> dataRepository, IDataRepository<Email> dataRepositoryEmails,  DBEntities context)
        {
            _dataRepository = dataRepository;
            _dataRepositoryEmails = dataRepositoryEmails;
            _dbContext = context;
        }
        // GET: api/Contact/GetAllContacts
        [HttpGet]
        [Route("GetAllContacts")]
        public IActionResult GetAllContacts()
        {
            List<ContactVM> lst = new List<ContactVM>();
            try
            {
                var allContacts = _dataRepository.GetAll();
                if (allContacts.Count() > 0)
                {
                    foreach (var contact in allContacts)
                    {
                        var emails = _dbContext.Email.Where(x => x.ContactId == contact.Id).ToList();
                        var lstEmails = new List<EmailVM>();
                        foreach (var e in emails)
                        {
                            lstEmails.Add(new EmailVM
                            {
                                Id = e.Id,
                                ContactId = e.ContactId,
                                IsPrimary = e.IsPrimary,
                                Address = e.Address

                            });
                        }
                        lst.Add(new ContactVM
                        {
                            Id = contact.Id,
                            Name = contact.Name,
                            BirthDate = contact.BirthDate,
                            Email = lstEmails.Count() > 0 ? lstEmails : new List<EmailVM>()
                        });

                    }
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
        // GET: api/Contact/GetContactByName?name
        [HttpGet]
        [Route("GetContactByName")]
        public IActionResult GetContactByName(string name)
        {
            List<ContactVM> lst = new List<ContactVM>();
            try
            {
                var filteredContacts = _dataRepository.GetAll().Where(x=> x.Name == name.Trim()).ToList();
                if (filteredContacts != null && filteredContacts.Count() > 0)
                {
                    foreach (var contact in filteredContacts)
                    {
                        var emails = _dbContext.Email.Where(x => x.ContactId == contact.Id).ToList();
                        var lstEmails = new List<EmailVM>();
                        foreach (var e in emails)
                        {
                            lstEmails.Add(new EmailVM
                            {
                                Id = e.Id,
                                ContactId = e.ContactId,
                                IsPrimary = e.IsPrimary,
                                Address = e.Address

                            });
                        }
                        lst.Add(new ContactVM
                        {
                            Id = contact.Id,
                            Name = contact.Name,
                            BirthDate = contact.BirthDate,
                            Email = lstEmails.Count() > 0 ? lstEmails : new List<EmailVM>()
                        });

                    }
                    return Ok(lst);
                }
                else
                {
                    return StatusCode(404,"No Record found against given name: " + name);
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
        // GET: api/Contact/GetContactByBirthdateRange
        [HttpGet]
        [Route("GetContactByBirthdateRange")]
        public IActionResult GetContactByDOBRange(DateTime birthStartDate, DateTime birthEndDate)
        {
            List<ContactVM> lst = new List<ContactVM>();
            try
            {
                var filteredContacts = _dataRepository.GetAll().Where(x => x.BirthDate >= birthStartDate && x.BirthDate <= birthEndDate).ToList();
                if (filteredContacts != null && filteredContacts.Count() > 0)
                {
                    foreach (var contact in filteredContacts)
                    {
                        var emails = _dbContext.Email.Where(x => x.ContactId == contact.Id).ToList();
                        var lstEmails = new List<EmailVM>();
                        foreach (var e in emails)
                        {
                            lstEmails.Add(new EmailVM
                            {
                                Id = e.Id,
                                ContactId = e.ContactId,
                                IsPrimary = e.IsPrimary,
                                Address = e.Address

                            });
                        }
                        lst.Add(new ContactVM
                        {
                            Id = contact.Id,
                            Name = contact.Name,
                            BirthDate = contact.BirthDate,
                            Email = lstEmails.Count() > 0 ? lstEmails : new List<EmailVM>()
                        });

                    }
                    return Ok(lst);
                }
                else
                {
                    return StatusCode(404, "No Record found against given Birthdate Range: " + birthStartDate.ToString("yyyy-MM-dd") + " - " + birthEndDate.ToString("yyyy-MM-dd"));
                }

            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        // POST: api/Contact/Create
        [HttpPost]
        [Route("Create")]
        public IActionResult Post([FromBody] ContactVM model)
        {
            try
            {
                if (model == null)
                {
                    return StatusCode(400, "Contact Request model is empty.");
                }
                Contact obj = new Contact();
                obj.Name = model.Name;
                obj.BirthDate = model.BirthDate;
                _dataRepository.Add(obj);

                bool firstEmailflg = true;
                foreach (var email in model.Email)
                {
                    Email emailObj = new Email();
                    emailObj.ContactId = obj.Id;
                    emailObj.Address = email.Address;
                    emailObj.IsPrimary = firstEmailflg;
                    _dataRepositoryEmails.Add(emailObj);
                    firstEmailflg = false;
                }
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
        // PUT: api/Contact/Update
        [HttpPut]
        [Route("Update")]
        public IActionResult Update([FromBody] ContactVM model)
        {
            try
            {
                if (model == null)
                {
                    return StatusCode(400, "Contact Request model is empty.");
                }
                Contact ContactToUpdate = _dataRepository.Get(model.Id);
                var obj = ContactToUpdate;
                if (ContactToUpdate == null)
                {
                    return StatusCode(400, "The Contact record couldn't be found.");
                }
                obj.Name = model.Name;
                obj.BirthDate = model.BirthDate;
                _dataRepository.Update(ContactToUpdate, obj);

                bool firstEmailflg = true;
                foreach (var email in model.Email)
                {
                    var existingEmail = _dataRepositoryEmails.Get(email.Id);
                    if(existingEmail != null)
                    {
                        Email emailObj = new Email();
                        emailObj.Address = email.Address;
                        emailObj.ContactId = emailObj.ContactId;
                        emailObj.IsPrimary = existingEmail.IsPrimary;
                        _dataRepositoryEmails.Update(existingEmail, emailObj);
                    }
                    else
                    {
                        Email emailObj = new Email();
                        emailObj.ContactId = obj.Id;
                        emailObj.Address = email.Address;
                        emailObj.IsPrimary = firstEmailflg;
                        _dataRepositoryEmails.Add(emailObj);
                        firstEmailflg = false;
                    }
                    
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
        // DELETE: api/Contact/id
        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(long id)
        {
            try
            {
                Contact contact = _dataRepository.Get(id);
                if (contact == null)
                {
                    return StatusCode(400, "The Contact record couldn't be found.");
                }
                _dataRepository.Delete(contact);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}

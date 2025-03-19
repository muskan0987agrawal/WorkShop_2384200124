using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{ 
    public interface IContactRL
    {
        ResponseModel<ContactEntity> AddContact(CreateContactRequestModel contact);
        List<ContactEntity> FetchAllContact(int userId);
        ResponseModel<ContactEntity> FetchContactById(int id);
        ResponseModel<ContactEntity> DeleteContactById(int id);
        ResponseModel<ContactEntity> UpdateContactById(int id, UpdateContactRequestModel contact);
    
}
}

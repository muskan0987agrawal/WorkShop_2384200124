using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Interface
{
    public interface IContactBL
    {
        ResponseModel<ContactEntity> AddContact(CreateContactRequestModel contact);
        public List<ContactEntity> FetchAllContact();
        //ResponseModel<ContactEntity> FetchContactById(int id);
        //ResponseModel<ContactEntity> DeleteContactById(int id);
        //ResponseModel<ContactEntity> UpdateContactById(int id, UpdateContactRequestModel contact);


    }
}

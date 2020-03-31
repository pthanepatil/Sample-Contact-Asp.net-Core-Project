using Evolent.Database.Contracts;
using Evolent.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Evolent.Database.Repository
{
    public class ContactRepository : IContactRepository
    {
        #region Declarations
        private readonly IDefaultDatabase _database;

        public ContactRepository(IDefaultDatabase database)
        {
            this._database = database;
        }

        #endregion

        public async Task<List<ContactModel>> LoadUserContacts(int userId, int? contactId)
        {
            var dbCommand = _database.GetStoredProcCommand("sp_get_user_contacts");
            _database.AddInParameter(dbCommand, "@userId", DbType.Int32, userId);
            _database.AddInParameter(dbCommand, "@contactId", DbType.Int32, contactId);

            List<ContactModel> contactList = new List<ContactModel>();
            using (var reader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (reader.Read())
                {
                    ContactModel contact = new ContactModel();
                    contact.ContactId = Convert.ToInt32(reader["ContactId"]);
                    contact.FirstName = Convert.ToString(reader["FirstName"]);
                    contact.LastName = Convert.ToString(reader["LastName"]);
                    contact.Email = Convert.ToString(reader["Email"]);
                    contact.PhoneNumber = Convert.ToString(reader["PhoneNumber"]);
                    contact.Status = Convert.ToBoolean(reader["isActive"]);

                    contactList.Add(contact);
                }
            }

            return contactList;
        }

        public async Task<int> SaveUserContacts(int userId, ContactModel contact)
        {
            var dbCommand = _database.GetStoredProcCommand("sp_save_user_contact");
            _database.AddInParameter(dbCommand, "@userId", DbType.Int32, userId);
            _database.AddInParameter(dbCommand, "@contactId", DbType.Int32, contact.ContactId);
            _database.AddInParameter(dbCommand, "@firstName", DbType.String, contact.FirstName);
            _database.AddInParameter(dbCommand, "@lastName", DbType.String, contact.LastName);
            _database.AddInParameter(dbCommand, "@phoneNumber", DbType.String, contact.PhoneNumber);
            _database.AddInParameter(dbCommand, "@email", DbType.String, contact.Email);
            _database.AddInParameter(dbCommand, "@isActive", DbType.Boolean, contact.Status);

            return Convert.ToInt32(await _database.ExecuteScalarAsync(dbCommand));
        }

        public async Task<int> UpdateContactStatus(int userId, int contactId, bool status)
        {
            var dbCommand = _database.GetStoredProcCommand("sp_update_contact_status");
            _database.AddInParameter(dbCommand, "@userId", DbType.Int32, userId);
            _database.AddInParameter(dbCommand, "@contactId", DbType.Int32, contactId);
            _database.AddInParameter(dbCommand, "@isActive", DbType.Boolean, status);

            return Convert.ToInt32(await _database.ExecuteScalarAsync(dbCommand));
        }
    }
}

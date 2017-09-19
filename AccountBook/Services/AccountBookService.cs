using AccountBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountBook.Services
{
    public class AccountBookService
    {
        private readonly IRepository<Models.AccountBook> _AccountBookRep;
        private readonly IUnitOfWork _unitOfWork;

        public AccountBookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _AccountBookRep = new Repository<Models.AccountBook>(unitOfWork);
        }

        public IEnumerable<Models.AccountBook> Lookup()
        {
            return _AccountBookRep.LookupAll();
        }

        public IEnumerable<Models.AccountBook> Lookup(string UserID)
        {
            return this.Lookup().Where(d => d.Creator == UserID);
        }

        public Models.AccountBook GetSingle(Guid RecordId)
        {
            return _AccountBookRep.GetSingle(d => d.Id == RecordId);
        }

        public Models.AccountBook GetSingle(Guid RecordId, string UserID)
        {
            return _AccountBookRep.GetSingle(d => d.Id == RecordId && d.Creator == UserID);
        }

        public void Add(Models.AccountBook AccountBookList)
        {
            _AccountBookRep.Create(AccountBookList);
        }

        public void Edit(Models.AccountBook pageData, Models.AccountBook oldData)
        {
            oldData.Date = pageData.Date;
            oldData.Category = pageData.Category;
            oldData.Amount = pageData.Amount;
            oldData.Remark = pageData.Remark;
        }

        public void Delete(Models.AccountBook data)
        {
            _AccountBookRep.Remove(data);
        }

        public void Save()
        {
            _unitOfWork.Save();
        }
    }
}
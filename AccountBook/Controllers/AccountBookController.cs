using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AccountBook.Models;
using AccountBook.Services;
using AccountBook.Repositories;
using PagedList;
using AccountBook.Models.ViewModels;

namespace AccountBook.Controllers
{
    [Authorize]
    [RoutePrefix("SkillTree")]  /*記帳簿路由改為SkillTree*/
    [Route("{action=Index}")]
    public class AccountBookController : Controller
    {
        private readonly AccountBookService _AccountBookSvc;

        public AccountBookController()
        {
            var unitOfWork = new EFUnitOfWork();
            _AccountBookSvc = new AccountBookService(unitOfWork);
        }

        
        public ActionResult Index(int? Page)
        {
            ViewData["CurrentPage"] = Page;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Category,Amount,Date,Remark")] Models.ViewModels.EditRecordViewModel accountBook)
        {
            if (ModelState.IsValid)
            {
                var CreatorID = User.Identity.Name;
                var recordId = Guid.NewGuid();
                var NewRecord = new Models.AccountBook
                {
                    Id = recordId,
                    Category = (int)accountBook.Category,
                    Date = accountBook.Date,
                    Amount = accountBook.Amount,
                    Remark = accountBook.Remark,
                    CreateDT = DateTime.Now,
                    Creator = CreatorID
                };

                _AccountBookSvc.Add(NewRecord);
                _AccountBookSvc.Save();
            }

            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.AccountBook accountBook = _AccountBookSvc.GetSingle(id.Value, User.Identity.Name);
            if (accountBook == null)
            {
                return HttpNotFound();
            }

            var result = new EditRecordViewModel
            {
                Id = accountBook.Id,
                Date = accountBook.Date,
                Category = (BookType)accountBook.Category,
                Remark = accountBook.Remark,
                Amount = accountBook.Amount
            };

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Category,Amount,Date,Remark")] Models.ViewModels.EditRecordViewModel accountBook)
        {
            var oldData = _AccountBookSvc.GetSingle(accountBook.Id);
            if (oldData != null && ModelState.IsValid)
            {
                var theRecord = new Models.AccountBook
                {
                    Id = accountBook.Id,
                    Category = (int)accountBook.Category,
                    Date = accountBook.Date,
                    Amount = accountBook.Amount,
                    Remark = accountBook.Remark
                };

                _AccountBookSvc.Edit(theRecord, oldData);
                _AccountBookSvc.Save();
                return RedirectToAction("Index");
            }
            return View(accountBook);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.AccountBook accountBook = _AccountBookSvc.GetSingle(id.Value, User.Identity.Name);
            if (accountBook == null)
            {
                return HttpNotFound();
            }
            return View(accountBook);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Models.AccountBook accountBook = _AccountBookSvc.GetSingle(id);
            _AccountBookSvc.Delete(accountBook);
            _AccountBookSvc.Save();
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult List(int? Page)
        {
            int pageSize = 10;
            int currenPage = (Page == null || Page < 1) ? 1 : Page.Value;
           
            var result = _AccountBookSvc.Lookup(User.Identity.Name).OrderByDescending(x => x.Date).ToPagedList(currenPage, pageSize);
            
            return View(result);

        }

    }
}

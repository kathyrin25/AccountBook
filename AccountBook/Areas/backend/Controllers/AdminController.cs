using AccountBook.Repositories;
using AccountBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Net;


namespace AccountBook.Areas.backend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AccountBookService _AccountBookSvc;

        public AdminController()
        {
            var unitOfWork = new EFUnitOfWork();
            _AccountBookSvc = new AccountBookService(unitOfWork);
        }

        // GET: backend/Admin
        public ActionResult Index(int? page, string q, string column = "Date", EnumSort order = EnumSort.Ascending)
        {
            //分頁套件： Install-Package PagedList.Mvc 
            var pageIndex = page.HasValue ? page.Value < 1 ? 1 : page.Value : 1;
            var pageSize = 10;

            //為了範例最簡化，因此直接在 Controller 操作 DB ，實務上請盡量避免
            var source = _AccountBookSvc.Lookup().AsQueryable();//_dbContext.AccountBook.AsQueryable();

            if (string.IsNullOrWhiteSpace(q) == false)
            {
                // 只是單純示範搜尋條件應該如何累加
                var category = Convert.ToInt32(q);
                source = source.Where(d => d.Category == category);
            }

            var result = new QueryOption<AccountBook.Models.AccountBook>
            {
                Order = order,
                Column = column,
                Page = pageIndex,
                PageSize = pageSize,
                Keyword = q
            };
            //利用 SetSource 將資料塞入（塞進去之前不能將資料讀出來）
            result.SetSource(source);
            ViewData.Model = result;
            return View();
            //return View(_AccountBookSvc.Lookup());
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountBook.Models.AccountBook accountBook = _AccountBookSvc.GetSingle(id.Value);
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
            AccountBook.Models.AccountBook accountBook = _AccountBookSvc.GetSingle(id);
            _AccountBookSvc.Delete(accountBook);
            _AccountBookSvc.Save();
            return RedirectToAction("Index");
        }
    }
}
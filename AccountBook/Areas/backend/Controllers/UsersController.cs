using AccountBook.Areas.backend.Models;
using AccountBook.Models;
using AccountBook.Models.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AccountBook.Areas.backend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {

        private List<SelectListItem> RoleSelectListItems(string selected = "")
        {
            var selectedRoles = string.IsNullOrWhiteSpace(selected) ? null : selected.Split(',');
            var _RoleManager = this.HttpContext.GetOwinContext().Get<ApplicationRoleManager>();

            var roles = _RoleManager.Roles.OrderBy(x => x.Name);            
            var Items = new List<SelectListItem>();
            foreach (var role in roles)
            {
                Items.Add(new SelectListItem
                {
                    Value = role.Name,
                    Text = role.Name,
                    Selected = (selectedRoles == null) ? false : selectedRoles.Contains(role.Name)
                });
            }

            return Items;
        }

        // GET: backend/Users
        public ActionResult Index()
        {
            var _RoleManager = this.HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            var _UserManager = this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var users = _UserManager.Users.OrderBy(x => x.Email).ToList();
            
            var result = new List<UserModel>();
            foreach (var user in users)
            {
                var selectedRoleNames = from r in _RoleManager.Roles
                                        where r.Users.Select(u => u.UserId).Contains(user.Id)
                                        select r.Name;

                result.Add(new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    NickName = user.NickName,
                    UserName = user.UserName,
                    status = (user.LockoutEnabled == true && user.LockoutEndDateUtc <= DateTime.UtcNow) ? UserStatus.失效 : UserStatus.正常,
                    Role = string.Join(",", selectedRoleNames.ToArray())
                });

            }

            return View(result);
        }

        public ActionResult Edit(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _UserManager = this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = _UserManager.FindById(Id);
            
            if (user == null)
            {
                return HttpNotFound();
            }

            //角色
            var _RoleManager = this.HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            var selectedRoleNames = from r in _RoleManager.Roles
                                    where r.Users.Select(u => u.UserId).Contains(user.Id)
                                    select r.Name;

            var UserInRoles = string.Join(",", selectedRoleNames.ToArray());
            var UserStatusValue = (user.LockoutEnabled == true && user.LockoutEndDateUtc <= DateTime.UtcNow) ? UserStatus.失效 : UserStatus.正常;
            var result = new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                NickName = user.NickName,
                UserName = user.UserName,
                status = UserStatusValue,
                Role = UserInRoles
            };            

            var items = this.RoleSelectListItems(UserInRoles);
            ViewBag.RoleItems = items;

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,NickName,UserName,status,Role")] UserModel userData, string[] Role)
        {
            var _UserManager = this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = _UserManager.FindById(userData.Id);
            
            if (user != null && ModelState.IsValid)
            {
                user.NickName = userData.NickName;
                user.UserName = userData.UserName;

                if (userData.status == UserStatus.正常)
                {
                    _UserManager.SetLockoutEnabled(userData.Id, false);
                }
                else if (userData.status == UserStatus.失效)
                {
                    _UserManager.SetLockoutEnabled(userData.Id, true);
                    _UserManager.SetLockoutEndDate(userData.Id, DateTime.UtcNow.AddDays(-1));
                }

                await _UserManager.UpdateAsync(user);

                //重設角色                 
                var oldRoles = _UserManager.GetRolesAsync(userData.Id);
                await _UserManager.RemoveFromRolesAsync(userData.Id, oldRoles.Result.ToArray());

                foreach (var r in Role)
                {
                    await _UserManager.AddToRoleAsync(userData.Id, r);
                }                

                return RedirectToAction("Index");
            }

            var roles = string.Join(",", Role);
            var items = this.RoleSelectListItems(roles);
            ViewBag.RoleItems = items;

            return View(userData);
        }

        
    }
}
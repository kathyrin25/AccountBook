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
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        
        public UsersController()
        {
            
        }

        public UsersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;            
        }        

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        private List<SelectListItem> RoleSelectListItems(string selected = "")
        {
            var selectedRoles = string.IsNullOrWhiteSpace(selected) ? null : selected.Split(',');

            var roles = RoleManager.Roles.OrderBy(x => x.Name);            
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
            var users = UserManager.Users.OrderBy(x => x.Email).ToList();    

            var result = new List<UserModel>();
            foreach (var user in users)
            {
                var selectedRoleNames = from r in RoleManager.Roles
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

            var user = UserManager.FindById(Id);            
            if (user == null)
            {
                return HttpNotFound();
            }

            //角色
            var selectedRoleNames = from r in RoleManager.Roles
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
            var user = UserManager.FindById(userData.Id);
            
            if (user != null && ModelState.IsValid)
            {
                user.NickName = userData.NickName;
                user.UserName = userData.UserName;

                if (userData.status == UserStatus.正常)
                {
                    UserManager.SetLockoutEnabled(userData.Id, false);
                }
                else if (userData.status == UserStatus.失效)
                {
                    UserManager.SetLockoutEnabled(userData.Id, true);
                    UserManager.SetLockoutEndDate(userData.Id, DateTime.UtcNow.AddDays(-1));
                }

                await UserManager.UpdateAsync(user);

                //重設角色                 
                var oldRoles = UserManager.GetRolesAsync(userData.Id);
                await UserManager.RemoveFromRolesAsync(userData.Id, oldRoles.Result.ToArray());

                foreach (var r in Role)
                {
                    await UserManager.AddToRoleAsync(userData.Id, r);
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
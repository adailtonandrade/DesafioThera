using Application.Interfaces;
using Application.ViewModels;
using Domain.Enum;
using Microsoft.AspNet.Identity;
using Domain.Util;
using Identity.Configuration;
using Identity.Model;
using Identity.ViewModels;
using RedWillow.MvcToastrFlash;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using DesafioThera.CustomAttribute;

namespace DesafioThera.Controllers
{
    public class AdministratorController : Controller
    {
        List<string> errors = new List<string>();
        private string activeStatus = ((int)GenericStatusEnum.Active).ToString();
        private readonly IUserAppService _userAppService;
        private ApplicationUserManager _userManager;
        private object defaultBackPage;

        public AdministratorController(ApplicationUserManager userManager, IUserAppService userAppService)
        {
            _userManager = userManager;
            _userAppService = userAppService;
            defaultBackPage = new { Controller = "Administrator", Action = "Index" };
        }

        // GET: Administrator
        [ClaimsAuthorize(claimType: TypePermissionEnum.Administrators, claimValue: ValuePermissionEnum.Consult)]
        public ActionResult Index()
        {
            var administrators = _userAppService.Get(u => u.ProfileId == (int)ProfileEnum.Administrator && u.Active == ((int)GenericStatusEnum.Active).ToString());
            return View(administrators);
        }
        
        // GET: Administrator/Create
        [ClaimsAuthorize(claimType: TypePermissionEnum.Administrators, claimValue: ValuePermissionEnum.Create)]
        public ActionResult Create()
        {
            RegisterVM administrator = new RegisterVM();
            administrator.ProfileId = (int)ProfileEnum.Administrator;
            return View(administrator);
        }

        // POST: Administrator/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Administrators, claimValue: ValuePermissionEnum.Create)]
        public async Task<ActionResult> Create(RegisterVM administrator)
        {
            if (ModelState.IsValid)
            {
                if (administrator != null && administrator.ProfileId != (int)ProfileEnum.Administrator)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = new ApplicationUser
                {
                    UserName = administrator.Email.Trim(),
                    Email = administrator.Email.Trim(),
                    Cpf = Formatter.RemoveFormattingOfCnpjOrCpf(administrator.Cpf),
                    Name = administrator.Name.Trim(),
                    NickName = administrator.NickName.Trim(),
                    ProfileId = administrator.ProfileId,
                    CreatedAt = DateTime.Now,
                    Active = activeStatus
                };
                String passwd = RandomizePassword.GenerateRandom(8);
                var result = await _userManager.CreateAsync(user, passwd);
                if (result.Succeeded)
                {
                    if (errors.Count == 0)
                    {
                        var route = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                        MailMessages mailMessage = new MailMessages(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Templates/MailTemplate.txt"));
                        string body = mailMessage.RegisterMessage(user.Name, passwd, route);
                        await _userManager.SendEmailAsync(user.Id, "Cadastro de Administrador(a)", body);
                        this.Flash(Toastr.SUCCESS, "Administrador(a) cadastrado(a) com sucesso, verifique o e-mail com a senha para realizar o primeiro acesso");
                    }
                    else
                    {
                        _userManager.Delete(user);
                    }
                    return RedirectToAction(
                        defaultBackPage.GetType().GetProperty("Action").GetValue(defaultBackPage, null).ToString(),
                        defaultBackPage.GetType().GetProperty("Controller").GetValue(defaultBackPage, null).ToString());
                }
                else
                {
                    foreach (var key in ApplicationUserManager.dicErrors.Keys)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(ApplicationUserManager.dicErrors[key]);
                        ModelStateMessage.AddModelStateError(errors, key, ModelState);
                    }
                }
            }
            return View(administrator);
        }

        // GET: Administrator/Edit/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Administrators, claimValue: ValuePermissionEnum.Update)]
        public ActionResult Edit(int id)
        {
            UserVM administrator = _userAppService.GetById(id);
            if (administrator == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { action = "NotFound", controller = "Error" }));
            }
            else if (administrator != null && administrator.ProfileId != (int)ProfileEnum.Administrator)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(administrator);
        }

        // POST: Administrator/Edit
        [HttpPost]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Administrators, claimValue: ValuePermissionEnum.Update)]
        public ActionResult Edit(UserVM administrator)
        {
            if (!ModelState.IsValid)
            {
                return View(administrator);
            }
            errors = _userAppService.Update(administrator);
            if (errors.Count == 0)
            {
                this.Flash(Toastr.SUCCESS, String.Format("Administrador(a) editado(a) com sucesso"));
                return RedirectToAction("Index");
            }
            ModelStateMessage.AddModelStateError(errors, string.Empty, ModelState);
            return View(administrator);

        }

        // GET: Secretary/Details/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Administrators, claimValue: ValuePermissionEnum.Consult)]
        public ActionResult Details(int id)
        {
            var administrator = _userAppService.GetById(id);
            if (administrator != null && administrator.ProfileId != (int)ProfileEnum.Administrator)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(administrator);
        }

        // GET: Secretary/Delete/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Administrators, claimValue: ValuePermissionEnum.Deactivate)]
        public ActionResult Delete(int administratorId)
        {
            var administrator = _userAppService.GetById(administratorId);
            if (administrator != null && administrator.ProfileId != (int)ProfileEnum.Administrator)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(administrator);
        }

        // POST: Secretary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Administrators, claimValue: ValuePermissionEnum.Deactivate)]
        public ActionResult DeleteConfirmed(int administratorId)
        {
            errors = _userAppService.Delete(administratorId);
            if (errors.Count == 0)
            {
                this.Flash(Toastr.SUCCESS, String.Format("Administrador(a) Desativado(a) com sucesso"));
                return RedirectToAction("Index");
            }
            ModelStateMessage.AddModelStateError(errors, string.Empty, ModelState);
            return RedirectToAction("Delete", new { administratorId });
        }
    }
}

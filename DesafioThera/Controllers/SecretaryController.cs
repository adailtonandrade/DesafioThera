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
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DesafioThera.CustomAttribute;

namespace DesafioThera.Controllers
{
    public class SecretaryController : Controller
    {
        List<string> errors = new List<string>();
        private string activeStatus = ((int)GenericStatusEnum.Active).ToString();
        private readonly IUserAppService _userAppService;
        private ApplicationUserManager _userManager;
        private object defaultBackPage;

        public SecretaryController(ApplicationUserManager userManager, IUserAppService userAppService)
        {
            _userManager = userManager;
            _userAppService = userAppService;
            defaultBackPage = new { Controller = "Secretary", Action = "Index" };
        }

        // GET: Secretary
        [ClaimsAuthorize(claimType: TypePermissionEnum.Secretaries, claimValue: ValuePermissionEnum.Consult)]
        public ActionResult Index()
        {
            var secretaries = _userAppService.Get(u => u.ProfileId == (int)ProfileEnum.Secretary && u.Active == ((int)GenericStatusEnum.Active).ToString());
            return View(secretaries);
        }

        // GET: Secretary/Create
        [ClaimsAuthorize(claimType: TypePermissionEnum.Secretaries, claimValue: ValuePermissionEnum.Create)]
        public ActionResult Create()
        {
            RegisterVM secretary = new RegisterVM();
            secretary.ProfileId = (int)ProfileEnum.Secretary;
            return View(secretary);
        }

        // POST: Secretary/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Secretaries, claimValue: ValuePermissionEnum.Create)]
        public async Task<ActionResult> Create(RegisterVM secretary)
        {
            if (ModelState.IsValid)
            {
                if (secretary != null && secretary.ProfileId != (int)ProfileEnum.Secretary)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = new ApplicationUser
                {
                    UserName = secretary.Email.Trim(),
                    Email = secretary.Email.Trim(),
                    Cpf = Formatter.RemoveFormattingOfCnpjOrCpf(secretary.Cpf),
                    Name = secretary.Name.Trim(),
                    NickName = secretary.NickName.Trim(),
                    ProfileId = secretary.ProfileId,
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
                        await _userManager.SendEmailAsync(user.Id, "Cadastro de Secretária(o)", body);
                        this.Flash(Toastr.SUCCESS, "Secretário(a) cadastrada(o) com sucesso, verifique o e-mail com a senha para realizar o primeiro acesso");
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
            return View(secretary);
        }

        // GET: Secretary/Edit/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Secretaries, claimValue: ValuePermissionEnum.Update)]
        public ActionResult Edit(int id)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            UserVM user = _userAppService.GetById(id);
            if (user == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { action = "NotFound", controller = "Error" }));
            }
            else if (user != null && user.ProfileId != (int)ProfileEnum.Secretary)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(user);
        }

        // POST: Secretary/Edit
        [HttpPost]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Secretaries, claimValue: ValuePermissionEnum.Update)]
        public ActionResult Edit(UserVM user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            errors = _userAppService.Update(user);
            if (errors.Count == 0)
            {
                this.Flash(Toastr.SUCCESS, String.Format("Secretária(o) editada(o) com sucesso"));
                return RedirectToAction("Index");

            }
            ModelStateMessage.AddModelStateError(errors, string.Empty, ModelState);
            return View(user);
        }

        // GET: Secretary/Details/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Secretaries, claimValue: ValuePermissionEnum.Consult)]
        public ActionResult Details(int id)
        {
            var secretary = _userAppService.GetById(id);
            if (secretary != null && secretary.ProfileId != (int)ProfileEnum.Secretary)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(secretary);
        }

        // GET: Secretary/Delete/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Secretaries, claimValue: ValuePermissionEnum.Deactivate)]
        public ActionResult Delete(int secretaryId)
        {
            var secretary = _userAppService.GetById(secretaryId);
            if (secretary != null && secretary.ProfileId != (int)ProfileEnum.Secretary)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(secretary);
        }

        // POST: Secretary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Secretaries, claimValue: ValuePermissionEnum.Deactivate)]
        public ActionResult DeleteConfirmed(int secretaryId)
        {
            errors = _userAppService.Delete(secretaryId);
            if (errors.Count == 0)
            {
                this.Flash(Toastr.SUCCESS, String.Format("Secretária(o) Desativada(o) com sucesso"));
                return RedirectToAction("Index");
            }
            ModelStateMessage.AddModelStateError(errors, string.Empty, ModelState);
            return RedirectToAction("Delete", new { secretaryId });
        }
    }
}

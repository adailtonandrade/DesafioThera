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
    public class ReaderController : Controller
    {
        List<string> errors = new List<string>();
        private string activeStatus = ((int)GenericStatusEnum.Active).ToString();
        private readonly IUserAppService _userAppService;
        private ApplicationUserManager _userManager;
        private object defaultBackPage;

        public ReaderController(ApplicationUserManager userManager, IUserAppService userAppService)
        {
            _userManager = userManager;
            _userAppService = userAppService;
            defaultBackPage = new { Controller = "Reader", Action = "Index" };
        }

        [ClaimsAuthorize(claimType: TypePermissionEnum.Readers, claimValue: ValuePermissionEnum.Consult)]
        // GET: Reader
        public ActionResult Index()
        {
            if (TempData["password"] != null)
            {
                ViewBag.Password = "Aqui está a senha gerada: " + TempData["password"].ToString();
                TempData.Remove("password");
            }
            var readers = _userAppService.Get(u => u.ProfileId == (int)ProfileEnum.Reader && u.Active == ((int)GenericStatusEnum.Active).ToString());
            return View(readers);
        }

        // GET: Reader/Create
        [ClaimsAuthorize(claimType: TypePermissionEnum.Readers, claimValue: ValuePermissionEnum.Create)]
        public ActionResult Create()
        {
            RegisterVM reader = new RegisterVM();
            reader.ProfileId = (int)ProfileEnum.Reader;
            return View(reader);
        }


        // POST: Reader/Create
        [HttpPost]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Readers, claimValue: ValuePermissionEnum.Create)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterVM reader)
        {
            if (ModelState.IsValid)
            {
                if (reader != null && reader.ProfileId != (int)ProfileEnum.Reader)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = new ApplicationUser
                {
                    UserName = reader.Email.Trim(),
                    Email = reader.Email.Trim(),
                    Cpf = Formatter.RemoveFormattingOfCnpjOrCpf(reader.Cpf),
                    Name = reader.Name.Trim(),
                    NickName = reader.NickName.Trim(),
                    ProfileId = reader.ProfileId,
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
                        await _userManager.SendEmailAsync(user.Id, "Cadastro de Leitor(a)", body);
                        this.Flash(Toastr.SUCCESS, "Leitor(a) cadastrado(a) com sucesso, verifique o e-mail com a senha para realizar o primeiro acesso");
                    }
                    else
                    {
                        _userManager.Delete(user);
                    }
                    TempData["password"] = passwd;
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
            return View(reader);
        }

        // GET: Reader/Edit/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Readers, claimValue: ValuePermissionEnum.Update)]
        public ActionResult Edit(int id)
        {
            UserVM reader = _userAppService.GetById(id);
            if (reader == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { action = "NotFound", controller = "Error" }));
            }
            else if (reader != null && reader.ProfileId != (int)ProfileEnum.Reader)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(reader);
        }

        // POST: Reader/Edit
        [ClaimsAuthorize(claimType: TypePermissionEnum.Readers, claimValue: ValuePermissionEnum.Update)]
        [HttpPost]
        public ActionResult Edit(UserVM reader)
        {
            if (!ModelState.IsValid)
            {
                return View(reader);
            }
            errors = _userAppService.Update(reader);
            if (errors.Count == 0)
            {
                this.Flash(Toastr.SUCCESS, String.Format("Leitor(a) editado(a) com sucesso"));
                return RedirectToAction("Index");

            }
            ModelStateMessage.AddModelStateError(errors, string.Empty, ModelState);
            return View(reader);

        }

        // GET: Reader/Details/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Readers, claimValue: ValuePermissionEnum.Consult)]
        public ActionResult Details(int id)
        {
            var reader = _userAppService.GetById(id);
            if (reader == null)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { action = "NotFound", controller = "Error" }));
            }
            if (reader != null && reader.ProfileId != (int)ProfileEnum.Reader)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(reader);
        }

        // GET: Reader/Delete/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Readers, claimValue: ValuePermissionEnum.Deactivate)]
        public ActionResult Delete(int id)
        {
            var reader = _userAppService.GetById(id);
            if (reader != null && reader.ProfileId != (int)ProfileEnum.Reader)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(reader);
        }

        // POST: Reader/Delete/5
        [HttpPost, ActionName("Delete")]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Readers, claimValue: ValuePermissionEnum.Deactivate)]
        public ActionResult DeleteConfirmed(int id)
        {
            errors = _userAppService.Delete(id);
            if (errors.Count == 0)
            {
                this.Flash(Toastr.SUCCESS, String.Format("Leitor(a) Desativado(a) com sucesso"));
                return RedirectToAction("Index");
            }
            ModelStateMessage.AddModelStateError(errors, string.Empty, ModelState);
            return RedirectToAction("Delete", new { id });
        }
    }
}

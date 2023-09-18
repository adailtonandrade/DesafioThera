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
            var readers = _userAppService.Get(u => u.ProfileId == (int)ProfileEnum.Reader);
            return View(readers);
        }

        //
        // GET: Reader/Create
        [ClaimsAuthorize(claimType: TypePermissionEnum.Readers, claimValue: ValuePermissionEnum.Create)]
        public ActionResult Create()
        {
            RegisterVM reader = new RegisterVM();
            reader.ProfileId = (int)ProfileEnum.Reader;
            return View(reader);
        }

        //
        // POST: /Reader/Create
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
                    IdProfile = reader.ProfileId,
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

        [ClaimsAuthorize(claimType: TypePermissionEnum.Readers, claimValue: ValuePermissionEnum.Update)]
        public ActionResult Edit(int id)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
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

        [ClaimsAuthorize(claimType: TypePermissionEnum.Readers, claimValue: ValuePermissionEnum.Consult)]
        // GET: Reader/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Reader/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reader/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

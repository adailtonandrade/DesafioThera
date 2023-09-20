using Application.Interfaces;
using Application.ViewModels;
using DesafioThera.CustomAttribute;
using Domain.Enum;
using Domain.Util;
using Microsoft.AspNet.Identity;
using RedWillow.MvcToastrFlash;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace DesafioThera.Controllers
{
    public class TalentController : Controller
    {
        private List<string> _errors = new List<string>();
        private readonly ITalentAppService _talentAppService;

        public TalentController(ITalentAppService talentAppService)
        {
            _talentAppService = talentAppService;
        }

        // GET: Talent
        [ClaimsAuthorize(claimType: TypePermissionEnum.Talents, claimValue: ValuePermissionEnum.Consult)]
        public ActionResult Index()
        {
            var talents = _talentAppService.Get(t => t.Active == ((int)GenericStatusEnum.Active).ToString());
            return View(talents);
        }

        // GET: Talent/Create
        [ClaimsAuthorize(claimType: TypePermissionEnum.Talents, claimValue: ValuePermissionEnum.Create)]
        public ActionResult Create()
        {
            return View(new TalentVM());
        }

        // POST: Talent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Talents, claimValue: ValuePermissionEnum.Create)]
        public ActionResult Create(TalentVM talentVM)
        {
            talentVM.UpdatedBy = Int32.Parse(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                _errors = _talentAppService.Insert(talentVM);
                if (_errors.Count == 0)
                {
                    this.Flash(Toastr.SUCCESS, String.Format("Novo Talento cadastrado com sucesso"));
                    return RedirectToAction("Index");
                }
            }
            ModelStateMessage.AddModelStateError(_errors, string.Empty, ModelState);
            return View(talentVM);
        }


        // GET: Talent/Edit/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Talents, claimValue: ValuePermissionEnum.Update)]
        public ActionResult Edit(int id)
        {
            var userId = Int32.Parse(User.Identity.GetUserId());
            TalentVM talent = _talentAppService.GetById(id);
            talent.UpdatedBy = userId;
            if (talent == null)
                return new RedirectToRouteResult(new RouteValueDictionary(new { action = "NotFound", controller = "Error" }));

            return View(talent);
        }

        // POST: Talent/Edit/
        [ClaimsAuthorize(claimType: TypePermissionEnum.Talents, claimValue: ValuePermissionEnum.Update)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TalentVM talent)
        {
            if (!ModelState.IsValid)
                return View(talent);

            _errors = _talentAppService.Update(talent);
            if (_errors.Count == 0)
            {
                this.Flash(Toastr.SUCCESS, String.Format("Talento editado com sucesso"));
                return RedirectToAction("Index");

            }
            ModelStateMessage.AddModelStateError(_errors, string.Empty, ModelState);
            return View(talent);
        }

        // GET: Talent/Details/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Talents, claimValue: ValuePermissionEnum.Consult)]
        public ActionResult Details(int id)
        {
            TalentDetailsVM talent = _talentAppService.GetDetailsById(id);
            if (talent == null)
                return new RedirectToRouteResult(new RouteValueDictionary(new { action = "NotFound", controller = "Error" }));

            return View(talent);
        }

        // GET: Talent/DownloadResume
        [ClaimsAuthorize(claimType: TypePermissionEnum.Talents, claimValue: ValuePermissionEnum.Consult)]
        public ActionResult DownloadResume(int talentId)
        {
            var talentResume = _talentAppService.GetResumeByTalentId(talentId);
            if (talentResume != null && talentResume.Errors.Count > 0)
                return HttpNotFound();

            return File(talentResume.FileContent, talentResume.ContentType, talentResume.FileName);
        }

        // GET: Talent/Delete/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Talents, claimValue: ValuePermissionEnum.Deactivate)]
        public ActionResult Delete(int id)
        {
            var talent = _talentAppService.GetById(id);
            if (talent == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(talent);
        }

        // POST: Talent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Talents, claimValue: ValuePermissionEnum.Deactivate)]
        public ActionResult DeleteConfirmed(int id)
        {
            _errors = _talentAppService.Delete(id);
            if (_errors.Count == 0)
            {
                this.Flash(Toastr.SUCCESS, String.Format("Talento Desativado com sucesso"));
                return RedirectToAction("Index");
            }
            ModelStateMessage.AddModelStateError(_errors, string.Empty, ModelState);
            return RedirectToAction("Delete", new { id });
        }
    }
}

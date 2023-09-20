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

        // GET: Talent/Details/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Talents, claimValue: ValuePermissionEnum.Consult)]
        public ActionResult Details(int id)
        {
            TalentDetailsVM talent = _talentAppService.GetDetailsById(id);
            if (talent == null)
                return new RedirectToRouteResult(new RouteValueDictionary(new { action = "NotFound", controller = "Error" }));

            return View(talent);
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

        // GET: Talent/DownloadResume
        [ClaimsAuthorize(claimType: TypePermissionEnum.Talents, claimValue: ValuePermissionEnum.Consult)]
        public ActionResult DownloadResume(int talentId)
        {
            var talent = _talentAppService.GetById(talentId);
            if (talent != null && talent.Active != ((int)GenericStatusEnum.Active).ToString())
                return HttpNotFound();
            string mimeType;
            switch (Path.GetExtension(talent.ResumeFileName).ToLower())
            {
                case ".pdf":
                    mimeType = "application/pdf";
                    break;
                case ".doc":
                    mimeType = "application/msword";
                    break;
                case ".docx":
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                default:
                    mimeType = "application/octet-stream";
                    break;
            }

            byte[] fileBytes = talent.ResumeFileData;

            return File(fileBytes, mimeType, talent.ResumeFileName);
        }


        // GET: Talent/Delete/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Talents, claimValue: ValuePermissionEnum.Deactivate)]
        public ActionResult Delete(int talentId)
        {
            var talent = _talentAppService.GetById(talentId);
            if (talent == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(talent);
        }

        // POST: Talent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Talents, claimValue: ValuePermissionEnum.Deactivate)]
        public ActionResult DeleteConfirmed(int talentId)
        {
            _errors = _talentAppService.Delete(talentId);
            if (_errors.Count == 0)
            {
                this.Flash(Toastr.SUCCESS, String.Format("Talento Desativado com sucesso"));
                return RedirectToAction("Index");
            }
            ModelStateMessage.AddModelStateError(_errors, string.Empty, ModelState);
            return RedirectToAction("Delete", new { talentId });
        }
    }
}

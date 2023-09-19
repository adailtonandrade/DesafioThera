using Application.Interfaces;
using Application.ViewModels;
using Domain.Enum;
using Domain.Util;
using Microsoft.AspNet.Identity;
using RedWillow.MvcToastrFlash;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
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
        public ActionResult Index()
        {
            var talents = _talentAppService.Get(t => t.Active == ((int)GenericStatusEnum.Active).ToString());
            return View(talents);
        }

        // GET: Talent/Details/5
        public ActionResult Details(int id)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            TalentDetailsVM talent = _talentAppService.GetDetailsById(id);
            if (talent == null)
                return new RedirectToRouteResult(new RouteValueDictionary(new { action = "NotFound", controller = "Error" }));

            return View(talent);
        }

        // GET: Talent/Create
        public ActionResult Create()
        {
            return View(new TalentVM());
        }

        // POST: Talent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

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
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            var userId = Int32.Parse(User.Identity.GetUserId());
            TalentVM talent = _talentAppService.GetById(id);
            talent.UpdatedBy = userId;
            if (talent == null)
                return new RedirectToRouteResult(new RouteValueDictionary(new { action = "NotFound", controller = "Error" }));

            return View(talent);
        }

        // POST: Talent/Edit/5
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
        public ActionResult DownloadResume(string resumeUniqueName, string resumeFileName)
        {
            if (!string.IsNullOrEmpty(resumeUniqueName) && !string.IsNullOrEmpty(resumeFileName))
            {
                var folderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
                var filePath = Path.Combine(folderPath, resumeUniqueName);

                // Check if the file exists
                if (System.IO.File.Exists(filePath))
                {
                    // Determine the MIME type based on the file extension
                    string mimeType;
                    switch (Path.GetExtension(resumeUniqueName).ToLower())
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

                    return File(filePath, mimeType, resumeFileName);
                }
            }

            // If the file doesn't exist or the file name is invalid, return an error
            return HttpNotFound();
        }

        // GET: Talent/Delete/5
        public ActionResult Delete(int id)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            TalentVM talent = _talentAppService.GetById(id);
            if (talent == null)
                return new RedirectToRouteResult(new RouteValueDictionary(new { action = "NotFound", controller = "Error" }));

            return View(talent);
        }

        // POST: Talent/Delete/5
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

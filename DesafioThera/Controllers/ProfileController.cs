using Application.Interfaces;
using System.Web.Mvc;
using Application.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Domain.Util;
using System.Web;
using System;
using Domain.Enum;
using System.Net;
using RedWillow.MvcToastrFlash;

namespace DesafioThera.Controllers
{
    public class ProfileController : Controller
    {
        private List<string> _errors = new List<string>();
        private readonly IProfileAppService _profileAppService;
        private readonly IPermissionAppService _permissionAppService;

        public ProfileController(IProfileAppService profileAppService, IPermissionAppService permissionAppService)
        {
            _profileAppService = profileAppService;
            _permissionAppService = permissionAppService;
        }

        // GET: Profile
        [ClaimsAuthorize(claimType: TypePermissionEnum.Profiles, claimValue: ValuePermissionEnum.Consult)]
        public ActionResult Index()
        {
            var profiles = _profileAppService.Get(p => p.Active.Equals(((int)GenericStatusEnum.Active).ToString())).OrderBy(p => p.Name).ToList();
            return View(profiles);
        }

        // GET: Profile/Create
        [ClaimsAuthorize(claimType: TypePermissionEnum.Profiles, claimValue: ValuePermissionEnum.Create)]
        public ActionResult Create()
        {
            var profileViewCreate = new ProfileVM();
            GroupPermissions(profileViewCreate);
            return View(profileViewCreate);
        }

        // POST: Profile/Create
        [HttpPost]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Profiles, claimValue: ValuePermissionEnum.Create)]
        public ActionResult Create(ProfileVM profileViewCreate)
        {
            GroupPermissions(profileViewCreate);
            if (!ModelState.IsValid)
                return View(profileViewCreate);

            _errors = _profileAppService.Insert(profileViewCreate);
            if (_errors?.Count == 0)
            {
                this.Flash(Toastr.SUCCESS, "O Perfil foi cadastrado com sucesso");
                return RedirectToAction("Index");
            }

            ModelStateMessage.AddModelStateError(_errors, string.Empty, ModelState);
            return View(profileViewCreate);
        }

        // GET: Profile/Edit/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Profiles, claimValue: ValuePermissionEnum.Update)]
        public ActionResult Edit(int id)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (id > 0)
            {
                ProfileVM profileViewEdit = _profileAppService.Get(u => u.Id == id, null, "ProfilePermissions").FirstOrDefault();
                if (profileViewEdit == null)
                    throw new HttpException((int)HttpStatusCode.BadRequest, String.Format("Identificador(Id) inválido. Não foi encontrado nenhum perfil com o Id {0} na base de dados", id));

                GroupPermissions(profileViewEdit);
                GetPermissionIds(profileViewEdit);

                profileViewEdit.SelectedPermissionIdList = profileViewEdit.ProfilePermissions.Select(a => a.PermissionId).ToList();

                return View(profileViewEdit);
            }
            else
            {
                throw new HttpException((int)System.Net.HttpStatusCode.BadRequest, String.Format("Identificador(Id) inválido. Não foi encontrado nenhum perfil com o Id {0} na base de dados", id));
            }
        }

        // POST: Profile/Edit/5
        [HttpPost]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Profiles, claimValue: ValuePermissionEnum.Update)]
        public ActionResult Edit(ProfileVM profileViewEdit)
        {
            GroupPermissions(profileViewEdit);
            if (!ModelState.IsValid)
                return View(profileViewEdit);

            _errors = _profileAppService.Update(profileViewEdit);
            if (_errors?.Count == 0)
            {
                this.Flash(Toastr.SUCCESS, "O perfil foi editado com sucesso");
                return RedirectToAction("Index");
            }
            ModelStateMessage.AddModelStateError(_errors, string.Empty, ModelState);
            return View(profileViewEdit);
        }

        // GET: Profile/Delete/5
        [ClaimsAuthorize(claimType: TypePermissionEnum.Profiles, claimValue: ValuePermissionEnum.Deactivate)]
        public ActionResult Delete(int profileId)
        {
            var profile = _profileAppService.GetById(profileId);
            if (profile != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(profile);
        }

        // POST: Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ClaimsAuthorize(claimType: TypePermissionEnum.Profiles, claimValue: ValuePermissionEnum.Deactivate)]
        public ActionResult DeleteConfirmed(int profileId)
        {
            _errors = _profileAppService.Delete(profileId);
            if (_errors?.Count == 0)
            {
                this.Flash(Toastr.SUCCESS, String.Format("Secretária(o) Desativada(o) com sucesso"));
                return RedirectToAction("Index");

            }
            ModelStateMessage.AddModelStateError(_errors, string.Empty, ModelState);
            return RedirectToAction("Delete", new { profileId });
        }

        private void GetPermissionIds(ProfileVM profile)
        {
            if (profile?.ProfilePermissions?.Count > 0)
            {
                var permissionIds = profile.ProfilePermissions.Select(p => p.PermissionId).ToList();
                profile.SelectedPermissionIdList = permissionIds;
            }
        }

        private void GroupPermissions(ProfileVM profile)
        {
            profile.PermissionList = _permissionAppService.GetAll().ToList();
            profile.PermissionGrouped = profile.PermissionList.GroupBy(u => u.ClaimType).Select(t => t.ToList()).ToList();
        }
    }
}

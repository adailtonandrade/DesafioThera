using Application.Interfaces;
using Application.ViewModels;
using DesafioApi.CustomAttribute;
using DesafioApi.ViewModel;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace DesafioApi.Controllers
{
    [RoutePrefix("api/talents")]
    public class TalentController : ApiController
    {
        List<string> errors = new List<string>();
        private readonly ITalentAppService _talentAppService;

        public TalentController(ITalentAppService talentAppService)
        {
            _talentAppService = talentAppService;
        }

        // GET: api/talents
        [Route("")]
        [ClaimsAuthorization(TypePermissionEnum.Talents, ValuePermissionEnum.Consult)]
        public IHttpActionResult Get()
        {
            var talents = _talentAppService.GetAll();
            return base.Ok(new ResponseViewModel<IEnumerable<Application.ViewModels.TalentVM>>() { Message = "Lista de Talentos", Content = talents });
        }

        //// GET: api/talents/5
        [Route("{id:int}")]
        [ClaimsAuthorization(TypePermissionEnum.Talents, ValuePermissionEnum.Consult)]
        public IHttpActionResult Get(int id)
        {
            var talent = _talentAppService.GetDetailsById(id);
            if (talent != null)
                return Ok(new ResponseViewModel<TalentDetailsVM>() { Content = talent });
            return NotFound();
        }

        // POST: api/talents
        [Route("")]
        [ClaimsAuthorization(TypePermissionEnum.Talents, ValuePermissionEnum.Create)]
        public IHttpActionResult Post(TalentVM talent)
        {
            if (!ModelState.IsValid)
            {
                errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<TalentVM>()
                {
                    Message = "Falha ao cadastrar o talento",
                    Errors = errors,
                    Content = talent
                });
            }
            try
            {
                var claimsPrincipal = (ClaimsPrincipal)User;
                talent.UpdatedBy = Int32.Parse(claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                errors = _talentAppService.Insert(talent);
                if (errors.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, new ResponseViewModel<TalentVM>()
                    {
                        Message = "Falha ao cadastrar o talento",
                        Errors = errors,
                        Content = talent
                    });
                }
                return Content(HttpStatusCode.Created, new ResponseViewModel<TalentVM>()
                {
                    Message = "Talento cadastrado com sucesso",
                    Content = talent
                });
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        // PUT: api/talents/5
        [Route("{id:int}")]
        [ClaimsAuthorization(TypePermissionEnum.Talents, ValuePermissionEnum.Update)]
        public IHttpActionResult Put(int id, TalentVM talent)
        {
            if (id == 0)
            {
                errors.Add("Informe um ID");
                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<TalentVM>()
                {
                    Message = "Falha ao editar o talento",
                    Errors = errors,
                    Content = talent
                });
            }
            if (!ModelState.IsValid)
            {
                errors.AddRange(ModelState.Values
                    .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList());
                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<TalentVM>()
                {
                    Message = "Falha ao editar o talento",
                    Errors = errors,
                    Content = talent
                });
            }
            var claimsPrincipal = (ClaimsPrincipal)User;
            talent.UpdatedBy = Int32.Parse(claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            talent.Id = id;

            errors.AddRange(_talentAppService.Update(talent));
            if (errors.Count > 0)
            {
                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<TalentVM>()
                {
                    Message = "Falha ao ediar o talento",
                    Errors = errors,
                    Content = talent
                });
            }
            return Ok(new ResponseViewModel<TalentVM>() { Message = "Talento alterado com sucesso", Content = talent });
        }

        // DELETE: api/talents/5
        [Route("{id:int}")]
        [ClaimsAuthorization(TypePermissionEnum.Talents, ValuePermissionEnum.Deactivate)]
        public IHttpActionResult Delete(int id)
        {
            if (id == 0)
            {
                errors.Add("Informe um ID");
                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<UserVM>()
                {
                    Message = "Falha ao desativar o talento",
                    Errors = errors
                });
            }
            errors = _talentAppService.Delete(id);
            if (errors.Count == 0)
            {
                return Ok(new ResponseViewModel<string>() { Message = "O talento de ID " + id + " foi desativado com sucesso" });
            }
            errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            return Content(HttpStatusCode.BadRequest, new ResponseViewModel<string>()
            {
                Message = "Falha ao desativar o talento",
                Errors = errors
            });
        }
        // DELETE: api/talents/5
        [Route("~/api/talents/{id:int}/resume")]
        [HttpGet]
        [ClaimsAuthorization(TypePermissionEnum.Talents, ValuePermissionEnum.Consult)]
        public IHttpActionResult DownloadResume(int id)
        {
            var talentResume = _talentAppService.GetResumeByTalentId(id);
            if (talentResume != null && talentResume.Errors.Count > 0)
            {
                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<string>()
                {
                    Message = "Falha ao obter arquivo de currículo",
                    Errors = talentResume.Errors
                });
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(talentResume.FileContent)
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue(talentResume.ContentType);

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = talentResume.FileName
            };

            return ResponseMessage(response);
        }
    }
}
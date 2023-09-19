using Application.Interfaces;
using Application.ViewModels;
using DesafioApi.CustomAttribute;
using DesafioApi.ViewModel;
using Domain.Enum;
using Domain.Util;
using Identity.Configuration;
using Identity.Model;
using Identity.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DesafioApi.Controllers
{
    public class SecretaryController : ApiController
    {
        List<string> errors = new List<string>();
        private string activeStatus = ((int)GenericStatusEnum.Active).ToString();
        private readonly IUserAppService _userAppService;
        private ApplicationUserManager _userManager;

        public SecretaryController(ApplicationUserManager userManager, IUserAppService userAppService)
        {
            _userManager = userManager;
            _userAppService = userAppService;
        }
        [ClaimsAuthorization(TypePermissionEnum.Secretaries, ValuePermissionEnum.Consult)]
        // GET: api/Secretary
        public ResponseViewModel<IEnumerable<UserVM>> Get()
        {

            var secretaries = _userAppService.GetAll();

            return new ResponseViewModel<IEnumerable<UserVM>>() { Message = "Lista de Secretárias", Content = secretaries };
        }

        [ClaimsAuthorization(TypePermissionEnum.Secretaries, ValuePermissionEnum.Consult)]
        // GET: api/Secretary/5
        public IHttpActionResult Get(int id)
        {
            var secretary = _userAppService.GetById(id);
            if (secretary != null)
                return Ok(new ResponseViewModel<UserVM>() { Content = secretary });
            return NotFound();
        }

        [ClaimsAuthorization(TypePermissionEnum.Secretaries, ValuePermissionEnum.Create)]
        // POST: api/Secretary
        public async Task<IHttpActionResult> Post([FromBody] RegisterVM secretary)
        {
            if (ModelState.IsValid)
            {
                if (secretary != null && secretary.ProfileId != (int)ProfileEnum.Secretary)
                {
                    errors.Add("O ProfileId informado não corresponde ao de screatária(o)");
                    return Content(HttpStatusCode.BadRequest, new ResponseViewModel<RegisterVM>()
                    {
                        Message = "Falha ao cadastrar secretária(o)",
                        Content = secretary,
                        Errors = errors
                    });
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
                        var route = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
                        MailMessages mailMessage = new MailMessages(HttpContext.Current.Server.MapPath("~/App_Data/Templates/MailTemplate.txt"));
                        string body = mailMessage.RegisterMessage(user.Name, passwd, route);
                        await _userManager.SendEmailAsync(user.Id, "Cadastro de Secretária(o)", body);
                        return Content(HttpStatusCode.Created, new ResponseViewModel<RegisterVM>()
                        {
                            Message = "Secretário(a) cadastrada(o) com sucesso, verifique o e-mail com a senha para realizar o primeiro acesso, caso não chegue, utilize essa: " + passwd
                        });
                    }
                    else
                    {
                        _userManager.Delete(user);
                    }
                    return InternalServerError();
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
            errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            return Content(HttpStatusCode.BadRequest, new ResponseViewModel<RegisterVM>()
            {
                Message = "Falha ao cadastrar secretária(o)",
                Errors = errors,
                Content = secretary
            });
        }

        [ClaimsAuthorization(TypePermissionEnum.Secretaries, ValuePermissionEnum.Update)]
        // PUT: api/Secretary/5
        public IHttpActionResult Put(int id, [FromBody] UserVM secretary)
        {
            if (id == 0)
            {
                errors.Add("Inform um ID");
                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<UserVM>()
                {
                    Message = "Falha ao editar secretária(o)",
                    Errors = errors,
                    Content = secretary
                });
            }
            if (!ModelState.IsValid)
            {
                errors.AddRange(ModelState.Values
                    .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList());
                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<UserVM>()
                {
                    Message = "Falha ao editar secretária(o)",
                    Errors = errors,
                    Content = secretary
                });
            }
            secretary.Id = id;
            errors = _userAppService.Update(secretary);
            if (errors.Count == 0)
            {
                return Ok(new ResponseViewModel<UserVM>() { Message = "Secretária(o) alterado(o) com sucesso", Content = secretary });
            }
            errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            return Content(HttpStatusCode.BadRequest, new ResponseViewModel<UserVM>()
            {
                Message = "Falha ao editar secretária(o)",
                Errors = errors,
                Content = secretary
            });
        }

        [ClaimsAuthorization(TypePermissionEnum.Secretaries, ValuePermissionEnum.Deactivate)]
        // DELETE: api/Secretary/5
        public IHttpActionResult Delete(int secretaryId)
        {
            if (secretaryId == 0)
            {
                errors.Add("Informe um ID");
                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<UserVM>()
                {
                    Message = "Falha ao desativar secretária(o)",
                    Errors = errors
                });
            }
            errors = _userAppService.Delete(secretaryId);
            if (errors.Count == 0)
            {
                return Ok(new ResponseViewModel<string>() { Message = "A(O) Secretária(o) de ID + " + secretaryId + "desativada(o) com sucesso" });
            }
            errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            return Content(HttpStatusCode.BadRequest, new ResponseViewModel<string>()
            {
                Message = "Falha ao desativar secretária(o)",
            });
        }
    }
}

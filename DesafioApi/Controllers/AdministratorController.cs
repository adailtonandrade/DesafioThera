using Application.Interfaces;
using Application.ViewModels;
using DesafioApi.CustomAttribute;
using DesafioApi.ViewModel;
using Domain.Enum;
using Domain.Util;
using Identity.Configuration;
using Identity.Model;
using Identity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DesafioApi.Controllers
{
    [RoutePrefix("api/administrators")]
    public class AdministratorController : ApiController
    {
        List<string> errors = new List<string>();
        private string activeStatus = ((int)GenericStatusEnum.Active).ToString();
        private readonly IUserAppService _userAppService;
        private ApplicationUserManager _userManager;

        public AdministratorController(ApplicationUserManager userManager, IUserAppService userAppService)
        {
            _userManager = userManager;
            _userAppService = userAppService;
        }
        // GET: api/Administrator
        [Route("")]
        [ClaimsAuthorization(TypePermissionEnum.Administrators, ValuePermissionEnum.Consult)]
        public IHttpActionResult Get()
        {
            var administrators = _userAppService.GetAll();
            return Ok(new ResponseViewModel<IEnumerable<UserVM>>() { Message = "Lista de Administradores", Content = administrators });
        }

        // GET: api/Administrator/5
        [Route("{id:int}")]
        [ClaimsAuthorization(TypePermissionEnum.Administrators, ValuePermissionEnum.Consult)]
        public IHttpActionResult Get(int id)
        {
            var administrator = _userAppService.GetById(id);
            if (administrator != null)
                return Ok(new ResponseViewModel<UserVM>() { Content = administrator });
            return NotFound();
        }

        // POST: api/Administrator
        [Route("")]
        [ClaimsAuthorization(TypePermissionEnum.Administrators, ValuePermissionEnum.Create)]
        public async Task<IHttpActionResult> Post([FromBody] RegisterVM administrator)
        {
            if (!ModelState.IsValid)
            {
                errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<RegisterVM>()
                {
                    Message = "Falha ao cadastrar administrador(a)",
                    Errors = errors,
                    Content = administrator
                });
            }
            if (administrator != null && administrator.ProfileId != (int)ProfileEnum.Secretary)
            {
                errors.Add("O ProfileId informado não corresponde ao de administrador(a)");
                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<RegisterVM>()
                {
                    Message = "Falha ao cadastrar administrador(a)",
                    Content = administrator,
                    Errors = errors
                });
            }
            try
            {
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
                if (!result.Succeeded)
                {
                    foreach (var key in ApplicationUserManager.dicErrors.Keys)
                    {
                        errors.Add(ApplicationUserManager.dicErrors[key]);
                        return Content(HttpStatusCode.BadRequest, new ResponseViewModel<RegisterVM>()
                        {
                            Message = "Falha ao cadastrar administrador(a)",
                            Errors = errors,
                            Content = administrator
                        });
                    }
                }
                administrator.Id = user.Id;
                var route = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
                MailMessages mailMessage = new MailMessages(HttpContext.Current.Server.MapPath("~/App_Data/Templates/MailTemplate.txt"));
                string body = mailMessage.RegisterMessage(user.Name, passwd, route);
                await _userManager.SendEmailAsync(user.Id, "Cadastro de Administrador(a)", body);
                return Content(HttpStatusCode.Created, new ResponseViewModel<RegisterVM>()
                {
                    Message = "Administrador(a) cadastrado(a) com sucesso, verifique o e-mail com a senha para realizar o primeiro acesso, caso não chegue, aqui esta a senha que foi gerada para você: " + passwd,
                    Content = administrator
                });
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        // PUT: api/Administrator/5
        [Route("{id:int}")]
        [ClaimsAuthorization(TypePermissionEnum.Administrators, ValuePermissionEnum.Update)]
        public IHttpActionResult Put(int id, [FromBody] UserVM administrator)
        {
            if (id == 0)
            {
                errors.Add("Informe um ID");
                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<UserVM>()
                {
                    Message = "Falha ao editar administrador(a)",
                    Errors = errors,
                    Content = administrator
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
                    Message = "Falha ao editar administrador(a)",
                    Errors = errors,
                    Content = administrator
                });
            }
            administrator.Id = id;
            errors = _userAppService.Update(administrator);
            if (errors.Count == 0)
            {
                return Ok(new ResponseViewModel<UserVM>() { Message = "Administrador(a) alterado(o) com sucesso", Content = administrator });
            }
            errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            return Content(HttpStatusCode.BadRequest, new ResponseViewModel<UserVM>()
            {
                Message = "Falha ao editar administrador(a)",
                Errors = errors,
                Content = administrator
            });
        }

        // DELETE: api/Administrator/5
        [Route("{id:int}")]
        [ClaimsAuthorization(TypePermissionEnum.Administrators, ValuePermissionEnum.Deactivate)]
        public IHttpActionResult Delete(int id)
        {
            if (id == 0)
            {
                errors.Add("Informe um ID");
                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<UserVM>()
                {
                    Message = "Falha ao desativar administrador(a)",
                    Errors = errors
                });
            }
            errors = _userAppService.Delete(id);
            if (errors.Count == 0)
            {
                return Ok(new ResponseViewModel<string>() { Message = "O(A) administrador(a) de ID + " + id + " foi desativado(a) com sucesso" });
            }
            errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            return Content(HttpStatusCode.BadRequest, new ResponseViewModel<string>()
            {
                Message = "Falha ao desativar administrador(a)",
            });
        }
    }
}
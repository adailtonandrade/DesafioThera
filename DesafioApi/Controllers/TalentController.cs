using Application.Interfaces;
using Application.ViewModels;
using DesafioApi.CustomAttribute;
using DesafioApi.ViewModel;
using Domain.Enum;
using System.Collections.Generic;
using System.Web.Http;

namespace DesafioApi.Controllers
{
    [RoutePrefix("api/talents")]
    public class TalentController : ApiController
    {
        //List<string> errors = new List<string>();
        //private string activeStatus = ((int)GenericStatusEnum.Active).ToString();
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
            return Ok(new ResponseViewModel<IEnumerable<TalentVM>>() { Message = "Lista de Talentos", Content = talents });
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

        //// POST: api/talents
        //[Route("")]
        //[ClaimsAuthorization(TypePermissionEnum.Readers, ValuePermissionEnum.Create)]
        //public async Task<IHttpActionResult> Post([FromBody] RegisterVM reader)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
        //        return Content(HttpStatusCode.BadRequest, new ResponseViewModel<RegisterVM>()
        //        {
        //            Message = "Falha ao cadastrar leitor(a)",
        //            Errors = errors,
        //            Content = reader
        //        });
        //    }
        //    if (reader != null && reader.ProfileId != (int)ProfileEnum.Reader)
        //    {
        //        errors.Add("O ProfileId informado não corresponde ao de leitor(a)");
        //        return Content(HttpStatusCode.BadRequest, new ResponseViewModel<RegisterVM>()
        //        {
        //            Message = "Falha ao cadastrar leitor(a)",
        //            Content = reader,
        //            Errors = errors
        //        });
        //    }
        //    try
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = reader.Email.Trim(),
        //            Email = reader.Email.Trim(),
        //            Cpf = Formatter.RemoveFormattingOfCnpjOrCpf(reader.Cpf),
        //            Name = reader.Name.Trim(),
        //            NickName = reader.NickName.Trim(),
        //            ProfileId = reader.ProfileId,
        //            CreatedAt = DateTime.Now,
        //            Active = activeStatus
        //        };
        //        String passwd = RandomizePassword.GenerateRandom(8);
        //        var result = await _userManager.CreateAsync(user, passwd);
        //        if (!result.Succeeded)
        //        {
        //            foreach (var key in ApplicationUserManager.dicErrors.Keys)
        //            {
        //                errors.Add(ApplicationUserManager.dicErrors[key]);
        //                return Content(HttpStatusCode.BadRequest, new ResponseViewModel<RegisterVM>()
        //                {
        //                    Message = "Falha ao cadastrar leitor(a)",
        //                    Errors = errors,
        //                    Content = reader
        //                });
        //            }
        //        }
        //        reader.Id = user.Id;
        //        var route = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
        //        MailMessages mailMessage = new MailMessages(HttpContext.Current.Server.MapPath("~/App_Data/Templates/MailTemplate.txt"));
        //        string body = mailMessage.RegisterMessage(user.Name, passwd, route);
        //        await _userManager.SendEmailAsync(user.Id, "Cadastro de Leitor(a)", body);
        //        return Content(HttpStatusCode.Created, new ResponseViewModel<RegisterVM>()
        //        {
        //            Message = "Leitor(a) cadastrado(a) com sucesso, verifique o e-mail com a senha para realizar o primeiro acesso, caso não chegue, aqui esta a senha que foi gerada para você: " + passwd,
        //            Content = reader
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        return InternalServerError();
        //    }
        //}

        //// PUT: api/talents/5
        //[Route("{id:int}")]
        //[ClaimsAuthorization(TypePermissionEnum.Readers, ValuePermissionEnum.Update)]
        //public IHttpActionResult Put(int id, [FromBody] UserVM reader)
        //{
        //    if (id == 0)
        //    {
        //        errors.Add("Informe um ID");
        //        return Content(HttpStatusCode.BadRequest, new ResponseViewModel<UserVM>()
        //        {
        //            Message = "Falha ao editar leitor(a)",
        //            Errors = errors,
        //            Content = reader
        //        });
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        errors.AddRange(ModelState.Values
        //            .SelectMany(v => v.Errors)
        //                .Select(e => e.ErrorMessage)
        //                .ToList());
        //        return Content(HttpStatusCode.BadRequest, new ResponseViewModel<UserVM>()
        //        {
        //            Message = "Falha ao editar leitor(a)",
        //            Errors = errors,
        //            Content = reader
        //        });
        //    }
        //    reader.Id = id;
        //    errors = _userAppService.Update(reader);
        //    if (errors.Count == 0)
        //    {
        //        return Ok(new ResponseViewModel<UserVM>() { Message = "Leitor(a) alterado(a) com sucesso", Content = reader });
        //    }
        //    errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
        //    return Content(HttpStatusCode.BadRequest, new ResponseViewModel<UserVM>()
        //    {
        //        Message = "Falha ao editar leitor(a)",
        //        Errors = errors,
        //        Content = reader
        //    });
        //}

        //// DELETE: api/talents/5
        //[Route("{id:int}")]
        //[ClaimsAuthorization(TypePermissionEnum.Readers, ValuePermissionEnum.Deactivate)]
        //public IHttpActionResult Delete(int id)
        //{
        //    if (id == 0)
        //    {
        //        errors.Add("Informe um ID");
        //        return Content(HttpStatusCode.BadRequest, new ResponseViewModel<UserVM>()
        //        {
        //            Message = "Falha ao desativar leitor(a)",
        //            Errors = errors
        //        });
        //    }
        //    errors = _userAppService.Delete(id);
        //    if (errors.Count == 0)
        //    {
        //        return Ok(new ResponseViewModel<string>() { Message = "(O)(A) leitor(a) de ID + " + id + "foi desativado(a) com sucesso" });
        //    }
        //    errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
        //    return Content(HttpStatusCode.BadRequest, new ResponseViewModel<string>()
        //    {
        //        Message = "Falha ao desativar leitor(a)",
        //    });
        //}
    }
}
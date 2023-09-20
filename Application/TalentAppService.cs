using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.ViewModels;
using Domain.Interfaces.Services;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces.Data;
using Application.Interfaces;
using System.Web;
using System.IO;
using AutoMapper.Extensions.ExpressionMapping;

namespace Application
{
    public class TalentAppService : GenericAppService, ITalentAppService
    {
        private readonly ITalentService _talentService;
        private readonly AutoMapper.IMapper _mapper;
        List<string> _errors = new List<string>();

        public TalentAppService(ITalentService talentService, AutoMapper.IMapper mapper,
            IUnitOfWork uow) : base(uow)
        {
            _talentService = talentService;
            _mapper = mapper;
        }

        public List<string> Delete(int id)
        {
            Talent talent = _talentService.GetById(id);
            try
            {
                if (talent.Active.Equals(((int)GenericStatusEnum.Inactive).ToString()))
                    _errors = Reactivate(talent);
                else
                    _errors = Deactivate(talent);
            }
            catch (Exception e)
            {
                if (talent != null)
                    _errors.Add("Erro: " + e.Message.ToString() + "\nInner Exception: " + e.InnerException.Message.ToString());
                else
                    _errors.Add("O Talento não foi encontrado");
            }
            return _errors;
        }

        private List<string> Deactivate(Talent talent)
        {
            try
            {
                var users = _talentService.Get(t => t.Id == talent.Id);
                if (users == null || users.Count() == 0)
                {
                    BeginTransaction();
                    talent.Active = ((int)GenericStatusEnum.Inactive).ToString();
                    _talentService.Update(talent);
                    SaveChanges();
                    Commit();
                }
                else
                    _errors.Add(String.Format("O Talento {0} não pode ser desativado pois não foi encontrado na base de dados", talent.FullName));
            }
            catch (Exception e)
            {
                _errors.Add(String.Format("Ocorreu um erro ao desativar o Perfil"));
                Rollback();
            }
            return _errors;
        }

        private List<string> Reactivate(Talent talent)
        {
            try
            {
                BeginTransaction();
                talent.Active = ((int)GenericStatusEnum.Active).ToString();
                _talentService.Update(talent);
                SaveChanges();
                Commit();
            }
            catch (Exception e)
            {
                _errors.Add(String.Format("Ocorreu uma falha ao reativar o Talento"));
                Rollback();
            }
            return _errors;
        }

        public IEnumerable<TalentVM> Get(Expression<Func<TalentVM, bool>> filter = null, Expression<Func<IQueryable<TalentVM>, IOrderedQueryable<TalentVM>>> orderBy = null, string includeProperties = "")
        {
            var filterNew = filter != null ? _mapper.MapExpression<Expression<Func<TalentVM, bool>>, Expression<Func<Talent, bool>>>(filter) : null;
            var orderByNew = orderBy != null ? _mapper.MapExpression<Expression<Func<IQueryable<TalentVM>, IOrderedQueryable<TalentVM>>>
                , Expression<Func<IQueryable<Talent>, IOrderedQueryable<Talent>>>>(orderBy) : null;
            return _mapper.Map<IEnumerable<Talent>, IEnumerable<TalentVM>>(_talentService.Get(filterNew, orderByNew, includeProperties));
        }

        public IEnumerable<TalentVM> GetAll()
        {
            return _mapper.Map<IEnumerable<Talent>, IEnumerable<TalentVM>>(_talentService.GetAll());
        }

        public TalentVM GetById(int id)
        {
            return _mapper.Map<Talent, TalentVM>(_talentService.GetById(id));
        }

        public TalentDetailsVM GetDetailsById(int id)
        {
            var talent = _talentService.Get(t => t.Id == id, null, "UserWhoUpdated").FirstOrDefault();
            TalentDetailsVM talentDetails = _mapper.Map<Talent, TalentDetailsVM>(talent);
            talentDetails.UpdatedBy = talent.UserWhoUpdated.Name;
            return talentDetails;
        }

        public List<string> Insert(TalentVM obj)
        {
            try
            {
                Talent talent = _mapper.Map<TalentVM, Talent>(obj);
                talent.Active = ((int)GenericStatusEnum.Active).ToString();
                if (!IsFileValid(obj.Resume))
                    _errors.Add("O Arquivo de Currículo deve esta no formato PDF, DOC ou DOCX");
                _errors = _talentService.Validate(talent);
                if (_errors?.Count == 0)
                {
                    talent.ResumeFileName = obj.Resume.FileName;
                    AddOrUpdateNewFile(obj.Resume, talent);
                    talent.CreatedAt = DateTime.Now;
                    talent.UpdatedAt = DateTime.Now;
                    BeginTransaction();
                    _talentService.Insert(talent);
                    SaveChanges();
                    Commit();
                }
            }
            catch (Exception e)
            {
                _errors.Add("Erro: " + e.Message.ToString() + "\nInner Exception: " + e.InnerException.Message.ToString());
                Rollback();
            }
            return _errors;
        }

        private bool IsFileValid(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                return fileExtension == ".pdf" || fileExtension == ".doc" || fileExtension == ".docx";
            }
            return false;
        }
        public List<string> Update(TalentVM talentEdited)
        {
            try
            {
                bool fileHaschanged = false;
                Talent talent = _mapper.Map<TalentVM, Talent>(talentEdited);
                talent.Active = ((int)GenericStatusEnum.Active).ToString();
                _errors = _talentService.Validate(talent);
                if (_errors?.Count == 0)
                {
                    if (talentEdited.Resume != null && talentEdited.Resume.ContentLength > 0)
                    {
                        if (!IsFileValid(talentEdited.Resume))
                        {
                            _errors.Add("O Arquivo de Currículo deve estar no formato PDF, DOC ou DOCX");
                        }
                        else
                        {
                            AddOrUpdateNewFile(talentEdited.Resume, talent);
                            fileHaschanged = true;
                        }
                    }
                    if (_errors?.Count == 0)
                    {
                        BeginTransaction();
                        var talentToBeEdited = _talentService.GetByIdNoTracking(talentEdited.Id);
                        talentToBeEdited.Email = talent.Email;
                        talentToBeEdited.Cpf = talent.Cpf;
                        talentToBeEdited.FullName = talent.FullName;
                        talentToBeEdited.UpdatedAt = DateTime.Now;
                        talentToBeEdited.UpdatedBy = talent.UpdatedBy;
                        if (fileHaschanged)
                        {
                            talentToBeEdited.ResumeFileName = talentEdited.Resume.FileName;
                            talentToBeEdited.ResumeFileData = talent.ResumeFileData;
                        }
                        _talentService.Update(talentToBeEdited);
                        SaveChanges();
                        Commit();
                    }
                }
            }
            catch (Exception e)
            {
                _errors.Add("Erro: " + e.Message.ToString() + "\nInner Exception: " + e.InnerException.Message.ToString());
                Rollback();
            }
            return _errors;
        }

        private void AddOrUpdateNewFile(HttpPostedFileBase file, Talent talent)
        {
            talent.ResumeFileData = new byte[file.ContentLength];
            file.InputStream.Read(talent.ResumeFileData, 0, file.ContentLength);
        }
    }
}

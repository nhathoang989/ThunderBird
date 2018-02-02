using CockFighting.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using CockFighting.Repositories;
using CockFighting.Interfaces;

namespace CockFighting.ViewModels
{

    public class SWUserViewModel<TView> : SWViewModelBase<CockFightingEntities, User, TView>
       where TView : ExpandViewModelBase<CockFightingEntities, User>
    {
        public SWUserViewModel(TView view) : base(view)
        {
        }
        public SWUserViewModel(User model) : base(model)
        {

        }
    }

    public class DerbyRegisterViewModel : ExpandViewModelBase<CockFightingEntities, User>
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Address { get; set; }
        public System.DateTime CreatedDate { get; set; }

        //View
        public virtual List<TeamViewModel> Teams { get; set; }
        public TeamViewModel ActivedTeam { get; set; }

        public override void ExpandModel(User model)
        {
            if (Id == 0)
            {
                CreatedDate = DateTime.UtcNow;
                ActivedTeam = new TeamViewModel()
                {
                    Cocks = new List<CockViewModel>(4)
                };
            }
        }

        public override void ExpandView(CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {
            if (Id > 0)
            {
                Teams = SWTeamRepository<TeamViewModel>.Instance.GetModelListBy(t => t.UserId == Id, _context, _transaction);
            }
        }

        public override bool SaveModel(bool isSaveSubModels = false, CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {
            var saveResult = SWUserRepository<DerbyRegisterViewModel>.Instance.SaveModel(this, isSaveSubModels, _context, _transaction);
            return saveResult.IsSucceed;
        }

        public override bool SaveSubModels(User parent, CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {
            ActivedTeam.UserId = parent.Id;
            var saveResult = SWTeamRepository<TeamViewModel>.Instance.SaveModel(ActivedTeam, true, _context, _transaction);
            return saveResult.IsSucceed;
        }
    }

}
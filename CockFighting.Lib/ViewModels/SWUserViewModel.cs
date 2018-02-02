using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using CockFighting.Repositories;
using CockFighting.Interfaces;
using CockFighting.Lib.Models;
using Newtonsoft.Json;
using System.Linq;
using AutoMapper;

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

        public override IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, DerbyRegisterViewModel>().ReverseMap()
                .ForMember(src => src.Teams, opt => opt.Ignore());
                cfg.CreateMap<User, UserViewModel>().ReverseMap();
            }
            );

            
            var mapper = new Mapper(config);
            return mapper;
        }
        public override User ParseModel()
        {
            Mapper = CreateMapper();
            return base.ParseModel();
        }
    }

    public class DerbyRegisterViewModel : ExpandViewModelBase<CockFightingEntities, User>
    {
        [Key]
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("createdDate")]
        public System.DateTime CreatedDate { get; set; }

        //View
        [JsonProperty("teams")]
        public virtual List<TeamViewModel> Teams { get; set; }
        [JsonProperty("team")]
        public TeamViewModel Team { get; set; }

        public override void ExpandModel(User model)
        {
            if (!string.IsNullOrEmpty(Phone))
            {
                CreatedDate = DateTime.UtcNow;
               
            }
        }

        public override void ExpandView(CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Phone))
            {
                CreatedDate = DateTime.UtcNow;
                if (Team==null)
                {
                    Team = new TeamViewModel()
                    {
                        Cocks = new List<CockViewModel>()
                    };
                    for (int i = 0; i < 4; i++)
                    {
                        CockViewModel cock = new CockViewModel();
                        Team.Cocks.Add(cock);
                    }
                }                
            }
            else {
                Teams = SWTeamRepository<TeamViewModel>.Instance.GetModelListBy(t => t.UserPhone == Phone, _context, _transaction);
                Team = Teams.FirstOrDefault();
            }
        }

        public override bool SaveModel(bool isSaveSubModels = false, CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {
            var saveResult = SWUserRepository<DerbyRegisterViewModel>.Instance.SaveModel(this, isSaveSubModels, _context, _transaction);
            return saveResult.IsSucceed;
        }

        public override bool SaveSubModels(User parent, CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {
            bool result = true;
            foreach (var team in Teams)
            {
                team.UserPhone = parent.Phone;
                team.UserName = parent.UserName;
                if (team.Id == 0)
                {
                    team.CreatedDate = DateTime.UtcNow;
                }
                SWTeamViewModel<TeamViewModel> vmTeam = new SWTeamViewModel<TeamViewModel>(team);
                var saveResult = vmTeam.SaveModel(true, _context, _transaction);
                result = result && saveResult.IsSucceed;
            }
            return result;
        }
    }

    public class UserViewModel: ExpandViewModelBase<CockFightingEntities, User>
    {
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
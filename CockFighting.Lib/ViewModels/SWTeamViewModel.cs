using System.Collections.Generic;
using System.Data.Entity;
using CockFighting.Repositories;
using CockFighting.Interfaces;
using CockFighting.Lib.Models;
using System;
using AutoMapper;
using Newtonsoft.Json;

namespace CockFighting.ViewModels
{

    public class SWTeamViewModel<TView> : SWViewModelBase<CockFightingEntities, Team, TView>
       where TView : ExpandViewModelBase<CockFightingEntities, Team>
    {
        public SWTeamViewModel(TView view) : base(view)
        {
        }
        public SWTeamViewModel(Team model) : base(model)
        {
        }
        
        public override IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Team, TeamViewModel>()
            .ReverseMap()
            .ForMember(src => src.Cocks, opt => opt.Ignore())
            );

            var mapper = new Mapper(config);
            return mapper;
        }
        public override Team ParseModel()
        {
            Mapper = CreateMapper();
            return base.ParseModel();
        }
    }
    public class TeamViewModel : ExpandViewModelBase<CockFightingEntities, Team>
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("createdDate")]
        public System.DateTime CreatedDate { get; set; }
        [JsonProperty("userPhone")]
        public string UserPhone { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
        //View
        [JsonProperty("cocks")]
        public virtual List<CockViewModel> Cocks { get; set; }

        public override void ExpandModel(Team model)
        {
            if (Id==0)
            {
                model.CreatedDate = DateTime.UtcNow;
            }
        }
        public override void ExpandView(CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {
            if (Id > 0)
            {
                Cocks = SWCockRepository<CockViewModel>.Instance.GetModelListBy(c => c.TeamId == Id, _context, _transaction);
            }
            else
            {
                CreatedDate = DateTime.UtcNow;
            }
        }

        public override bool SaveModel(bool isSaveSubModels = false, CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {
            var saveResult = SWTeamRepository<TeamViewModel>.Instance.SaveModel(this, isSaveSubModels, _context, _transaction);
            return saveResult.IsSucceed;
        }

        public override bool SaveSubModels(Team parent, CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {
            bool result = true;
            foreach (var cock in Cocks)
            {
                cock.TeamId = parent.Id;
                cock.UserPhone = parent.UserPhone;
                cock.TeamName = parent.Name;
                cock.UserName = parent.UserName;
                SWCockViewModel<CockViewModel> vmCock = new SWCockViewModel<CockViewModel>(cock);
                var saveResult = vmCock.SaveModel(false, _context, _transaction);
                result = result && saveResult.IsSucceed;
            }
            return result;
        }
    }


}
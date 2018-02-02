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

    public class SWMatchViewModel<TView> : SWViewModelBase<CockFightingEntities, Match, TView>
       where TView : ExpandViewModelBase<CockFightingEntities, Match>
    {
        public SWMatchViewModel(TView view) : base(view)
        {
        }
        public SWMatchViewModel(Match model) : base(model)
        {
        }
        public override IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Match, MatchViewModel>()
            .ReverseMap()
            .ForMember(src => src.Cock, opt => opt.Ignore())
            .ForMember(src => src.Cock1, opt => opt.Ignore())
            );

            var mapper = new Mapper(config);
            return mapper;
        }
        public override Match ParseModel()
        {
            Mapper = CreateMapper();
            return base.ParseModel();
        }
    }
    public class MatchViewModel : ExpandViewModelBase<CockFightingEntities, Match>
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("cockId1")]
        public int CockId1 { get; set; }
        [JsonProperty("cockId2")]
        public Nullable<int> CockId2 { get; set; }
        [JsonProperty("winnerId")]
        public Nullable<int> WinnerId { get; set; }
        [JsonProperty("createdDate")]
        public System.DateTime CreatedDate { get; set; }
        
        //View
        [JsonProperty("cock1")]
        public virtual CockViewModel Cock1 { get; set; }
        [JsonProperty("cock2")]
        public virtual CockViewModel Cock2 { get; set; }
        
        public override void ExpandModel(Match model)
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
                Cock1 = SWCockRepository<CockViewModel>.Instance.GetSingleModel(c => c.Id == CockId1, _context, _transaction);
                Cock2 = SWCockRepository<CockViewModel>.Instance.GetSingleModel(c => c.Id == CockId2, _context, _transaction);
                Cock2 = Cock2 ?? new CockViewModel();
            }
            else
            {
                CreatedDate = DateTime.UtcNow;
            }
        }

        public override bool SaveModel(bool isSaveSubModels = false, CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {
            var saveResult = SWMatchRepository<MatchViewModel>.Instance.SaveModel(this, isSaveSubModels, _context, _transaction);
            return saveResult.IsSucceed;
        }

    }

}
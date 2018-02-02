using System;
using System.Data.Entity;
using CockFighting.Repositories;
using CockFighting.Interfaces;
using CockFighting.Lib.Models;
using Newtonsoft.Json;

namespace CockFighting.ViewModels
{

    public class SWCockViewModel<TView> : SWViewModelBase<CockFightingEntities, Cock, TView>
       where TView : ExpandViewModelBase<CockFightingEntities, Cock>
    {
        public SWCockViewModel(TView view) : base(view)
        {
        }
        public SWCockViewModel(Cock model) : base(model)
        {

        }


    }

    public class CockViewModel : ExpandViewModelBase<CockFightingEntities, Cock>
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("weight")]
        public double Weight { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("teamId")]
        public Nullable<int> TeamId { get; set; }
        [JsonProperty("userPhone")]
        public string UserPhone { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("teamName")]
        public string TeamName { get; set; }
        //View
        [JsonProperty("user")]
        public UserViewModel User { get; set; }


        public override void ExpandView(CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {
            if (!string.IsNullOrEmpty(UserPhone))
            {
                User = SWUserRepository<UserViewModel>.Instance.GetSingleModel(u => u.Phone == UserPhone, _context, _transaction);
            }            
        }

        public override bool SaveModel(bool isSaveSubModels = false, CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {            
            var saveResult = SWCockRepository<CockViewModel>.Instance.SaveModel(this, false, _context, _transaction);
            return saveResult.IsSucceed;
        }
    }

}
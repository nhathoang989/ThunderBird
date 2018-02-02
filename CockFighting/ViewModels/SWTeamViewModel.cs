using CockFighting.Models;
using System.Collections.Generic;
using System.Data.Entity;
using CockFighting.Repositories;
using CockFighting.Interfaces;

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
    }
    public class TeamViewModel : ExpandViewModelBase<CockFightingEntities, Team>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int UserId { get; set; }

        //View
        public virtual List<CockViewModel> Cocks { get; set; }

        public override void ExpandView(CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {
            if (Id > 0)
            {
                Cocks = SWCockRepository<CockViewModel>.Instance.GetModelListBy(c => c.TeamId == Id, _context, _transaction);
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
                var saveResult = SWCockRepository<CockViewModel>.Instance.SaveModel(cock, true, _context, _transaction);
                result = result && saveResult.IsSucceed;
            }
            return result;
        }
    }


}
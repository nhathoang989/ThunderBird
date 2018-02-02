using CockFighting.Models;
using System;
using System.Data.Entity;
using CockFighting.Repositories;
using CockFighting.Interfaces;

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
        public int Id { get; set; }
        public string Code { get; set; }
        public Nullable<double> Weight { get; set; }
        public string Name { get; set; }
        public Nullable<int> TeamId { get; set; }

        //View

        public override bool SaveModel(bool isSaveSubModels = false, CockFightingEntities _context = null, DbContextTransaction _transaction = null)
        {            
            var saveResult = SWCockRepository<CockViewModel>.Instance.SaveModel(this, false, _context, _transaction);
            return saveResult.IsSucceed;
        }
    }

}
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CockFighting.Interfaces
{
    /// <summary>
    /// Implement sth to expend the model to View
    /// </summary>
    //public interface IExpandViewModel<TDbContext, TModel>
    //where TDbContext : DbContext
    //where TModel : class
    //{
    //    void ExpandView();
    //    void ExpandModel(TModel model);
    //    bool Validate(out List<string> errors);
    //    bool SaveModel(TModel model, TDbContext _context = null, DbContextTransaction _transaction = null);
    //}
    public abstract class ExpandViewModelBase<TDbContext, TModel>
   where TDbContext : DbContext
   where TModel : class
    {

        [JsonIgnore]
        public TModel Model { get; set; }
        //protected SWBaseModelRepository<TDbContext, TModel> _repo = new SWBaseModelRepository<TDbContext, TModel>();

        [JsonIgnore]
        public string ResponseKey { get; set; }

        [JsonIgnore]
        public List<string> errors = new List<string>();
        [JsonIgnore]
        public bool IsValid = true;
        public virtual void ExpandView(TDbContext _context = null, DbContextTransaction _transaction = null) { }
        public virtual void ExpandModel(TModel model) { }
        public virtual void Validate()
        {
        }

        public virtual Task<bool> RemoveModelAsync(TModel model, TDbContext _context, DbContextTransaction _transaction)
        {
            return default(Task<bool>);
        }

        public virtual Task<bool> SaveModelAsync(bool isSaveSubModels = false, TDbContext _context = null, DbContextTransaction _transaction = null)
        {
            return default(Task<bool>);
        }

        public virtual Task<bool> SaveSubModelsAsync(TModel parent, TDbContext _context = null, DbContextTransaction _transaction = null)
        {
            return default(Task<bool>);
        }

        public virtual bool SaveModel(bool isSaveSubModels = false, TDbContext _context = null, DbContextTransaction _transaction = null)
        {
            return false;
        }
        public virtual bool SaveSubModels(TModel parent, TDbContext _context = null, DbContextTransaction _transaction = null)
        {
            return false;
        }
    }
}

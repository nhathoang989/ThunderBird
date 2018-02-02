using System.Collections.Generic;
using System.Web.Http;
using CockFighting.ViewModels;
using CockFighting.Repositories;
//using System.Web.Http.Cors;
using System;

namespace CockFighting.OnePage.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TeamController : BaseApiController
    {
        // GET: api/Team
        public ApiResult<List<TeamViewModel>> Get()
        {
            var data =SWTeamRepository<TeamViewModel>.Instance.GetModelList();

            return GetResult(1, data);
        }

        // GET: api/Team/5
        public ApiResult<TeamViewModel> Get(int id)
        {
            var data =SWTeamRepository<TeamViewModel>.Instance.GetSingleModel(m=> m.Id == id);
            return GetResult(1, data); ;
        }

        // POST: api/Team
        public ApiResult<TeamViewModel> Post([FromBody]TeamViewModel view)
        {
           SWTeamViewModel<TeamViewModel> register = new SWTeamViewModel<TeamViewModel>(view);
            var saveResult = register.SaveModel(true);
            return GetResult(1, saveResult.Data);
        }

        // PUT: api/Team/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Team/5
        [HttpGet]
        [Route("api/Team/Delete/{id}")]
        public ApiResult<bool> Delete(int id)
        {
            SWMatchRepository<MatchViewModel>.Instance.RemoveListModel(m => m.CreatedDate < DateTime.UtcNow);
            var result =SWTeamRepository<TeamViewModel>.Instance.RemoveModel(m => m.Id == id);
            return GetResult(1, result.IsSucceed);
        }

    }
}

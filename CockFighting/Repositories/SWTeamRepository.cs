using CockFighting.Interfaces;
using CockFighting.Models;
using CockFighting.ViewModels;
using System;

namespace CockFighting.Repositories
{
    public class SWTeamRepository<TView> : SWBaseRepository<Team, SWTeamViewModel<TView>, TView, CockFightingEntities>
        where TView : ExpandViewModelBase<CockFightingEntities, Team>
    {
        private static volatile SWTeamRepository<TView> instance;
        private static object syncRoot = new Object();

        private SWTeamRepository() { }

        public static SWTeamRepository<TView> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SWTeamRepository<TView>();
                    }
                }

                return instance;
            }
        }

    }
}
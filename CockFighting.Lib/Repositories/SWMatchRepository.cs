using CockFighting.Interfaces;
using CockFighting.ViewModels;
using System;
using CockFighting.Lib.Models;

namespace CockFighting.Repositories
{
    public class SWMatchRepository<TView> : SWBaseRepository<Match, SWMatchViewModel<TView>, TView, CockFightingEntities>
        where TView : ExpandViewModelBase<CockFightingEntities, Match>
    {
        private static volatile SWMatchRepository<TView> instance;
        private static object syncRoot = new Object();

        private SWMatchRepository() { }

        public static SWMatchRepository<TView> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SWMatchRepository<TView>();
                    }
                }

                return instance;
            }
        }


    }
}
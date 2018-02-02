using CockFighting.Interfaces;
using CockFighting.Models;
using CockFighting.ViewModels;
using System;

namespace CockFighting.Repositories
{
    public class SWCockRepository<TView> : SWBaseRepository<Cock, SWCockViewModel<TView>, TView, CockFightingEntities>
        where TView : ExpandViewModelBase<CockFightingEntities, Cock>
    {
        private static volatile SWCockRepository<TView> instance;
        private static object syncRoot = new Object();

        private SWCockRepository() { }

        public static SWCockRepository<TView> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SWCockRepository<TView>();
                    }
                }

                return instance;
            }
        }

    }
}
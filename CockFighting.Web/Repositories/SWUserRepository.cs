using CockFighting.Interfaces;
using CockFighting.ViewModels;
using CockFighting.Web.Models;
using System;

namespace CockFighting.Repositories
{
    public class SWUserRepository<TView> : SWBaseRepository<User, SWUserViewModel<TView>, TView, CockFightingEntities>
        where TView : ExpandViewModelBase<CockFightingEntities, User>
    {
        private static volatile SWUserRepository<TView> instance;
        private static object syncRoot = new Object();

        private SWUserRepository() { }

        public static SWUserRepository<TView> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SWUserRepository<TView>();
                    }
                }

                return instance;
            }
        }

    }
}
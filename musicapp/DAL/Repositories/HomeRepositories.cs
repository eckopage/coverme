using musicapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicapp.DAL.Repositories
{
    public class HomeRepositories
    {
        private DatabaseContext _context;

        public HomeRepositories(DatabaseContext context)
        {
            this._context = context;
        }

        //public IQueryable<VideoTable> SearchCover(string phraze) 
        //{
        //    IQueryable<VideoTable> query;
        //    if (!String.IsNullOrEmpty(phraze))
        //    {
        //         query = from cov in _context.VideoTables
        //                 where (cov.musicFileName.Contains(phraze))
        //                 select cov;
        //    }
        //}
    }
}
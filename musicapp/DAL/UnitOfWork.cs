using musicapp.DAL.Interfaces;
using musicapp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace musicapp.DAL
{
    public class UnitOfWork: IDisposable
    {
        private DatabaseContext context = new DatabaseContext();
        private IUsersRepository usersRepository;
        private IUsersRolesRepository usersRolesRepository;
        private IVideoRepository videoRepository; 

        public IVideoRepository VideoRepository 
        {
            get 
            {
                if (this.videoRepository == null)
                {
                    this.videoRepository = new VideoRepository(context);
                }
                return videoRepository;
            }
        }

        public IUsersRepository UsersRepository
        {
            get
            {
                if (this.usersRepository == null)
                {
                    this.usersRepository = new UsersRepository(context);
                }
                return usersRepository;
            }
        }


        public IUsersRolesRepository UsersRolesRepository
        {
            get
            {
                if (this.usersRolesRepository == null)
                {
                    this.usersRolesRepository = new UsersRolesRepository(context);
                }
                return usersRolesRepository;
            }
        }



        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
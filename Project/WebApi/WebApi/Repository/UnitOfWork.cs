using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class UnitOfWork : IDisposable
    {
        //private DataContext context = new DataContext();

        private GenericRepository<Airline> airlineRepository;
        private GenericRepository<Person> authenticationRepository;

        public GenericRepository<Airline> DepartmentRepository
        {
            get
            {

                if (this.airlineRepository == null)
                {
                    //this.airlineRepository = new GenericRepository<Airline>(context);
                }
                return airlineRepository;
            }
        }

        public GenericRepository<Person> CourseRepository
        {
            get
            {

                if (this.authenticationRepository == null)
                {
                    //this.authenticationRepository = new GenericRepository<Person>(context);
                }
                return authenticationRepository;
            }
        }

        public void Save()
        {
            //context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //context.Dispose();
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

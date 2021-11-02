using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Salvo.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        //Necesito mi contexto para que funcione, ya que en el contexto realizo las acciones
        //que van a permitir realizar esas operaciones
        protected SalvoContext RepositoryContext { get; set; }

        //Constructor publico y es de la clase repositorybase
            public RepositoryBase(SalvoContext repositoryContext)
            {
                this.RepositoryContext = repositoryContext;
            }
        //Metodos
        //Metodo FINDALL()
            public IQueryable<T> FindAll()
            {
                return this.RepositoryContext.Set<T>().AsNoTracking();
            }
        //Metodo FINDALL()
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
            {
                return this.RepositoryContext.Set<T>().Where(expression).AsNoTracking();
            }
        //Metodo create
            public void Create(T entity)
            {
                this.RepositoryContext.Set<T>().Add(entity);
            }
        //Metodo Update
            public void Update(T entity)
            {
                this.RepositoryContext.Set<T>().Update(entity);
            }
        //Metodo Delete
            public void Delete(T entity)
            {
                this.RepositoryContext.Set<T>().Remove(entity);
            }

        public IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            IQueryable<T> queryable=this.RepositoryContext.Set<T>();
            if (includes != null) 
            {
                queryable = includes(queryable);
            }
            return queryable.AsNoTracking();
        }
    }
}


using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Salvo.Repositories
{
    public interface IRepositoryBase<T>
    {
        //5 metodos comunes:
        
        //Entrega todos los registros de la clase T
        IQueryable<T> FindAll();
        IQueryable<T> FindAll(Func<IQueryable<T> ,IIncludableQueryable<T,object>> includes = null);
        //Entrega todos los registros de la clase T que cumpla la expresion
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        //Crea un registro de esta clase T
        void Create(T entity);
        //Actualiza un registro de esta clase T
        void Update(T entity);
        //Borra un registro de esta clase T
        void Delete(T entity);
    }
}

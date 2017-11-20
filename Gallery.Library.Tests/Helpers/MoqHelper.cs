using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Library.Helpers
{
    class MoqHelper
    {
        internal static Mock<DbSet<T>> MockDbSet<T>(IQueryable<T> queryable) where T : class
        {
            Mock<DbSet<T>> mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            return mockDbSet;
        }
        
        internal static Mock<DbSet<TEntity>> MockDbSet<TEntity>(ICollection<TEntity> collection) where TEntity : class
        {
            //Generic placeholder for an entity
            TEntity entity = It.IsAny<TEntity>();

            //Mocked ICollection methods
            Mock<DbSet<TEntity>> mockDbSet = new Mock<DbSet<TEntity>>();
            mockDbSet.Setup(x => x.Add(entity)).Callback<TEntity>(x => collection.Add(x));
            mockDbSet.Setup(x => x.Remove(entity)).Callback<TEntity>(x => collection.Remove(x));

            //Mocked IQueryable implementation
            IQueryable<TEntity> queryable = collection.AsQueryable();
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return mockDbSet;
        }
    }
}

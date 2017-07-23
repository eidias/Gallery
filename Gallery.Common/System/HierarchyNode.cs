using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gallery.Common.System
{
    // Stefan Cruysberghs, July 2008, http://www.scip.be
    /// <summary>
    /// Hierarchy node class which contains a nested collection of hierarchy nodes
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public class HierarchyNode<T> where T : class
    {
        public T Entity { get; set; }
        public IEnumerable<HierarchyNode<T>> ChildNodes { get; set; }
        public int Depth { get; set; }
        public T Parent { get; set; }
        public double? Sum { get; set; }
        public int? Count { get; set; }
    }
    /// AsHierarchy extension methods for LINQ to Objects IEnumerable
    /// </summary>
    public static class LinqToObjectsExtensionMethods
    {
        private static int intCount;
        private static double? dblSum;

        private static IEnumerable<HierarchyNode<TEntity>>
          CreateHierarchy<TEntity, TProperty>(
            IEnumerable<TEntity> allItems,
            TEntity parentItem,
            Func<TEntity, TProperty> idProperty,
            Func<TEntity, TProperty> parentIdProperty,
            Func<TEntity, double?> sumProperty,
            object rootItemId,
            int maxDepth,
            int depth) where TEntity : class
        {
            IEnumerable<TEntity> childs;

            if (rootItemId != null)
            {
                childs = allItems.Where(i => idProperty(i).Equals(rootItemId));
            }
            else
            {
                if (parentItem == null)
                {
                    childs = allItems.Where(i => parentIdProperty(i).Equals(default(TProperty)));
                }
                else
                {
                    childs = allItems.Where(i => parentIdProperty(i).Equals(idProperty(parentItem)));
                }
            }

            if (childs.Count() > 0)
            {
                depth++;
                dblSum = 0;
                intCount = 0;
                if ((depth <= maxDepth) || (maxDepth == 0))
                {
                    foreach (var item in childs)
                    {
                        var lItems = CreateHierarchy(allItems, item, idProperty, parentIdProperty, sumProperty, null, maxDepth, depth);
                        HierarchyNode<TEntity> lNode = new HierarchyNode<TEntity>()
                        {
                            Entity = item,
                            ChildNodes = lItems,
                            Depth = depth,
                            Parent = parentItem
                        };
                        double? dblValue = null;
                        if (sumProperty != null)
                        {
                            if (sumProperty(item).HasValue)
                            {
                                dblValue = sumProperty(item).Value;
                                dblSum += dblValue;
                            }
                        }
                        intCount++;
                        if (lNode.ChildNodes.Count() > 0)
                        {
                            lNode.Sum = dblSum;
                            lNode.Count = intCount;
                        }
                        else
                        {
                            lNode.Sum = dblValue;
                        }
                        yield return lNode;
                    }
                }
            }
        }

        /// <summary>
        /// LINQ to Objects (IEnumerable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <typeparam name="TProperty">Property of entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="idProperty">Func delegete to Id/Key of entity</param>
        /// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
          this IEnumerable<TEntity> allItems,
          Func<TEntity, TProperty> idProperty,
          Func<TEntity, TProperty> parentIdProperty,
          Func<TEntity, double?> sumProperty) where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, sumProperty, null, 0, 0);
        }

        /// <summary>
        /// LINQ to Objects (IEnumerable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <typeparam name="TProperty">Property of entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="idProperty">Func delegete to Id/Key of entity</param>
        /// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
        /// <param name="rootItemId">Value of root item Id/Key</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
          this IEnumerable<TEntity> allItems,
          Func<TEntity, TProperty> idProperty,
          Func<TEntity, TProperty> parentIdProperty,
          Func<TEntity, double?> sumProperty,
          object rootItemId) where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, sumProperty, rootItemId, 0, 0);
        }

        /// <summary>
        /// LINQ to Objects (IEnumerable) AsHierachy() extension method
        /// </summary>
        /// <typeparam name="TEntity">Entity class</typeparam>
        /// <typeparam name="TProperty">Property of entity class</typeparam>
        /// <param name="allItems">Flat collection of entities</param>
        /// <param name="idProperty">Func delegete to Id/Key of entity</param>
        /// <param name="parentIdProperty">Func delegete to parent Id/Key</param>
        /// <param name="rootItemId">Value of root item Id/Key</param>
        /// <param name="maxDepth">Maximum depth of tree</param>
        /// <returns>Hierarchical structure of entities</returns>
        public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TProperty>(
          this IEnumerable<TEntity> allItems,
          Func<TEntity, TProperty> idProperty,
          Func<TEntity, TProperty> parentIdProperty,
          Func<TEntity, double?> sumProperty,
          object rootItemId,
          int maxDepth) where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, sumProperty, rootItemId, maxDepth, 0);
        }
    }
}
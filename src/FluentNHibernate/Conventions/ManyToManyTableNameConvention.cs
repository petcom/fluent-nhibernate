using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions
{
    public abstract class ManyToManyTableNameConvention : IHasManyToManyConvention
    {
        public void Apply(IManyToManyCollectionInstance instance)
        {
            if (instance.OtherSide == null)
            {
                // uni-directional
                var tableName = GetUniDirectionalTableName(instance);

                instance.Table(tableName);
            }
            else
            {
                // bi-directional
                if (instance.HasExplicitTable && instance.OtherSide.HasExplicitTable)
                {
                    // TODO: We could check if they're the same here and warn the user if they're not
                    return;
                }

                if (instance.HasExplicitTable && !instance.OtherSide.HasExplicitTable)
                    instance.OtherSide.Table(instance.TableName);
                else if (!instance.HasExplicitTable && instance.OtherSide.HasExplicitTable)
                    instance.Table(instance.OtherSide.TableName);
                else
                {
                    var tableName = GetBiDirectionalTableName(instance, instance.OtherSide);

                    instance.Table(tableName);
                    instance.OtherSide.Table(tableName);
                }
            }
        }

        protected abstract string GetBiDirectionalTableName(IManyToManyCollectionInstance collection, IManyToManyCollectionInstance otherSide);
        protected abstract string GetUniDirectionalTableName(IManyToManyCollectionInstance collection);
    }
}
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Testing.AutoMap.ManyToMany;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class AutoMapTests : BaseAutoMapFixture
    {
        [Test]
        public void AutoMapIdentification()
        {
            Model<ExampleClass>(model =>
                model.Where(type => type == typeof(ExampleClass)));

            Test<ExampleClass>(mapping =>
                mapping.Element("//id")
                    .HasAttribute("column", "Id")
                    .HasAttribute("name", "Id"));
        }

        [Test]
        public void AutoMapVersion()
        {
            Model<ExampleClass>(model =>
                model.Where(type => type == typeof(ExampleClass)));

            Test<ExampleClass>(mapping =>
                mapping.Element("//version")
                    .HasAttribute("column", "Timestamp")
                    .HasAttribute("name", "Timestamp"));
        }

        [Test]
        public void AutoMapProperty()
        {
            Model<ExampleClass>(model =>
                model.Where(type => type == typeof(ExampleClass)));

            Test<ExampleClass>(mapping =>
                mapping.Element("//property[@name='LineOne']/column").HasAttribute("name", "LineOne"));
        }

        [Test]
        public void AutoMapIgnoreProperty()
        {
            Model<ExampleClass>(model => model
                .Where(type => type == typeof(ExampleClass))
                .ForTypesThatDeriveFrom<ExampleClass>(m => m.IgnoreProperty(x => x.LineOne)));

            Test<ExampleClass>(mapping =>
                mapping.Element("//property[@name='LineOne']").DoesntExist());
        }

        [Test]
        public void ShouldAutoMapEnums()
        {
            Model<ExampleClass>(model => model
                .ForTypesThatDeriveFrom<ExampleClass>(mapping =>
                    mapping.Map(x => x.Enum).SetAttribute("type", "Int32"))
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures"));

            Test<ExampleClass>(mapping =>
                mapping.Element("//property[@name='Enum']").Exists());
        }

        [Test]
        public void AutoMapManyToOne()
        {
            Model<ExampleClass>(model =>
                model.Where(type => type == typeof(ExampleClass)));

            Test<ExampleClass>(mapping =>
                mapping.Element("//many-to-one")
                    .HasAttribute("column", "Parent_id")
                    .HasAttribute("name", "Parent"));
        }

        [Test]
        public void AutoMapManyToMany()
        {
            Model<ManyToMany1>(model =>
                model.Where(type => type == typeof(ManyToMany1)));

            Test<ManyToMany1>(mapping =>
                mapping.Element("//many-to-many")
                    .HasAttribute("column", "ManyToMany2_id"));
        }

        [Test]
        public void AutoMapManyToMany_ShouldRecognizeSet_BaseOnType()
        {
            Model<ManyToMany1>(model =>
                model.Where(type => type == typeof(ManyToMany1)));

            Test<ManyToMany1>(mapping =>
                mapping.Element("//set[@name='Many1']").Exists());
        }

        [Test]
        public void AutoMapOneToMany()
        {
            Model<ExampleParentClass>(model =>
                model.Where(type => type == typeof(ExampleParentClass)));

            Test<ExampleParentClass>(mapping =>
                mapping.Element("//bag[@name='Examples']").Exists());
        }

        [Test]
        public void AutoMapDoesntSetCacheWithDefaultConvention()
        {
            Model<ExampleClass>(model =>
                model.Where(type => type == typeof(ExampleClass)));

            Test<ExampleClass>(mapping =>
                mapping.Element("//cache").DoesntExist());
        }

        [Test]
        public void AutoMapSetsCacheOnClassUsingConvention()
        {
            Model<ExampleClass>(model => model
                .ConventionDiscovery.Add(Cache.Is(cache => cache.AsReadOnly()))
                .Where(type => type == typeof(ExampleClass)));

            Test<ExampleClass>(mapping =>
                mapping.Element("//cache").Exists());
        }

        [Test]
        public void ShouldAutoMapBitmapPropertyAsColumn()
        {
            //This is a regression test for issue 198
            Model<ClassWithBitmap>(model =>
                model.Where(type => type == typeof(ClassWithBitmap)));

            Test<ClassWithBitmap>(mapping =>
                mapping.Element("//property[@name='Bitmap']/column").HasAttribute("name", "Bitmap"));
        }
    }
}


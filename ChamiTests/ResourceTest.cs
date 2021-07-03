using System.Windows;
using System.Windows.Data;
using ChamiUI.PresentationLayer.Utils;
using Xunit;

namespace ChamiTests
{
    
    public class ResourceTest
    {
        static ResourceTest()
        {
            _resourceDictionary  = new ResourceDictionary();
            _resourceDictionary.Add("existing", "hello!");
            _resourceDictionary.Add("viewSource", new CollectionViewSource());
        }

        private static ResourceDictionary _resourceDictionary;
        [Fact]
        public void TestNotExistingResource()
        {
            var result = _resourceDictionary.TryGetResource("blah", out var obj);

            Assert.False(result);
            Assert.Null(obj);
        }

        [Fact]
        public void TestNotExistingCollectionViewSource()
        {
            var result = _resourceDictionary.TryGetCollectionViewSource("blah", out var obj);
            Assert.False(result);
            Assert.Null(obj);
        }

        [Fact]
        public void TestExistingResource()
        {
            var result = _resourceDictionary.TryGetResource("existing", out var obj);
            Assert.True(result);
            Assert.NotNull(obj);
            Assert.Equal("hello!", obj);
        }


        [Fact]
        public void TestExistingCollectionViewSource()
        {
            var result = _resourceDictionary.TryGetCollectionViewSource("viewSource", out var obj);
            Assert.True(result);
            Assert.NotNull(obj);
        }
    }
}
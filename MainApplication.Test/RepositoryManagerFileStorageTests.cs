using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageLibrary;

namespace StorageLibrary.Test
{
    public class RepositoryManagerFileStorageTests : IDisposable
    {
        private readonly string _filePath;

        public RepositoryManagerFileStorageTests()
        {
            _filePath = Path.GetTempFileName();
        }

        public void Dispose()
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }

        private RepositoryManager CreateRepoWithJsonItem(string itemName, string content = "{}")
        {
            var storage = new FileItemStorage(_filePath);
            storage.Add(new ItemString
            {
                ItemName = itemName,
                ItemContent = content,
                ItemType = ItemType.JSON
            });
            return new RepositoryManager(storage);
        }

        private RepositoryManager CreateRepoWithXmlItem(string itemName, string content)
        {
            var storage = new FileItemStorage(_filePath);
            storage.Add(new ItemString
            {
                ItemName = itemName,
                ItemContent = content,
                ItemType = ItemType.XML
            });
            return new RepositoryManager(storage);
        }

        [Fact]
        public void Register_StoresAndRetrieves_JsonItem()
        {
            var repo = new RepositoryManager(new FileItemStorage(_filePath));
            string json = "{\"name\":\"test\"}";

            repo.Register("TestJson", json, (int)ItemType.JSON);

            var result = repo.Retrieve("TestJson");
            Assert.Equal(json, result);
        }

        [Fact]
        public void Register_StoresAndRetrieves_XmlItem()
        {
            var repo = new RepositoryManager(new FileItemStorage(_filePath));
            string xml = "<?xml version=\"1.0\"?><root><data>abc</data></root>";

            repo.Register("TestXml", xml, (int)ItemType.XML);

            var result = repo.Retrieve("TestXml");
            Assert.Equal(xml, result);
        }

        [Fact]
        public void Deregister_RemovesItem()
        {
            var repo = CreateRepoWithJsonItem("ToRemove");

            repo.Deregister("ToRemove");

            Assert.Throws<KeyNotFoundException>(() => repo.Retrieve("ToRemove"));
        }

        [Fact]
        public void Register_ThrowsException_WhenItemAlreadyExists()
        {
            var repo = CreateRepoWithJsonItem("Duplicate");

            Assert.Throws<InvalidOperationException>(() =>
                repo.Register("Duplicate", "{\"name\":\"again\"}", (int)ItemType.JSON));
        }

        [Fact]
        public void GetType_ReturnsCorrectType()
        {
            var repo = CreateRepoWithXmlItem("MyXml", "<root />");

            var type = repo.GetType("MyXml");

            Assert.Equal((int)ItemType.XML, type);
        }
    }

}

using Xunit;
using StorageLibrary;
using System;
using System.Text.Json;

namespace StorageLibrary.Tests
{
    public class RepositoryManagerTests
    {
        private static RepositoryManager CreateRepoWithJsonItem(string itemName, string content = "{}")
        {
            var storage = new InMemoryStorage();
            storage.Add(new ItemString
            {
                ItemName = itemName,
                ItemContent = content,
                ItemType = ItemType.JSON
            });
            return new RepositoryManager(storage);
        }

        private static RepositoryManager CreateRepoWithXmlItem(string itemName, string content)
        {
            var storage = new InMemoryStorage();
            storage.Add(new ItemString
            {
                ItemName = itemName,
                ItemContent = content,
                ItemType = ItemType.XML
            });
            return new RepositoryManager(storage);
        }

        [Fact]
        public void DeRegister_RemovesItemCorrectly()
        {
            // Arrange
            var repo = CreateRepoWithJsonItem("TestItem");

            // Act
            repo.Deregister("TestItem");

            // Assert
            Assert.Throws<KeyNotFoundException>(() => repo.Retrieve("TestItem"));
        }

        [Fact]
        public void DeRegister_ReturnException_WhenItemNotExist()
        {
            var repo = new RepositoryManager(new InMemoryStorage());
            Assert.Throws<KeyNotFoundException>(() => repo.Deregister("NonExistingItem"));
        }

        [Fact]
        public void Register_StoresJson_WhenValidJson()
        {
            var repo = new RepositoryManager(new InMemoryStorage());
            var jsonContent = "{\"name\":\"foo\"}";

            repo.Register("TestItem", jsonContent, (int)ItemType.JSON);

            var result = repo.Retrieve("TestItem");
            Assert.Equal(jsonContent, result);
        }

        [Fact]
        public void Register_StoresXml_WhenValidXml()
        {
            var repo = new RepositoryManager(new InMemoryStorage());

            string xmlContent = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n" +
                "<root>\r\n" +
                "  <book category=\"web\" cover=\"paperback\">\r\n" +
                "    <title lang=\"en\">Learning XML</title>\r\n" +
                "    <author>Erik T. Ray</author>\r\n" +
                "    <year>2003</year>\r\n" +
                "    <price>39.95</price>\r\n" +
                "  </book>\r\n" +
                "</root>\r\n";

            repo.Register("TestItem", xmlContent, (int)ItemType.XML);

            var result = repo.Retrieve("TestItem");
            Assert.Equal(xmlContent, result);
        }

        [Fact]
        public void Register_ThrowsException_WhenItemTypeUnsupported()
        {
            var repo = new RepositoryManager(new InMemoryStorage());

            Assert.Throws<NotSupportedException>(() => repo.Register("Unknown", "data", 999));
        }

        [Fact]
        public void Register_ThrowsException_WhenContentInvalid()
        {
            var repo = new RepositoryManager(new InMemoryStorage());

            Assert.Throws<InvalidDataException>(() => repo.Register("InvalidJson", "", (int)ItemType.JSON));
            Assert.Throws<InvalidDataException>(() => repo.Register("InvalidXml", "<invalid", (int)ItemType.XML));
        }

        [Fact]
        public void Register_ThrowsException_WhenItemAlreadyExists()
        {
            var repo = CreateRepoWithJsonItem("TestItem");

            Assert.Throws<InvalidOperationException>(() =>
                repo.Register("TestItem", "{\"name\":\"again\"}", (int)ItemType.JSON));
        }

        [Fact]
        public void Retrieve_ReturnsCorrectValue_WhenExists()
        {
            var repo = CreateRepoWithJsonItem("MyData", "{\"key\":\"val\"}");

            var result = repo.Retrieve("MyData");

            Assert.Equal("{\"key\":\"val\"}", result);
        }

        [Fact]
        public void Retrieve_ThrowsException_WhenNotExists()
        {
            var repo = new RepositoryManager(new InMemoryStorage());

            Assert.Throws<KeyNotFoundException>(() => repo.Retrieve("NotFound"));
        }

        [Fact]
        public void GetType_ReturnsCorrectItemType()
        {
            var repo = CreateRepoWithJsonItem("TypeCheck");

            var result = repo.GetType("TypeCheck");

            Assert.Equal((int)ItemType.JSON, result);
        }

        [Fact]
        public void GetType_ThrowsException_WhenItemNotFound()
        {
            var repo = new RepositoryManager(new InMemoryStorage());

            Assert.Throws<KeyNotFoundException>(() => repo.GetType("GhostItem"));
        }
    }
}

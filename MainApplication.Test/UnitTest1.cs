using Xunit;
using MainApplication;
using System;
using System.Text.Json;
using System.Reflection;
using System.Collections.Concurrent;

namespace MainApplication.Tests
{
    public class RepositoryManagerTests
    {
        [Fact]
        public void DeRegister_ReturnException_WhenItemExist()
        {
            // Arrange
            var repo = new RepositoryManager();

            var itemsField = typeof(RepositoryManager).GetProperty("items", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var list = new ConcurrentDictionary<string, ItemString>();
            list.TryAdd("TestItem", new ItemString { ItemName = "TestItem", ItemType = ItemType.JSON });

            itemsField.SetValue(repo, list);

            repo.Deregister("TestItem");

            // Act + Assert
            Assert.Throws<KeyNotFoundException>(() => repo.Retrieve("TestItem"));
        }
        [Fact]
        public void DeRegister_ReturnException_ItemNotExist()
        {
            // Arrange
            var repo = new RepositoryManager();

            // Act + Assert
            Assert.Throws<KeyNotFoundException>(() => repo.Deregister("TestItem"));
        }
        [Fact]
        public void Register_ReturnsRegisteredItemValue_WhenItemNotExist_JSON()
        {
            // Arrange
            var repo = new RepositoryManager();

            var item = new ItemString
            {
                ItemName = "foo",
                ItemContent = "bar",
                ItemType = ItemType.JSON
            };

            var inJson = JsonSerializer.Serialize(item);

            repo.Register("TestItem", inJson, 1);

            // Act
            var result = repo.Retrieve("TestItem");

            // Assert
            Assert.Equal(inJson, result);
        }
        [Fact]
        public void Register_ReturnsRegisteredItemValue_WhenItemNotExist_XML()
        {
            // Arrange
            var repo = new RepositoryManager();

            string inXml = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n <root>\r\n     <book category=\"web\" cover=\"paperback\">\r\n         <title lang=\"en\">Learning XML</title>\r\n         <author>Erik T. Ray</author>\r\n         <year>2003</year>\r\n         <price>39.95</price>\r\n     </book>\r\n </root>\r\n";

            repo.Register("TestItem", inXml, 2);

            // Act
            var result = repo.Retrieve("TestItem");

            // Assert
            Assert.Equal(inXml, result);
        }
        [Fact]
        public void Register_ReturnException_WrongItemType()
        {
            // Arrange
            var repo = new RepositoryManager();

            // Act + Assert
            Assert.Throws<InvalidDataException>(() => repo.Register("TestItemNO", "", 8068));
            Assert.Throws<InvalidDataException>(() => repo.Register("TestItemJSON", "", 1));
            Assert.Throws<InvalidDataException>(() => repo.Register("TestItemXML", "", 2));
        }
        [Fact]
        public void Register_ThrowsException_WhenItemAlreadyExists()
        {
            // Arrange
            var repo = new RepositoryManager();

            // Masukkan item awal via reflection
            var itemsField = typeof(RepositoryManager).GetProperty("items", BindingFlags.NonPublic | BindingFlags.Instance);
            var initialList = new ConcurrentDictionary<string, ItemString>();
            var item = new ItemString { ItemName = "TestItem", ItemType = ItemType.JSON };
            initialList.TryAdd("TestItem", item);

            itemsField.SetValue(repo, initialList);

            // Act + Assert
            Assert.Throws<InvalidOperationException>(() => repo.Register("TestItem", JsonSerializer.Serialize(item), 1));
        }
        [Fact]
        public void GetType_ReturnsCorrectItemType_WhenItemExists()
        {
            // Arrange
            var repo = new RepositoryManager();

            var itemsField = typeof(RepositoryManager).GetProperty("items", BindingFlags.NonPublic | BindingFlags.Instance);
            var list = new ConcurrentDictionary<string, ItemString>();
            list.TryAdd("TestItem", new ItemString { ItemName = "TestItem", ItemType = ItemType.JSON });

            itemsField.SetValue(repo, list);

            // Act
            var result = repo.GetType("TestItem");

            // Assert
            Assert.Equal((int)ItemType.JSON, result);
        }
        [Fact]
        public void GetType_ReturnException_ItemDoesNotExist()
        {
            // Arrange
            var repo = new RepositoryManager();

            // Act + Assert
            Assert.Throws<KeyNotFoundException>(() => repo.GetType("NonExistentItem"));
        }
    }
}

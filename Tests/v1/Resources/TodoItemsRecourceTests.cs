using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Contract;
using TodoApi.Data;
using TodoApi.Domain;
using TodoApiDTO.Infrasctructure.CustomException;
using TodoApiDTO.v1.Converter;
using TodoApiDTO.v1.Resources;

namespace Tests.v1.Resources
{
    [TestFixture]
    public class TodoItemsRecourceTests
    {
        private static readonly Random random = new Random();

        private Mock<IItemConverter> itemConverter;
        private TodoContext context;

        private ITodoItemsRecource underTest;

        [SetUp]
        public void SetUp()
        {
            itemConverter = new Mock<IItemConverter>();
            context = WithDbContext();

            underTest = new TodoItemsRecource(context, itemConverter.Object);
        }

        [Test]
        public async Task GetAllAsync_Should_Return_Expected_Result()
        {
            //arrange
            var expectedItem = new TodoItemDTO
            {
                Id = 1,
                Name = "Test1",
                IsComplete = false
            };

            var expectedResult = new List<TodoItemDTO>
            {
                expectedItem
            };
            var expectedCount = 1;

            itemConverter.Setup(x => x.ToContract(It.IsAny<TodoItem>())).Returns(expectedItem);

            //act
            var result = await underTest.GetAllAsync();

            //assert
            Assert.AreEqual(expectedCount, result.Count());
            CollectionAssert.AreEqual(expectedResult, result);
            itemConverter.Verify(x => x.ToContract(It.IsAny<TodoItem>()), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_When_Input_Is_Invalid_Than_Should_Throw_Exception()
        {
            //arrange
            var invalidItemId = -1;

            //act assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await underTest.GetByIdAsync(invalidItemId));
        }

        [Test]
        public async Task GetByIdAsync_When_Input_Id_Is_Invalid_Than_Should_Throw_Exception()
        {
            //arrange
            var invalidItemId = 10;

            //act assert
            Assert.ThrowsAsync<ResourceNotFoundException>(async () => await underTest.GetByIdAsync(invalidItemId));
        }

        [Test]
        public async Task GetByIdAsync_Should_Return_Expected_Result()
        {
            //arrange
            var expectedItem = new TodoItemDTO
            {
                Id = 1,
                Name = "Test1",
                IsComplete = false
            };

            itemConverter.Setup(x => x.ToContract(It.IsAny<TodoItem>())).Returns(expectedItem);

            //act
            var result = await underTest.GetByIdAsync(expectedItem.Id);

            //assert
            Assert.AreEqual(expectedItem, result);
            itemConverter.Verify(x => x.ToContract(It.IsAny<TodoItem>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_When_Input_Is_Invalid_Than_Should_Throw_Exception()
        {
            //arrange
            TodoItemDTO invalidItem = null;

            //act assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await underTest.CreateAsync(invalidItem));
        }

        [Test]
        public async Task CreateAsync_Should_Return_Expected_Result()
        {
            //arrange
            var itemToCreate = new TodoItem()
            {
                Name = "Test1",
                IsComplete = false
            };

            var expectedItem = new TodoItemDTO
            {
                Id = 1,
                Name = "Test1",
                IsComplete = false
            };

            itemConverter.Setup(x => x.ToDomain(It.IsAny<TodoItemDTO>())).Returns(itemToCreate);
            itemConverter.Setup(x => x.ToContract(It.IsAny<TodoItem>())).Returns(expectedItem);

            //act
            var result = await underTest.CreateAsync(expectedItem);

            //assert
            Assert.AreEqual(expectedItem, expectedItem);
            itemConverter.Verify(x => x.ToDomain(expectedItem), Times.Once);
            itemConverter.Verify(x => x.ToContract(itemToCreate), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_When_Input_Id_Is_Invalid_Than_Should_Throw_Exception()
        {
            //arrange
            var invalidItemId = -1;
            var item = new TodoItemDTO();

            //act assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await underTest.UpdateAsync(invalidItemId, item));
        }

        [Test]
        public async Task UpdateAsync_When_Input_Item_Is_Invalid_Than_Should_Throw_Exception()
        {
            //arrange
            var itemId = 1;
            TodoItemDTO invalidItem = null;

            //act assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await underTest.UpdateAsync(itemId, invalidItem));
        }

        [Test]
        public async Task UpdateAsync_When_Entity_Not_Found_Should_Throw_Exception()
        {
            //arrange
            var itemId = 10;
            var item = new TodoItemDTO();

            //act assert
            Assert.ThrowsAsync<ResourceNotFoundException>(async () => await underTest.UpdateAsync(itemId, item));
        }

        [Test]
        public async Task UpdateAsync_Should_Return_Expected_Result()
        {
            //arrange
            var itemToCreate = new TodoItem()
            {
                Name = "Test1",
                IsComplete = false
            };

            var expectedItem = new TodoItemDTO
            {
                Id = 1,
                Name = "Test1",
                IsComplete = false
            };

            itemConverter.Setup(x => x.UpdateDomain(It.IsAny<TodoItem>(), expectedItem)).Returns(itemToCreate);
            itemConverter.Setup(x => x.ToContract(itemToCreate)).Returns(expectedItem);

            //act
            var result = await underTest.UpdateAsync(expectedItem.Id, expectedItem);

            //assert
            Assert.AreEqual(expectedItem, result);
            itemConverter.Verify(x => x.UpdateDomain(It.IsAny<TodoItem>(), expectedItem), Times.Once);
            itemConverter.Verify(x => x.ToContract(It.IsAny<TodoItem>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_When_Input_Id_Is_Invalid_Than_Should_Throw_Exception()
        {
            //arrange
            var invalidItemId = -1;

            //act assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await underTest.DeleteAsync(invalidItemId));
        }

        [Test]
        public async Task DeleteAsync_When_Entity_Not_Found_Should_Throw_Exception()
        {
            //arrange
            var itemId = 10;

            //act assert
            Assert.ThrowsAsync<ResourceNotFoundException>(async () => await underTest.DeleteAsync(itemId));
        }

        [Test]
        public async Task DeleteAsync_Should_Return_Expected_Result()
        {
            //arrange
            var itemId = 1;

            //act assert
            await underTest.DeleteAsync(itemId);
            Assert.ThrowsAsync<ResourceNotFoundException>(async () => await underTest.DeleteAsync(itemId));
        }

        private static TodoContext WithDbContext()
        {
            var context = new List<TodoItem>
            {
                new TodoItem
                {
                    Id = 1,
                    Name = "Test1",
                    IsComplete = false
                }
            }.CreateContext($"Session-{random.Next()}");
            return context;
        }
    }
}
using Castle.Core.Logging;
using CustomerMVCSite.Controllers;
using CustomerMVCSite.Models;
using CustomerMVCSite.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class CategoryControllerTest
    {
        public CategoryControllerTest()
        {
        }

        [Fact]
        public void Get_Cateories_Return_In_Array_Correct_Items()
        {
            // Arrange
            List<CategoryModel> expected = getListCategoryModels();

            var mockDatabaseService = new Mock<IDatabaseService>();

            mockDatabaseService.Setup(x => x.getAllCategoryOnly())
                .Returns(expected);

            var mockCastService = new Mock<ICastService>();

            var controller = new CategoryController(null, mockDatabaseService.Object, mockCastService.Object);

            // Act
            var actual = controller.Get();
            var actualToList = new List<CategoryModel>(actual);

            // Assert
            Assert.NotEmpty(actual);
            Assert.Equal(expected.Count, actualToList.Count);

            foreach (CategoryModel eCategory in expected)
            {
                Assert.Equal(eCategory, actualToList.Find(aCategory => aCategory.ID == eCategory.ID));
            }
        }

        [Theory]
        [InlineData(1, "A", "B")]
        [InlineData(1, null, null)]
        [InlineData(-1, null, null)]
        public void Get_Correct_CategoryModel_By_Id(int id, string name, string description)
        {
            // Arrange
            CategoryModel expected = null;

            if (id > 0)
            {
                expected = getCategoryModel(id, name, description);
            }

            var mockDatabaseService = new Mock<IDatabaseService>();

            mockDatabaseService.Setup(x => x.getCategoryByID(id))
                .Returns(expected);

            var mockCastService = new Mock<ICastService>();

            var controller = new CategoryController(null, mockDatabaseService.Object, mockCastService.Object);

            // Act
            var actual = controller.GetCategoryByCategory(id);


            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actual);
            var actualCategoryModel = Assert.IsType<CategoryModel>(okResult.Value);
            Assert.Equal(expected, actualCategoryModel);
        }

        [Fact]
        public void Get_List_Subcategory_By_Category_Id_Return_Correct_Items()
        {
            // Arrange
            var expected = GetListSubcategoryModels();
            int categoryId = 1;

            var mockDatabaseService = new Mock<IDatabaseService>();

            mockDatabaseService.Setup(x => x.getSubCategoriesByCategoryID(categoryId))
                .Returns(expected);

            var mockCastService = new Mock<ICastService>();

            var controller = new CategoryController(null, mockDatabaseService.Object, mockCastService.Object);

            // Act
            var actual = controller.GetSubcategoryByCategory(categoryId);
            var actualToList = new List<SubcategoryModel>(actual);

            // Assert
            Assert.NotEmpty(actualToList);
            Assert.Equal(expected.Count, actualToList.Count);

            foreach (SubcategoryModel eSubcategory in expected)
            {
                Assert.Equal(eSubcategory, actualToList.Find(aSubcategory => aSubcategory.ID == eSubcategory.ID));
            }
        }

        [Theory]
        [InlineData(1, "sub A", "sub B")]
        [InlineData(1, "sub A", null)]
        [InlineData(1, null, null)]
        [InlineData(-1, null, null)]
        public void Get_Correct_SubCategoryModel_By_Id(int id, string name, string description)
        {
            // Arrange
            SubcategoryModel expected = null;

            if (id > 0)
            {
                expected = getSubategoryModel(id, name, description);
            }

            var mockDatabaseService = new Mock<IDatabaseService>();

            mockDatabaseService.Setup(x => x.getSubCategoryByID(id))
                .Returns(expected);

            var mockCastService = new Mock<ICastService>();

            var controller = new CategoryController(null, mockDatabaseService.Object, mockCastService.Object);

            // Act
            var actual = controller.getSubcategoryById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actual);
            if (expected == null)
            {
                Assert.Null(okResult.Value);
            }
            else
            {
                var actualSubcategoryModel = Assert.IsType<SubcategoryModel>(okResult.Value);
                Assert.Equal(expected, actualSubcategoryModel);
            }
        }

        [Fact]
        public async Task Check_Delete_Category_Success()
        {
            // Arrange
            int categoryId = 1;
            var expected = getCategoryModel(categoryId, null, null);

            var mockDatabaseService = new Mock<IDatabaseService>();

            mockDatabaseService.Setup(x => x.deleteCategory(categoryId))
                .Returns(Task.FromResult(true));

            var mockCastService = new Mock<ICastService>();

            var controller = new CategoryController(null, mockDatabaseService.Object, mockCastService.Object);

            // Act
            var actual = await controller.Delete(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actual);
            var actualCategoryModel = Assert.IsType<CategoryModel>(okResult.Value);
            Assert.Equal(expected.ID, actualCategoryModel.ID);
        }

        [Fact]
        public async Task Check_Delete_Category_Fail()
        {
            // Arrange
            var expected = "Cannot delete Category";

            int categoryId = 1;
            var categoryModel = getCategoryModel(categoryId, null, null);

            var mockDatabaseService = new Mock<IDatabaseService>();

            mockDatabaseService.Setup(x => x.deleteCategory(categoryId))
                .Returns(Task.FromResult(false));

            var mockCastService = new Mock<ICastService>();

            var controller = new CategoryController(null, mockDatabaseService.Object, mockCastService.Object);

            // Act
            var actual = await controller.Delete(categoryId);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.Equal(expected, badRequest.Value);
        }

        [Fact]
        public async Task Check_Delete_Subcategory_Success()
        {
            // Arrange
            int subcategoryId = 1;
            var expected = getSubategoryModel(subcategoryId, null, null);

            var mockDatabaseService = new Mock<IDatabaseService>();

            mockDatabaseService.Setup(x => x.deleteSubcategory(subcategoryId))
                .Returns(Task.FromResult(true));

            var mockCastService = new Mock<ICastService>();

            var controller = new CategoryController(null, mockDatabaseService.Object, mockCastService.Object);

            // Act
            var actual = await controller.deleteSubcategoryById(subcategoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actual);
            var actualSubcategoryModel = Assert.IsType<SubcategoryModel>(okResult.Value);
            Assert.Equal(expected.ID, actualSubcategoryModel.ID);
        }

        [Fact]
        public async Task Check_Delete_Subcategory_Fail()
        {
            // Arrange
            var expected = "Cannot delete Subcategory";

            int subcategoryId = 1;
            var categoryModel = getSubategoryModel(subcategoryId, null, null);

            var mockDatabaseService = new Mock<IDatabaseService>();

            mockDatabaseService.Setup(x => x.deleteCategory(subcategoryId))
                .Returns(Task.FromResult(false));

            var mockCastService = new Mock<ICastService>();

            var controller = new CategoryController(null, mockDatabaseService.Object, mockCastService.Object);

            // Act
            var actual = await controller.deleteSubcategoryById(subcategoryId);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.Equal(expected, badRequest.Value);
        }

        [Fact]
        public async Task Check_Post_NUll_Category_Return_BadRequest()
        {
            // Arrange 
            var expected = "Category is null";
            var expectedLogger = "Category object sent from client is null.";

            var mockDatabaseService = new Mock<IDatabaseService>();

            var mockLogger = new Mock<ILogger<CategoryController>>();

            var mockCastService = new Mock<ICastService>();

            var controller = new CategoryController(mockLogger.Object, mockDatabaseService.Object, mockCastService.Object);

            // Act
            var actual = await controller.Post(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.Equal(expected, badRequest.Value);
        }

        public List<CategoryModel> getListCategoryModels()
        {
            return new List<CategoryModel>
                {
                    new CategoryModel
                    {
                        ID = 1,
                        Name = "A",
                        Description = "A des"
                    },
                    new CategoryModel
                    {
                        ID = 2,
                        Name = "B",
                        Description = "B des"
                    }
                };
        }

        public List<SubcategoryModel> GetListSubcategoryModels()
        {
            return new List<SubcategoryModel>
            {
                new SubcategoryModel
                    {
                        ID = 1,
                        Name = "sub A",
                        Description = "sub A des"
                    },
                    new SubcategoryModel
                    {
                        ID = 2,
                        Name = "sub B",
                        Description = "sub B des"
                    }
            };
        }

        public List<ProductModel> GetListProductModels()
        {
            return new List<ProductModel>
            {
                new ProductModel
                {
                    ID = 1,
                    Name = "product A",
                    Description = "product A description",
                    Price = 10,
                    Stock = 0,
                    SubcategoryId = 1,
                    CategoryId = 1
                },
                new ProductModel
                {
                    ID = 1,
                    Name = "product B",
                    Description = null,
                    Price = 0,
                    Stock = 1,
                    SubcategoryId = 0,
                    CategoryId = 0
                },
                new ProductModel
                {
                    ID = 1,
                    Name = "product A",
                    Description = "product C description",
                    Price = 12,
                    Stock = 12,
                    SubcategoryId = 3,
                    CategoryId = 2
                }
            };
        }

        public List<ProductModel> GetListProductModelsBySubcategoryId(int id)
        {
            return new List<ProductModel>
            {
                new ProductModel
                {
                    ID = 1,
                    Name = "product A",
                    Description = "product A description",
                    Price = 10,
                    Stock = 0,
                    SubcategoryId = id,
                    CategoryId = 1
                },
                new ProductModel
                {
                    ID = 1,
                    Name = "product B",
                    Description = null,
                    Price = 0,
                    Stock = 1,
                    SubcategoryId = id,
                    CategoryId = 0
                },
                new ProductModel
                {
                    ID = 1,
                    Name = "product A",
                    Description = "product C description",
                    Price = 12,
                    Stock = 12,
                    SubcategoryId = id,
                    CategoryId = 0
                }
            };
        }

        public CategoryModel getCategoryModel(int id, string name, string description)
        {
            return new CategoryModel
            {
                ID = id,
                Name = name,
                Description = description
            };
        }

        public SubcategoryModel getSubategoryModel(int id, string name, string description)
        {
            return new SubcategoryModel
            {
                ID = id,
                Name = name,
                Description = description
            };
        }

        /* 
         Hello from me under salad
        baybe comming to hear me
         */

        void Randomnumber()
        {
            Console.WriteLine("lets me random it for you");
        }

        /*
         * Touch the sky
         * Let me fly
         */
    }
}

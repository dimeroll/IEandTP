using Lab2_IEandTP.Controllers;
using Lab2_IEandTP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;
using Moq;

namespace Lab2_IEandTPTests
{
    public class RolesTest
    {
        [Fact]
        public void TestGetRole()
        {
            //Arrange
            var mock = new Mock<Lab2Context>();
            Dictionary<int, Role> roles = new Dictionary<int, Role>();
            roles[1] = new Role { RoleId = 1, RoleName = "new role" };
            mock.Setup(x => x.Roles.FindAsync(1)).Returns(async () => { return await Task.Run(() => roles[1]); });
            var controller = new RolesController(mock.Object);

            //Act
            Role result = controller.GetRole(1).Result.Value;

            //Assert
            Assert.Equal("new role", result.RoleName);
        }

        [Fact]
        public void TestGetAllRoleS()
        {
            //Arrange
            var mock = new Mock<Lab2Context>();
            Dictionary<int, Role> roles = new Dictionary<int, Role>();
            roles[1] = new Role { RoleId = 1, RoleName = "new role" };
            mock.Setup(x => x.Roles.FindAsync(1)).Returns(async () => { return await Task.Run(() => roles[1]); });
            var controller = new RolesController(mock.Object);

            //Act
            var result1 = controller.GetRoles();

            //Assert
            Assert.NotNull(result1);
        }

        [Fact]
        public void TestGetItemCategories()
        {
            //Arrange
            var mock = new Mock<Lab2Context>();
            Dictionary<int, ItemCategory> cat = new Dictionary<int, ItemCategory>();
            cat[1] = new ItemCategory { ItemCategoryId = 5, ItemCategoryName = "new category" };
            mock.Setup(x => x.ItemCategories.FindAsync(5)).Returns(async () => { return await Task.Run(() => cat[1]); });
            var controller = new ItemCategoriesController(mock.Object);

            //Act
            ItemCategory category = controller.GetItemCategory(5).Result.Value;

            //Assert
            Assert.Equal("new category", category.ItemCategoryName);
        }
            
    }
}

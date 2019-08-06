using System;
using System.Collections.Generic;
using System.Text;
using Cribbly.Models;
using Cribbly.Areas.Identity.Pages.Account;
using Microsoft.EntityFrameworkCore;
using Cribbly.Data;
using Xunit;
using Autofac.Extras.Moq;

namespace Cribbly.Tests
{
    public class UserTests
    {
        [Theory]
        [InlineData("Michael", "Scott", "DundarMifflin!", "mscott@dundermifflin.com")]
        [InlineData("Dwight", "Schrute", "x", "shouldfail@dundermifflin.com")]
        public void Should_DataIsValid(string FirstName, string LastName, string Password, string Email)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Should_DataIsValid")
                .Options;

            RegisterModel.InputModel im = new RegisterModel.InputModel()
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password,
                ConfirmPassword = Password
            };

        }


        [Theory]
        [InlineData("Stanley", "Hudson", "Waffles123!", "shudson@dundermifflin.com")]
        public void Should_DataIsInvalid(string FirstName, string LastName, string Password, string Email)
        {
            using (var mock = AutoMock.GetLoose())
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "Should_DataIsValid")
                    .Options;

                var controller = mock.Create<RegisterModel>();
                
                controller.Input = new RegisterModel.InputModel()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Password = Password,
                    ConfirmPassword = Password + "aaa"
                };

                Assert.False(controller.ModelState.IsValid);

            }
        }
    }
}
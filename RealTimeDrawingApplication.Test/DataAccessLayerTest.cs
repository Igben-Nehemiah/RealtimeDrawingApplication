using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace RealTimeDrawingApplication.Test
{
    public class DataAccessLayerTest
    {
        IUnitOfWork unit;
        Mock<IUserRepository> mockUserRepository;
        Mock<IUnitOfWork> mockUnitOfWork;
        List<User> users;
        
        [SetUp]
        public void Setup()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockUnitOfWork = new Mock<IUnitOfWork>();

            var user1 = new User();
            user1.UserId = 1;
            user1.UserEmailAddress = "user1";

            var user2 = new User();
            user2.UserId = 2;
            user2.UserEmailAddress = "user2";

            var user3 = new User();
            user3.UserId = 3;
            user3.UserEmailAddress = "user3";

            users = new List<User>();
            users.Add(user1);
            users.Add(user2);
            users.Add(user3);

            mockUnitOfWork.SetupGet(x => x.Users).Returns(mockUserRepository.Object);
        }

        [Test]
        public void Add_AddNewUserToUserRepository()
        {
            var user = new User();
            user.UserId = 0;

            mockUserRepository.Setup(x => x.Add(user)).Callback(()=> { user.UserId = users.Count + 1; users.Add(user); });

            Assert.DoesNotThrow(() => mockUnitOfWork.Object.Users.Add(user));
        }

        [Test]
        public void Add_UserWithAnIdOtherThanZero()
        {
            var user = new User();
            user.UserId = 1;

            mockUserRepository.Setup(x => x.Add(user)).Throws(new Exception());

            Assert.Throws<Exception>(() => mockUnitOfWork.Object.Users.Add(user));
        }

        [Test]
        public void GetAll_ReturnsListOfUsers()
        {
            //Arrange
            mockUserRepository.Setup(x => x.GetAll()).Returns(users);

            //Act
            unit = mockUnitOfWork.Object;
            var expectedUsers = unit.Users.GetAll();
            unit.Complete();

            //Assert
            Assert.NotNull(expectedUsers);
            Assert.AreSame(expectedUsers, users);
        }

        [Test]
        public void ContainsUser_CheckAlreadyRegisteredEmailAddress_ReturnsFalse()
        {
            //Arrange
            var emailAddress = users[0].UserEmailAddress;
            mockUserRepository.Setup(x => x.ContainsUser(emailAddress)).Returns(true);

            unit = mockUnitOfWork.Object;
            var expectedResult = unit.Users.ContainsUser(emailAddress);
            unit.Complete();


            //Assert 
            Assert.IsTrue(expectedResult);
        }

        [Test]
        public void ContainsUser_CheckAnUnregisteredEmailAddress_ReturnsFalse()
        {
            var emailAddress = "randomEmailAddress";

            mockUserRepository.Setup(x => x.ContainsUser(emailAddress)).Returns(false);

            var expectedResult = mockUnitOfWork.Object.Users.ContainsUser(emailAddress);
            unit.Complete();


            Assert.IsFalse(expectedResult);
        }
    }
}
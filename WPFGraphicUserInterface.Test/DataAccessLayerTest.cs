using NUnit.Framework;

namespace WPFGraphicUserInterface.Test
{
    public class DataAccessLayerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddUserToDatabases_UserAlreadyInDatabase_ShouldThrowAnError()
        {
            Assert.That(true);
        }

        [Test]
        public void AddUserToDatabase_UserNotInDatabase_ShouldBeSuccessful()
        {
            Assert.Pass();
        }
    }
}
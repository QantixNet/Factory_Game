using Diage2022.Factory_Game.Server.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Diiage2022.Factory_Games.Server.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        Services services = new Services();
        [TestMethod]
        public void TestMethodGenerateSchools()
        {
            services.CreateGame(1, 2, 3);
            Assert.AreEqual(1, services.GenerateSchoolTrainingSessions());
            Assert.AreEqual(2, services.GenerateSchoolTrainingSessions());
            Assert.AreEqual(3, services.GenerateSchoolTrainingSessions());
        }

        [TestMethod]
        public void TestMethodGenerateDevelopers()
        {
            services.CreateGame(1, 2, 3);
            Assert.AreEqual(3, services.GenerateDevelopers(3, 10000));
            
        }

        [TestMethod]
        public void TestMethodFireDeveloper()
        {
            services.CreateGame(1, 2, 3);
            services.GenerateDevelopers(3, 10000);
            services.AddCompany("toto", 1);
            services.AddDeveloperToCompany(1, 1);
            services.SetPlayerTurn(1, new List<TcpClient>());
            Assert.AreEqual(1, services.AddDeveloperToCompany(1, 1));
            Assert.AreEqual(0, services.FireDeveloper(1,1));
            
        }

        [TestMethod]
        public void TestMethodAddDeveloperToTrainingSession()
        {
            services.CreateGame(1, 2);
            services.GenerateDevelopers(3, 10000);
            services.GenerateSchoolTrainingSessions();
            services.AddCompany("toto", 1);
            services.AddDeveloperToCompany(1, 1);
            services.SetPlayerTurn(1, new List<TcpClient>());
            Assert.AreEqual(1, services.AddDeveloperToTrainingSession(1, 1, 1, 1));

        }

        [TestMethod]
        public void TestGenerateProjects()
        {
            services.CreateGame(1, 2);
            Assert.AreEqual(2, services.GenerateProjects(2));
        }
    }
}

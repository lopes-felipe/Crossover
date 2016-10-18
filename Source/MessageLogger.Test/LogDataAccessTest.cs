using MessageLogger.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace MessageLogger.Test
{
    [TestClass]
    public class LogDataAccessTest
    {
        [TestInitialize]
        public void TestIntialize()
        {
            this.connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Crossover"].ConnectionString);
            this.connection.Open();
        }

        private SqlConnection connection;

        [TestCleanup]
        public void TestCleanup()
        {
            this.connection.Dispose();
            this.connection = null;
        }

        [TestMethod]
        public void CreateTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.Log log = new Entities.Log(0, "1", "Logger1", "Level1", "Message1");
            Entities.Log secondLog = new Entities.Log(0, "1", "Logger2", "Level2", "Message2");

            LogDataAccess logDataAccess = new LogDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            log = logDataAccess.Create(log);
            secondLog = logDataAccess.Create(secondLog);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(log.LogID > 0);
            Assert.IsTrue(secondLog.LogID > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.Log log = new Entities.Log(1, "1", "Logger3", "Level3", "Message3");
            LogDataAccess logDataAccess = new LogDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            int affectedRows = logDataAccess.Update(log);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(affectedRows == 1);
        }


        [TestMethod]
        public void DeleteTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.Log log = new Entities.Log(1);
            Entities.Log secondLog = new Entities.Log(2);

            LogDataAccess logDataAccess = new LogDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            int affectedRows = logDataAccess.Delete(log);
            int secondAffectedRows = logDataAccess.Delete(secondLog);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(affectedRows == 1);
            Assert.IsTrue(secondAffectedRows == 1);
        }

        [TestMethod]
        public void RetrieveAllTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            LogDataAccess logDataAccess = new LogDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            IEnumerable<Entities.Log> logs = logDataAccess.Retrieve();

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(logs.Count() == 2);
        }

        [TestMethod]
        public void RetrieveByID()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.Log log = new Entities.Log(1);
            LogDataAccess logDataAccess = new LogDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            IEnumerable<Entities.Log> logs = logDataAccess.Retrieve(log);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(logs.Count() == 1);
            Assert.IsTrue(logs.First().LogID == 1);
        }
    }
}

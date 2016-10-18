using MessageLogger.Business;
using MessageLogger.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Test
{
    [TestClass]
    public class SessionManagerTest
    {
        [TestInitialize]
        public void SessionManagerTestInitialize()
        {
            this.applicationDataAccessMock = new Mock.ApplicationDataAccessMock();
            this.sessionDataAccessMock = new Mock.ApplicationSessionDataAccessMock();
            this.sessionConfigurationMock = new Mock.SessionConfigurationDataAccessMock();
        }

        // TODO: Use a Mock Framework instead
        private Mock.ApplicationDataAccessMock applicationDataAccessMock = null;
        private Mock.ApplicationSessionDataAccessMock sessionDataAccessMock = null;
        private Mock.SessionConfigurationDataAccessMock sessionConfigurationMock = null;

        [TestMethod]
        public void AuthenticateSuccessWithValidSessionTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            SessionManager sessionManager = new SessionManager(this.applicationDataAccessMock, this.sessionDataAccessMock, this.sessionConfigurationMock);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            Guid accessToken = sessionManager.Authenticate("2", "2");

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.AreEqual(accessToken, Guid.Parse("79B9D3FE-57A9-4736-810C-22AF672198CB"));
        }

        [TestMethod]
        public void AuthenticateSuccessWithInvalidSessionTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            SessionManager sessionManager = new SessionManager(this.applicationDataAccessMock, this.sessionDataAccessMock, this.sessionConfigurationMock);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            Guid accessToken = sessionManager.Authenticate("1", "1");

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(this.sessionDataAccessMock.Retrieve().Count() == 5);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void AuthenticateFailWithInvalidSessionTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            SessionManager sessionManager = new SessionManager(this.applicationDataAccessMock, this.sessionDataAccessMock, this.sessionConfigurationMock);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            Guid accessToken = sessionManager.Authenticate("1", "2");

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(false);
        }
    }
}

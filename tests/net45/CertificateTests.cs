using Avalara.AvaTax.RestClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Tests.Avalara.AvaTax.RestClient.netstandard
{
    [TestFixture]
    public class CertificateTests
    {
        public AvaTaxClient Client { get; set; }
        public string DefaultCompanyCode { get; set; }
        public int DefaultCompanyId { get; set; }
        public CertificateModel CreatedCert { get; set; }
        

        #region Setup / TearDown
        /// <summary>
        /// Create a company for use with these tests
        /// </summary>
        [SetUp]
        public void Setup()
        {
            try {
                // Create a client and set up authentication
                Client = new AvaTaxClient(typeof(TransactionTests).Assembly.FullName,
                    typeof(TransactionTests).Assembly.GetName().Version.ToString(),
                    Environment.MachineName,
                    AvaTaxEnvironment.Sandbox)
                    .WithSecurity(Environment.GetEnvironmentVariable("SANDBOX_USERNAME"), Environment.GetEnvironmentVariable("SANDBOX_PASSWORD"));

                // Verify that we can ping successfully
                var pingResult = Client.Ping();

                // Assert that ping succeeded
                Assert.NotNull(pingResult, "Should be able to call Ping");
                Assert.True(pingResult.authenticated, "Environment variables should provide correct authentication");

                //Get the default company.
                var defaultCompanyModel = Client.QueryCompanies(string.Empty, "isDefault EQ true", null, null, string.Empty).value.FirstOrDefault();

                DefaultCompanyId = defaultCompanyModel.id;                

                // Shouldn't fail
            } catch (Exception ex) {
                Assert.Fail("Exception in SetUp: " + ex);
            }
        }        
        #endregion

        [Test, Order(1)]
        public void CreateCertificateTest()
        {
            string fileName = Guid.NewGuid().ToString() + ".pdf";

            //Create a cert for the test company.
            var certModels = new List<CertificateModel>() {
                new CertificateModel {
                signedDate = DateTime.Now,
                expirationDate = DateTime.Now.Add(new TimeSpan(2, 0, 0)),
                exemptionReason = new ExemptionReasonModel { id = 16, name = "EXPOSURE"  },
                filename = fileName,
                exposureZone = new ExposureZoneModel {
                    id = 89,
                    companyId = DefaultCompanyId,
                    name = "Washington",
                    country = "US"
                    }
                }
            };

            var certs = Client.CreateCertificates(DefaultCompanyId, null, certModels);
            Assert.True(certs.Count == 1);
            Assert.True(certs[0].filename == fileName);
            CreatedCert = certs[0];
        }

        [Test, Order(2)]
        public void DeleteCertificateTest()
        {
            try {
                //Delete the test cert.
                Client.DeleteCertificate(DefaultCompanyId, CreatedCert.id.Value);
            } catch (Exception exc) {
                //verify the cert is no longer available.
            }
        }
    }
}

    

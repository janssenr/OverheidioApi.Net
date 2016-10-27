using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using NUnit.Framework;
using OverheidioApi.Net;
using OverheidioApi.Net.Models;
using OverheidioApi.Net.Wrappers;

namespace OverheidioApi.Tests
{
    [TestFixture]
    public class VoertuigTests
    {
        private OverheidioApiClient _client;

        [SetUp]
        public void SetUp()
        {
            var apiKey = ConfigurationManager.AppSettings["ApiKey"];
            _client = new OverheidioApiClient(apiKey);
        }

        [Test]
        public void ZoekOpMerk()
        {
            string merk = "bmw";
            KeyValuePair<string, string>[] filters = { new KeyValuePair<string, string>("merk", merk) };

            Task<ApiHalResultWrapper> result = _client.ZoekVoertuig(filters: filters);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.TotalItemCount > 0);
            Assert.IsNotNull(result.Result.Results.Voertuigen);
            Assert.IsTrue(string.Equals(result.Result.Results.Voertuigen[0].Merk, merk, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void ZoekNaarMerkMetWilcardsEnToonExtraVelden()
        {
            string query = "*laren";
            string[] queryfields = { "merk" };
            string[] fields = { "eerstekleur", "vermogen" };

            Task<ApiHalResultWrapper> result = _client.ZoekVoertuig(query: query, queryfields: queryfields, fields: fields);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.TotalItemCount > 0);
            Assert.IsNotNull(result.Result.Results.Voertuigen);
        }

        [Test]
        public void GeefAlleInformatieVanEenKenteken()
        {
            string kenteken = "4-TFL-24";
            Task<Voertuig> result = _client.GetVoertuig(kenteken);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.AreEqual(kenteken, result.Result.Kenteken);
        }

        [Test]
        public void GeefSuggestiesVoorEenZoekterm()
        {
            string query = "Niet geregistreerd";
            int size = 5;
            Task<VoertuigSuggestieResultaten> result = _client.GetVoertuigSuggesties(query, size);
            result.Wait();
            Assert.IsNotNull(result);
            //Assert.IsTrue(result.Result.WoonplaatsSuggesties.Length > 0);
            //Assert.IsTrue(result.Result.OpenbareRuimteSuggesties.Length > 0);
        }
    }
}

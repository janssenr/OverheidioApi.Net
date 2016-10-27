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
    public class AdresTests
    {
        private OverheidioApiClient _client;

        [SetUp]
        public void SetUp()
        {
            var apiKey = ConfigurationManager.AppSettings["ApiKey"];
            _client = new OverheidioApiClient(apiKey);
        }

        [Test]
        public void ZoekOpPostcode()
        {
            string postcode = "3015BA";
            KeyValuePair<string, string>[] filters = { new KeyValuePair<string, string>("postcode", postcode) };

            Task<ApiHalResultWrapper> result = _client.ZoekAdres(filters: filters);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.TotalItemCount > 0);
            Assert.IsNotNull(result.Result.Results.Adressen);
            Assert.IsTrue(string.Equals(result.Result.Results.Adressen[0].Postcode, postcode, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void ZoekNaarPostcodeEnHuisnummer()
        {
            string postcode = "3015BA";
            string huisnummer = "10";
            KeyValuePair<string, string>[] filters = { new KeyValuePair<string, string>("postcode", postcode) };
            string query = huisnummer;
            string[] queryfields = { "huisnummer" };

            Task<ApiHalResultWrapper> result = _client.ZoekAdres(filters: filters, query: query, queryfields: queryfields);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.TotalItemCount > 0);
            Assert.IsNotNull(result.Result.Results.Adressen);
            Assert.IsTrue(string.Equals(result.Result.Results.Adressen[0].Postcode, postcode, StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(string.Equals(result.Result.Results.Adressen[0].Huisnummer, huisnummer, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GeefAlleInformatieVanEenAdres()
        {
            string id = "3015ba-nieuwe-binnenweg-10-a";
            Task<Adres> result = _client.GetAdres(id);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Result.Url);
        }

        [Test]
        public void GeefSuggestiesVoorEenZoekterm()
        {
            string query = "nieuwve";
            Task<AdresSuggestieResultaten> result = _client.GetAdresSuggesties(query);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.WoonplaatsSuggesties.Length > 0);
            Assert.IsTrue(result.Result.OpenbareRuimteSuggesties.Length > 0);
        }
    }
}

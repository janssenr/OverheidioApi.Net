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
    public class BedrijfTests
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
            string postcode = "3083cz";
            KeyValuePair<string, string>[] filters = { new KeyValuePair<string, string>("postcode", postcode) };

            Task<ApiHalResultWrapper> result = _client.ZoekBedrijf(filters: filters);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.TotalItemCount > 0);
            //Assert.AreEqual(result.Result.Results.Bedrijven[0].Postcode, postcode);
        }

        [Test]
        public void ZoekNaarNaamMetWildcardsEnToonExtraVelden()
        {
            string query = "d*size*";
            string[] fields = { "postcode", "vestigingsnummer" };

            Task<ApiHalResultWrapper> result = _client.ZoekBedrijf(query: query, fields: fields);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.TotalItemCount > 0);
            Assert.IsNotNull(result.Result.Results.Bedrijven);
        }

        [Test]
        public void GeefAlleRechtpersonenTerugMetEenPostcodeMetLettersEnToonDePostcode()
        {
            string query = "*XD";
            string[] queryfields = { "postcode" };
            string[] fields = { "postcode" };

            Task<ApiHalResultWrapper> result = _client.ZoekBedrijf(query: query, queryfields: queryfields, fields: fields);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.TotalItemCount > 0);
            Assert.IsNotNull(result.Result.Results.Bedrijven);
        }

        [Test]
        public void GeefAlleInformatieVanEenRechtspersoon()
        {
            string dossiernummer = "58488340";
            string subdossiernummer = "0000";
            Task<Bedrijf> result = _client.GetBedrijf(dossiernummer, subdossiernummer);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.AreEqual(dossiernummer, result.Result.Dossiernummer);
            Assert.AreEqual(subdossiernummer, result.Result.Subdossiernummer);
        }

        [Test]
        public void GeefSuggestiesVoorEenZoekterm()
        {
            string query = "oudet";
            int size = 5;
            Task<BedrijfSuggestieResultaten> result = _client.GetBedrijfSuggesties(query, size);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.HandelsnaamSuggesties.Length > 0);
            Assert.IsTrue(result.Result.StraatSuggesties.Length > 0);
        }

        [Test]
        public void GeefAlleenHandelsnamenTerug()
        {
            string query = "downsi";
            int size = 10;
            string[] fields = { "handelsnaam" };
            Task<BedrijfSuggestieResultaten> result = _client.GetBedrijfSuggesties(query, size, fields);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.IsNull(result.Result.DossiernummerSuggesties);
            Assert.IsTrue(result.Result.HandelsnaamSuggesties.Length > 0);
            Assert.IsNull(result.Result.StraatSuggesties);
        }

        [Test]
        public void ZoekOpDossiernummer()
        {
            string query = "1002";
            string[] fields = { "dossiernummer" };
            Task<BedrijfSuggestieResultaten> result = _client.GetBedrijfSuggesties(query, fields: fields);
            result.Wait();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.DossiernummerSuggesties.Length > 0);
            Assert.IsNull(result.Result.HandelsnaamSuggesties);
            Assert.IsNull(result.Result.StraatSuggesties);
        }
    }
}

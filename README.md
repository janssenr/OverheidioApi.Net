# OverheidioApi.Net [![Build status](https://ci.appveyor.com/api/projects/status/2vww4qykint9wnr5?svg=true)](https://ci.appveyor.com/project/jfversluis/overheidioapi-net) [![NuGet version](https://badge.fury.io/nu/OverheidioApi.Net.svg)](https://badge.fury.io/nu/OverheidioApi.Net)
A C#/.net wrapper for the Overheid.io API

# Getting started
Just create a new `OverheidioApiClient` with your own API key.
API keys are available on [overheid.io](https://www.overheid.io) check the rest of the API documentation [here (in Dutch)](https://overheid.io/documentatie)

```
var client = new OverheidioApiClient("FILL IN KEY HERE");
```

Now you can start getting data.

## ZoekBedrijf
This method provides you with the functionality to perform search operations on the dataset.
Check out the IntelliSense for the options on limiting your find results.

```
var resultBedrijven = await client.ZoekBedrijf();
foreach (var r in resultBedrijven.Results.Bedrijven)
{
	Console.WriteLine(r.Handelsnaam);
}
```

## GetBedrijf
Gets the details from a specific Corporation by the dossiernumber and subdossiernumber

```
var bedrijf = await client.GetBedrijf("58488340", "0000");
Console.WriteLine(bedrijf.Handelsnaam + " " + bedrijf.Straat);
```

## GetBedrijfSuggesties
Provides autocomplete suggestions based on the entered query

```
var suggesties = await client.GetBedrijfSuggesties("downsi");
Console.WriteLine("Suggesties " + suggesties.HandelsnaamSuggesties.Length);
```

## ZoekVoertuig
This method provides you with the functionality to perform search operations on the dataset.
Check out the IntelliSense for the options on limiting your find results.

```
var resultVoertuigen = await client.ZoekVoertuig();
foreach (var r in resultVoertuigen.Results.Voertuigen)
{
	Console.WriteLine(r.Merk);
}
```

## GetVoertuig
Gets the details from a specific Vehicle by licenseplate

```
var voertuig = await client.GetVoertuig("4-TFL-24");
Console.WriteLine(voertuig.Merk);
```

// See https://docs.nethereum.com/en/latest/getting-started/#5-code-to-retrieve-account-balance
using Microsoft.Extensions.Configuration;
using Nethereum.Web3;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if(string.IsNullOrEmpty(environment)) {
    #if DEBUG
    environment = "Development";
    #else
    environment = "Production";
    #endif
}
Console.WriteLine($"environment={environment}");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

// const string infuraKey = "...";
// // const string endpointUrl = $"https://mainnet.infura.io/v3/{infuraKey}"; // MainNet
// const string endpointUrl = $"https://rinkeby.infura.io/v3/{infuraKey}"; // Rinkeby
// // const string endpointUrl = $"https://ropsten.infura.io/v3/{infuraKey}"; // Ropsten

// const string ethAddress = "0x742130C807117f0BC190d48B0C122E76D27596B6"; // Rinkeby test account (James)

string endpointUrl = configuration.GetRequiredSection("Web3EndpointUrl").Value;
string ethAddress = configuration.GetRequiredSection("EthereumAddress").Value;

if(string.IsNullOrEmpty(endpointUrl)) {
    Console.WriteLine($"ERROR: missing application setting \"Web3EndpointUrl\", please set this value in appsettings.{environment}.json");
    Environment.Exit((int)ErrorCodes.Missing_Web3EndpointUrl_AppSetting);
}
if(string.IsNullOrEmpty(ethAddress)) {
    Console.WriteLine($"ERROR: missing application setting \"EthereumAddress\", please set this value in appsettings.{environment}.json");
    Environment.Exit((int)ErrorCodes.Missing_EthereumAddress_AppSetting);
}

// GetAccountBalance().Wait();
// await GetAccountBalance();
await GetAccountBalance(endpointUrl, ethAddress);
Console.ReadLine();

/////////////////////////////////

// static async Task GetAccountBalance()
static async Task GetAccountBalance(string endpointUrl, string ethAddress)
{
    var web3 = new Web3(endpointUrl);

    Console.WriteLine($"Ethereum Address: {ethAddress}");

    var balance = await web3.Eth.GetBalance.SendRequestAsync(ethAddress);
    Console.WriteLine($"Balance in Wei: {balance.Value}");

    var etherAmount = Web3.Convert.FromWei(balance.Value);
    Console.WriteLine($"Balance in Ether: {etherAmount}");

    Console.WriteLine("... Press enter to exit ...");
}

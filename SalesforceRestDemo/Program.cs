using System.Net.Http.Headers;
using System.Text.Json;

string? instanceUrl = Environment.GetEnvironmentVariable("SALESFORCE_INSTANCE_URL");
string? accessToken = Environment.GetEnvironmentVariable("SALESFORCE_ACCESS_TOKEN");

if (string.IsNullOrWhiteSpace(instanceUrl) || string.IsNullOrWhiteSpace(accessToken))
{
    Console.WriteLine("FAIL: Salesforce environment variables are not configured.");
    return;
}

using HttpClient client = new HttpClient();

client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", accessToken);

bool runNegativeTest = args.Contains("--negative");

string requestUrl = runNegativeTest
    ? $"{instanceUrl}/services/data/v65.0/sobjects/DefinitelyNotARealObject"
    : $"{instanceUrl}/services/data/v65.0/query/?q=SELECT+Id,Name+FROM+Account+LIMIT+5";

Console.WriteLine(
    runNegativeTest
        ? "Running negative Salesforce REST API test..."
        : "Running positive Salesforce REST API test..."
);

HttpResponseMessage response = await client.GetAsync(requestUrl);

if (!response.IsSuccessStatusCode)
{
    if (runNegativeTest)
    {
        Console.WriteLine(
            $"PASS: Salesforce rejected the invalid request with HTTP {(int)response.StatusCode}."
        );
        return;
    }

    Console.WriteLine(
        $"FAIL: Salesforce returned HTTP {(int)response.StatusCode}."
    );
    return;
}

string json = await response.Content.ReadAsStringAsync();

using JsonDocument document = JsonDocument.Parse(json);

JsonElement root = document.RootElement;

int totalSize = root.GetProperty("totalSize").GetInt32();

if (totalSize > 0)
{
    Console.WriteLine("PASS: Salesforce REST API returned Account records.");
    Console.WriteLine($"Records returned: {totalSize}");
}
else
{
    Console.WriteLine("FAIL: Salesforce returned no Account records.");
}

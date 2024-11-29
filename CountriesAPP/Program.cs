var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

Dictionary<int, string> countries = new Dictionary<int, string>()
{
    { 1, "United States" },
    { 2, "Canada" },
    { 3, "United Kingdom" },
    { 4, "India" },
    { 5, "Japan" }
};

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapGet("/countries", async context =>
    {
        foreach (KeyValuePair<int, string> kvp in countries)
        {
            await context.Response.WriteAsync($"{kvp.Key}, {kvp.Value}\n");
        }
    });

    _ = endpoints.MapGet("/countries/{countryID:int:range(1, 100)}", async context =>
    {

        if (context.Request.RouteValues.ContainsKey("countryID") == false)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync($"The CountryID should be between 1 and 100");
        }

        int countryID = Convert.ToInt32(context.Request.RouteValues.ContainsKey("countryID"));

        if (countries.ContainsKey(countryID))
        {
            string country = countries[countryID];
            await context.Response.WriteAsync($"{country}");
        }
        else
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync($"[No country]");
        }
    });

    _ = endpoints.MapGet("/countries/{countryID:int:min(101)}", async context =>
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("The CountryID sholud be between 1 and 100.");
    });


});

app.Run(async context =>
{
    await context.Response.WriteAsync("No Response");
});

app.Run();

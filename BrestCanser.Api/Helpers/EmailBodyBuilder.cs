namespace BrestCanser.Api.Helpers;

public static class EmailBodyBuilder
{
    public static string GenerateEmailBody(string template, Dictionary<string, string> templateModel)
    {

        var basePath = AppDomain.CurrentDomain.BaseDirectory; // production environment
        //var basePath = Directory.GetCurrentDirectory(); // development environment
        var templatePath = Path.Combine(basePath, "Templates", $"{template}.html");


        if (!File.Exists(templatePath))
            throw new FileNotFoundException($"Template file not found at path: {templatePath}");

        var body = File.ReadAllText(templatePath);

        foreach (var item in templateModel)
            body = body.Replace(item.Key, item.Value);

        return body;
    }
}
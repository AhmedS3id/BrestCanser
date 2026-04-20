using Google.GenAI;

namespace BrestCanser.Api.Services;


public class GeminiService : IChatService
{
	private readonly Client _client;
	private readonly string _modelName;
	public GeminiService(IConfiguration config)
	{
		var apiKey = config["Gemini:ApiKey"];
		_modelName = config["Gemini:Model"] ?? "gemini-2.5-flash";
		_client = new Client(apiKey: apiKey);
	}
	public async Task<string> GetResponseAsync(string message)
	{
		var response = await _client.Models.GenerateContentAsync(
		 model: _modelName,
		 contents: message
	 );


		return response.Candidates[0].Content.Parts[0].Text ?? "No response generated.";
	}
}
namespace BrestCanser.Api.Abstractions.Consts;

public static class RegexPatterns
{
	public const string Password = "(?=(.*[0-9]))(?=.*[\\!@#$%^&*()\\\\[\\]{}\\-_+=~`|:;\"'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}";
	public const string PhoneNumber = "^01[0,1,2,5]{1}[0-9]{8}$";
}
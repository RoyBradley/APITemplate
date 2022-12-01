namespace Application.Configuration;


public class EmailSettings
{
	public string SmtpHost { get; set; }
	public int SmtpPort { get; set; }
	public string EmailFrom { get; set; }
	public string SmtpUsername { get; set; }
	public string SmtpPassword { get; set; }
	public bool SmtpSecurity { get; set; }
}

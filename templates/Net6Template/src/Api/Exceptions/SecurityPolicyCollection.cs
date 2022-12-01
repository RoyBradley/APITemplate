namespace Api.Exceptions;


public static class SecurityPolicyCollection
{
	public static HeaderPolicyCollection PolicyCollection() {
		HeaderPolicyCollection policyCollection = new HeaderPolicyCollection()
			.AddFrameOptionsSameOrigin()
			.AddXssProtectionBlock()
			.AddContentTypeOptionsNoSniff()
			.AddStrictTransportSecurityMaxAgeIncludeSubDomains()
			//	max age = one year in seconds
			.AddReferrerPolicyStrictOriginWhenCrossOrigin()
			.RemoveServerHeader()
			.AddContentSecurityPolicy(builder => {
				builder.AddObjectSrc().None();
				builder.AddFormAction().Self();
				builder.AddFrameAncestors().Self();
				builder.AddDefaultSrc();
			})
			//	for old INTERNET explorer
			.AddCustomHeader("X-Content-Security-Policy", "default-src 'self'");

		return policyCollection;
	}
}

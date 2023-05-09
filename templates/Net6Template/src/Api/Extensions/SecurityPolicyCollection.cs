namespace Api.Extensions;


public static class SecurityHeaderPolicy
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
				_ = builder.AddObjectSrc().None();
				_ = builder.AddFormAction().Self();
				_ = builder.AddFrameAncestors().Self();
				_ = builder.AddDefaultSrc();
			})
			//	for old INTERNET explorer
			.AddCustomHeader("X-Content-Security-Policy", "default-src 'self'");

		return policyCollection;
	}
}

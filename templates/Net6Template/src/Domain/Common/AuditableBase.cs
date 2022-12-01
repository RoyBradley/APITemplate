using System.ComponentModel.DataAnnotations;


namespace Domain.Common;

public class AuditableBase : BaseEntity
{
	[MaxLength(50)]
	public string CreatedBy { get; set; } = string.Empty;
	public DateTime CreateDate { get; set; }
	[MaxLength(50)]
	public string UpdatedBy { get; set; } = string.Empty;
	public DateTime UpdateDate { get; set; }
}


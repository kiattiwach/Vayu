using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Vayu.Localization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace Vayu.Staffs;

public class StaffCreateUpdateDto : EntityDto<Guid>, IValidatableObject
{
    [Required]
    [DynamicMaxLength(typeof(StaffConsts),nameof(StaffConsts.MaxCodeLength))]
    public string Code { get; set; }

    [Required]
    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxNameLength))]
    public string Name { get; set; }

    [Required]
    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxCodeNameLength))]
    public string CodeName { get; set; }

    [Required]
    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxContactNumberLength))]
    public string ContactNumber { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? PassAssessmentDate { get; set; }

    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxEmailLength))]
    public string Email { get; set; }

    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxAddressLength))]
    public string Address { get; set; }

    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxRadioCodeLength))]
    public string RadioCode { get; set; }
    
    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxNameLength))]
    public string ContactPerson { get; set; }

    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxContactNumberLength))]
    public string ContactPersonNumber { get; set; }

    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxEmailLength))]
    public string Line { get; set; }

    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxEmailLength))]
    public string Facebook { get; set; }

    [Required]
    public Guid RoleId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var stringLocalize = validationContext.GetRequiredService<IStringLocalizer<VayuResource>>();
        if (StartDate < StaffConsts.MinStartDate)
        {
            yield return new ValidationResult(
                stringLocalize["InvalidStartDate", StaffConsts.MinStartDate],
                new[] { nameof(StartDate) }
            );
        }

        if (StartDate > PassAssessmentDate )
        {
            yield return new ValidationResult(
                stringLocalize["InvalidPassAssessmentDateDate", StartDate],
                new[] { nameof(PassAssessmentDate) }
            );
        }
    }
}
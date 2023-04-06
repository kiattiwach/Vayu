using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Vayu.Localization;
using Vayu.Staffs;

namespace Vayu.Tasks;

public class TaskCreateUpdateDto : EntityDto<Guid>, IValidatableObject
{
    [Required]
    public Guid TaskTypeId { get; set; }

    [Required]
    public Status Status { get; set; }

    [Required]
    [DynamicMaxLength(typeof(TaskConsts), nameof(TaskConsts.MaxDescriptionLength))]
    public string Description { get; set; }

    [Required]
    public DateTime CreateDateTime { get; set; }

    public DateTime CompleteDateTime { get; set; }

    public string Note { get; set; }

    [Required]
    public Guid LeaderId { get; set; }

    [DynamicMaxLength(typeof(TaskConsts), nameof(TaskConsts.MaxContactPersonLength))]
    public string ContactPerson { get; set; }

    [DynamicMaxLength(typeof(TaskConsts), nameof(TaskConsts.MaxContactPersonNumberLength))]
    public string ContactNumber { get; set; }

    public List<Guid> TeamMemberIds { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var stringLocalize = validationContext.GetRequiredService<IStringLocalizer<VayuResource>>();

        if (!TeamMemberIds.Any())
        {
            yield return new ValidationResult(
                stringLocalize["TeamMemberShouldNotBeZero"],
                new[] { nameof(TeamMemberIds) }
            );
        }

        if ((Id == Guid.Empty || Id == default) && Status != Status.Initial)
        {
            yield return new ValidationResult(
                stringLocalize["NewTaskShouldBeInitial", stringLocalize[$"Enum:Status:{Status.Initial.ToString()}"]],
                new[] { nameof(Status) }
            );
        }
    }
}
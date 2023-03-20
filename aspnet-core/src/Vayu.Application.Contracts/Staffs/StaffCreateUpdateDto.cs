using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace Vayu.Staffs;

public class StaffCreateUpdateDto : EntityDto<Guid>
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
    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxEmailLength))]
    public string Email { get; set; }
    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxAddressLength))]
    public string Address { get; set; }
    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxRadioCodeLength))]
    public string RadioCode { get; set; }
    [Required]
    [DynamicMaxLength(typeof(StaffConsts), nameof(StaffConsts.MaxContactNumberLength))]
    public string ContactNumber { get; set; }
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
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vayu.Controllers;
using Vayu.Permissions;

namespace Vayu.VayuPermission;

[Route("api/vayu-permission")]
public class VayuPermissionController : VayuController
{
    [HttpGet]
    public Task<VayuPermissions> GetAsync()
    {
        return Task.FromResult(new VayuPermissions());
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Vayu.Localization;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Vayu.Controllers;

/* Inherit your controllers from this class.
 */
[Area(VayuRemoteServiceConsts.ModuleName)]
[RemoteService(Name = VayuRemoteServiceConsts.RemoteServiceName)]
public abstract class VayuController : AbpControllerBase
{
    protected VayuController()
    {
        LocalizationResource = typeof(VayuResource);
    }
}

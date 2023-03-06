using Vayu.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Vayu.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class VayuController : AbpControllerBase
{
    protected VayuController()
    {
        LocalizationResource = typeof(VayuResource);
    }
}

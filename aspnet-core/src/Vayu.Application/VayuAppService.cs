using System;
using System.Collections.Generic;
using System.Text;
using Vayu.Localization;
using Volo.Abp.Application.Services;

namespace Vayu;

/* Inherit your application services from this class.
 */
public abstract class VayuAppService : ApplicationService
{
    protected VayuAppService()
    {
        LocalizationResource = typeof(VayuResource);
    }
}

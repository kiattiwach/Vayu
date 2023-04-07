using System.Drawing;
using System;
using Vayu.Tasks;

namespace Vayu.Shared;

public static class Extensions
{
    public static string GetColor(this Status status)
    {
        var instance = Activator.CreateInstance(typeof(StatusColor));
        var color = (Color)instance?.GetType()?
            .GetProperty(status.ToString())?
            .GetValue(instance)!;
        return $"{color.R:X2}{color.G:X2}{color.B:X2}";
    }
}
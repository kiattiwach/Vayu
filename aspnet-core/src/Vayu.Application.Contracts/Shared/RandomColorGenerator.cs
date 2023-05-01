namespace Vayu.Shared;

using System;
using System.Collections.Generic;
using System.Drawing;

public static class RandomColorGenerator
{
    private static readonly Random random = new Random();

    public static List<Color> Generate(int count)
    {
        List<Color> result = new List<Color>();

        // Generate an initial random color
        Color color = GenerateRandomColor();

        // Add the initial color to the result list
        result.Add(color);

        // Generate additional colors until the desired count is reached
        while (result.Count < count)
        {
            // Generate a new color
            Color newColor = GenerateRandomColor();

            // Check if the new color is distinguishable from all existing colors in the result list
            bool isDistinguishable = true;
            foreach (Color existingColor in result)
            {
                if (!IsDistinguishable(newColor, existingColor))
                {
                    isDistinguishable = false;
                    break;
                }
            }

            // If the new color is distinguishable, add it to the result list
            if (isDistinguishable)
            {
                result.Add(newColor);
            }
        }

        return result;
    }

    private static Color GenerateRandomColor()
    {
        // Generate random RGB values between 0 and 255
        int red = random.Next(256);
        int green = random.Next(256);
        int blue = random.Next(256);

        // Return a new Color object with the generated RGB values
        return Color.FromArgb(red, green, blue);
    }

    private static bool IsDistinguishable(Color color1, Color color2)
    {
        // Calculate the Euclidean distance between the two colors in RGB space
        int redDiff = color1.R - color2.R;
        int greenDiff = color1.G - color2.G;
        int blueDiff = color1.B - color2.B;
        double distance = Math.Sqrt(redDiff * redDiff + greenDiff * greenDiff + blueDiff * blueDiff);

        // If the distance is greater than a certain threshold, the colors are distinguishable
        return distance > 100;
    }

    public static string GeneratePastelRgbaColors(this Color color, decimal backgroundOpacity)
    {
        int r = Convert.ToInt16(color.R);
        int g = Convert.ToInt16(color.G);
        int b = Convert.ToInt16(color.B);
        return $"rgba({r}, {g}, {b}, {backgroundOpacity})";
    }
}

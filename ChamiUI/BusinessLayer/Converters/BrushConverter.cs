using System;
using System.Windows.Media;
using ChamiUI.DataLayer.Entities;

namespace ChamiUI.BusinessLayer.Converters
{
    public class BrushConverter
    {
        public Brush Convert(Setting entity)
        {
            byte green = 0;
            byte red = 0;
            byte blue = 0;
            if (entity.Type == "System.Windows.Media.SolidColorBrush")
            {
                var redPart = entity.Value.Substring(1, 2);
                var greenPart = entity.Value.Substring(3, 2);
                var bluePart = entity.Value.Substring(5, 2);
                red = System.Convert.FromHexString(redPart)[0];
                blue = System.Convert.FromHexString(bluePart)[0];
                green = System.Convert.FromHexString(greenPart)[0];
                return new SolidColorBrush(Color.FromRgb(red, green, blue));
            }

            return null;
        }
    }
}
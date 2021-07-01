using System.Windows.Media;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer.Converters
{
    /// <summary>
    /// Converts a <see cref="Setting"/> entity whose Type property is System.Windows.Media.SolidColorBrush to a Brush object.
    /// </summary>
    public class BrushConverter
    {
        /// <summary>
        /// Converts a <see cref="Setting"/> entity whose Type property is System.Windows.Media.SolidColorBrush to a Brush object.
        /// The Value property in the <see cref="Setting"/> entity must be a string in the format #FFFFFFFF or #FFFFFF
        /// </summary>
        /// <param name="entity">The <see cref="Setting"/> to convert to a <see cref="Brush"/> object.</param>
        /// <returns>A <see cref="Brush"/> object.</returns>
        public Brush Convert(Setting entity)
        {
            if (entity.Type == "System.Windows.Media.SolidColorBrush")
            {
                // Converting a SolidColorBrush to string creates a string 9 characters long.
                // The first two characters are the alpha channel, which we aren't using.
                var offset = 1;
                if (entity.Value.Length == 9)
                {
                    offset += 2;
                }
                var redPart = entity.Value.Substring(offset, 2);
                var greenPart = entity.Value.Substring(offset + 2, 2);
                var bluePart = entity.Value.Substring(offset + 4, 2);
                var red = System.Convert.FromHexString(redPart)[0];
                var blue = System.Convert.FromHexString(bluePart)[0];
                var green = System.Convert.FromHexString(greenPart)[0];
                return new SolidColorBrush(Color.FromRgb(red, green, blue));
            }

            return null;
        }
    }
}
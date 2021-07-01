namespace ChamiUI.BusinessLayer.Converters
{
    /// <summary>
    /// Helper class used to convert strings to other types.
    /// </summary>
    public static class StringConverter
    {
        /// <summary>
        /// Converts a string to a boolean value.
        /// </summary>
        /// <param name="value">The value to convert to boolean.</param>
        /// <returns>If the value parameter is "true" or "1", returns true. If the value parameter is null, empty, or whitespeace, returns false. In all other cases, returns false.</returns>
        public static bool StringToBool(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (value.ToLowerInvariant() == "true" || value == "1")
            {
                return true;
            }

            return false;
        }
    }
}
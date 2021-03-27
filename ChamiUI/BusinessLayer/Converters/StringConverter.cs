namespace ChamiUI.BusinessLayer.Converters
{
    public static class StringConverter
    {
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
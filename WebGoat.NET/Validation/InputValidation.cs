using System;
using System.Text.RegularExpressions;
// Dette er den nye version, med opdateret Domæne-primitiv for Country.
namespace WebGoat.Validation
{
    public static class InputValidation
    {
        // Function to validate and normalize a country name
        public static string ValidateAndNormalizeCountry(string countryName)
        {
            // Check for null or whitespace
            if (string.IsNullOrWhiteSpace(countryName))
            {
                throw new ArgumentException("Country name cannot be null or empty.");
            }

            // Trim leading and trailing spaces
            countryName = countryName.Trim();

            // Recheck for empty input after trimming
            if (countryName.Length == 0)
            {
                throw new ArgumentException("Country name cannot be empty or only spaces.");
            }

            // Check length constraints
            if (countryName.Length < 2 || countryName.Length > 60)
            {
                throw new ArgumentException("Country name must be between 2 and 60 characters long.");
            }

            // Check allowed characters: A-Z (case insensitive) and spaces
            string pattern = @"^[A-Za-z\s]+$";
            if (!Regex.IsMatch(countryName, pattern))
            {
                throw new ArgumentException("Country name can only contain letters and spaces.");
            }

            // Normalize capitalization (e.g., "united states" -> "United States")
            countryName = NormalizeCapitalization(countryName);

            // Return the validated and normalized country name
            return countryName;
        }

        // Helper function to normalize capitalization
        private static string NormalizeCapitalization(string countryName)
        {
            string[] words = countryName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
            return string.Join(' ', words);
        }
    }
}

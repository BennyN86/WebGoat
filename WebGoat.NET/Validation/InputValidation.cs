using System;
using System.Text.RegularExpressions;

namespace WebGoat.Validation
{
    public static class InputValidation
    {
        /// <summary>
        /// Validates and normalizes a country name input.
        /// </summary>
        /// <param name="countryName">The input country name to validate.</param>
        /// <returns>The validated and normalized country name.</returns>
        /// <exception cref="ArgumentException">Thrown when the country name fails validation.</exception>
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

            // Check allowed characters: A-Z (any casing) and spaces
            string pattern = @"^[A-Za-z\s]+$";
            if (!Regex.IsMatch(countryName, pattern))
            {
                throw new ArgumentException("Country name can only contain letters and spaces.");
            }

            // Normalize capitalization (ensure the first letter of each word is uppercase)
            return NormalizeCapitalization(countryName);
        }

        /// <summary>
        /// Normalizes the capitalization of a country name, ensuring the first letter of each word is uppercase.
        /// </summary>
        /// <param name="countryName">The country name to normalize.</param>
        /// <returns>The country name with the first letter of each word capitalized.</returns>
        private static string NormalizeCapitalization(string countryName)
        {
            string[] words = countryName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(words[i]))
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
                }
            }
            return string.Join(' ', words);
        }

        /// <summary>
        /// Adds any future validation logic for other input fields.
        /// </summary>
        public static bool ValidateInput(string input)
        {
            // Placeholder for other input validations
            throw new NotImplementedException();
        }
    }
}

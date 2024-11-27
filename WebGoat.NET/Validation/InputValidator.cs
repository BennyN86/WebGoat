using System.Text.RegularExpressions;

namespace WebGoatCore.Validation
{
    public static class InputValidator
    {
        /// <summary>
        /// Validates general user input against specific rules.
        /// Allows letters (A-Z, a-z), numbers (0-9), and selected special characters.
        /// Input must not exceed 2400 characters.
        /// </summary>
        /// <param name="input">The user input to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public static bool ValidateInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false; // Input cannot be empty or whitespace.

            if (input.Length > 2400)
                return false; // Input exceeds the maximum allowed length.

            // Regex pattern to allow only letters, numbers, and selected special characters
            string pattern = @"^[A-Za-z0-9.,;:!?@#$%^&*()\-_=+/\\|<>\[\]{}~ '""?]+$";




            return Regex.IsMatch(input, pattern);
        }
    }
}

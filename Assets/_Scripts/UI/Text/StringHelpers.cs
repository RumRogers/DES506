using System.Collections;
using System.Collections.Generic;

namespace UI.Text
{
    public static class StringHelpers
    {
        /// <summary>
        /// Add the 'next' character from the full string to the partially complete string.
        /// Call in a loop to gradually fill the partial string.
        /// There's no verification for whether the strings match, so watch out for that!
        /// </summary>
        /// <param name="partial string"></param>
        /// <param name="full string"></param>
        public static void AddNextChar(ref string partial, string full)
        {
            if (partial.Length >= full.Length || string.IsNullOrEmpty(full)) return;

            partial += full[partial.Length];
            
        }
    }
}
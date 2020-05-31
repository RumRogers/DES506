using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Dialogue
{
    public static class StringHelpers
    {
        /// <summary>
        /// Add the 'next' character from the full string to the partially complete string.
        /// Call in a loop to gradually fill the partial string.
        /// There's no verification for whether the strings match, so watch out for that!
        /// </summary>
        /// <param name="partial">The string you're writing to.</param>
        /// <param name="full">The string you're copying.</param>
        public static void AddNextChar(ref string partial, string full)
        {
            if (partial.Length >= full.Length || string.IsNullOrEmpty(full)) return;

            partial += full[partial.Length];
        }

        /// <summary>
        /// Add the 'next' character from the full string to the partially complete string.
        /// Call in a loop to gradually fill the partial string.
        /// There's no verification for whether the strings match, so watch out for that!
        /// </summary>
        /// <param name="full">The string you're copying.</param>
        public static void AddNextChar(this string partial, string full)
        {
            if (partial.Length >= full.Length || string.IsNullOrEmpty(full)) return;

            partial += full[partial.Length];
        }

        /// <summary>
        /// Function which checks whether two strings have the same length. Function named in the context of other fucntions in this class. 
        /// In the interest of performance, this doesn't actually check whether the strings match.
        /// I trust you (me) not to be stupid! (Probably a mistake).
        /// </summary>
        /// <returns>Returns True if the strings are the same length (and implicitly the "AddNextChar" loop is finished).</returns>
        public static bool IsFull(string partial, string full)
        {
            if (partial.Length == full.Length)
            {
                return true;
            }
            return false;
        }

        public static IEnumerator FillDialogueBox(Text text, string line, float delta)
        {
            string tmp = "";

            while (text.text.Length < line.Length)
            {

                AddNextChar(ref tmp, line);
                text.text = tmp;

                yield return new WaitForSeconds(delta);
            }
        }
    }
}
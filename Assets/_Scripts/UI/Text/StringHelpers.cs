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
        /// <param name="partial string"></param>
        /// <param name="full string"></param>
        public static void AddNextChar(ref string partial, string full)
        {
            if (partial.Length >= full.Length || string.IsNullOrEmpty(full)) return;

            partial += full[partial.Length];
        }

        public static void AddNextChar(this string partial, string full)
        {
            if (partial.Length >= full.Length || string.IsNullOrEmpty(full)) return;

            partial += full[partial.Length];
        }

        public static IEnumerator FillDialogueBox(Text text, string line, float delta)
        {
            string tmp = "";

            while (text.text.Length < line.Length)
            {

                StringHelpers.AddNextChar(ref tmp, line);
                text.text = tmp;

                yield return new WaitForSeconds(delta);
            }
        }
    }
}
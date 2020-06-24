using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCore.Utils
{
    public static class Bits
    {
        public static bool GetBit(int n, int pos)
        {
            // 11010110 <- get 5th bit, pos = 4
            //   AND
            // 00010000 <- 1 << pos
            //----------
            // 00010000 != 0 -> true

            return (n & (1 << pos)) != 0;
        }

        public static void SetBit(ref int n, int pos)
        {
            // 11010110 <- set 5th bit, pos = 5
            //   OR
            // 00100000 <- 1 << pos
            //----------
            // 11110110

            n |= (1 << pos);
        }

        public static string GetBinaryString(int n)
        {
            string bin = "";

            int bits = 8 * sizeof(int);
            for (int i = 1; i <= bits; i++)
            {
                bin += GetBit(n, bits - i) ? '1' : '0';                
            }

            return bin;
        }
    }
}


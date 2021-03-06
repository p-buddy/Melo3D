using System;
using UnityEngine;

namespace DefaultNamespace
{
    public static class Notes
    {
        private const int NoteCount = 12 * 4;
        public static readonly float[] Frequencies = new float[NoteCount];

        static Notes()
        {
            Frequencies[0] = 65.41f;
            for (int i = 1; i < NoteCount; i++)
            {
                Frequencies[i] = Frequencies[i - 1] * Mathf.Pow(2, 1 / (float)12);
            }
        }
    }
}
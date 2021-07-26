using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player : MonoBehaviour
    {
        private static double? frequency;
        private double increment;
        private double phase;
        private double samplingFrequency = 48000.0;
        private float gain = 0.1f;
        private void OnAudioFilterRead(float[] data, int channels)
        {
            if (!frequency.HasValue)
            {
                return;
            }
            
            increment = frequency.Value * 2.0 * Mathf.PI / samplingFrequency;

            for (int i = 0; i < data.Length; i += channels)
            {
                phase += increment;
                data[i] = gain * Mathf.Sin((float) phase);

                if (channels == 2)
                {
                    data[i + 1] = data[i];
                }

                if (phase > (Mathf.PI * 2))
                {
                    phase = 0.0;
                }
            }
        }

        public void SetFrequency(double freq)
        {
            frequency = freq;
        }
        
        public static void Play(int octave, float angleDeg)
        {
            try
            {
                int start = (int) (angleDeg / 30f);
                float weight = (angleDeg - 30f * start) / 30f;
                frequency = Mathf.Lerp(Notes.Frequencies[start + 12 * octave], Notes.Frequencies[start + 12 * octave], weight);
            }
            catch
            {
                
            }
        }
    }
}
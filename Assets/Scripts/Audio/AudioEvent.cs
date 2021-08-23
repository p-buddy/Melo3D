using System;
using NoteElements;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace.Audio
{
    [BurstCompile]
    public readonly struct AudioEvent
    {
        private const double SamplingFrequency = 48000.0;
        
        public float StartTime { get; }
        public float Duration { get; }
        public float EndTime { get; }
        
        private readonly float phase;
        public delegate float2 GetCoordinateDelegate(float interpolationPoint);
        private readonly GetCoordinateDelegate getCoordinate;

        public AudioEvent(float startTime, float duration, GetCoordinateDelegate getCoordinate)
        {
            StartTime = startTime;
            Duration = duration;
            EndTime = startTime + duration;
            phase = 0f;
            this.getCoordinate = getCoordinate;
        }

        public AudioEvent(in AudioEvent previous, float newPhase)
        {
            StartTime = previous.StartTime;
            Duration = previous.Duration;
            EndTime = previous.EndTime;
            getCoordinate = previous.getCoordinate;
            phase = newPhase;
        }

        public void SetData(float[] data, int channels, float currentTime, out AudioEvent updatedEvent)
        {
            float windowLength = data.Length / (float)channels / (float) SamplingFrequency;
            if (currentTime < StartTime || currentTime + windowLength > EndTime)
            {
                updatedEvent = this;
                return;
            }

            float timeOffset = currentTime - StartTime;
            float timeIncrement = data.Length / (float)SamplingFrequency;
            float newPhase = phase;
            int sampleCount = data.Length / channels;
            
            for (int i = 0; i < data.Length; i += channels)
            {
                if (currentTime + timeOffset > StartTime + Duration)
                {
                    break;
                }
                float2 coordinate = getCoordinate.Invoke(timeOffset / Duration);
                float frequency = CoordinateTo.Frequency(coordinate);
                float gain = CoordinateTo.Gain(coordinate);
                
                newPhase += GetIncrement(frequency);
                data[i] = gain * Mathf.Sin(newPhase);

                if (channels == 2)
                {
                    data[i + 1] = data[i];
                }

                if (newPhase > (math.PI * 2))
                {
                    newPhase = 0.0f;
                }

                timeOffset += timeIncrement * i / sampleCount;
            }

            updatedEvent = new AudioEvent(in this, newPhase);
        }
            
        [BurstCompile]
        private static int GetIncrement(float frequency)
        {
            return (int)(frequency * 2.0 * math.PI / SamplingFrequency);
        }
    }
}
using System;
using System.Collections.Generic;
using DefaultNamespace.Audio;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player : MonoBehaviour
    {
        private readonly List<AudioEvent> audioEvents = new List<AudioEvent>();
        private float startTime;
        
        private void OnAudioFilterRead(float[] data, int channels)
        {
            for (var index = 0; index < audioEvents.Count; index++)
            {
                audioEvents[index].SetData(data, channels, Time.time - startTime, out AudioEvent updatedEvent);
                audioEvents[index] = updatedEvent;
            }
        }

        public void EnqueueEvents(AudioEvent[] events)
        {
            if (events == null)
            {
                return;
            }
            audioEvents.AddRange(events);
        }

        public void ClearEvents()
        {
            audioEvents.Clear();
        }

        public void PlayEvents()
        {
            startTime = Time.time;
        }
    }
}
using System.Collections.Generic;
using DefaultNamespace.Audio;
using UIControllers;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

namespace NoteElements
{
    public class NoteStart : INoteStart
    {
        public float2? Current => noteStartUI.GetData();
        public float? CurrentDuration => noteStartUI.GetDuration();

        private readonly INoteStartUI noteStartUI;
        private readonly ITrack track;

        public NoteStart(in UIComponentsContainer uiContainer, ITrack track)
        {
            noteStartUI = uiContainer.GetUIComponent<INoteStartUI>();
            Assert.IsNotNull(noteStartUI);

            this.track = track;

            noteStartUI.OnUIEdit += OnEdit;
        }

        private void OnEdit(float2? previous, float2? current)
        {
            track.StartingCoordinateChange(current);
        }

        public bool TryAppendAudioEvents(List<AudioEvent> events, float currentTime)
        {
            if (Current.HasValue && CurrentDuration.HasValue)
            {
                float2 current = CurrentDuration.Value;
                events.Add(new AudioEvent(currentTime, CurrentDuration.Value, point => current));
                return true;
            }

            return false;
        }
    }
}
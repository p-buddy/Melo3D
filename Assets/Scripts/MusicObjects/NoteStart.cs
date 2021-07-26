using DataUIInterface;
using NUnit.Framework;
using UI;
using Unity.Mathematics;
using UnityEngine;

namespace MusicObjects
{
    public class NoteStart : INoteStart
    {
        public float2? Current => noteStartUI.GetData();

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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Audio;
using UIControllers;
using Unity.Mathematics;
using UnityEngine;

namespace NoteElements
{
    /// <summary>
    /// Implementation of <see cref="ITrack"/>.
    /// </summary>
    public class Track : ITrack
    {
        public int ActionsCount => actionResultBundles.Count;

        private readonly NoteElementsContainer container;

        private readonly NoteUIBundle<INoteStart> startingNoteBundle;
        private readonly List<NoteUIBundle<INoteActionResult>> actionResultBundles;
        private readonly NoteUIBundle<IActionAdder> adderBundle;

        public Track(in UIComponentsContainer topLevelUIContainer,
                     in UIComponentsContainer startingNoteUIContainer,
                     in UIComponentsContainer adderUIContainer)
        {
            actionResultBundles = new List<NoteUIBundle<INoteActionResult>>();
            container = topLevelUIContainer.GetUIComponent<NoteElementsContainer>();

            startingNoteBundle = new NoteUIBundle<INoteStart>(new NoteStart(startingNoteUIContainer, this), startingNoteUIContainer);
            adderBundle = new NoteUIBundle<IActionAdder>(new ActionAdder(), adderUIContainer);

            container.Append(startingNoteBundle.UIContainer);
            container.Append(adderBundle.UIContainer);
        }
        
        public float2? GetInput(int myIndex)
        {
            string context = $"{typeof(Track)}::{nameof(GetInput)}() --";
            context = this.Context(nameof(GetInput));
            
            if (myIndex == 0)
            {
                return startingNoteBundle.NoteElement.Current;
            }
            
            if (actionResultBundles.Count == 0)
            {
                throw new InvalidOperationException($"{context} No actions have been queued");
            }
            
            if (myIndex >= actionResultBundles.Count || myIndex < 0)
            {
                throw new ArgumentException($"{context} Index of {myIndex} is not valid");
            }

            return actionResultBundles[myIndex - 1].NoteElement.CurrentResult;
        }

        public float2? GetResult()
        {
            if (actionResultBundles.Count == 0)
            {
                return startingNoteBundle.NoteElement.Current;
            }

            return actionResultBundles[ActionsCount - 1].NoteElement.CurrentResult;
        }

        public void StartingCoordinateChange(float2? value)
        {
            foreach (NoteUIBundle<INoteActionResult> actionResult in actionResultBundles)
            {
                actionResult.NoteElement.RefreshResult();
            }
            Drawer.DrawVector(GetResult());
        }

        public void ActionDataChange<TData>(TData? value, int myIndex) where TData : struct
        {
            for (int i = myIndex + 1; i < actionResultBundles.Count; i++)
            {
                actionResultBundles[i].NoteElement.RefreshResult();
            }
            Drawer.DrawVector(GetResult());
        }

        public void RemoveAtIndex(int index)
        {
            actionResultBundles.RemoveAt(index);
            UpdateActionResultsIndices(index);
            ResizeChildren();
        }

        public void AddAtIndex(in NoteUIBundle<INoteActionResult> actionResultBundle, int index)
        {
            actionResultBundles.Insert(index, actionResultBundle);
            UpdateActionResultsIndices(index + 1);
        }
        
        public void Add(in NoteUIBundle<INoteActionResult> actionResultBundle)
        {
            actionResultBundles.Add(actionResultBundle);
            container.Append(actionResultBundle.UIContainer);
            container.Append(adderBundle.UIContainer);
            actionResultBundle.NoteElement.SetIndexWithinTrack(ActionsCount - 1);
            ResizeChildren();
        }
        
        public AudioEvent[] GetEvents()
        {
            List<AudioEvent> audioEvents = new List<AudioEvent>();
            float currentTime = 0f;
            if (!startingNoteBundle.NoteElement.TryAppendAudioEvents(audioEvents, currentTime))
            {
                return null;
            }
            currentTime = audioEvents.Last().EndTime;
            
            foreach (NoteUIBundle<INoteActionResult> actionResultBundle in actionResultBundles)
            {
                if (!actionResultBundle.NoteElement.TryAppendAudioEvents(audioEvents, currentTime))
                {
                    return audioEvents.ToArray();
                }

                currentTime = audioEvents.Last().EndTime;
            }
            
            return audioEvents.ToArray();
        }

        private void UpdateActionResultsIndices(int index)
        {
            for (int i = index; i < actionResultBundles.Count; i++)
            {
                actionResultBundles[i].NoteElement.SetIndexWithinTrack(i);
            }
        }

        private void ResizeChildren()
        {
            List<NoteElement> noteElements = new List<NoteElement>();
            noteElements.Add(startingNoteBundle.UIContainer.GetUIComponent<NoteElement>());
            float maxHeight = noteElements[0].OriginalSize.y;
            
            foreach (NoteUIBundle<INoteActionResult> actionResultBundle in actionResultBundles)
            {
                foreach (NoteElement noteElement in actionResultBundle.UIContainer.GetUIComponents<NoteElement>())
                {
                    noteElements.Add(noteElement);
                    maxHeight = (noteElement.OriginalSize.y > maxHeight) ? noteElement.OriginalSize.y : maxHeight;
                }
            }

            foreach (NoteElement noteElement in noteElements)
            {
                noteElement.SetHeight(maxHeight);
            }
        }
    }
}
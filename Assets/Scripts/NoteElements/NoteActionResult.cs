using System;
using System.Collections.Generic;
using UIControllers;
using DefaultNamespace;
using DefaultNamespace.Audio;
using NUnit.Framework;
using Unity.Mathematics;

/*
 * NOTES:
 * - UI holds onto internal state (like its internal values, duration, etc.)
 * --- Essentially any data that only one piece of UI needs to know about, UI is allowed to control that
 * - This class WILL coordinate updates between encapsulated UI
 * - This class WILL coordinate actions relating to the "Track" object
 */

namespace NoteElements
{
    public class NoteActionResult<TActionData> : INoteActionResult where TActionData : struct
    {
        private readonly NoteUIBundle<ITrack> trackBundle;
        private int trackIndex;
        
        private readonly NoteActionFunctions.NoteAction<TActionData> action;
        private readonly INoteActionUI<TActionData> noteActionUI;
        private readonly INoteResultUI noteResultUI;
        public float2? CurrentInput => trackBundle.NoteElement.GetInput(trackIndex);
        public TActionData? CurrentActionData => noteActionUI.GetData();
        public float2? CurrentResult => noteResultUI.GetData();
        public float? CurrentActionDuration => noteActionUI.GetDuration();
        public float? CurrentResultDuration => noteResultUI.GetDuration();
        
        public NoteActionResult(NoteUIBundle<ITrack> trackBundle,
                                in NoteActionFunctions.NoteAction<TActionData> action,
                                in UIComponentsContainer uiContainer)
        {
            this.trackBundle = trackBundle;
            this.action = action;

            noteActionUI = uiContainer.GetUIComponent<INoteActionUI<TActionData>>();
            noteResultUI = uiContainer.GetUIComponent<INoteResultUI>();
            
            Assert.IsNotNull(noteActionUI);
            Assert.IsNotNull(noteResultUI);
            
            noteActionUI.OnUIEdit += OnActionEdit;
            noteActionUI.OnUIDelete += Delete;
        }

        private void OnActionEdit(TActionData? previous, TActionData? current)
        {
            RefreshResult();
            trackBundle.NoteElement.ActionDataChange(current, trackIndex);
        }

        public void RefreshResult()
        {
            if (!CurrentInput.HasValue || !CurrentActionData.HasValue)
            {
                noteResultUI.Set(null);
                return;
            }
            
            noteResultUI.Set(action.Invoke(CurrentInput.Value, CurrentActionData.Value));
        }

        private void Delete()
        {
            trackBundle.NoteElement.RemoveAtIndex(trackIndex);
        }

        public void SetIndexWithinTrack(int index)
        {
            trackIndex = index;
        }

        public bool TryAppendAudioEvents(List<AudioEvent> events, float currentTime)
        {
            if (CurrentActionData.HasValue && CurrentActionDuration.HasValue && CurrentInput.HasValue)
            {
                float2 input = CurrentInput.Value;
                TActionData actionData = CurrentActionData.Value;
                
                events.Add(new AudioEvent(currentTime,
                    CurrentActionDuration.Value,
                    point => action.Invoke(input, actionData, point)));

                float resultDuration = CurrentResultDuration ?? 0f;
                float2 result = CurrentResult ?? float2.zero;
                events.Add(new AudioEvent(currentTime + CurrentActionDuration.Value, resultDuration, point => result));
                return true;
            }

            return false;
        }
    }
}
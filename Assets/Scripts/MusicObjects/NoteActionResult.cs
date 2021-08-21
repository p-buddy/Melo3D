using System;
using UIControllers;
using DefaultNamespace;
using NUnit.Framework;
using Unity.Mathematics;

/*
 * NOTES:
 * - UI holds onto internal state (like its internal values, duration, etc.)
 * --- Essentially any data that only one piece of UI needs to know about, UI is allowed to control that
 * - This class WILL coordinate updates between encapsulated UI
 * - This class WILL coordinate actions relating to the "Track" object
 */

namespace MusicObjects
{
    public class NoteActionResult<TActionData> : INoteActionResult where TActionData : struct
    {
        private readonly ITrack track;
        private int trackIndex;
        
        private readonly NoteActionFunctions.NoteAction<TActionData> action;
        private readonly INoteActionUI<TActionData> noteActionUI;
        private readonly INoteResultUI noteResultUI;
        public float2? CurrentInput => track.GetInput(trackIndex);
        public TActionData? CurrentActionData => noteActionUI.GetData();
        public float2? CurrentResult => noteResultUI.GetData();
        public float? CurrentActionDuration => noteActionUI.GetDuration();
        public float? CurrentResultDuration => noteResultUI.GetDuration();
        
        public NoteActionResult(ITrack track,
                                int index,
                                NoteActionFunctions.NoteAction<TActionData> action,
                                in UIComponentsContainer uiContainer)
        {
            this.track = track;
            trackIndex = index;
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
            track.ActionDataChange(current, trackIndex);
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
            track.RemoveAtIndex(trackIndex);
        }

        public void SetIndexWithinTrack(int index)
        {
            trackIndex = index;
        }
    }
}
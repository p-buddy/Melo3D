using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UIControllers;
using Unity.Mathematics;
using UnityEngine;

namespace MusicObjects
{
    public class Track : ITrack
    {
        public int ActionsCount => actionResults.Count;
        private NoteElementsContainer container;

        private ControllerUIBundle<INoteStart> startingNote;
        private List<ControllerUIBundle<INoteActionResult>> actionResults;
        private ControllerUIBundle<IActionAdder> adder;

        public Track(in UIComponentsContainer uiContainer,
                     in UIComponentsContainer startingNoteContainer,
                     in UIComponentsContainer adderContainer)
        {
            actionResults = new List<ControllerUIBundle<INoteActionResult>>();
            container = uiContainer.GetUIComponent<NoteElementsContainer>();

            startingNote = new ControllerUIBundle<INoteStart>(new NoteStart(startingNoteContainer, this), startingNoteContainer);
            adder = new ControllerUIBundle<IActionAdder>(new ActionAdder(), adderContainer);

            container.Append(startingNote.UIContainer);
            container.Append(adder.UIContainer);
        }
        
        public float2? GetInput(int myIndex)
        {
            string context = $"{typeof(Track)}::{nameof(GetInput)}() --";
            
            if (myIndex == 0)
            {
                return startingNote.Controller.Current;
            }
            
            if (actionResults.Count == 0)
            {
                throw new InvalidOperationException($"{context} No actions have been queued");
            }
            
            if (myIndex >= actionResults.Count || myIndex < 0)
            {
                throw new ArgumentException($"{context} Index of {myIndex} is not valid");
            }

            return actionResults[myIndex - 1].Controller.CurrentResult;
        }

        public float2? GetResult()
        {
            if (actionResults.Count == 0)
            {
                return startingNote.Controller.Current;
            }

            return actionResults[ActionsCount - 1].Controller.CurrentResult;
        }

        public void StartingCoordinateChange(float2? value)
        {
            foreach (ControllerUIBundle<INoteActionResult> actionResult in actionResults)
            {
                actionResult.Controller.RefreshResult();
            }
            Drawer.DrawVector(GetResult());
        }

        public void ActionDataChange<TData>(TData? value, int myIndex) where TData : struct
        {
            for (int i = myIndex + 1; i < actionResults.Count; i++)
            {
                actionResults[i].Controller.RefreshResult();
            }
            Drawer.DrawVector(GetResult());
        }

        public void RemoveAtIndex(int index)
        {
            actionResults.RemoveAt(index);
            UpdateActionResultsIndices(index);
            ResizeChildren();
        }

        public void AddAtIndex(ControllerUIBundle<INoteActionResult> actionResultBundle, int index)
        {
            actionResults.Insert(index, actionResultBundle);
            UpdateActionResultsIndices(index + 1);
        }
        
        public void Add(ControllerUIBundle<INoteActionResult> actionResultBundle)
        {
            actionResults.Add(actionResultBundle);
            container.Append(actionResultBundle.UIContainer);
            container.Append(adder.UIContainer);
            ResizeChildren();
        }

        private void UpdateActionResultsIndices(int index)
        {
            for (int i = index; i < actionResults.Count; i++)
            {
                actionResults[i].Controller.SetIndexWithinTrack(i);
            }
        }

        private void ResizeChildren()
        {
            List<NoteElement> noteElements = new List<NoteElement>();
            noteElements.Add(startingNote.UIContainer.GetUIComponent<NoteElement>());
            float maxHeight = noteElements[0].OriginalSize.y;
            
            foreach (ControllerUIBundle<INoteActionResult> actionResultBundle in actionResults)
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
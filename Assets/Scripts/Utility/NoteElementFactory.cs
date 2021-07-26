using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataUIInterface;
using MusicObjects;
using UI;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public static class NoteElementFactory
    {
        private const string UIDirectory = "UI";

        private static GameObject ElementsRowUI = Resources.Load<GameObject>(Path.Combine(UIDirectory, "ElementsContainer"));
        
        private static GameObject MultiplicationUI = Resources.Load<GameObject>(Path.Combine(UIDirectory, "MultiplicationSign"));
        private static GameObject EqualsUI = Resources.Load<GameObject>(Path.Combine(UIDirectory, "EqualsSign"));

        private static GameObject AddButon = Resources.Load<GameObject>(Path.Combine(UIDirectory, "AddButton"));
        
        private static GameObject MatrixUI = Resources.Load<GameObject>(Path.Combine(UIDirectory, "MatrixElement"));
        private static GameObject StartVectorUI = Resources.Load<GameObject>(Path.Combine(UIDirectory, "RowVectorStartElement"));
        private static GameObject ResultVectorUI = Resources.Load<GameObject>(Path.Combine(UIDirectory, "RowVectorResultElement"));
        private static GameObject VectorAdditionUI = Resources.Load<GameObject>(Path.Combine(UIDirectory, "VectorAdditionElement"));

        public static ControllerUIBundle<ITrack> AddTrack(Transform parent)
        {
            GameObject row = Object.Instantiate(ElementsRowUI, parent);
            GameObject rowVector = Object.Instantiate(StartVectorUI);
            GameObject addButton = Object.Instantiate(AddButon);

            var parentContainer = new UIComponentsContainer(row);
            var startNoteContainer = new UIComponentsContainer(rowVector);
            var addButtonContainer = new UIComponentsContainer(addButton);

            ITrack track = new Track(parentContainer, startNoteContainer, addButtonContainer);
            return new ControllerUIBundle<ITrack>(track, parentContainer);
        }
        
        public static ControllerUIBundle<INoteActionResult> AddMatrixMultiplication(ITrack track)
        {
            GameObject multiplication = Object.Instantiate(MultiplicationUI);
            GameObject matrix = Object.Instantiate(MatrixUI);
            GameObject equals = Object.Instantiate(EqualsUI);
            GameObject resultVector = Object.Instantiate(ResultVectorUI);

            var uiContainer = new UIComponentsContainer(multiplication, matrix, equals, resultVector);
            INoteActionResult noteActionResult = new NoteActionResult<float2x2>(track, track.ActionsCount, NoteActionFunctions.MatrixMultiply, uiContainer);

            var bundle = new ControllerUIBundle<INoteActionResult>(noteActionResult, uiContainer);
            track.Add(bundle);
            return bundle;
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UIControllers;
using NoteElements;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public static class NoteElementFactory
    {
        private static string Context(string functionName)
        {
            return $"{nameof(NoteElementFactory)}::{functionName}() --";
        }
        private const string UIDirectory = "UI";
        public static readonly Dictionary<Type, List<GameObject>> UIPrefabsByClassType;
        
        private static GameObject InstantiateUIPrefab<T>(Transform parent,
                                                         Func<T, bool> validator = null,
                                                         bool showMultiValidError = true)
        {
            string context = Context(nameof(InstantiateUIPrefab));
            if (!UIPrefabsByClassType.TryGetValue(typeof(T), out List<GameObject> objects))
            {
                throw new ArgumentException($"{context} No UI prefabs of type '{typeof(T)}', cannot instantiate one");
            }

            if (validator is null)
            {
                if (objects.Count > 1)
                {
                    Debug.LogError($"{context} More than one prefab of type '{typeof(T)}' found. Consider using {nameof(validator)} argument to get only the prefab you desire.");
                }
                return (parent == null) ? Object.Instantiate(objects[0]) : Object.Instantiate(objects[0], parent);
            }

            GameObject toReturn = null;
            foreach (GameObject prefab in objects)
            {
                T uiComponent = prefab.GetComponentInChildren<T>();
                bool valid = validator.Invoke(uiComponent);
                if (!valid)
                {
                    continue;
                }
                
                if (toReturn == null)
                {
                    toReturn = (parent == null) ? Object.Instantiate(prefab) : Object.Instantiate(prefab, parent);
                    continue;
                }

                if (showMultiValidError)
                {
                    Debug.LogError($"{context} Multiple prefabs of type '{typeof(T)}' are valid. This is likely an error");
                }
            }

            if (toReturn == null)
            {
                Debug.LogError($"{context} No prefabs of type '{typeof(T)}' met validation criteria");
            }
            return toReturn;
        }

        private static GameObject InstantiateUIPrefab<T>(Func<T, bool> validator = null,
                                                         bool showMultiValidError = true)
        {
            return InstantiateUIPrefab(null, validator, showMultiValidError);
        }
        
        static NoteElementFactory()
        {
            UIPrefabsByClassType = new Dictionary<Type, List<GameObject>>();
            
            GameObject[] uiObjects = Resources.LoadAll<GameObject>(UIDirectory);
            foreach (GameObject prefab in uiObjects)
            {
                IUIComponent uiComponent = prefab.GetComponentInChildren<IUIComponent>();
                if (uiComponent is null)
                {
                    Debug.LogError($"{Context(nameof(NoteElementFactory))} Prefab '{prefab.name}' in Resources/{UIDirectory} does not contain an implementation of {nameof(IUIComponent)}");
                    continue;
                }

                Type classType = uiComponent.GetType();
                if (UIPrefabsByClassType.TryGetValue(classType, out List<GameObject> objects))
                {
                    objects.Add(prefab);
                    continue;
                }
                
                UIPrefabsByClassType.Add(classType, new List<GameObject>{prefab});
            }
        }
        
        public static NoteUIBundle<ITrack> AddTrack(Transform parent)
        {
            GameObject row = InstantiateUIPrefab<NoteElementsContainer>(parent);
            GameObject rowVector = InstantiateUIPrefab<Note2DStartUI>();
            GameObject addButton = InstantiateUIPrefab<AddButton>();

            var parentContainer = new UIComponentsContainer(row);
            var startNoteContainer = new UIComponentsContainer(rowVector);
            var addButtonContainer = new UIComponentsContainer(addButton);
            
            startNoteContainer.GetUIComponent<Note2DStartUI>().Set(new float2(1, 0));

            ITrack track = new Track(parentContainer, startNoteContainer, addButtonContainer);
            return new NoteUIBundle<ITrack>(track, parentContainer);
        }
        
        public static NoteUIBundle<INoteActionResult> AddMatrixMultiplication<TMultiplicationType>(NoteUIBundle<ITrack> trackBundle) 
            where TMultiplicationType : MatrixMultiplicationUI
        {
            GameObject multiplication = InstantiateUIPrefab<Sign>(ui => ui.SignType == Sign.Type.Multiplication);
            GameObject matrix = InstantiateUIPrefab<TMultiplicationType>();
            GameObject equals = InstantiateUIPrefab<Sign>(ui => ui.SignType == Sign.Type.Equals);
            GameObject resultVector = InstantiateUIPrefab<Note2DResultUI>();

            var uiContainer = new UIComponentsContainer(multiplication, matrix, equals, resultVector);
            
            uiContainer.GetUIComponent<TMultiplicationType>().Set(new float2x2(0));
            
            INoteActionResult noteActionResult = new NoteActionResult<float2x2>(trackBundle, NoteActionFunctions.MatrixMultiply, uiContainer);
            var bundle = new NoteUIBundle<INoteActionResult>(noteActionResult, uiContainer);
            trackBundle.NoteElement.Add(in bundle);
            return bundle;
        }
    }
}
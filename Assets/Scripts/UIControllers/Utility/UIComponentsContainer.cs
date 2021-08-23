using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UIControllers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UIControllers
{
    public readonly struct UIComponentsContainer
    {
        private static Type[] uiComponentTypes;
        static UIComponentsContainer()
        {
            uiComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
                                        .SelectMany(s => s.GetTypes())
                                        .Where(p => 
                                            typeof(IUIComponent).IsAssignableFrom(p) &&
                                            typeof(Component).IsAssignableFrom(p) &&
                                            p.IsClass &&
                                            !p.IsAbstract).ToArray();
        }

        private readonly Dictionary<Type, List<object>> componentsByType;
        private readonly GameObject[] gameObjects;

        public GameObject[] GameObjects => gameObjects;
        
        public UIComponentsContainer(params GameObject[] gameObjects)
        {
            componentsByType = new Dictionary<Type, List<object>>();
            foreach (Type uiComponentType in uiComponentTypes)
            {
                componentsByType[uiComponentType] = new List<object>();
                foreach (GameObject gameObject in gameObjects)
                {
                    componentsByType[uiComponentType].AddRange(gameObject.GetComponentsInChildren(uiComponentType));
                }
            }

            this.gameObjects = gameObjects;
        }

        public T[] GetUIComponents<T>() where T : class, IUIComponent
        {
            List<T> objs = new List<T>();
            foreach (KeyValuePair<Type, List<object>> kvp in componentsByType)
            {
                if (!typeof(T).IsAssignableFrom(kvp.Key))
                {
                    continue;
                }

                objs.AddRange(kvp.Value.Select(x => x as T).Where(x => x != null));
            }

            if (objs.Count == 0)
            {
                Debug.LogError($"{this.Context(nameof(GetUIComponents))} No objects of type '{typeof(T)}' could be found.");
                return null;
            }

            return objs.ToArray();
        }
        
        public T GetUIComponent<T>() where T : class, IUIComponent
        {
            T[] objs = GetUIComponents<T>();
            if (objs is null || objs.Length == 0)
            {
                return null;
            }

            if (objs.Length > 1 && objs.Distinct().Count() > 1)
            {
                string objsPrintOut = String.Join(", ", objs.ToList().Select(ob => ob.GetType()));
                Debug.LogError($@"{this.Context(nameof(GetUIComponent))} More than one object of type '{typeof(T)}' found. 
                                Returning the first non null element. [{objsPrintOut}]");
            }

            foreach (T obj in objs)
            {
                if (obj is null)
                {
                    continue;
                }

                return obj;
            }

            return null;
        }

        public void Delete()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                Object.Destroy(gameObject);
            }
        }
    }
}
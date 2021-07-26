using UnityEngine;

namespace DefaultNamespace
{
    public class SingletonBehaviour<T> : MonoBehaviour where T: SingletonBehaviour<T>
    {
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    if (container == null)
                    {
                        container = new GameObject("Singleton Container");
                    }

                    container.AddComponent<T>();
                }

                return instance;
            }
        }
        
        private static T instance;
        private static GameObject container;
 
        void Awake()
        {
            if (instance != null && instance != this)
            {
                string error = $"An instance of {this} singleton already exists.";
                Destroy(this);
                throw new System.Exception(error);
            }
        }
    }
}
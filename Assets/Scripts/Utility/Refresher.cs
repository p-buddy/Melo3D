using UnityEngine;

namespace DefaultNamespace
{
    public class Refresher
    {
        private readonly float refreshInterval;
        private float timeElapsed;
        
        public Refresher(float refreshFrequencyHz = 10f)
        {
            timeElapsed = 0f;
            refreshInterval = 1f / refreshFrequencyHz;
        }
        
        public virtual bool IsTimeToRefresh()
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed < refreshInterval)
            {
                return false;
            }
            timeElapsed = 0f;
            return true;
        }
    }
    
    public class EditModeRefresher : Refresher
    {
        public EditModeRefresher(float refreshFrequencyHz = 10f) : base(refreshFrequencyHz)
        {
        }
        
        public override bool IsTimeToRefresh()
        {
            if (Application.isPlaying)
            {
                return false;
            }

            return base.IsTimeToRefresh();
        }
    }
}
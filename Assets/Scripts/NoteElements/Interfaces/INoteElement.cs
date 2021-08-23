using System.Collections.Generic;
using DefaultNamespace.Audio;

namespace NoteElements
{
    public interface INoteElement
    {
        public bool TryAppendAudioEvents(List<AudioEvent> events, float currentTime);
    }
}
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using MusicObjects;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ControllerUIBundle<ITrack> track = NoteElementFactory.AddTrack(transform);
        NoteElementFactory.AddMatrixMultiplication(track.Controller);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using NoteElements;
using UIControllers;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NoteUIBundle<ITrack> trackBundle = NoteElementFactory.AddTrack(transform);
        var matrixBundle = NoteElementFactory.AddMatrixMultiplication<MatrixMultiplicationUI>(trackBundle);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

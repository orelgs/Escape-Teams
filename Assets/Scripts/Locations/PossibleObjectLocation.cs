using UnityEngine;

public class PossibleObjectLocation
{
    public Vector3 Position {get;}
    public Quaternion Rotation {get;}

    public PossibleObjectLocation(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}
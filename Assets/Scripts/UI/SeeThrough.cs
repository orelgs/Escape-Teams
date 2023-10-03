using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeeThrough : MonoBehaviour
{
    public Transform Target;
    public Transform Obstruction;
    float zoomSpeed = 2f;
    private Dictionary<Transform, UnityEngine.Rendering.ShadowCastingMode> originalShadowModes = new Dictionary<Transform, UnityEngine.Rendering.ShadowCastingMode>();


    // Start is called before the first frame update
    void Awake()
    {
        Obstruction = Target;
    }

    void LateUpdate()
    {
        ViewObstructed();
    }

    void ViewObstructed()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Target.position - transform.position, out hit, 1.5f) &&
            hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Ground" && hit.collider.gameObject.tag != "Door")
        {
            Obstruction = hit.transform;
            Obstruction.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            //if (Obstruction != null)
            if (!Obstruction.IsDestroyed())
            {
                Obstruction.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }

    }




}

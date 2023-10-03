using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoserFace : MonoBehaviour
{
    Animator animator;
    Renderer[] characterMaterials;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterMaterials = GetComponentsInChildren<Renderer>();
        Vector2 offset = Vector2.zero;
        offset = new Vector2(.33f, .66f);

        for (int i = 0; i < characterMaterials.Length; i++)
        {
            if (characterMaterials[i].transform.CompareTag("PlayerEyes"))
                characterMaterials[i].material.SetTextureOffset("_MainTex", offset);
        }

    }


}

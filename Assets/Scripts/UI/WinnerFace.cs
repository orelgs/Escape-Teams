using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerFace : MonoBehaviour
{
    Animator animator;
    Renderer[] characterMaterials;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterMaterials = GetComponentsInChildren<Renderer>();
        Vector2 offset = Vector2.zero;
        offset = new Vector2(.33f, 0);

        for (int i = 0; i < characterMaterials.Length; i++)
        {
            if (characterMaterials[i].transform.CompareTag("PlayerEyes"))
                characterMaterials[i].material.SetTextureOffset("_MainTex", offset);
        }
    }
}

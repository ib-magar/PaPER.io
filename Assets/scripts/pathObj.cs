using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathObj : MonoBehaviour
{
    public CharacterController characterController;
    private void Start()
    {
        
    }
    public void Initialize(CharacterController c)
    {
        characterController = c;
    }

}

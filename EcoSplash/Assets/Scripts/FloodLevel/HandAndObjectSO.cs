using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu]
public class HandAndObjectSO : ScriptableObject
{   
    [SerializeField] public Transform input;
    [SerializeField] public List<Transform> Tubs;
}



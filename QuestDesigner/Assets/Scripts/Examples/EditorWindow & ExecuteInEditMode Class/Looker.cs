using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Looker : MonoBehaviour
{
    public Transform tr;
	
	void Update ()
    {
        transform.LookAt(tr);
	}
}

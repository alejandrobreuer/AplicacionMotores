using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridSnap : MonoBehaviour
{
    public float gridX;
    public float gridY;
    public float gridZ;

    void Update ()
    {
        //chequeamos que solo ocurra durante el editor;
        //y chequeamos que la escala de la grilla no sea "0", para no tener errores
        if (!Application.isEditor || gridX == 0 || gridY == 0 || gridZ == 0) return;

        Transform[] childs = transform.GetComponentsInChildren<Transform>(); //conseguimos los childs
        for (int i = 0; i < childs.Length; i++)
        {
            //conseguimos la posición
            var pos = childs[i].position;
            
            //calculamos la diferencia segun la escala de la grilla
            var differenceX = pos.x % gridX;
            var differenceY = pos.y % gridY;
            var differenceZ = pos.z % gridZ;

            childs[i].position = new Vector3(pos.x - differenceX, pos.y - differenceY, pos.z - differenceZ);
        }
	}
}

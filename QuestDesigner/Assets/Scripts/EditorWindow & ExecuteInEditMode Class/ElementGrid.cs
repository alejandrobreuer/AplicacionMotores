using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ElementGrid : MonoBehaviour
{
    public int rowAmount;
    public int columnAmount;
    public int distanceX;
    public int distanceZ;
    public GameObject prefab;
    private Transform container;

    public float timeToSpawn = 0.5f;
    public float currenttimeToSpawn = 0;

    IEnumerator cor;

    private IEnumerator InstantianteElements()
    {
        for (int i = 0; i < rowAmount; i++)
        {
            var pos = transform.position + transform.right * distanceX * (i + 1);
            InstanceObject(pos);
            for (int t = 0; t < columnAmount - 1; t++)
            {
                var p = pos + transform.forward * distanceZ * (t + 1);
                InstanceObject(p);
                yield return null;
            }
        }
    }

    private void Update()
    {
        if (cor == null) return;

        currenttimeToSpawn += Time.deltaTime;
        if(currenttimeToSpawn > timeToSpawn)
        {
            currenttimeToSpawn = 0;
            cor.MoveNext();
        }
    }


    public void SpawnElements()
    {
        container = new GameObject().transform;
        cor = InstantianteElements();
    }

    private void InstanceObject(Vector3 currentPos)
    {
        var o = GameObject.Instantiate(prefab);
        o.transform.position = currentPos;
        o.transform.SetParent(container);
    }
}

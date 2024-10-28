using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setGround : MonoBehaviour
{
    public GameObject block;
    public GameObject badRockSites;
    public float scale = 1.0f;
    private float y = 0.5f;
    private bool setted=false;


    public void set()
    {
    if(!setted)
        {
            for (int x = -4; x < 5; x++)
            {
                for (int z = -4; z < 5; z++)
                {
                    GameObject blockSet = GameObject.Instantiate(block);
                    blockSet.transform.parent = badRockSites.transform;
                    blockSet.transform.localScale = new Vector3(scale, scale, scale);
                    blockSet.transform.localPosition = new Vector3(x, y, z);
                }
            }
            setted = true;
        }
    } 
}

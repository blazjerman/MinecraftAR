using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonListener : MonoBehaviour
{
    
    public GameObject[] blocks;

    public Animator m_Animator;//


    public GameObject hand;

    public void set(int name){


        

        
        variables.blockHold = blocks[name];
        if(name==8){
            variables.isSmall = true;
        }else{
            variables.isSmall = false;
        }

        if(name==2 || name==5){
            variables.canRotate = true;
        }else{
            variables.canRotate = false;
        }

        blockUpdate();//posodobi kocko ki jo dr�i player
        m_Animator.Play("");

    }

    //posodobi kocko ki jo dr�i player
    public void blockUpdate()
    {
        int size=1;
        float height=0;
        if(variables.isSmall){
            size=3;
            height=0.7f;
        }

        GameObject.Destroy(hand.transform.GetChild(0).gameObject);
        GameObject blockSet = GameObject.Instantiate(variables.blockHold.transform.GetChild(0).gameObject);
        blockSet.transform.parent = hand.transform;
        blockSet.transform.localPosition = new Vector3(0,height,0);
        blockSet.transform.localScale = new Vector3(size, size, size);
        blockSet.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
}

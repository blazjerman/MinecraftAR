using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class BlockListener : MonoBehaviour
{

   
    public GameObject allBlocksParent;
    

    public GameObject select;
    private GameObject selectItem;

    public GameObject destroy;
    private GameObject destroyItem;
    public Material[] destroyMaterials;

    public float scale=1.0f;
    
    private GameObject lastBlock;
    private bool notDelited=true;

    public Animator m_Animator;
    
    private float handTime = 0.23f;//Animacija roke
    private float time = 0.0f;
    private float handTime1 = 0.15f;//Uničevanje kocke
    private float time1 = 0.0f;
    private float holdTime = 0.8f;//Čas uničevanja
    private float acumTime = 0;
    

    

    

    private int timesHit = 0;

    void Start(){
        selectItem = GameObject.Instantiate(select);
        destroyItem = GameObject.Instantiate(destroy);
        selectItem.SetActive(false);
        destroyItem.SetActive(false);
    }

    void Update()
    {
        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit Hit;

            acumTime+=Input.GetTouch(0).deltaTime;

            //Naredi animacijo tovčenja, uničevanja kocke in sledi kocki
            if (Physics.Raycast(ray, out Hit))
            {
                if (Hit.collider != null)
                {
                    time += Time.deltaTime;
                    time1 += Time.deltaTime;
 
                    if (time >= handTime) {
                        time = 0.0f;
                        m_Animator.Play("");
                    }
                    if (time1 >= handTime1/2.0f) {
                        time1 = 0.0f;
                        updateDestroy(Hit,timesHit);
                        timesHit++;
                    }
                    followHand(Hit);
                }
            }

            //Ugotovi če je igralec držal 
            if (acumTime>=holdTime)
            {
                notDelited=false;
                if(Physics.Raycast(ray, out Hit)){
                            if(Hit.collider!=null){
                                removeBlock(Hit);
                                selectItem.SetActive(false);
                                destroyItem.SetActive(false);
                                timesHit=0;
                            }
                    }
                acumTime = 0;
            }else{
                if(Hit.collider!=null){
                    if(lastBlock!=Hit.collider.transform.parent.parent.gameObject){
                    acumTime = 0;
                    timesHit=0;
                    lastBlock=Hit.collider.transform.parent.parent.gameObject;
                    }
                }
            }

            //Ugotovi če je igralec samo pritisnil
            if( Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                acumTime = 0;
                if(notDelited)
                {
                    if(Physics.Raycast(ray, out Hit))
                    {
                        if(Hit.collider!=null){
                            if(variables.blockHold!=null){
                                addBlock(Hit);
                            }
                            m_Animator.Play("");
                        }
                    }
                }else
                {
                    notDelited=true;
                }

                selectItem.SetActive(false);
                destroyItem.SetActive(false);
                timesHit=0;
            }
        }
    }

    void addBlock(RaycastHit Hit){
        
        string name=Hit.transform.name;

        float x=Hit.collider.transform.parent.parent.localPosition.x;
        float y=Hit.collider.transform.parent.parent.localPosition.y;
        float z=Hit.collider.transform.parent.parent.localPosition.z;

        GameObject blockSet = GameObject.Instantiate(variables.blockHold);
        blockSet.transform.localScale = new Vector3(blockSet.transform.localScale.x*scale, blockSet.transform.localScale.y*scale, blockSet.transform.localScale.z*scale);
        blockSet.transform.parent = allBlocksParent.transform;
        blockSet.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        
        switch(name){
            case "right":
            blockSet.transform.localPosition = new Vector3(x, y, z-scale);
            break;
            case "left":
            blockSet.transform.localPosition = new Vector3(x, y, z+scale);
            break;
            case "top":
            blockSet.transform.localPosition = new Vector3(x, y+scale, z);
            break;
            case "bottom":
            blockSet.transform.localPosition = new Vector3(x, y-scale, z);
            break;
            case "front":
            blockSet.transform.localPosition = new Vector3(x-scale, y, z);
            break;
            case "back":
            blockSet.transform.localPosition = new Vector3(x+scale, y, z);
            break;
            default:
            Destroy(blockSet.transform.gameObject);
            break;  
        }
            

        if(variables.canRotate){
            switch(name){
                case "right":
                blockSet.transform.GetChild(0).GetChild(0).localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                break;
                case "left":
                blockSet.transform.GetChild(0).GetChild(0).localRotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
                break;
                case "top":
                blockSet.transform.GetChild(0).GetChild(0).localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                break;
                case "bottom":
                blockSet.transform.GetChild(0).GetChild(0).localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                break;
                case "front":
                blockSet.transform.GetChild(0).GetChild(0).localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                break;
                case "back":
                blockSet.transform.GetChild(0).GetChild(0).localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                break;
            }
        }
    
            
    
    
    }


    void removeBlock(RaycastHit Hit){
        Destroy(Hit.transform.gameObject);
        Destroy(Hit.transform.parent.gameObject);
        Destroy(Hit.transform.parent.parent.gameObject);
    }


    void updateDestroy(RaycastHit Hit,int i){
        if(i!=0){
            float x=Hit.collider.transform.parent.parent.localPosition.x;
            float y=Hit.collider.transform.parent.parent.localPosition.y;
            float z=Hit.collider.transform.parent.parent.localPosition.z;//

            destroyItem.transform.localScale = new Vector3(destroyItem.transform.localScale.x*scale, destroyItem.transform.localScale.y*scale, destroyItem.transform.localScale.z*scale);
            destroyItem.transform.parent = allBlocksParent.transform;
            destroyItem.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            destroyItem.transform.localPosition = new Vector3(x, y, z);
            destroyItem.SetActive(true);//
            for (int j = 0; j < 4; j++) 
            {
                destroyItem.transform.GetChild(0).GetChild(0).GetChild(j).GetComponent<Renderer>().material = destroyMaterials [i+1];
            }   
        }
                            
    }



    void followHand(RaycastHit Hit){
            float x=Hit.collider.transform.parent.parent.localPosition.x;
            float y=Hit.collider.transform.parent.parent.localPosition.y;
            float z=Hit.collider.transform.parent.parent.localPosition.z;//

            selectItem.transform.localScale = new Vector3(selectItem.transform.localScale.x*scale, selectItem.transform.localScale.y*scale, selectItem.transform.localScale.z*scale);
            selectItem.transform.parent = allBlocksParent.transform;
            selectItem.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            selectItem.transform.localPosition = new Vector3(x, y, z);
            selectItem.SetActive(true);
    }



 
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingEffectDisabler : MonoBehaviour
{

    public GameObject selectedGameObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "UIHelper")
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "UIHelper")
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}

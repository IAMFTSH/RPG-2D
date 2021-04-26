using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxCollController : MonoBehaviour
{
    GameObject Dialog;
     private void Awake() {
        Dialog=GameObject.Find("Canvas").gameObject.transform.Find("Dialog").gameObject;
     }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Dialog.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Dialog.SetActive(false);
        }
    }

}

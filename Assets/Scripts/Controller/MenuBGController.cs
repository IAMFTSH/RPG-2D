using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBGController : MonoBehaviour
{
    // Start is called before the first frame update
    public void EnableUI(){
        transform.parent.Find("UI").gameObject.SetActive(true);
    }
}

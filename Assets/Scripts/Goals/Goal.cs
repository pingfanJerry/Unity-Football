using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    void OnTriggerEnter()
    {
        GameManager.Instance.Goal(this.tag);
    }
}

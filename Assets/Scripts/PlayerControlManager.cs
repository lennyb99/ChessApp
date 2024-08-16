using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsMouseHovering(GameObject targetObject)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) 
        {
            if (hit.collider.gameObject == targetObject)
            {
                return true;
            }
        }
        return false;
    }

}

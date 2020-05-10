using UnityEngine;
using System.Collections;

public class TargetName : MonoBehaviour
{
    //private new Camera camera;
    public string targetsTag;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag(targetsTag))
                {
                    Debug.Log(hit.collider.gameObject.name);
                }

            }
        }
    }
}

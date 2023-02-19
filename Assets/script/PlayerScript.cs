using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    float baseSpeed;
    private void Start()
    {
        baseSpeed = (float)Variables.Object(this).Get("speed");
    }
    private void Update()
    {
        if(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.collider.tag == "Player"){
                    StopAllCoroutines();
                    StartCoroutine(speedBoost());
                }
            }
        }
    }

    IEnumerator speedBoost()
    {
        Variables.Object(this).Set("speed", baseSpeed + 1);
        yield return new WaitForSeconds(.25f);
        Variables.Object(this).Set("speed", baseSpeed);
    }
}

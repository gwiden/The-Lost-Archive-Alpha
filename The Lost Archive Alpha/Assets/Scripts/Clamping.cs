using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clamping : MonoBehaviour {

    [SerializeField] private float Xx, Yy, Zz;
    [SerializeField] private Vector3 pos;
    

	void Update () {
        pos = transform.localPosition;
        Xx = pos.x;
        Yy = pos.y;
        Zz = pos.z;
        Clamp();
        //CheckXYZ();


        //GetComponent<Transform>().localPosition = new Vector3(0.0f, Yy, Zz);
    }

    private void CheckXYZ()
    {
        if ((Yy > 2.3f) || (Zz > 5.0f))
        {
            GetComponent<Transform>().localPosition = new Vector3(Xx, 2.3f, 5.0f);
        }
        else if ((Yy < -1.2f) || (Zz < 2.8f))
        {
            GetComponent<Transform>().localPosition = new Vector3(Xx, -1.2f, 2.8f);
        }
    }

    private void Clamp()
    {
        pos.y = Mathf.Clamp(pos.y, -1.2f, 2.3f); // clamp position
        pos.x = Xx;
        pos.z = Mathf.Clamp(pos.z, 2.8f, 5.0f);
        transform.localPosition = pos;
    }

}

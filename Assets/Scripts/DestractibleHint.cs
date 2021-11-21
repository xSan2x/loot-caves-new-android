using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestractibleHint : MonoBehaviour
{
    static float lifeTime = 0.30f;
    float counter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += Time.fixedDeltaTime;
        if (counter > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKiller : MonoBehaviour
{
    public float killTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, killTime);
    }
}

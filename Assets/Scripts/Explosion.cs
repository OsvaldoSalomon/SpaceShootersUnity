using UnityEngine;

public class Explosion : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 1.8f);   
    }
    
}

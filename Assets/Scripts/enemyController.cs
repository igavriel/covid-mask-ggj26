using UnityEngine;

public class enemyController : MonoBehaviour
{
    Quaternion fixedRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fixedRotation = Quaternion.Euler(0,0,90f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.rotation = fixedRotation;
    }
}

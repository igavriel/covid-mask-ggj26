using UnityEngine;

public class enemyController : MonoBehaviour
{
    Quaternion fixedRotation;
    [SerializeField] bool isWalking;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        fixedRotation = Quaternion.Euler(0f, transform.parent.rotation.eulerAngles.y, 0f);
        GetComponent<Animator>().SetBool("isWalking", isWalking);
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

using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1.2f);
    }
}

using UnityEngine;
public class CameraPosition : MonoBehaviour
{
    [SerializeField] Transform target;
    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, 6, target.position.z - 4.2f);
    }
}

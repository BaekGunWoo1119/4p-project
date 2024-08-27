using UnityEngine;

public class DestroyDebug : MonoBehaviour
{
    void OnDestroy()
    {
        Debug.Log($"Object '{gameObject.name}' has been destroyed in {this.GetType().Name}");
    }

}

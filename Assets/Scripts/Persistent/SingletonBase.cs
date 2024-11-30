using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        // Prevent duplicates
        if (Instance != null && Instance != this as T) {
            Destroy(gameObject); // Destroy duplicates
        } else {
            Instance = this as T;
            // Unparent the object to allow DDOL
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);  // Ensure it persists across scenes
        }
    }
}

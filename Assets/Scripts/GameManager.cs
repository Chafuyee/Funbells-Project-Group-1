using UnityEngine;
using System.Collections.Generic;

// Class to manage the game state and data
// Caches values in memory and saves them to PlayerPrefs
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Dictionary to act as a cache for different types of PlayerPrefs data
    private Dictionary<string, object> cache = new Dictionary<string, object>();

    void Awake()
    {
        // Singleton pattern: Ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Make this GameObject persistent across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroys duplicate instances
        }

        // SET DEFAULT VALUES
        SetProgress(0);
        SetMaxProgress(100);
    }

    // EXAMPLE GETTERS AND SETTERS FOR PLAYER PROGRESS

    public int GetProgress()
    {
        // Debug.Log("Getting progress: " + GetValue<int>("Progress", 0));
        return GetValue<int>("Progress", 0);
    }

    public void SetProgress(int progress)
    {
        // Debug.Log("Setting progress to: " + progress);
        SetValue<int>("Progress", progress);
    }

    public int GetMaxProgress()
    {
        // Debug.Log("Getting max progress: " + GetValue<int>("MaxProgress", 100));
        return GetValue<int>("MaxProgress", 100);
    }

    public void SetMaxProgress(int maxProgress)
    {
        // Debug.Log("Setting max progress to: " + maxProgress);
        SetValue<int>("MaxProgress", maxProgress);
    }

    public float GetForwardInput()
    {
        // Debug.Log("Getting forward input: " + GetValue<int>("ForwardInput", 0));
        return GetValue<float>("ForwardInput", 0);
    }   

    public void SetForwardInput(float forwardInput)
    {
        // Debug.Log("Setting forward input to: " + forwardInput);
        SetValue<float>("ForwardInput", forwardInput);
    }

    // MAKE GETTERS AND SETTERS FROM THESE TO STORE DIFFERENT DATA

    // Generic method to get a cached value or load from PlayerPrefs
    private T GetValue<T>(string key, T defaultValue = default(T))
    {
        // Check if the key exists in the cache
        if (cache.ContainsKey(key))
        {
            return (T)cache[key];
        }

        // If not cached, load from PlayerPrefs based on type
        object value;

        if (typeof(T) == typeof(int))
        {
            value = PlayerPrefs.GetInt(key, (int)(object)defaultValue);
        }
        else if (typeof(T) == typeof(float))
        {
            value = PlayerPrefs.GetFloat(key, (float)(object)defaultValue);
        }
        else if (typeof(T) == typeof(string))
        {
            value = PlayerPrefs.GetString(key, (string)(object)defaultValue);
        }
        else
        {
            throw new System.NotSupportedException($"Type {typeof(T)} is not supported.");
        }

        // Store the value in the cache
        cache[key] = value;

        return (T)value;
    }

    // Generic method to set a value in both the cache and PlayerPrefs
    private void SetValue<T>(string key, T value)
    {
        // Update the cache
        cache[key] = value;

        // Save to PlayerPrefs based on the type
        if (typeof(T) == typeof(int))
        {
            PlayerPrefs.SetInt(key, (int)(object)value);
        }
        else if (typeof(T) == typeof(float))
        {
            PlayerPrefs.SetFloat(key, (float)(object)value);
        }
        else if (typeof(T) == typeof(string))
        {
            PlayerPrefs.SetString(key, (string)(object)value);
        }
        else
        {
            throw new System.NotSupportedException($"Type {typeof(T)} is not supported.");
        }
    }

    // ADMIN FUNCTIONS TO CLEAR CACHE AND PERSIST DATA

    // Save all changes to PlayerPrefs
    public void Save()
    {
        PlayerPrefs.Save();
    }

    // Method to clear the cache for a specific key
    public void ClearCache(string key)
    {
        if (cache.ContainsKey(key))
        {
            cache.Remove(key);
        }
    }

    // Method to clear the entire cache
    public void ClearAllCache()
    {
        cache.Clear();
    }
}

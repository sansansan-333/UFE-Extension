using UnityEngine;
using System;

// https://qiita.com/Teach/items/c146c7939db7acbd7eee
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {

    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);
                if (instance == null) {
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }
            }

            return instance;
        }
    }

    protected bool CheckInstance() {
        if (instance == null) {
            instance = this as T;
            return true;
        }
        else if (Instance == this) {
            return true;
        }
        Destroy(this);
        return false;
    }
}
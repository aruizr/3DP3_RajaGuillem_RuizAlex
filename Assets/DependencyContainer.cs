using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Singleton;

public class DependencyContainer : SingletonMonoBehaviour<DependencyContainer>
{
    private Dictionary<Type, System.Object> dependencies = new Dictionary<Type, System.Object>();

    public static T GetDependency<T>()
    {
        if (!Instance.dependencies.ContainsKey(typeof(T)))
        {
            Debug.LogError("Cannot find: " + typeof(T).ToString() + ".");
            throw new Exception("No dependency found");
        }

        return (T) Instance.dependencies[typeof(T)];
    }

    public static void AddDependency<T>(System.Object obj)
    {
        if (Instance.dependencies.ContainsKey(typeof(T)))
        {
            Debug.Log("There's already an object of type: " + typeof(T).ToString());
            Debug.Log("Object 1: " + Instance.dependencies[typeof(T)].GetType().ToString());
            Debug.Log("Object 2: " + obj.GetType().ToString());
            Instance.dependencies.Remove(typeof(T));
        }

        Instance.dependencies.Add(typeof(T), obj);
    }
}
using System;
using UnityEngine;

/// <summary>
/// JsonUtility de Unity no puede parsear un array JSON en la raíz (ej: "[ {...}, {...} ]").
/// Esta clase envuelve el array en un objeto para poder parsearlo.
/// </summary>
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string wrapped = "{\"Items\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrapped);
        return wrapper.Items;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
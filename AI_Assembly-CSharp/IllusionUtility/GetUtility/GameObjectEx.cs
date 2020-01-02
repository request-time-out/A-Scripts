// Decompiled with JetBrains decompiler
// Type: IllusionUtility.GetUtility.GameObjectEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace IllusionUtility.GetUtility
{
  public static class GameObjectEx
  {
    public static T Get<T>(this GameObject obj) where T : MonoBehaviour
    {
      T component = obj.GetComponent<T>();
      if (!Object.op_Implicit((Object) (object) component))
        Debug.LogError((object) ("Expected to find component of type " + (object) typeof (T) + " but found none"), (Object) obj);
      return component;
    }

    public static T SearchComponent<T>(this GameObject obj, string searchName) where T : MonoBehaviour
    {
      foreach (T componentsInChild in obj.GetComponentsInChildren<T>(true))
      {
        if (searchName == componentsInChild.get_name())
          return componentsInChild;
      }
      return (T) null;
    }

    public static GameObject[] CreateChild(this GameObject topObj, string pathName)
    {
      GameObject gameObject1 = (GameObject) null;
      List<GameObject> gameObjectList = new List<GameObject>();
      string str1 = pathName;
      char[] chArray = new char[1]{ '/' };
      foreach (string str2 in str1.Split(chArray))
      {
        GameObject gameObject2 = new GameObject(str2);
        gameObjectList.Add(gameObject2);
        if (Object.op_Inequality((Object) gameObject1, (Object) null))
          gameObject2.get_transform().SetParent(gameObject1.get_transform());
        gameObject1 = gameObject2;
      }
      gameObjectList[0].get_transform().SetParent(topObj.get_transform());
      return gameObjectList.ToArray();
    }
  }
}

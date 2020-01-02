// Decompiled with JetBrains decompiler
// Type: Utils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
  public static void DestroyChildren(Transform parent)
  {
    int childCount = parent.get_childCount();
    GameObject[] gameObjectArray = new GameObject[childCount];
    for (int index = 0; index < childCount; ++index)
      gameObjectArray[index] = ((Component) parent.GetChild(index)).get_gameObject();
    parent.DetachChildren();
    for (int index = 0; index < childCount; ++index)
      Object.Destroy((Object) gameObjectArray[index]);
  }

  public static T StringToEnumType<T>(string value, T defaultValue)
  {
    T obj;
    try
    {
      obj = !string.IsNullOrEmpty(value) ? (T) Enum.Parse(typeof (T), value) : defaultValue;
    }
    catch (ArgumentException ex)
    {
      throw new UnityException(ex.Message + Environment.NewLine + "failed to parse string [" + value + "] -> enum type [" + typeof (T).ToString() + "]");
    }
    return obj;
  }

  public static List<T> GetComponentsRecursive<T>(Transform t) where T : Component
  {
    List<T> objList = new List<T>();
    T component = ((Component) t).GetComponent<T>();
    if (Object.op_Inequality((Object) (object) component, (Object) null))
      objList.Add(component);
    int num = 0;
    for (int childCount = t.get_childCount(); num < childCount; ++num)
      objList.AddRange((IEnumerable<T>) Utils.GetComponentsRecursive<T>(t.GetChild(num)));
    return objList;
  }

  public static T FindComponentInParents<T>(Transform t) where T : Component
  {
    T component = ((Component) t).GetComponent<T>();
    if (Object.op_Inequality((Object) (object) component, (Object) null))
      return component;
    return Object.op_Inequality((Object) t.get_parent(), (Object) null) ? Utils.FindComponentInParents<T>(t.get_parent()) : (T) null;
  }

  public static void ConvertMeshIntoWireFrame(Mesh mesh)
  {
    if (mesh.GetTopology(0) != null)
      return;
    int[] indices = mesh.GetIndices(0);
    int[] numArray = new int[indices.Length * 2];
    for (int index = 0; index < indices.Length / 3; ++index)
    {
      int num1 = indices[index * 3];
      int num2 = indices[index * 3 + 1];
      int num3 = indices[index * 3 + 2];
      numArray[index * 6] = num1;
      numArray[index * 6 + 1] = num2;
      numArray[index * 6 + 2] = num2;
      numArray[index * 6 + 3] = num3;
      numArray[index * 6 + 4] = num3;
      numArray[index * 6 + 5] = num1;
    }
    mesh.SetIndices(numArray, (MeshTopology) 3, 0);
  }
}

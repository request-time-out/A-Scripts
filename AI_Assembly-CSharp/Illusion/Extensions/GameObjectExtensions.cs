// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.GameObjectExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Illusion.Extensions
{
  public static class GameObjectExtensions
  {
    public static List<GameObject> Children(this GameObject self)
    {
      List<GameObject> gameObjectList = new List<GameObject>();
      Transform transform = self.get_transform();
      for (int index = 0; index < transform.get_childCount(); ++index)
        gameObjectList.Add(((Component) transform.GetChild(index)).get_gameObject());
      return gameObjectList;
    }

    public static void ChildrenAction(this GameObject self, Action<GameObject> act)
    {
      Transform transform = self.get_transform();
      for (int index = 0; index < transform.get_childCount(); ++index)
        act(((Component) transform.GetChild(index)).get_gameObject());
    }

    public static GameObject[] CreateChild(
      this GameObject self,
      string pathName,
      bool worldPositionStays = true)
    {
      GameObject[] array = ((IEnumerable<string>) pathName.Split('/')).Select<string, GameObject>((Func<string, GameObject>) (s => new GameObject(s))).ToArray<GameObject>();
      ((IEnumerable<GameObject>) array).Select<GameObject, Transform>((Func<GameObject, Transform>) (go => go.get_transform())).Aggregate<Transform>((Func<Transform, Transform, Transform>) ((parent, child) =>
      {
        child.SetParent(parent);
        return child;
      }));
      array[0].get_transform().SetParent(self.get_transform(), worldPositionStays);
      return array;
    }

    public static bool SetActiveIfDifferent(this GameObject self, bool active)
    {
      if (self.get_activeSelf() == active)
        return false;
      self.SetActive(active);
      return true;
    }
  }
}

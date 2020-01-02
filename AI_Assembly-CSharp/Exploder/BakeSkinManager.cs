// Decompiled with JetBrains decompiler
// Type: Exploder.BakeSkinManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
  internal class BakeSkinManager
  {
    private readonly List<GameObject> bakedObjects = new List<GameObject>();
    private readonly GameObject parent;

    public BakeSkinManager(Core core)
    {
      this.parent = new GameObject("BakeSkinParent");
      this.parent.get_gameObject().get_transform().set_parent(((Component) core).get_transform());
      this.parent.get_transform().set_position(Vector3.get_zero());
    }

    public GameObject CreateBakeObject(string name)
    {
      GameObject gameObject = new GameObject(name);
      gameObject.get_transform().set_parent(this.parent.get_transform());
      this.bakedObjects.Add(gameObject);
      return gameObject;
    }

    public void Clear()
    {
      for (int index = 0; index < this.bakedObjects.Count; ++index)
      {
        if (Object.op_Implicit((Object) this.bakedObjects[index]))
          Object.Destroy((Object) this.bakedObjects[index]);
      }
      this.bakedObjects.Clear();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: AIProject.EventTransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class EventTransform : MonoBehaviour
  {
    public int _mapID;
    public int _id;

    public EventTransform()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      Dictionary<int, Transform> dictionary1;
      if (!Singleton<Manager.Map>.Instance.EventStartPointDic.TryGetValue(this._mapID, out dictionary1))
      {
        Dictionary<int, Transform> dictionary2 = new Dictionary<int, Transform>();
        Singleton<Manager.Map>.Instance.EventStartPointDic[this._mapID] = dictionary2;
        dictionary1 = dictionary2;
      }
      dictionary1[this._id] = ((Component) this).get_transform();
    }
  }
}

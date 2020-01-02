// Decompiled with JetBrains decompiler
// Type: AIProject.DebugUtil.ActionPointMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.DebugUtil
{
  [AddComponentMenu("YK/Debug/ActionPointMarker")]
  public class ActionPointMarker : MonoBehaviour
  {
    private ActionPoint _actionPoint;

    public ActionPointMarker()
    {
      base.\u002Ector();
    }

    public static Dictionary<int, ActionPoint> ActionPointTable { get; } = new Dictionary<int, ActionPoint>();

    public static List<ActionPoint> ActionPointList { get; } = new List<ActionPoint>();

    private void Awake()
    {
      this._actionPoint = (ActionPoint) ((Component) this).GetComponent<ActionPoint>();
    }

    private void OnEnable()
    {
      if (!Object.op_Inequality((Object) this._actionPoint, (Object) null))
        return;
      ActionPointMarker.ActionPointTable[((Object) this._actionPoint).GetInstanceID()] = this._actionPoint;
      ActionPointMarker.ActionPointList.Add(this._actionPoint);
    }

    private void OnDisable()
    {
      if (!Object.op_Inequality((Object) this._actionPoint, (Object) null))
        return;
      ActionPointMarker.ActionPointTable.Remove(((Object) this._actionPoint).GetInstanceID());
      ActionPointMarker.ActionPointList.RemoveAll((Predicate<ActionPoint>) (x => Object.op_Equality((Object) x, (Object) this._actionPoint)));
    }
  }
}

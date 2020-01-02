// Decompiled with JetBrains decompiler
// Type: AIProject.ActionPointGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public class ActionPointGroup : MonoBehaviour
  {
    [SerializeField]
    private ActionPoint[] _joinPoints;

    public ActionPointGroup()
    {
      base.\u002Ector();
    }

    public ActionPoint[] JoinPoints
    {
      get
      {
        return this._joinPoints;
      }
    }

    private void Start()
    {
      foreach (ActionPoint joinPoint1 in this._joinPoints)
      {
        if (!Object.op_Equality((Object) joinPoint1, (Object) null))
        {
          foreach (ActionPoint joinPoint2 in this._joinPoints)
          {
            if (!Object.op_Equality((Object) joinPoint2, (Object) null) && !Object.op_Equality((Object) joinPoint1, (Object) joinPoint2))
              joinPoint1.GroupActionPoints.Add(joinPoint2);
          }
        }
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: AIProject.DebugUtil.AgentMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.DebugUtil
{
  [AddComponentMenu("YK/Debug/AgentMarker")]
  public class AgentMarker : MonoBehaviour
  {
    private AgentActor _agent;

    public AgentMarker()
    {
      base.\u002Ector();
    }

    public static Dictionary<int, AgentActor> AgentMarkerTable { get; } = new Dictionary<int, AgentActor>();

    public static List<int> Keys { get; } = new List<int>();

    private void Awake()
    {
      this._agent = (AgentActor) ((Component) this).GetComponent<AgentActor>();
    }

    private void OnEnable()
    {
      if (!Object.op_Inequality((Object) this._agent, (Object) null))
        return;
      int instanceId = ((Object) this._agent).GetInstanceID();
      AgentMarker.AgentMarkerTable[instanceId] = this._agent;
      AgentMarker.Keys.Add(instanceId);
    }

    private void OnDisable()
    {
      if (!Object.op_Inequality((Object) this._agent, (Object) null))
        return;
      int instanceID = ((Object) this._agent).GetInstanceID();
      AgentMarker.AgentMarkerTable.Remove(instanceID);
      AgentMarker.Keys.RemoveAll((Predicate<int>) (x => x == instanceID));
    }
  }
}

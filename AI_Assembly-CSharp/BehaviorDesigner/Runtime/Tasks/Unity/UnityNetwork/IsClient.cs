﻿// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityNetwork.IsClient
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine.Networking;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityNetwork
{
  public class IsClient : Conditional
  {
    public IsClient()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      return NetworkClient.get_active() ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}

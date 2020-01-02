// Decompiled with JetBrains decompiler
// Type: AIProject.StateEventList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject
{
  public class StateEventList : MonoBehaviour
  {
    [SerializeField]
    private bool _debugMode;

    public StateEventList()
    {
      base.\u002Ector();
    }

    public bool DebugMode
    {
      get
      {
        return this._debugMode;
      }
      set
      {
        this._debugMode = value;
      }
    }

    [Serializable]
    public class StateEvent
    {
      public string stateName;
      public float time;
      public StateEventList.StateEventType type;
      public StateEventList.JointType jointTarget;
      public int id;
    }

    public enum StateEventType
    {
      SE,
      FootStep,
      ActivateItem,
      DeactivateItem,
    }

    public enum JointType
    {
      LeftHand,
      RightHand,
      LeftFoot,
      RightFoot,
    }
  }
}

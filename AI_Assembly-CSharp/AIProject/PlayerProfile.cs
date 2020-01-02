// Decompiled with JetBrains decompiler
// Type: AIProject.PlayerProfile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class PlayerProfile : SerializedScriptableObject
  {
    [SerializeField]
    private int _defaultInventoryMaxSlot;
    [SerializeField]
    private int _agentInventoryMaxSlot;
    [SerializeField]
    private int _commonActionIconID;
    [SerializeField]
    private PlayerProfile.PoseIDCollection _poseIDData;
    [SerializeField]
    private List<int> _exPantryCommandActPTIDs;
    [SerializeField]
    private Dictionary<int, int[]> _disableWaterVFXAreaList;
    [SerializeField]
    [DisableInPlayMode]
    private EnvironmentSimulator.DateTimeThreshold[] _canSleepTime;
    [SerializeField]
    [DisableInPlayMode]
    private EnvironmentSimulator.DateTimeSerialization _wakeTime;
    [SerializeField]
    [DisableInPlayMode]
    private int _hizamakuraPTID;

    public PlayerProfile()
    {
      base.\u002Ector();
    }

    public int DefaultInventoryMax
    {
      get
      {
        return this._defaultInventoryMaxSlot;
      }
    }

    public int AgentInventoryMaxSlot
    {
      get
      {
        return this._agentInventoryMaxSlot;
      }
    }

    public int CommonActionIconID
    {
      get
      {
        return this._commonActionIconID;
      }
    }

    public PlayerProfile.PoseIDCollection PoseIDData
    {
      get
      {
        return this._poseIDData;
      }
    }

    public List<int> ExPantryCommandActPTIDs
    {
      get
      {
        return this._exPantryCommandActPTIDs;
      }
    }

    public Dictionary<int, int[]> DisableWaterVFXAreaList
    {
      get
      {
        return this._disableWaterVFXAreaList;
      }
    }

    public EnvironmentSimulator.DateTimeThreshold[] CanSleepTime
    {
      get
      {
        return this._canSleepTime;
      }
    }

    public EnvironmentSimulator.DateTimeSerialization WakeTime
    {
      get
      {
        return this._wakeTime;
      }
    }

    public int HizamakuraPTID
    {
      get
      {
        return this._hizamakuraPTID;
      }
    }

    [Serializable]
    public class PoseIDCollection
    {
      [SerializeField]
      private PoseKeyPair _leftPoseID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _menuPoseID = new PoseKeyPair();
      [SerializeField]
      private PoseKeyPair _wakeupPoseID = new PoseKeyPair();
      [SerializeField]
      private string _normalLocoStateName = string.Empty;
      [SerializeField]
      private int _torchLocoID;
      [SerializeField]
      private int _lampLocoID;
      [SerializeField]
      private int _torchOnbuLocoID;
      [SerializeField]
      private int _lampOnbuLocoID;

      public PoseKeyPair LeftPoseID
      {
        get
        {
          return this._leftPoseID;
        }
      }

      public PoseKeyPair MenuPoseID
      {
        get
        {
          return this._menuPoseID;
        }
      }

      public PoseKeyPair WakeupPoseID
      {
        get
        {
          return this._wakeupPoseID;
        }
      }

      public string NormalLocoStateName
      {
        get
        {
          return this._normalLocoStateName;
        }
      }

      public int TorchLocoID
      {
        get
        {
          return this._torchLocoID;
        }
      }

      public int LampLocoID
      {
        get
        {
          return this._lampLocoID;
        }
      }

      public int TorchOnbuLocoID
      {
        get
        {
          return this._torchOnbuLocoID;
        }
      }

      public int LampOnbuLocoID
      {
        get
        {
          return this._lampOnbuLocoID;
        }
      }
    }
  }
}

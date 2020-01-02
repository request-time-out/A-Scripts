// Decompiled with JetBrains decompiler
// Type: AIProject.CameraTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Cinemachine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIProject
{
  public class CameraTable : SerializedMonoBehaviour
  {
    [SerializeField]
    private Dictionary<ShotType, CinemachineVirtualCameraBase> _table;

    public CameraTable()
    {
      base.\u002Ector();
    }

    public CinemachineVirtualCameraBase this[ShotType key]
    {
      get
      {
        CinemachineVirtualCameraBase virtualCameraBase;
        return this._table.TryGetValue(key, out virtualCameraBase) ? virtualCameraBase : (CinemachineVirtualCameraBase) null;
      }
    }

    public ShotType[] Keys
    {
      get
      {
        return ((IEnumerable<ShotType>) this._table.Keys).ToArray<ShotType>();
      }
    }

    public Transform Duplicate()
    {
      return ((GameObject) Object.Instantiate<GameObject>((M0) ((Component) this).get_gameObject(), ((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation())).get_transform();
    }

    public void DestroySelf()
    {
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }

    public Dictionary<ShotType, CinemachineVirtualCameraBase> ToDictionary()
    {
      return ((IEnumerable<KeyValuePair<ShotType, CinemachineVirtualCameraBase>>) this._table).ToDictionary<KeyValuePair<ShotType, CinemachineVirtualCameraBase>, ShotType, CinemachineVirtualCameraBase>((Func<KeyValuePair<ShotType, CinemachineVirtualCameraBase>, ShotType>) (x => x.Key), (Func<KeyValuePair<ShotType, CinemachineVirtualCameraBase>, CinemachineVirtualCameraBase>) (x => x.Value));
    }
  }
}

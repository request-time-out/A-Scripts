// Decompiled with JetBrains decompiler
// Type: AIProject.SpaceTypeChangeVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public class SpaceTypeChangeVolume : MonoBehaviour
  {
    [SerializeField]
    private SpaceTypeChangeVolume.SpaceType _type;
    [SerializeField]
    private Collider[] _colliders;

    public SpaceTypeChangeVolume()
    {
      base.\u002Ector();
    }

    public SpaceTypeChangeVolume.SpaceType Type
    {
      get
      {
        return this._type;
      }
    }

    public bool Check(Vector3 point)
    {
      bool flag = false;
      foreach (Collider collider in this._colliders)
      {
        Vector3 vector3 = collider.ClosestPoint(point);
        if (Mathf.Approximately(Vector3.Distance(point, vector3), 0.0001f))
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public enum SpaceType
    {
      Outdoor,
      Indoor,
      Roof,
    }
  }
}

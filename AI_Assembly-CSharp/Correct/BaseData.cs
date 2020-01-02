// Decompiled with JetBrains decompiler
// Type: Correct.BaseData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Correct
{
  public class BaseData : MonoBehaviour
  {
    public Transform bone;
    [SerializeField]
    private Vector3 _pos;
    [SerializeField]
    private Vector3 _ang;
    [SerializeField]
    private Quaternion _rot;
    [SerializeField]
    private static GameObject _handle;

    public BaseData()
    {
      base.\u002Ector();
    }

    public Vector3 pos
    {
      get
      {
        return this._pos;
      }
      set
      {
        this._pos = value;
      }
    }

    public Vector3 ang
    {
      get
      {
        return this._ang;
      }
      set
      {
        this._rot = Quaternion.Euler(value);
        this._ang = value;
      }
    }

    public Quaternion rot
    {
      get
      {
        return this._rot;
      }
      set
      {
        this._rot = value;
        this._ang = ((Quaternion) ref this._rot).get_eulerAngles();
      }
    }

    public static GameObject handle
    {
      get
      {
        return BaseData._handle;
      }
    }

    private void Reset()
    {
      this.bone = (Transform) null;
      this._pos = Vector3.get_zero();
      this._rot = Quaternion.get_identity();
      this._ang = Vector3.get_zero();
    }

    private void Awake()
    {
      if (!Object.op_Equality((Object) BaseData._handle, (Object) null))
        return;
      BaseData._handle = GameObject.Find("IKhandle");
    }
  }
}

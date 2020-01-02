// Decompiled with JetBrains decompiler
// Type: Studio.Preparation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  public class Preparation : MonoBehaviour
  {
    [SerializeField]
    private Transform _lookAtTarget;
    [SerializeField]
    private CharAnimeCtrl _charAnimeCtrl;
    [SerializeField]
    private IKCtrl _IKCtrl;
    [SerializeField]
    private PVCopy _pvCopy;
    [SerializeField]
    private YureCtrl _yureCtrl;

    public Preparation()
    {
      base.\u002Ector();
    }

    public Transform LookAtTarget
    {
      get
      {
        return this._lookAtTarget;
      }
    }

    public CharAnimeCtrl CharAnimeCtrl
    {
      get
      {
        return this._charAnimeCtrl;
      }
    }

    public IKCtrl IKCtrl
    {
      get
      {
        return this._IKCtrl;
      }
    }

    public PVCopy PvCopy
    {
      get
      {
        return this._pvCopy;
      }
    }

    public YureCtrl YureCtrl
    {
      get
      {
        return this._yureCtrl;
      }
    }
  }
}

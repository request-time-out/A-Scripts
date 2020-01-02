// Decompiled with JetBrains decompiler
// Type: Studio.PVCopy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using IllusionUtility.SetUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Studio
{
  public class PVCopy : MonoBehaviour
  {
    [SerializeField]
    private GameObject[] pv;
    [SerializeField]
    private GameObject[] bone;
    private bool[] _enable;

    public PVCopy()
    {
      base.\u002Ector();
    }

    private bool enable
    {
      get
      {
        return ((IEnumerable<bool>) this._enable).Any<bool>();
      }
    }

    public bool this[int _idx]
    {
      get
      {
        return this._enable.SafeGet<bool>(_idx);
      }
      set
      {
        if (!MathfEx.RangeEqualOn<int>(0, _idx, this._enable.Length - 1))
          return;
        this._enable[_idx] = value;
      }
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.LateUpdateAsObservable((Component) this), (Func<M0, bool>) (_ => this.enable)), (Action<M0>) (_ =>
      {
        for (int index = 0; index < this.pv.Length; ++index)
        {
          if (this._enable[index])
            this.bone[index].get_transform().CopyPosRotScl(this.pv[index].get_transform());
        }
      }));
    }

    private void Reset()
    {
      string[] strArray1 = new string[8]
      {
        "f_pv_arm_L",
        "f_pv_elbo_L",
        "f_pv_arm_R",
        "f_pv_elbo_R",
        "f_pv_leg_L",
        "f_pv_knee_L",
        "f_pv_leg_R",
        "f_pv_knee_R"
      };
      this.pv = new GameObject[8];
      for (int index = 0; index < strArray1.Length; ++index)
        this.pv[index] = ((Component) this).get_transform().FindLoop(strArray1[index]);
      string[] strArray2 = new string[8]
      {
        "f_t_arm_L",
        "f_t_elbo_L",
        "f_t_arm_R",
        "f_t_elbo_R",
        "f_t_leg_L",
        "f_t_knee_L",
        "f_t_leg_R",
        "f_t_knee_R"
      };
      this.bone = new GameObject[8];
      for (int index = 0; index < strArray2.Length; ++index)
        this.bone[index] = ((Component) this).get_transform().FindLoop(strArray2[index]);
    }
  }
}

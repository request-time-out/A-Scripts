// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsB_SubmenuEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsB_SubmenuEx : MonoBehaviour
  {
    [SerializeField]
    private Toggle tglFutanari;

    public CvsB_SubmenuEx()
    {
      base.\u002Ector();
    }

    protected CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    protected ChaControl chaCtrl
    {
      get
      {
        return this.customBase.chaCtrl;
      }
    }

    protected ChaFileParameter parameter
    {
      get
      {
        return this.chaCtrl.fileParam;
      }
    }

    public void UpdateCustomUI()
    {
      this.tglFutanari.SetIsOnWithoutCallback(this.parameter.futanari);
    }

    private void Start()
    {
      this.customBase.actUpdateCvsFutanari += new Action(this.UpdateCustomUI);
      ObservableExtensions.Subscribe<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this.tglFutanari.onValueChanged), (Action<M0>) (isOn => this.parameter.futanari = isOn));
    }
  }
}

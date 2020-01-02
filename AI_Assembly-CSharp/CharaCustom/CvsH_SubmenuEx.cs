// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsH_SubmenuEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsH_SubmenuEx : MonoBehaviour
  {
    [SerializeField]
    private Toggle[] tglShaderType;

    public CvsH_SubmenuEx()
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

    protected ChaFileHair hair
    {
      get
      {
        return this.chaCtrl.fileHair;
      }
    }

    public void UpdateCustomUI()
    {
      this.tglShaderType[this.hair.shaderType].SetIsOnWithoutCallback(true);
      this.tglShaderType[(this.hair.shaderType + 1) % 2].SetIsOnWithoutCallback(false);
    }

    private void Start()
    {
      this.customBase.actUpdateCvsHair += new Action(this.UpdateCustomUI);
      if (!((IEnumerable<Toggle>) this.tglShaderType).Any<Toggle>())
        return;
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<Toggle>) this.tglShaderType).Select<Toggle, \u003C\u003E__AnonType12<Toggle, byte>>((Func<Toggle, int, \u003C\u003E__AnonType12<Toggle, byte>>) ((p, idx) => new \u003C\u003E__AnonType12<Toggle, byte>(p, (byte) idx))).ToList<\u003C\u003E__AnonType12<Toggle, byte>>().ForEach((Action<\u003C\u003E__AnonType12<Toggle, byte>>) (p => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) p.toggle.onValueChanged), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (isOn =>
      {
        this.hair.shaderType = (int) p.index;
        this.chaCtrl.ChangeSettingHairShader();
        this.chaCtrl.ChangeSettingHairTypeAccessoryShaderAll();
      }))));
    }
  }
}

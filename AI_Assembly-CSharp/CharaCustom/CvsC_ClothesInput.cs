// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsC_ClothesInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Illusion.Extensions;
using Manager;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsC_ClothesInput : MonoBehaviour
  {
    [SerializeField]
    private InputField inpName;
    [SerializeField]
    private GameObject objNameDummy;
    [SerializeField]
    private Text textNameDummy;
    [SerializeField]
    private Button btnEntry;
    [SerializeField]
    private Button btnBack;
    public Action<string> actEntry;

    public CvsC_ClothesInput()
    {
      base.\u002Ector();
    }

    private CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    public void SetupInputCoordinateNameWindow(string name)
    {
      this.customBase.customCtrl.showInputCoordinate = true;
      this.inpName.set_text(name);
      if (Object.op_Inequality((Object) null, (Object) this.textNameDummy))
        this.textNameDummy.set_text(name);
      this.inpName.ActivateInputField();
    }

    private void Start()
    {
      this.customBase.lstInputField.Add(this.inpName);
      if (!Object.op_Inequality((Object) null, (Object) this.inpName))
        return;
      ObservableExtensions.Subscribe<string>((IObservable<M0>) UnityUIComponentExtensions.OnEndEditAsObservable(this.inpName), (Action<M0>) (buf =>
      {
        if (!Object.op_Inequality((Object) null, (Object) this.textNameDummy))
          return;
        this.textNameDummy.set_text(this.inpName.get_text());
      }));
      if (Object.op_Inequality((Object) null, (Object) this.objNameDummy))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable(this.objNameDummy), (Action<M0>) (_ =>
        {
          bool isFocused = this.inpName.get_isFocused();
          if (this.objNameDummy.get_activeSelf() != isFocused)
            return;
          this.objNameDummy.SetActiveIfDifferent(!isFocused);
        }));
      if (Object.op_Inequality((Object) null, (Object) this.btnBack))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnBack), (Action<M0>) (_ =>
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
          this.customBase.customCtrl.showInputCoordinate = false;
        }));
      if (!Object.op_Inequality((Object) null, (Object) this.btnEntry))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.btnEntry), (Action<M0>) (_ => ((Selectable) this.btnEntry).set_interactable(!this.inpName.get_text().IsNullOrEmpty())));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnEntry), (Action<M0>) (_ =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Save);
        this.customBase.customCtrl.showInputCoordinate = false;
        if (this.actEntry == null)
          return;
        this.actEntry(this.inpName.get_text());
      }));
    }
  }
}

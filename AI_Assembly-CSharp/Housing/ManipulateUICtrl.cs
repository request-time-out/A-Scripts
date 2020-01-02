// Decompiled with JetBrains decompiler
// Type: Housing.ManipulateUICtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Housing.Command;
using Illusion.Extensions;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Housing
{
  [Serializable]
  public class ManipulateUICtrl : UIDerived
  {
    private BoolReactiveProperty visibleReactive = new BoolReactiveProperty(false);
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Button buttonRotL;
    [SerializeField]
    private Button buttonRotR;
    private Graphic[] graphics;

    public bool Visible
    {
      get
      {
        return ((ReactiveProperty<bool>) this.visibleReactive).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this.visibleReactive).set_Value(value);
      }
    }

    public override void Init(UICtrl _uiCtrl, bool _tutorial)
    {
      base.Init(_uiCtrl, _tutorial);
      this.graphics = new Graphic[2]
      {
        (Graphic) ((Selectable) this.buttonRotL).get_image(),
        (Graphic) ((Selectable) this.buttonRotR).get_image()
      };
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonRotL), (Action<M0>) (_ => this.Rotation(90f)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonRotR), (Action<M0>) (_ => this.Rotation(-90f)));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.visibleReactive, (Action<M0>) (_b =>
      {
        this.canvasGroup.Enable(_b, false);
        foreach (Graphic graphic in this.graphics)
          graphic.set_raycastTarget(_b);
      }));
    }

    public override void UpdateUI()
    {
    }

    private void Rotation(float _value)
    {
      ObjectCtrl selectObject = Singleton<Selection>.Instance.SelectObject;
      Vector3 localEulerAngles1 = selectObject.LocalEulerAngles;
      Vector3 localEulerAngles2 = selectObject.LocalEulerAngles;
      for (int index = 0; index < 3; ++index)
      {
        localEulerAngles2.y = (__Null) ((localEulerAngles2.y + (double) _value) % 360.0);
        selectObject.LocalEulerAngles = localEulerAngles2;
        if (Singleton<GuideManager>.Instance.CheckRot(selectObject))
        {
          Singleton<UndoRedoManager>.Instance.Push((ICommand) new RotationCommand(selectObject, localEulerAngles1));
          Singleton<Manager.Housing>.Instance.CheckOverlap((ObjectCtrl) (selectObject as OCItem));
          Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RefreshList();
          return;
        }
      }
      selectObject.LocalEulerAngles = localEulerAngles1;
    }
  }
}

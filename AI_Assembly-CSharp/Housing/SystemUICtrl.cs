// Decompiled with JetBrains decompiler
// Type: Housing.SystemUICtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.Scene;
using Manager;
using System;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Housing
{
  [Serializable]
  public class SystemUICtrl : UIDerived
  {
    private BoolReactiveProperty cameraReactive = new BoolReactiveProperty(true);
    private BoolReactiveProperty axisReactive = new BoolReactiveProperty(true);
    private BoolReactiveProperty gridReactive = new BoolReactiveProperty(true);
    [SerializeField]
    private Button buttonAdd;
    [SerializeField]
    private Button buttonUndo;
    [SerializeField]
    private Button buttonRedo;
    [SerializeField]
    private Button buttonCamera;
    [SerializeField]
    private Button buttonAxis;
    [SerializeField]
    private Button buttonGrid;
    [SerializeField]
    private Button buttonSave;
    [SerializeField]
    private Button buttonLoad;
    [SerializeField]
    private Button buttonReset;
    [SerializeField]
    private Button buttonEnd;
    [SerializeField]
    [Header("カメラ関係")]
    private Sprite[] spritesCamera;
    [SerializeField]
    [Header("操作軸関係")]
    private Sprite[] spritesAxis;
    [SerializeField]
    [Header("グリッド関係")]
    private Sprite[] spritesGrid;

    public bool Camera
    {
      get
      {
        return ((ReactiveProperty<bool>) this.cameraReactive).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this.cameraReactive).set_Value(value);
      }
    }

    public bool Axis
    {
      get
      {
        return ((ReactiveProperty<bool>) this.axisReactive).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this.axisReactive).set_Value(value);
      }
    }

    public bool Grid
    {
      get
      {
        return ((ReactiveProperty<bool>) this.gridReactive).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this.gridReactive).set_Value(value);
      }
    }

    public bool IsMessage { get; private set; }

    public override void Init(UICtrl _uiCtrl, bool _tutorial)
    {
      base.Init(_uiCtrl, _tutorial);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonAdd), (Action<M0>) (_ => this.UICtrl.AddUICtrl.Active = !this.UICtrl.AddUICtrl.Active));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonUndo), (Action<M0>) (_ =>
      {
        if (!Singleton<UndoRedoManager>.IsInstance())
          return;
        Singleton<UndoRedoManager>.Instance.Undo();
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonRedo), (Action<M0>) (_ =>
      {
        if (!Singleton<UndoRedoManager>.IsInstance())
          return;
        Singleton<UndoRedoManager>.Instance.Redo();
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonCamera), (Action<M0>) (_ => this.Camera = !this.Camera));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonAxis), (Action<M0>) (_ => this.Axis = !this.Axis));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonGrid), (Action<M0>) (_ => this.Grid = !this.Grid));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonSave), (Action<M0>) (_ => this.Save()));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonLoad), (Action<M0>) (_ => this.UICtrl.SaveLoadUICtrl.Open()));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonReset), (Action<M0>) (_ =>
      {
        ConfirmScene.Sentence = "初期化しますか？\n" + "セットされたアイテムは削除されます。".Coloring("#DE4529FF").Size(24);
        ConfirmScene.OnClickedYes = (Action) (() =>
        {
          this.UICtrl.ListUICtrl.VirtualizingTreeView.SelectedIndex = -1;
          Singleton<Manager.Housing>.Instance.ResetObject();
          this.UICtrl.ListUICtrl.UpdateUI();
          Singleton<UndoRedoManager>.Instance.Clear();
        });
        ConfirmScene.OnClickedNo = (Action) (() => {});
        Singleton<Game>.Instance.LoadDialog();
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.buttonEnd), (Action<M0>) (_ =>
      {
        if (Singleton<CraftScene>.Instance.IsShortcutUI)
          return;
        if (Singleton<CraftScene>.Instance.CraftInfo.IsOverlapNow)
        {
          this.IsMessage = true;
          MapUIContainer.PushMessageUI("配置に問題があるものが存在します", 2, 1, (Action) (() => this.IsMessage = false));
        }
        else
        {
          ConfirmScene.Sentence = "ハウジングを終了しますか？";
          ConfirmScene.OnClickedYes = (Action) (() =>
          {
            Singleton<CraftScene>.Instance.SceneEnd();
            ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (FadeType) 0, 1f, true), (Action<M0>) (__ => {}), (Action) (() => this.EndHousing()));
          });
          ConfirmScene.OnClickedNo = (Action) (() => Singleton<CraftScene>.Instance.IsEndCheck = false);
          Singleton<Game>.Instance.LoadDialog();
          Singleton<CraftScene>.Instance.IsEndCheck = true;
        }
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.cameraReactive, (Action<M0>) (_b => this.spritesCamera.SafeProc<Sprite>(!_b ? 1 : 0, (Action<Sprite>) (_s => ((Selectable) this.buttonCamera).get_image().set_sprite(_s)))));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.axisReactive, (Action<M0>) (_b =>
      {
        this.spritesAxis.SafeProc<Sprite>(!_b ? 1 : 0, (Action<Sprite>) (_s => ((Selectable) this.buttonAxis).get_image().set_sprite(_s)));
        GuideObject guideObject = Singleton<GuideManager>.Instance.GuideObject;
        if (!Object.op_Implicit((Object) guideObject))
          return;
        guideObject.visibleOutside = _b;
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.gridReactive, (Action<M0>) (_b =>
      {
        this.spritesGrid.SafeProc<Sprite>(!_b ? 1 : 0, (Action<Sprite>) (_s => ((Selectable) this.buttonGrid).get_image().set_sprite(_s)));
        if (!Singleton<GuideManager>.IsInstance())
          return;
        Singleton<GuideManager>.Instance.VisibleGrid = _b;
      }));
      CraftCamera craftCamera = this.UICtrl.CraftCamera;
      craftCamera.NoCtrlCondition = craftCamera.NoCtrlCondition + new VirtualCameraController.NoCtrlFunc(this.NoCameraCtrl);
      if (Singleton<UndoRedoManager>.IsInstance())
      {
        Singleton<UndoRedoManager>.Instance.CanUndoChange += new EventHandler<CanhangeArgs>(this.CanUndoChange);
        Singleton<UndoRedoManager>.Instance.CanRedoChange += new EventHandler<CanhangeArgs>(this.CanRedoChange);
      }
      ((Selectable) this.buttonUndo).set_interactable(false);
      ((Selectable) this.buttonRedo).set_interactable(false);
      if (!_tutorial)
        return;
      ((Selectable) this.buttonSave).set_interactable(false);
      ((Selectable) this.buttonLoad).set_interactable(false);
    }

    public override void UpdateUI()
    {
    }

    private void EndHousing()
    {
      // ISSUE: variable of a compiler-generated type
      SystemUICtrl.\u003CEndHousing\u003Ec__async0 endHousingCAsync0;
      // ISSUE: reference to a compiler-generated field
      endHousingCAsync0.\u0024builder = AsyncVoidMethodBuilder.Create();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: cast to a reference type
      ((AsyncVoidMethodBuilder) ref endHousingCAsync0.\u0024builder).Start<SystemUICtrl.\u003CEndHousing\u003Ec__async0>((M0&) ref endHousingCAsync0);
    }

    public void Save()
    {
      if (this.IsMessage)
        return;
      Singleton<CraftScene>.Instance.Capture((Action<byte[]>) (_png =>
      {
        int sizeType = Singleton<Manager.Housing>.Instance.GetSizeType(Singleton<CraftScene>.Instance.HousingID);
        DateTime now = DateTime.Now;
        string str = string.Format("{0}_{1:00}{2:00}_{3:00}{4:00}_{5:00}_{6:000}.png", (object) now.Year, (object) now.Month, (object) now.Day, (object) now.Hour, (object) now.Minute, (object) now.Second, (object) now.Millisecond);
        Singleton<CraftScene>.Instance.CraftInfo.Save(UserData.Create(string.Format("{0}{1:00}", (object) "housing/", (object) (sizeType + 1))) + str, _png);
        this.IsMessage = true;
        MapUIContainer.PushMessageUI("保存しました", 0, 1, (Action) (() => this.IsMessage = false));
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Save);
      }));
    }

    public bool NoCameraCtrl()
    {
      return !this.Camera && !(Input.GetKey((KeyCode) 306) | Input.GetKey((KeyCode) 305));
    }

    private void CanUndoChange(object _sender, CanhangeArgs _e)
    {
      ((Selectable) this.buttonUndo).set_interactable(_e.Can);
    }

    private void CanRedoChange(object _sender, CanhangeArgs _e)
    {
      ((Selectable) this.buttonRedo).set_interactable(_e.Can);
    }
  }
}

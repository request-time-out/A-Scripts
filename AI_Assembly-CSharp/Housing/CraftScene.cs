// Decompiled with JetBrains decompiler
// Type: Housing.CraftScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Cinemachine;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Housing
{
  public class CraftScene : Singleton<CraftScene>
  {
    [SerializeField]
    private int width = 320;
    [SerializeField]
    private int height = 180;
    [SerializeField]
    private UICtrl uiCtrl;
    [SerializeField]
    private CraftCamera craftCamera;
    [SerializeField]
    private BoxCollider[] testColliders;
    [SerializeField]
    [Header("画像作成関係")]
    private Camera[] cameras;
    [SerializeField]
    [Header("終了時処理関係")]
    private Image imageEnd;
    private CraftInfo craftInfo;
    [ReadOnly]
    private InputField inputFieldNow;
    private byte[] bytesCapture;
    private ObjectCtrl overlapObjectCtrl;
    private string[] addSceneNames;

    public bool IsInit { get; private set; }

    public bool IsInputNow
    {
      get
      {
        return Object.op_Implicit((Object) this.inputFieldNow) && this.inputFieldNow.get_isFocused();
      }
    }

    public UICtrl UICtrl
    {
      get
      {
        return this.uiCtrl;
      }
    }

    public WorkingUICtrl WorkingUICtrl { get; private set; }

    public int HousingID
    {
      get
      {
        return Singleton<Manager.Map>.IsInstance() ? Singleton<Manager.Map>.Instance.HousingID : 0;
      }
    }

    public CraftInfo CraftInfo
    {
      get
      {
        return this.craftInfo;
      }
    }

    public bool DisplayTutorial { get; set; }

    public BoxCollider[] TestColliders
    {
      get
      {
        return this.testColliders;
      }
    }

    public bool IsEnd { get; private set; }

    public bool IsEndCheck { get; set; }

    public bool IsDialog
    {
      get
      {
        if (!Singleton<Game>.IsInstance())
          return false;
        return Object.op_Implicit((Object) Singleton<Game>.Instance.Dialog) || Object.op_Implicit((Object) Singleton<Game>.Instance.ExitScene);
      }
    }

    public bool IsGuide
    {
      get
      {
        return Singleton<GuideManager>.IsInstance() && Singleton<GuideManager>.Instance.IsGuide;
      }
    }

    public bool IsShortcutUI
    {
      get
      {
        return Singleton<Game>.IsInstance() && Object.op_Implicit((Object) Singleton<Game>.Instance.MapShortcutUI);
      }
    }

    public bool IsWorkingUI
    {
      get
      {
        return Object.op_Inequality((Object) this.WorkingUICtrl, (Object) null) && this.WorkingUICtrl.Visible;
      }
    }

    public void SceneEnd()
    {
      this.IsEnd = true;
      this.uiCtrl.ListUICtrl.Select((ObjectCtrl) null);
      ((Behaviour) this.imageEnd).set_enabled(true);
    }

    public void SelectInputField(InputField _input)
    {
      this.inputFieldNow = _input;
    }

    public void DeselectInputField(InputField _input)
    {
      if (!Object.op_Equality((Object) this.inputFieldNow, (Object) _input))
        return;
      this.inputFieldNow = (InputField) null;
    }

    public void Capture(Action<byte[]> _onCompleted)
    {
      if (((IList<Camera>) this.cameras).IsNullOrEmpty<Camera>())
      {
        Action<byte[]> action = _onCompleted;
        if (action == null)
          return;
        action((byte[]) null);
      }
      else
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine(new Func<IEnumerator>(this.CaptureFunc), false), (Action<M0>) (_ => {}), (Action) (() =>
        {
          Action<byte[]> action = _onCompleted;
          if (action == null)
            return;
          action(this.bytesCapture);
        })), (Component) this);
    }

    [DebuggerHidden]
    private IEnumerator CaptureFunc()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CraftScene.\u003CCaptureFunc\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private bool CheckAddScene()
    {
      if (!Singleton<Scene>.IsInstance())
        return false;
      bool flag = false;
      foreach (string nowSceneName in Singleton<Scene>.Instance.NowSceneNames)
        flag |= ((IEnumerable<string>) this.addSceneNames).Contains<string>(nowSceneName);
      return flag;
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      this.addSceneNames = new string[2]{ "Config", "Exit" };
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CraftScene.\u003CStart\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void Update()
    {
      if (this.DisplayTutorial)
        return;
      bool flag1 = Input.GetKey((KeyCode) 306) | Input.GetKey((KeyCode) 305);
      if (this.uiCtrl.SaveLoadUICtrl.Visible || Singleton<Scene>.IsInstance() && Singleton<Scene>.Instance.IsNowLoadingFade || (this.IsEndCheck || this.IsEnd || (this.IsInputNow || this.IsGuide)) || (this.IsDialog || this.IsShortcutUI || (this.CheckAddScene() || this.IsWorkingUI)))
        return;
      if (Input.GetKeyDown((KeyCode) 283))
      {
        Singleton<Game>.Instance.LoadShortcut(4, (Action) null);
      }
      else
      {
        if (!flag1)
          return;
        bool flag2 = Input.GetKey((KeyCode) 304) | Input.GetKey((KeyCode) 303);
        if (Input.GetKeyDown((KeyCode) 122))
        {
          if (flag2)
          {
            if (!Singleton<UndoRedoManager>.IsInstance())
              return;
            Singleton<UndoRedoManager>.Instance.Redo();
          }
          else
          {
            if (!Singleton<UndoRedoManager>.IsInstance())
              return;
            Singleton<UndoRedoManager>.Instance.Undo();
          }
        }
        else if (Input.GetKeyDown((KeyCode) 113))
          this.uiCtrl.SystemUICtrl.Axis = !this.uiCtrl.SystemUICtrl.Axis;
        else if (Input.GetKeyDown((KeyCode) 97))
          this.uiCtrl.SystemUICtrl.Grid = !this.uiCtrl.SystemUICtrl.Grid;
        else if (Input.GetKeyDown((KeyCode) 115))
          this.uiCtrl.SystemUICtrl.Save();
        else if (Input.GetKeyDown((KeyCode) 100))
          this.uiCtrl.ListUICtrl.Duplicate();
        else if (Input.GetKeyDown((KeyCode) 102))
        {
          ObjectCtrl[] selectObjects = Singleton<Selection>.Instance.SelectObjects;
          ObjectCtrl objectCtrl = ((IList<ObjectCtrl>) selectObjects).IsNullOrEmpty<ObjectCtrl>() ? (ObjectCtrl) null : ((IEnumerable<ObjectCtrl>) selectObjects).Where<ObjectCtrl>((Func<ObjectCtrl, bool>) (v => Object.op_Inequality((Object) v.GameObject, (Object) null))).FirstOrDefault<ObjectCtrl>((Func<ObjectCtrl, bool>) (v => v.Kind == 0));
          if (objectCtrl == null)
            return;
          this.craftCamera.TargetPos = ((CinemachineVirtualCameraBase) this.craftCamera).get_LookAt().get_parent().InverseTransformPoint(objectCtrl.Position);
        }
        else
        {
          if (!Input.GetKeyDown((KeyCode) 103))
            return;
          this.overlapObjectCtrl = this.craftInfo.FindOverlapObject(this.overlapObjectCtrl);
          if (this.overlapObjectCtrl == null)
            return;
          this.uiCtrl.ListUICtrl.Select(this.overlapObjectCtrl);
        }
      }
    }
  }
}

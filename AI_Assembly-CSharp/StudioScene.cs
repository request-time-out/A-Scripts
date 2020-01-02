// Decompiled with JetBrains decompiler
// Type: StudioScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using Manager;
using Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StudioScene : BaseLoader
{
  [SerializeField]
  private StudioScene.DrawInfo drawInfo = new StudioScene.DrawInfo();
  [SerializeField]
  private StudioScene.ModeInfo modeInfo = new StudioScene.ModeInfo();
  public StudioScene.CameraInfo cameraInfo = new StudioScene.CameraInfo();
  [SerializeField]
  private StudioScene.MapInfo mapInfo = new StudioScene.MapInfo();
  [SerializeField]
  private StudioScene.InputInfo inputInfo = new StudioScene.InputInfo();
  [SerializeField]
  private Button buttonUndo;
  [SerializeField]
  private Button buttonRedo;
  [SerializeField]
  private Text textAdjustment;
  [SerializeField]
  private OptionCtrl optionCtrl;

  public void OnClickDraw(int _no)
  {
    Singleton<Studio.Studio>.Instance.workInfo.visibleFlags[_no] = !Singleton<Studio.Studio>.Instance.workInfo.visibleFlags[_no];
    SortCanvas.select = this.drawInfo.canvasSystemMenu;
    this.UpdateUI(_no);
  }

  private void UpdateUI(int _no)
  {
    bool visibleFlag = Singleton<Studio.Studio>.Instance.workInfo.visibleFlags[_no];
    switch (_no)
    {
      case 0:
        this.drawInfo.canvasMainMenuG.Enable(visibleFlag, false);
        this.drawInfo.systemButtonCtrl.visible = visibleFlag;
        this.drawInfo.imageMenu.set_sprite(this.drawInfo.spriteMenu[!visibleFlag ? 1 : 0]);
        break;
      case 1:
        ((Behaviour) this.drawInfo.canvasList).set_enabled(visibleFlag);
        this.drawInfo.imageWork.set_sprite(this.drawInfo.spriteWork[!visibleFlag ? 1 : 0]);
        break;
      case 2:
        Singleton<GuideObjectManager>.Instance.guideInput.outsideVisible = visibleFlag;
        this.drawInfo.imageMove.set_sprite(this.drawInfo.spriteMove[!visibleFlag ? 1 : 0]);
        break;
      case 3:
        Singleton<Studio.Studio>.Instance.colorPalette.outsideVisible = visibleFlag;
        this.drawInfo.imageColor.set_sprite(this.drawInfo.spriteColor[!visibleFlag ? 1 : 0]);
        break;
      case 4:
        this.drawInfo.objOption.SetActive(visibleFlag);
        if (!visibleFlag)
        {
          this.mapInfo.mapCtrl.active = false;
          this.inputInfo.outsideVisible = false;
        }
        else
        {
          this.mapInfo.Update();
          this.inputInfo.outsideVisible = true;
        }
        this.drawInfo.imageOption.set_sprite(this.drawInfo.spriteOption[!visibleFlag ? 1 : 0]);
        break;
      case 5:
        this.drawInfo.objCamera.SetActive(visibleFlag);
        this.drawInfo.imageCamera.set_sprite(this.drawInfo.spriteCamera[!visibleFlag ? 1 : 0]);
        break;
    }
  }

  private void UpdateUI()
  {
    for (int _no = 0; _no < 6; ++_no)
      this.UpdateUI(_no);
    this.cameraInfo.center = Singleton<Studio.Studio>.Instance.workInfo.visibleCenter;
    this.cameraInfo.axis = Singleton<Studio.Studio>.Instance.workInfo.visibleAxis;
    this.cameraInfo.axisTrans = Singleton<Studio.Studio>.Instance.workInfo.visibleAxisTranslation;
    this.cameraInfo.axisCenter = Singleton<Studio.Studio>.Instance.workInfo.visibleAxisCenter;
    this.cameraInfo.Gimmick = Singleton<Studio.Studio>.Instance.workInfo.visibleGimmick;
    this.cameraInfo.imageCamera.set_sprite(this.cameraInfo.spriteCamera[!Singleton<Studio.Studio>.Instance.workInfo.useAlt ? 0 : 1]);
    this.mapInfo.Update();
  }

  public void OnClickMode(int _mode)
  {
    SortCanvas.select = this.modeInfo.canvasInput;
    Singleton<GuideObjectManager>.Instance.mode = _mode;
  }

  private void Instance_ModeChangeEvent(object sender, EventArgs e)
  {
    int mode = Singleton<GuideObjectManager>.Instance.mode;
    this.modeInfo.imageMode[0].set_sprite(this.modeInfo.spriteModeMove[mode != 0 ? 0 : 1]);
    this.modeInfo.imageMode[1].set_sprite(this.modeInfo.spriteModeRotate[mode != 1 ? 0 : 1]);
    this.modeInfo.imageMode[2].set_sprite(this.modeInfo.spriteModeScale[mode != 2 ? 0 : 1]);
  }

  private bool NoCtrlCondition()
  {
    return Singleton<Studio.Studio>.IsInstance() && Singleton<Studio.Studio>.Instance.workInfo.useAlt && (Input.GetKey((KeyCode) 308) ? 1 : (Input.GetKey((KeyCode) 307) ? 1 : 0)) == 0;
  }

  private bool KeyCondition()
  {
    return !Singleton<Studio.Studio>.IsInstance() || !Singleton<Studio.Studio>.Instance.isInputNow;
  }

  public void OnClickCamera()
  {
    bool flag = !Singleton<Studio.Studio>.Instance.workInfo.useAlt;
    Singleton<Studio.Studio>.Instance.workInfo.useAlt = flag;
    this.cameraInfo.imageCamera.set_sprite(this.cameraInfo.spriteCamera[!flag ? 0 : 1]);
  }

  public void OnClickSaveCamera(int _no)
  {
    Singleton<Studio.Studio>.Instance.sceneInfo.cameraData[_no] = this.cameraInfo.cameraCtrl.Export();
  }

  public void OnClickLoadCamera(int _no)
  {
    this.cameraInfo.cameraCtrl.Import(Singleton<Studio.Studio>.Instance.sceneInfo.cameraData[_no]);
  }

  public void OnClickCenter()
  {
    bool flag = !Singleton<Studio.Studio>.Instance.workInfo.visibleCenter;
    Singleton<Studio.Studio>.Instance.workInfo.visibleCenter = flag;
    this.cameraInfo.center = flag;
  }

  public void OnClickAxis()
  {
    bool flag = !Singleton<Studio.Studio>.Instance.workInfo.visibleAxis;
    Singleton<Studio.Studio>.Instance.workInfo.visibleAxis = flag;
    this.cameraInfo.axis = flag;
  }

  public void OnClickAxisTrans()
  {
    this.cameraInfo.axisTrans = !Singleton<Studio.Studio>.Instance.workInfo.visibleAxisTranslation;
    Singleton<GuideObjectManager>.Instance.SetVisibleTranslation();
  }

  public void OnClickAxisCenter()
  {
    this.cameraInfo.axisCenter = !Singleton<Studio.Studio>.Instance.workInfo.visibleAxisCenter;
    Singleton<GuideObjectManager>.Instance.SetVisibleCenter();
  }

  public void OnClickGimmick()
  {
    this.cameraInfo.Gimmick = !Singleton<Studio.Studio>.Instance.workInfo.visibleGimmick;
    Singleton<Studio.Studio>.Instance.SetVisibleGimmick();
  }

  public void OnClickTarget()
  {
  }

  public void OnClickUndo()
  {
    Singleton<UndoRedoManager>.Instance.Undo();
  }

  public void OnClickRedo()
  {
    Singleton<UndoRedoManager>.Instance.Redo();
  }

  private void Instance_CanUndoChange(object sender, EventArgs e)
  {
    ((Selectable) this.buttonUndo).set_interactable(Singleton<UndoRedoManager>.Instance.CanUndo);
  }

  private void Instance_CanRedoChange(object sender, EventArgs e)
  {
    ((Selectable) this.buttonRedo).set_interactable(Singleton<UndoRedoManager>.Instance.CanRedo);
  }

  private void ChangeScale()
  {
    float num = Input.GetAxis("Mouse X") * 0.1f;
    OptionCtrl.InputCombination inputSize = this.optionCtrl.inputSize;
    Studio.Studio.optionSystem.manipulateSize = Mathf.Clamp(Studio.Studio.optionSystem.manipulateSize + num, inputSize.min, inputSize.max);
    Singleton<GuideObjectManager>.Instance.SetScale();
    this.optionCtrl.UpdateUIManipulateSize();
  }

  public void OnClickMap()
  {
    this.mapInfo.active = !this.mapInfo.active;
  }

  private void CreatePatternList()
  {
    PatternSelectListCtrl pslc = Singleton<Studio.Studio>.Instance.patternSelectListCtrl;
    List<ListInfoBase> list = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.st_pattern).Values.ToList<ListInfoBase>();
    list.ForEach((Action<ListInfoBase>) (info => pslc.AddList(info.Id, info.Name, info.GetInfo(ChaListDefine.KeyType.ThumbAB), info.GetInfo(ChaListDefine.KeyType.ThumbTex))));
    pslc.AddOutside(list.Max<ListInfoBase>((Func<ListInfoBase, int>) (l => l.Id)) + 1);
    pslc.Create((PatternSelectListCtrl.OnChangeItemFunc) null);
  }

  public void OnClickInput()
  {
    this.inputInfo.active = !this.inputInfo.active;
  }

  public void LoadItem()
  {
  }

  [DebuggerHidden]
  private IEnumerator LoadItemCoroutine()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    StudioScene.\u003CLoadItemCoroutine\u003Ec__Iterator0 coroutineCIterator0 = new StudioScene.\u003CLoadItemCoroutine\u003Ec__Iterator0();
    return (IEnumerator) coroutineCIterator0;
  }

  protected override void Awake()
  {
    base.Awake();
  }

  [DebuggerHidden]
  private IEnumerator Start()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new StudioScene.\u003CStart\u003Ec__Iterator1()
    {
      \u0024this = this
    };
  }

  [Serializable]
  private class DrawInfo
  {
    public Canvas canvasMainMenu;
    public CanvasGroup canvasMainMenuG;
    public SystemButtonCtrl systemButtonCtrl;
    public Canvas canvasList;
    public Canvas canvasColor;
    public GameObject objOption;
    public GameObject objCamera;
    public Image imageMenu;
    public Sprite[] spriteMenu;
    public Image imageWork;
    public Sprite[] spriteWork;
    public Image imageMove;
    public Sprite[] spriteMove;
    public Image imageColor;
    public Sprite[] spriteColor;
    public Image imageOption;
    public Sprite[] spriteOption;
    public Image imageCamera;
    public Sprite[] spriteCamera;
    public Canvas canvasSystemMenu;
  }

  [Serializable]
  private class ModeInfo
  {
    public Image[] imageMode;
    public Sprite[] spriteModeMove;
    public Sprite[] spriteModeRotate;
    public Sprite[] spriteModeScale;
    public Canvas canvasInput;
  }

  [Serializable]
  public class CameraInfo
  {
    public Image imageCamera;
    public Sprite[] spriteCamera;
    public Studio.CameraControl cameraCtrl;
    public Image imageCenter;
    public Image imageAxis;
    public Image imageAxisTrans;
    public Image imageAxisCenter;
    public Image imageGimmick;
    public Camera cameraSub;
    public PhysicsRaycaster physicsRaycaster;
    public Camera cameraUI;

    public bool center
    {
      get
      {
        return this.cameraCtrl.isOutsideTargetTex;
      }
      set
      {
        if (this.cameraCtrl.isOutsideTargetTex == value)
          return;
        this.cameraCtrl.isOutsideTargetTex = value;
        ((Graphic) this.imageCenter).set_color(!this.cameraCtrl.isOutsideTargetTex ? Color.get_white() : Color.get_green());
      }
    }

    public bool axis
    {
      set
      {
        Singleton<Studio.Studio>.Instance.workInfo.visibleAxis = value;
        ((Behaviour) this.physicsRaycaster).set_enabled(value);
        ((Graphic) this.imageAxis).set_color(!value ? Color.get_white() : Color.get_green());
        this.cameraCtrl.ReflectOption();
      }
    }

    public bool axisTrans
    {
      set
      {
        Singleton<Studio.Studio>.Instance.workInfo.visibleAxisTranslation = value;
        ((Graphic) this.imageAxisTrans).set_color(!value ? Color.get_white() : Color.get_green());
      }
    }

    public bool axisCenter
    {
      set
      {
        Singleton<Studio.Studio>.Instance.workInfo.visibleAxisCenter = value;
        ((Graphic) this.imageAxisCenter).set_color(!value ? Color.get_white() : Color.get_green());
      }
    }

    public bool Gimmick
    {
      set
      {
        Singleton<Studio.Studio>.Instance.workInfo.visibleGimmick = value;
        ((Graphic) this.imageGimmick).set_color(!value ? Color.get_white() : Color.get_green());
      }
    }

    public void Init()
    {
      ((Graphic) this.imageCenter).set_color(!this.cameraCtrl.isOutsideTargetTex ? Color.get_white() : Color.get_green());
      ((Graphic) this.imageAxis).set_color(!((Behaviour) this.cameraSub).get_enabled() ? Color.get_white() : Color.get_green());
    }
  }

  [Serializable]
  private class MapInfo
  {
    public MapCtrl mapCtrl;
    public Button button;
    private Image m_Image;
    private bool m_Active;

    private Image image
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Image, (Object) null))
          this.m_Image = ((Selectable) this.button).get_image();
        return this.m_Image;
      }
    }

    public bool active
    {
      get
      {
        return this.m_Active;
      }
      set
      {
        this.m_Active = value;
        this.Update();
      }
    }

    public void Update()
    {
      ((Graphic) this.image).set_color(!this.m_Active ? Color.get_white() : Color.get_green());
      this.mapCtrl.active = this.m_Active;
    }

    public void OnChangeMap()
    {
      ((Selectable) this.button).set_interactable(Singleton<Studio.Studio>.Instance.sceneInfo.map != -1);
      if (!((Selectable) this.button).get_interactable() && this.active)
        this.active = false;
      else
        this.mapCtrl.Reflect();
    }
  }

  [Serializable]
  private class InputInfo
  {
    private BoolReactiveProperty _outsideVisible = new BoolReactiveProperty(true);
    public ObjectCtrl objectCtrl;
    public Button button;
    private Image _image;
    private bool _active;

    private Image image
    {
      get
      {
        if (Object.op_Equality((Object) this._image, (Object) null))
          this._image = ((Selectable) this.button).get_image();
        return this._image;
      }
    }

    public bool active
    {
      get
      {
        return this._active;
      }
      set
      {
        this._active = value;
        this.Update();
      }
    }

    public bool outsideVisible
    {
      get
      {
        return ((ReactiveProperty<bool>) this._outsideVisible).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._outsideVisible).set_Value(value);
      }
    }

    public void Init()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._outsideVisible, (Action<M0>) (_b => this.UpdateVisible()));
    }

    public void Update()
    {
      ((Graphic) this.image).set_color(!this._active ? Color.get_white() : Color.get_green());
      this.objectCtrl.active = this._active;
    }

    public void OnVisible(bool _value)
    {
      ((Selectable) this.button).set_interactable(_value);
      this.UpdateVisible();
    }

    private void UpdateVisible()
    {
      this.objectCtrl.active = ((Selectable) this.button).get_interactable() & this.active & this.outsideVisible;
    }
  }
}

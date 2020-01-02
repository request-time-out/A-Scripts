// Decompiled with JetBrains decompiler
// Type: HSceneSprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using AIProject.Scene;
using AIProject.UI;
using ConfigScene;
using Illusion.Component.UI.ColorPicker;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx;

public class HSceneSprite : Singleton<HSceneSprite>
{
  public Color colorMGauge = Color.get_white();
  public Color colorFGauge = Color.get_white();
  public float fadeTime = 1f;
  [SerializeField]
  private GameObject[] MenuCategory = new GameObject[2];
  private List<int> lstFinishVisible = new List<int>();
  private HScene.LightInfo[] infoMapLight = new HScene.LightInfo[2]
  {
    new HScene.LightInfo(),
    new HScene.LightInfo()
  };
  public int endFade = -1;
  private bool[] canMainCategory = new bool[6];
  private HSceneNodePool hSceneMotionPool = new HSceneNodePool();
  private List<ScrollCylinderNode> hSceneScrollNodes = new List<ScrollCylinderNode>();
  private bool AllEquip = true;
  private string[][] GuidTiming = new string[4][]
  {
    new string[2]{ "Idle", "D_Idle" },
    new string[6]
    {
      "WLoop",
      "SLoop",
      "OLoop",
      "D_WLoop",
      "D_SLoop",
      "D_OLoop"
    },
    new string[2]{ "Orgasm_IN_A", "D_Orgasm_IN_A" },
    new string[7]
    {
      "Orgasm_A",
      "Orgasm_OUT_A",
      "Drink_A",
      "Vomit_A",
      "OrgasmM_OUT_A",
      "D_Orgasm_A",
      "D_OrgasmM_OUT_A"
    }
  };
  public readonly List<int> NonTokushuCheckIDs = new List<int>()
  {
    3,
    13,
    14
  };
  public Toggle objGaugeLockF;
  public Toggle objGaugeLockM;
  public Image imageMGauge;
  public Image imageFGauge;
  private Color colorMGaugeDef;
  private Color colorFGaugeDef;
  [SerializeField]
  private ParticleSystem AtariEffect;
  public GameObject buttonEnd;
  public HSceneSpriteFinishCategory categoryFinish;
  public HSceneSpriteClothCondition objCloth;
  public HSceneSpriteAccessoryCondition objAccessory;
  public HSceneSpriteCoordinatesCard objClothCard;
  public HSceneSpriteChaChoice charaChoice;
  public HsceneSpriteTaiiCategory categoryMain;
  private RotationScroll CategoryScroll;
  [SerializeField]
  private ScrollCylinder MotionScroll;
  public Toggle tglLeaveItToYou;
  public GameObject objMotionListPanel;
  public GameObject objMotionListInstanceButton;
  public GameObject objMotionList;
  public float HpointSearchRange;
  public GameObject objHItem;
  public HSceneSpriteHitem HItemCtrl;
  public GameObject objLightCategory;
  public HSceneSpriteLightCategory categoryLightDir;
  public PickerRect colorPicker;
  public AnimationCurve fadeAnimation;
  public HScene.AnimationListInfo StartAnimInfo;
  public RawImage imageFade;
  public float timeFadeBase;
  public bool isFade;
  [SerializeField]
  private ParticleSystem FeelHitEffect3D;
  [SerializeField]
  private Vector3 FeelHitEffect3DOffSet;
  public HSceneSpriteCategories categories;
  private HSceneFlagCtrl ctrlFlag;
  private HScene hScene;
  private HSceneSprite.FadeKind kindFade;
  private HSceneSprite.FadeKindProc kindFadeProc;
  private float timeFade;
  private float timeFadeTime;
  private bool isLeaveItToYou;
  private ChaControl[] chaFemales;
  public bool usePoint;
  private List<HScene.AnimationListInfo>[] lstAnimInfo;
  public Button categoryMainButton;
  public Button hPointButton;
  [SerializeField]
  private GameObject MenuCategorySub;
  [SerializeField]
  private GameObject GuageBase;
  private List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>> startList;
  public Animator anim_f;
  public Animator anim_m;
  [SerializeField]
  private CanvasGroup UIGroup;
  private HSceneManager hSceneManager;
  [SerializeField]
  private int PlayerSex;
  private float nowFadeTime;
  private HPointCtrl hPointCtrl;
  [SerializeField]
  private Text MotionListLabel;
  [SerializeField]
  private GameObject SelectArea;
  private Image[] motionListImages;
  private Toggle motionListToggle;
  private IDisposable beforeChoice;
  public bool ChangeStart;
  [SerializeField]
  private GameObject HelpBaseConfig;
  [SerializeField]
  private GameObject HelpBase;
  [SerializeField]
  private Text HelpTxt;
  private const string HelpTextDef = "エッチを開始する";
  private const string CheckmarkName = "Checkmark";
  private const string nowSelectName = "NowSelect";

  [DebuggerHidden]
  public IEnumerator Init()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HSceneSprite.\u003CInit\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void Update()
  {
    if (this.chaFemales == null)
      return;
    for (int index = 0; index < this.MenuCategory.Length; ++index)
      this.MenuCategory[index].SetActive(Manager.Config.HData.MenuIcon);
    this.MenuCategorySub.SetActive(Manager.Config.HData.MenuIcon);
    this.GuageBase.SetActive(Manager.Config.HData.FeelingGauge);
    this.HelpBaseActive(Manager.Config.HData.ActionGuide);
    if (Object.op_Inequality((Object) this.imageFGauge, (Object) null))
    {
      this.imageFGauge.set_fillAmount(this.ctrlFlag.feel_f);
      if ((double) this.ctrlFlag.feel_f >= 0.699999988079071)
        ((Graphic) this.imageFGauge).set_color(this.colorFGauge);
      else
        ((Graphic) this.imageFGauge).set_color(this.colorFGaugeDef);
    }
    if (Object.op_Inequality((Object) this.imageMGauge, (Object) null))
    {
      this.imageMGauge.set_fillAmount(this.ctrlFlag.feel_m);
      if ((double) this.ctrlFlag.feel_m >= 0.699999988079071)
        ((Graphic) this.imageMGauge).set_color(this.colorMGauge);
      else
        ((Graphic) this.imageMGauge).set_color(this.colorMGaugeDef);
    }
    this.FadeProc();
    int num1 = (int) this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1;
    int num2 = (int) this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item2;
    if (Object.op_Inequality((Object) this.anim_f, (Object) null) && ((Behaviour) this.anim_f).get_isActiveAndEnabled())
    {
      AnimatorStateInfo animatorStateInfo = this.anim_f.GetCurrentAnimatorStateInfo(0);
      if (this.ctrlFlag.isGaugeHit && !this.anim_f.IsInTransition(0) && ((AnimatorStateInfo) ref animatorStateInfo).IsName("idle"))
      {
        switch (num1)
        {
          case 1:
            goto label_18;
          case 7:
            if (num2 == 1 || num2 == 2)
              goto label_18;
            else
              break;
        }
        this.anim_f.CrossFade("hit", 0.3f);
      }
label_18:
      if (!this.ctrlFlag.isGaugeHit && !this.anim_f.IsInTransition(0) && ((AnimatorStateInfo) ref animatorStateInfo).IsName("hit"))
        this.anim_f.CrossFade("idle", 0.3f);
    }
    if (Object.op_Implicit((Object) this.anim_m) && ((Behaviour) this.anim_m).get_isActiveAndEnabled())
    {
      AnimatorStateInfo animatorStateInfo = this.anim_m.GetCurrentAnimatorStateInfo(0);
      if (this.ctrlFlag.isGaugeHit_M && !this.anim_m.IsInTransition(0) && ((AnimatorStateInfo) ref animatorStateInfo).IsName("idle"))
      {
        switch (num1)
        {
          case 1:
            this.anim_m.CrossFade("hit", 0.3f);
            break;
          case 7:
            if (num2 == 1 || num2 == 2)
              goto case 1;
            else
              break;
        }
      }
      if (!this.ctrlFlag.isGaugeHit_M && !this.anim_m.IsInTransition(0) && ((AnimatorStateInfo) ref animatorStateInfo).IsName("hit"))
        this.anim_m.CrossFade("idle", 0.3f);
    }
    this.UIFade();
    if (((HSceneSpriteHitem) this.objHItem.GetComponent<HSceneSpriteHitem>()).Effect(6))
      this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.RecoverFaintness;
    if (Object.op_Inequality((Object) this.SelectArea, (Object) null))
    {
      for (int index1 = 0; index1 < this.hSceneScrollNodes.Count; ++index1)
      {
        if (!Object.op_Equality((Object) this.hSceneScrollNodes[index1], (Object) null))
        {
          this.motionListImages = (Image[]) ((UnityEngine.Component) this.hSceneScrollNodes[index1]).GetComponentsInChildren<Image>();
          if (index1 == this.MotionScroll.GetTarget().Item1)
          {
            this.motionListToggle = (Toggle) ((UnityEngine.Component) this.hSceneScrollNodes[index1]).GetComponent<Toggle>();
            if (!this.motionListToggle.get_isOn())
            {
              for (int index2 = 0; index2 < this.motionListImages.Length; ++index2)
              {
                if (((Object) this.motionListImages[index2]).get_name() != "NowSelect")
                  ((Behaviour) this.motionListImages[index2]).set_enabled(false);
                else
                  ((Behaviour) this.motionListImages[index2]).set_enabled(true);
              }
            }
            else
            {
              for (int index2 = 0; index2 < this.motionListImages.Length; ++index2)
              {
                if (((Object) this.motionListImages[index2]).get_name() != "Checkmark")
                  ((Behaviour) this.motionListImages[index2]).set_enabled(false);
                else
                  ((Behaviour) this.motionListImages[index2]).set_enabled(true);
              }
            }
          }
          else
          {
            for (int index2 = 0; index2 < this.motionListImages.Length; ++index2)
            {
              if (((Object) this.motionListImages[index2]).get_name() == "NowSelect")
                ((Behaviour) this.motionListImages[index2]).set_enabled(false);
              else if (Object.op_Equality((Object) this.motionListImages[index2], (Object) this.hSceneScrollNodes[index1].BG))
                ((Behaviour) this.motionListImages[index2]).set_enabled(true);
            }
          }
        }
      }
    }
    if (!this.ChangeStart)
    {
      foreach (ScrollCylinderNode hSceneScrollNode in this.hSceneScrollNodes)
        ((Graphic) hSceneScrollNode.text).set_raycastTarget(true);
    }
    this.categories.Changebuttonactive(!this.ctrlFlag.nowOrgasm);
    if (!this.ctrlFlag.nowOrgasm)
      return;
    if (this.hPointCtrl.IsMarker)
      this.MarkerObjSet();
    if (this.HItemCtrl.ConfirmPanel.get_activeSelf())
      this.HItemCtrl.ConfirmPanel.SetActive(false);
    if (!this.objLightCategory.get_activeSelf())
      return;
    this.objLightCategory.SetActive(false);
  }

  private void LateUpdate()
  {
  }

  private void OnValidate()
  {
  }

  public void SetLightInfo(HScene.LightInfo[] _info)
  {
    for (int index1 = 0; index1 < 2; ++index1)
    {
      int index2 = index1;
      if (_info[index2] == null)
      {
        this.infoMapLight[index2] = (HScene.LightInfo) null;
      }
      else
      {
        this.infoMapLight[index2].objCharaLight = _info[index2].objCharaLight;
        this.infoMapLight[index2].light = _info[index2].light;
        this.infoMapLight[index2].initRot = _info[index2].initRot;
        this.infoMapLight[index2].initIntensity = _info[index2].initIntensity;
        this.infoMapLight[index2].initColor = _info[index2].initColor;
      }
    }
    for (int index1 = 0; index1 < 2; ++index1)
    {
      int index2 = index1;
      if (_info[index2] != null && ((Behaviour) _info[index2].light).get_isActiveAndEnabled())
      {
        this.categoryLightDir.SetValue((float) (((Quaternion) ref this.infoMapLight[index2].initRot).get_eulerAngles().x / 360.0), 0);
        this.categoryLightDir.SetValue((float) (((Quaternion) ref this.infoMapLight[index2].initRot).get_eulerAngles().y / 360.0), 1);
        this.categoryLightDir.SetValue(this.infoMapLight[index2].initIntensity, 2);
      }
    }
  }

  public bool IsSpriteOver()
  {
    EventSystem current = EventSystem.get_current();
    return !Object.op_Equality((Object) current, (Object) null) && current.IsPointerOverGameObject();
  }

  public void setAnimationList(List<HScene.AnimationListInfo>[] _lstAnimInfo)
  {
    this.lstAnimInfo = _lstAnimInfo;
  }

  public void Setting(ChaControl[] _females)
  {
    this.chaFemales = _females;
    if (Singleton<HSceneManager>.Instance.EventKind == HSceneManager.HEvent.GyakuYobai)
    {
      ((Selectable) this.categoryMainButton).set_interactable(false);
      ((Selectable) this.hPointButton).set_interactable(false);
    }
    this.RefleshAutoButtom();
    this.categoryFinish.SetActive(false, -1);
    this.SetAnimationMenu();
  }

  public void RefleshAutoButtom()
  {
    int num = 0;
    for (int index1 = 0; index1 < this.lstAnimInfo.Length; ++index1)
    {
      for (int index2 = 0; index2 < this.lstAnimInfo[index1].Count; ++index2)
      {
        if (this.CheckAutoMotionLimit(this.lstAnimInfo[index1][index2]))
          ++num;
      }
    }
    if (this.PlayerSex == -1)
      this.categoryMain.SetActive(true, -1);
    this.isLeaveItToYou = num != 0 && this.PlayerSex == 0;
    HSceneSprite hsceneSprite = this;
    hsceneSprite.isLeaveItToYou = ((hsceneSprite.isLeaveItToYou ? 1 : 0) & (this.hSceneManager.EventKind == HSceneManager.HEvent.FromFemale ? 1 : (this.hSceneManager.EventKind == HSceneManager.HEvent.GyakuYobai ? 1 : 0))) != 0;
    this.SetVisibleLeaveItToYou(this.isLeaveItToYou, false);
  }

  public void SetFinishSelect(int _mode, int _ctrl, int infomode = -1, int infoctrl = -1)
  {
    switch (_mode)
    {
      case 0:
        this.categoryFinish.SetActive(false, -1);
        this.lstFinishVisible.Clear();
        break;
      case 1:
        this.categoryFinish.SetActive(false, -1);
        this.lstFinishVisible.Clear();
        if (this.ctrlFlag.initiative == 2)
          break;
        ((Text) ((UnityEngine.Component) this.categoryFinish.lstButton[0]).GetComponentInChildren<Text>()).set_text("体にかける");
        this.lstFinishVisible.Add(0);
        if (_ctrl != 1 || this.ctrlFlag.isFaintness)
          break;
        this.lstFinishVisible.Add(4);
        this.lstFinishVisible.Add(5);
        break;
      case 2:
        this.categoryFinish.SetActive(false, -1);
        this.lstFinishVisible.Clear();
        if (this.ctrlFlag.initiative == 2)
          break;
        ((Text) ((UnityEngine.Component) this.categoryFinish.lstButton[0]).GetComponentInChildren<Text>()).set_text("外に出す");
        this.lstFinishVisible.Add(0);
        this.lstFinishVisible.Add(1);
        if (infomode == 2 && infoctrl == 0 || infomode == 3 && infoctrl == 1)
        {
          this.lstFinishVisible.Add(2);
          break;
        }
        if (infomode != 3 || _ctrl != 0 || !this.ctrlFlag.isFaintness)
          break;
        this.categoryFinish.SetActive(false, -1);
        this.lstFinishVisible.Clear();
        break;
      case 3:
        this.categoryFinish.SetActive(false, -1);
        this.lstFinishVisible.Clear();
        break;
      case 4:
        this.categoryFinish.SetActive(false, -1);
        this.lstFinishVisible.Clear();
        break;
      case 5:
        this.categoryFinish.SetActive(false, -1);
        this.lstFinishVisible.Clear();
        break;
      case 6:
        this.categoryFinish.SetActive(false, -1);
        this.lstFinishVisible.Clear();
        break;
      case 7:
        this.categoryFinish.SetActive(false, -1);
        this.lstFinishVisible.Clear();
        switch (_ctrl)
        {
          case 1:
          case 2:
            ((Text) ((UnityEngine.Component) this.categoryFinish.lstButton[0]).GetComponentInChildren<Text>()).set_text("体にかける");
            this.lstFinishVisible.Add(0);
            if (_ctrl != 2 || this.ctrlFlag.isFaintness)
              return;
            this.lstFinishVisible.Add(4);
            this.lstFinishVisible.Add(5);
            return;
          case 3:
          case 4:
            ((Text) ((UnityEngine.Component) this.categoryFinish.lstButton[0]).GetComponentInChildren<Text>()).set_text("外に出す");
            this.lstFinishVisible.Add(0);
            this.lstFinishVisible.Add(1);
            if (_ctrl == 3)
            {
              this.lstFinishVisible.Add(2);
              return;
            }
            if (_ctrl != 4 || !this.ctrlFlag.isFaintness)
              return;
            this.categoryFinish.SetActive(false, -1);
            this.lstFinishVisible.Clear();
            return;
          default:
            return;
        }
    }
  }

  public bool IsFinishVisible(int _num)
  {
    return this.lstFinishVisible.Contains(_num);
  }

  public void SetVisibleLeaveItToYou(bool _visible, bool _judgeLeaveItToYou = false)
  {
    if (_judgeLeaveItToYou)
      _visible = this.isLeaveItToYou;
    ((UnityEngine.Component) this.tglLeaveItToYou).get_gameObject().SetActive(_visible);
  }

  public void SetToggleLeaveItToYou(bool _on)
  {
    this.tglLeaveItToYou.set_isOn(_on);
  }

  public void SetEnableCategoryMain(bool _enable)
  {
    this.categoryMain.SetEnable(_enable, -1);
    ((UnityEngine.Component) this.categoryMain).get_gameObject().SetActive(_enable);
    ((Selectable) this.tglLeaveItToYou).set_interactable(_enable);
    if (_enable)
      return;
    this.categories.MainCategoryActive[3] = false;
  }

  public void SetEnableHItem(bool _enable)
  {
    this.objHItem.SetActive(_enable);
    if (_enable)
      return;
    this.categories.SubCategoryActive[0] = false;
    HSceneSpriteHitem component1 = (HSceneSpriteHitem) this.objHItem.GetComponent<HSceneSpriteHitem>();
    List<ScrollCylinderNode> list = component1.hSceneScroll.GetList();
    if (!((Behaviour) component1.hSceneScroll).get_enabled())
      ((Behaviour) component1.hSceneScroll).set_enabled(true);
    for (int index = 0; index < list.Count; ++index)
    {
      if (!Object.op_Equality((Object) list[index], (Object) null))
      {
        Toggle component2 = (Toggle) ((UnityEngine.Component) list[index]).GetComponent<Toggle>();
        if (!Object.op_Equality((Object) component2, (Object) null) && component2.get_isOn())
          component2.set_isOn(false);
      }
    }
  }

  public void MainCategoryOfLeaveItToYou(bool _isLeaveItToYou)
  {
    if (!_isLeaveItToYou)
    {
      this.SetAnimationMenu();
    }
    else
    {
      bool flag = true;
      for (int _array = 0; _array < this.lstAnimInfo.Length; ++_array)
      {
        int num = 0;
        for (int index = 0; index < this.lstAnimInfo[_array].Count; ++index)
        {
          if (Object.op_Inequality((Object) this.chaFemales[1], (Object) null))
          {
            if (_array >= 4)
            {
              if (this.PlayerSex == 0)
              {
                if (_array != 5)
                  continue;
              }
              else if (_array != 4)
                continue;
            }
            else
              continue;
          }
          else if (_array > 3)
            continue;
          if (this.ctrlFlag.initiative == 1)
          {
            if (this.lstAnimInfo[_array][index].nInitiativeFemale != 1 && (!flag || this.lstAnimInfo[_array][index].nInitiativeFemale != 2))
              continue;
          }
          else if (this.ctrlFlag.initiative != 2 || this.lstAnimInfo[_array][index].nInitiativeFemale != 2)
            continue;
          ++num;
        }
        if (this.PlayerSex != -1)
          this.categoryMain.SetActive(num != 0, _array);
      }
      if (this.PlayerSex == -1)
        this.categoryMain.SetActive(true, -1);
      this.CategoryScroll.ListNodeSet((List<ScrollCylinderNode>) null, true);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (Action<M0>) (x => this.CategoryScroll.SetTarget((int) this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1)));
    }
  }

  public bool IsEnableLeaveItToYou()
  {
    return ((Selectable) this.tglLeaveItToYou).get_interactable() && ((UnityEngine.Component) this.tglLeaveItToYou).get_gameObject().get_activeSelf();
  }

  public void OnChangePlaySelect(GameObject objClick)
  {
    Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
    if (!Object.op_Inequality((Object) null, (Object) objClick))
      return;
    HAnimationInfoComponent component = (HAnimationInfoComponent) objClick.GetComponent<HAnimationInfoComponent>();
    if (Object.op_Inequality((Object) null, (Object) component))
      this.ctrlFlag.selectAnimationListInfo = component.info;
    ((Toggle) objClick.GetComponent<Toggle>()).set_isOn(true);
    this.ChangeStart = true;
  }

  private void MotionListView(int taii)
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()) || this.endFade == 0)
      return;
    this.SetMotionListDraw(true, taii);
  }

  public void OnClickMotion(int _motion)
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    HRotationScrollNode component = (HRotationScrollNode) ((UnityEngine.Component) this.categoryMain.GetHScroll().GetTarget().Item2).GetComponent<HRotationScrollNode>();
    if (Object.op_Equality((Object) component, (Object) null) || component.id != _motion || this.endFade == 0)
      return;
    this.SetMotionListDraw(true, _motion);
  }

  public void SetMotionListDraw(bool _active, int _motion = -1)
  {
    this.objMotionListPanel.SetActive(_active);
    this.ctrlFlag.categoryMotionList = _motion;
    if (_active)
    {
      this.LoadMotionList(this.ctrlFlag.categoryMotionList);
      switch (_motion)
      {
        case 0:
          this.MotionListLabel.set_text("愛撫");
          break;
        case 1:
          this.MotionListLabel.set_text("奉仕");
          break;
        case 2:
          this.MotionListLabel.set_text("挿入");
          break;
        case 3:
          this.MotionListLabel.set_text("特殊");
          break;
        case 4:
          this.MotionListLabel.set_text("レズ");
          break;
        case 5:
          this.MotionListLabel.set_text("複数");
          break;
      }
    }
    else
    {
      if (!((UnityEngine.Component) this.categoryMain).get_gameObject().get_activeSelf())
        return;
      ((UnityEngine.Component) this.categoryMain).get_gameObject().SetActive(false);
    }
  }

  public void OnClickMainCategories(int _menu)
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()) || (this.endFade == 0 || this.HItemCtrl.ConfirmPanel.get_activeSelf()))
      return;
    GameObject[] gameObjectArray1 = new GameObject[4]
    {
      ((UnityEngine.Component) this.objCloth).get_gameObject(),
      ((UnityEngine.Component) this.objAccessory).get_gameObject(),
      ((UnityEngine.Component) this.objClothCard).get_gameObject(),
      ((UnityEngine.Component) this.categoryMain).get_gameObject()
    };
    if (gameObjectArray1[_menu].get_activeSelf())
    {
      gameObjectArray1[_menu].SetActive(false);
      if (_menu == 3)
        this.SetMotionListDraw(false, -1);
      this.charaChoice.CloseChoice();
      ((UnityEngine.Component) this.charaChoice).get_gameObject().SetActive(false);
      this.charaChoice.SetMaleSelectBtn(false);
      this.categories.MainCategoryActive[_menu] = false;
      if (_menu == 2)
        this.objClothCard.CloseSort();
    }
    else
    {
      if (_menu != 3)
      {
        gameObjectArray1[_menu].SetActive(true);
        this.categories.MainCategoryActive[_menu] = true;
      }
      else if (((ScrollCylinderNode[]) this.CategoryScroll.Contents.GetComponentsInChildren<ScrollCylinderNode>()).Length > 0)
      {
        this.SetEnableCategoryMain(true);
        this.categories.MainCategoryActive[_menu] = true;
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (Action<M0>) (_ =>
        {
          this.CategoryScroll.ListNodeSet((List<ScrollCylinderNode>) null, false);
          ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (Action<M0>) (x =>
          {
            this.CategoryScroll.SetTarget((int) this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1);
            this.MotionListView((int) this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1);
          }));
        }));
      }
      for (int index = 0; index < gameObjectArray1.Length; ++index)
      {
        if (index != _menu)
        {
          gameObjectArray1[index].SetActive(false);
          this.categories.MainCategoryActive[index] = false;
        }
      }
      if (this.hPointCtrl.IsMarker)
        this.MarkerObjSet();
      this.categories.MainCategoryActive[this.categories.MainCategoryActive.Length - 1] = false;
      if (_menu != 3)
        this.SetMotionListDraw(false, -1);
      else
        this.SetAnimationMenu();
      this.charaChoice.SetMaleSelectBtn(_menu == 1);
      switch (_menu)
      {
        case 0:
          this.objCloth.SetClothCharacter(false);
          break;
        case 1:
          this.objAccessory.SetAccessoryCharacter(false);
          break;
      }
      ((UnityEngine.Component) this.charaChoice).get_gameObject().SetActive(_menu < 3);
    }
    GameObject[] gameObjectArray2 = new GameObject[2]
    {
      this.objHItem,
      this.objLightCategory
    };
    foreach (GameObject gameObject in gameObjectArray2)
      gameObject.SetActive(false);
    this.HItemCtrl.ConfirmPanel.SetActive(false);
    for (int index = 0; index < this.categories.SubCategoryActive.Length; ++index)
      this.categories.SubCategoryActive[index] = false;
  }

  public void OnClickSubCategories(int _menu)
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()) || (this.endFade == 0 || this.HItemCtrl.ConfirmPanel.get_activeSelf()))
      return;
    GameObject[] gameObjectArray1 = new GameObject[4]
    {
      ((UnityEngine.Component) this.objCloth).get_gameObject(),
      ((UnityEngine.Component) this.objAccessory).get_gameObject(),
      ((UnityEngine.Component) this.objClothCard).get_gameObject(),
      ((UnityEngine.Component) this.categoryMain).get_gameObject()
    };
    foreach (GameObject gameObject in gameObjectArray1)
      gameObject.SetActive(false);
    this.objClothCard.CloseSort();
    this.SetMotionListDraw(false, -1);
    for (int index = 0; index < this.categories.MainCategoryActive.Length; ++index)
      this.categories.MainCategoryActive[index] = false;
    if (this.hPointCtrl.IsMarker)
      this.MarkerObjSet();
    GameObject[] gameObjectArray2 = new GameObject[2]
    {
      this.objHItem,
      this.objLightCategory
    };
    if (gameObjectArray2[_menu].get_activeSelf())
    {
      gameObjectArray2[_menu].SetActive(false);
      this.categories.SubCategoryActive[_menu != 0 ? 2 : _menu] = false;
    }
    else
    {
      gameObjectArray2[_menu].SetActive(true);
      this.categories.SubCategoryActive[_menu != 0 ? 2 : _menu] = true;
      if (_menu == 0)
      {
        ((HSceneSpriteHitem) this.objHItem.GetComponent<HSceneSpriteHitem>()).SetVisible(true);
      }
      else
      {
        for (int index = 0; index < this.infoMapLight.Length; ++index)
        {
          if (this.infoMapLight[index] != null && ((Behaviour) this.infoMapLight[index].light).get_isActiveAndEnabled())
          {
            this.colorPicker.SetColor(this.infoMapLight[index].light.get_color());
            break;
          }
        }
      }
      for (int index = 0; index < gameObjectArray2.Length; ++index)
      {
        if (index != _menu)
          gameObjectArray2[index].SetActive(false);
      }
      for (int index = 0; index < this.categories.SubCategoryActive.Length; ++index)
      {
        if (index != (_menu != 0 ? 2 : _menu))
          this.categories.SubCategoryActive[index] = false;
      }
    }
    this.charaChoice.CloseChoice();
    ((UnityEngine.Component) this.charaChoice).get_gameObject().SetActive(false);
  }

  public void OnClickFinishBefore()
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.FinishBefore;
  }

  public void OnClickFinishInSide()
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.FinishInSide;
  }

  public void OnClickFinishOutSide()
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.FinishOutSide;
  }

  public void OnClickFinishSame()
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.FinishSame;
  }

  public void OnClickFinishDrink()
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.FinishDrink;
  }

  public void OnClickFinishVomit()
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.FinishVomit;
  }

  public void OnClickSpanking()
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.Spnking;
  }

  public void OnClickSceneEnd()
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()) || this.endFade == 0)
      return;
    Singleton<GameCursor>.Instance.SetCursorLock(false);
    ConfirmScene.Sentence = "Hシーンを終了しますか";
    ConfirmScene.OnClickedYes = (Action) (() => this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.SceneEnd);
    ConfirmScene.OnClickedNo = (Action) (() => {});
    Singleton<Game>.Instance.LoadDialog();
  }

  public void OnClickStopFeel(int _sex)
  {
    if (Singleton<Manager.Scene>.IsInstance() && (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade) || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    if (_sex == 0)
      this.ctrlFlag.stopFeelMale = !this.ctrlFlag.stopFeelMale;
    else
      this.ctrlFlag.stopFeelFemal = !this.ctrlFlag.stopFeelFemal;
  }

  public void OnClickMovePoint()
  {
    if (this.endFade == 0 || this.HItemCtrl.ConfirmPanel.get_activeSelf())
      return;
    this.MarkerObjSet();
  }

  public void OnClickLeave()
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()) || (!this.IsEnableLeaveItToYou() || !Input.GetMouseButtonUp(0)))
      return;
    this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.LeaveItToYou;
    this.SetMotionListDraw(false, -1);
    ((UnityEngine.Component) this.categoryMain).get_gameObject().SetActive(false);
    this.categories.MainCategoryActive[3] = false;
  }

  public void OnValueLightColorChanged(Color color)
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    for (int index = 0; index < 2; ++index)
    {
      if (this.infoMapLight[index] != null)
        this.infoMapLight[index].light.set_color(color);
    }
    GlobalMethod.setCameraMoveFlag(this.ctrlFlag.cameraCtrl, false);
  }

  public void OnValueLightDireChanged()
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    float num1 = this.categoryLightDir.GetValue(0) * 360f;
    float num2 = this.categoryLightDir.GetValue(1) * 360f;
    for (int index = 0; index < 2; ++index)
    {
      if (this.infoMapLight[index] != null)
        this.infoMapLight[index].objCharaLight.get_transform().set_localRotation(Quaternion.Euler(num1, num2, 0.0f));
    }
    GlobalMethod.setCameraMoveFlag(this.ctrlFlag.cameraCtrl, false);
  }

  public void OnValuePowerChanged()
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    for (int index = 0; index < 2; ++index)
    {
      if (this.infoMapLight[index] != null)
        this.infoMapLight[index].light.set_intensity(Mathf.Lerp(this.infoMapLight[index].minIntensity, this.infoMapLight[index].maxIntensity, this.categoryLightDir.GetValue(2)));
    }
    GlobalMethod.setCameraMoveFlag(this.ctrlFlag.cameraCtrl, false);
  }

  public void ReSetLight()
  {
    this.ReSetLightDir();
    this.ReSetLightColor();
    this.ReSetLightPower();
  }

  public void ReSetLightDir()
  {
    for (int index = 0; index < 2; ++index)
    {
      if (this.infoMapLight[index] != null)
        this.infoMapLight[index].objCharaLight.get_transform().set_localRotation(this.infoMapLight[index].initRot);
    }
  }

  public void ReSetLightPower()
  {
    for (int index = 0; index < 2; ++index)
    {
      if (this.infoMapLight[index] != null)
        this.infoMapLight[index].light.set_intensity(Mathf.Lerp(this.infoMapLight[index].minIntensity, this.infoMapLight[index].maxIntensity, this.infoMapLight[index].initIntensity));
    }
  }

  public void ReSetLightColor()
  {
    for (int index = 0; index < 2; ++index)
    {
      if (this.infoMapLight[index] != null)
        this.infoMapLight[index].light.set_color(this.infoMapLight[index].initColor);
    }
  }

  public void OnClickConfig()
  {
    if (Object.op_Inequality((Object) Singleton<Game>.Instance.Config, (Object) null) | Object.op_Inequality((Object) Singleton<Game>.Instance.Dialog, (Object) null) | Object.op_Inequality((Object) Singleton<Game>.Instance.ExitScene, (Object) null) | Object.op_Inequality((Object) Singleton<Game>.Instance.MapShortcutUI, (Object) null))
      return;
    GameObject[] gameObjectArray1 = new GameObject[4]
    {
      ((UnityEngine.Component) this.objCloth).get_gameObject(),
      ((UnityEngine.Component) this.objAccessory).get_gameObject(),
      ((UnityEngine.Component) this.objClothCard).get_gameObject(),
      ((UnityEngine.Component) this.categoryMain).get_gameObject()
    };
    for (int index = 0; index < gameObjectArray1.Length; ++index)
    {
      if (gameObjectArray1[index].get_activeSelf())
      {
        gameObjectArray1[index].SetActive(false);
        this.categories.MainCategoryActive[index] = false;
      }
    }
    this.SetMotionListDraw(false, -1);
    this.charaChoice.CloseChoice();
    ((UnityEngine.Component) this.charaChoice).get_gameObject().SetActive(false);
    this.objClothCard.CloseSort();
    if (this.hPointCtrl.IsMarker)
    {
      this.hPointCtrl.MarkerObjDel();
      this.categories.MainCategoryActive[this.categories.MainCategoryActive.Length - 1] = false;
    }
    GameObject[] gameObjectArray2 = new GameObject[2]
    {
      this.objHItem,
      this.objLightCategory
    };
    foreach (GameObject gameObject in gameObjectArray2)
      gameObject.SetActive(false);
    this.categories.SubCategoryActive[0] = false;
    this.categories.SubCategoryActive[2] = false;
    this.SetEnableHItem(false);
    this.HItemCtrl.ConfirmPanel.SetActive(false);
    this.categories.AllForceClose(1);
    ConfigWindow.UnLoadAction = (Action) (() => {});
    ConfigWindow.TitleChangeAction = (Action) (() =>
    {
      this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.SceneEnd;
      this.hScene.NowStateIsEnd = true;
      this.hScene.ConfigEnd();
      ConfigWindow.UnLoadAction = (Action) null;
      Singleton<Game>.Instance.Dialog.TimeScale = 1f;
    });
    Singleton<Game>.Instance.LoadConfig();
  }

  public void FadeState(HSceneSprite.FadeKind _kind, float _timeFade = -1f)
  {
    this.isFade = true;
    this.timeFadeTime = 0.0f;
    this.timeFade = (double) _timeFade >= 0.0 ? (_kind == HSceneSprite.FadeKind.OutIn ? _timeFade * 2f : _timeFade) : (_kind == HSceneSprite.FadeKind.OutIn ? this.timeFadeBase * 2f : this.timeFadeBase);
    this.kindFade = _kind;
    switch (this.kindFade)
    {
      case HSceneSprite.FadeKind.Out:
        this.kindFadeProc = HSceneSprite.FadeKindProc.Out;
        break;
      case HSceneSprite.FadeKind.In:
        this.kindFadeProc = HSceneSprite.FadeKindProc.In;
        break;
      case HSceneSprite.FadeKind.OutIn:
        this.kindFadeProc = HSceneSprite.FadeKindProc.OutIn;
        break;
    }
  }

  public HSceneSprite.FadeKindProc GetFadeKindProc()
  {
    return this.kindFadeProc;
  }

  private bool FadeProc()
  {
    if (!Object.op_Implicit((Object) this.imageFade) || !this.isFade)
      return false;
    this.timeFadeTime += Time.get_deltaTime();
    Color color = ((Graphic) this.imageFade).get_color();
    float num = this.fadeAnimation.Evaluate(Mathf.Clamp01(this.timeFadeTime / this.timeFade));
    switch (this.kindFade)
    {
      case HSceneSprite.FadeKind.Out:
        color.a = (__Null) (double) num;
        break;
      case HSceneSprite.FadeKind.In:
        color.a = (__Null) (1.0 - (double) num);
        break;
      case HSceneSprite.FadeKind.OutIn:
        color.a = (__Null) (double) Mathf.Sin((float) Math.PI / 180f * Mathf.Lerp(0.0f, 180f, num));
        break;
    }
    ((Graphic) this.imageFade).set_color(color);
    if ((double) num >= 1.0)
    {
      this.isFade = false;
      switch (this.kindFade)
      {
        case HSceneSprite.FadeKind.Out:
          this.kindFadeProc = HSceneSprite.FadeKindProc.OutEnd;
          break;
        case HSceneSprite.FadeKind.In:
          this.kindFadeProc = HSceneSprite.FadeKindProc.InEnd;
          break;
        case HSceneSprite.FadeKind.OutIn:
          this.kindFadeProc = HSceneSprite.FadeKindProc.OutInEnd;
          break;
      }
    }
    return true;
  }

  public bool LoadMotionList(int _motion)
  {
    List<GameObject> list = this.hSceneMotionPool.GetList();
    for (int index = 0; index < list.Count; ++index)
    {
      if (list[index].get_gameObject().get_activeSelf())
        list[index].get_gameObject().SetActive(false);
    }
    this.hSceneScrollNodes.Clear();
    if (_motion < 0 || this.lstAnimInfo.Length <= _motion)
      return true;
    PointerClickTrigger pointerClickTrigger1 = (PointerClickTrigger) null;
    for (int index = 0; index < this.lstAnimInfo[_motion].Count; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HSceneSprite.\u003CLoadMotionList\u003Ec__AnonStorey1 listCAnonStorey1 = new HSceneSprite.\u003CLoadMotionList\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      listCAnonStorey1.\u0024this = this;
      if ((!this.hSceneManager.bMerchant || this.hSceneManager.MerchantLimit != 1 || (this.PlayerSex != 0 || _motion == 1) && (this.PlayerSex != 1 || _motion == 4)) && this.CheckMotionLimit(this.lstAnimInfo[_motion][index]))
      {
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey1.objClone = this.hSceneMotionPool.Get(this.hSceneScrollNodes.Count);
        // ISSUE: reference to a compiler-generated field
        HAnimationInfoComponent component1 = (HAnimationInfoComponent) listCAnonStorey1.objClone.GetComponent<HAnimationInfoComponent>();
        component1.info = this.lstAnimInfo[_motion][index];
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey1.objClone.get_transform().SetParent(this.objMotionList.get_transform(), false);
        // ISSUE: reference to a compiler-generated field
        GameObject loop = listCAnonStorey1.objClone.get_transform().FindLoop("Label");
        if (Object.op_Implicit((Object) loop))
        {
          Text component2 = (Text) loop.GetComponent<Text>();
          component2.set_text(component1.info.nameAnimation);
          if (component2.get_text() != this.ctrlFlag.nowAnimationInfo.nameAnimation)
          {
            // ISSUE: reference to a compiler-generated field
            ((Toggle) listCAnonStorey1.objClone.GetComponent<Toggle>()).set_isOn(false);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            ((Toggle) listCAnonStorey1.objClone.GetComponent<Toggle>()).set_isOn(true);
          }
        }
        pointerClickTrigger1 = (PointerClickTrigger) null;
        UITrigger.TriggerEvent triggerEvent = new UITrigger.TriggerEvent();
        // ISSUE: reference to a compiler-generated field
        listCAnonStorey1.no = this.hSceneScrollNodes.Count;
        // ISSUE: reference to a compiler-generated field
        PointerClickTrigger pointerClickTrigger2 = (PointerClickTrigger) listCAnonStorey1.objClone.GetComponent<PointerClickTrigger>();
        if (Object.op_Equality((Object) pointerClickTrigger2, (Object) null))
        {
          // ISSUE: reference to a compiler-generated field
          pointerClickTrigger2 = (PointerClickTrigger) listCAnonStorey1.objClone.AddComponent<PointerClickTrigger>();
        }
        ((UITrigger) pointerClickTrigger2).get_Triggers().Clear();
        ((UITrigger) pointerClickTrigger2).get_Triggers().Add(triggerEvent);
        // ISSUE: method pointer
        ((UnityEvent<BaseEventData>) triggerEvent).AddListener(new UnityAction<BaseEventData>((object) listCAnonStorey1, __methodptr(\u003C\u003Em__0)));
        if (this.lstAnimInfo[_motion][index] == this.ctrlFlag.nowAnimationInfo)
        {
          // ISSUE: reference to a compiler-generated field
          ((Toggle) listCAnonStorey1.objClone.GetComponent<Toggle>()).set_isOn(true);
        }
        // ISSUE: reference to a compiler-generated field
        this.hSceneScrollNodes.Add((ScrollCylinderNode) listCAnonStorey1.objClone.GetComponent<ScrollCylinderNode>());
      }
    }
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (Action<M0>) (_ => this.MotionScroll.ListNodeSet(this.hSceneScrollNodes)));
    return true;
  }

  public bool SetAnimationMenu()
  {
    int[] numArray = new int[this.lstAnimInfo.Length];
    for (int index1 = 0; index1 < this.lstAnimInfo.Length; ++index1)
    {
      if (this.hSceneManager.bMerchant && this.hSceneManager.MerchantLimit != 2)
      {
        if (this.hSceneManager.MerchantLimit == 1)
        {
          if ((this.hSceneManager.Player.ChaControl.sex == (byte) 0 || this.hSceneManager.bFutanari) && index1 != 1 || this.hSceneManager.Player.ChaControl.sex == (byte) 1 && !this.hSceneManager.bFutanari && index1 != 4)
            continue;
        }
        else
          break;
      }
      for (int index2 = 0; index2 < this.lstAnimInfo[index1].Count; ++index2)
      {
        if (this.CheckMotionLimit(this.lstAnimInfo[index1][index2]))
          ++numArray[index1];
      }
    }
    for (int _array = 0; _array < numArray.Length; ++_array)
    {
      this.canMainCategory[_array] = numArray[_array] != 0;
      this.categoryMain.SetActive(this.canMainCategory[_array], _array);
    }
    if (this.PlayerSex == -1)
      this.categoryMain.SetActive(true, -1);
    return true;
  }

  private void UIFade()
  {
    if (this.endFade != 0)
      return;
    this.nowFadeTime += Time.get_unscaledDeltaTime();
    this.UIGroup.set_alpha(Mathf.Lerp(0.0f, 1f, this.nowFadeTime / this.fadeTime));
    if ((double) this.UIGroup.get_alpha() < 1.0)
      return;
    this.endFade = 1;
    this.UIGroup.set_blocksRaycasts(true);
  }

  public void PopupCommands(bool isForce)
  {
    this.hScene.ctrlVoice.HBeforeHouchiTime = 0.0f;
    Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
    Singleton<Input>.Instance.SetupState();
    this.startList = Singleton<Resources>.Instance.HSceneTable.lstStartAnimInfo;
    MapUIContainer.CommandList.CancelEvent = (Action) null;
    if (this.hSceneManager.Player.ChaControl.sex == (byte) 0 || this.hSceneManager.Player.ChaControl.sex == (byte) 1 && this.ctrlFlag.bFutanari)
    {
      if (Object.op_Equality((Object) this.hScene.GetFemales()[1], (Object) null))
        this.CommandRefresh(isForce);
      else if (this.StartListContain(this.startList, this.hSceneManager.height, HSceneManager.HEvent.Normal, 5))
        MapUIContainer.RefreshCommands(0, new CommCommandList.CommandInfo[1]
        {
          new CommCommandList.CommandInfo("リードする", (Func<bool>) null, (Action<int>) (x =>
          {
            this.ctrlFlag.AddParam(8, 1);
            this.hScene.SetStartAnimationInfo(HSceneManager.HEvent.Normal, 5);
            this.CommandProc();
            Singleton<HSceneFlagCtrl>.Instance.BeforeHWait = false;
          }))
        });
      MapUIContainer.SetActiveCommandList(true, "どう始める？");
    }
    else
    {
      if (!this.StartListContain(this.startList, this.hSceneManager.height, HSceneManager.HEvent.Normal, 4))
        return;
      MapUIContainer.RefreshCommands(0, new CommCommandList.CommandInfo[1]
      {
        new CommCommandList.CommandInfo("リードする", (Func<bool>) null, (Action<int>) (x =>
        {
          this.ctrlFlag.AddParam(7, 1);
          this.hScene.SetStartAnimationInfo(HSceneManager.HEvent.Normal, 4);
          this.CommandProc();
          Singleton<HSceneFlagCtrl>.Instance.BeforeHWait = false;
        }))
      });
      MapUIContainer.SetActiveCommandList(true, "どう始める？");
    }
  }

  private void CommandRefresh(bool isForce)
  {
    List<CommCommandList.CommandInfo> commandInfoList = new List<CommCommandList.CommandInfo>();
    if (!isForce)
    {
      bool[] flagArray = new bool[3]
      {
        this.StartListContain(this.startList, this.hSceneManager.height, HSceneManager.HEvent.Normal, 0),
        this.StartListContain(this.startList, this.hSceneManager.height, HSceneManager.HEvent.Normal, 1),
        this.StartListContain(this.startList, this.hSceneManager.height, HSceneManager.HEvent.Normal, 2)
      };
      CommCommandList.CommandInfo[] commandInfoArray = new CommCommandList.CommandInfo[3]
      {
        new CommCommandList.CommandInfo("リードする", (Func<bool>) null, (Action<int>) (x =>
        {
          this.ctrlFlag.AddParam(0, 0);
          this.hScene.SetStartAnimationInfo(HSceneManager.HEvent.Normal, 0);
          this.CommandProc();
          Singleton<HSceneFlagCtrl>.Instance.BeforeHWait = false;
        })),
        new CommCommandList.CommandInfo("してほしい", (Func<bool>) null, (Action<int>) (x =>
        {
          this.ctrlFlag.AddParam(1, 0);
          this.hScene.SetStartAnimationInfo(HSceneManager.HEvent.Normal, 1);
          this.CommandProc();
          Singleton<HSceneFlagCtrl>.Instance.BeforeHWait = false;
        })),
        new CommCommandList.CommandInfo("いきなり挿入する", (Func<bool>) null, (Action<int>) (x =>
        {
          this.ctrlFlag.AddParam(2, 0);
          this.hScene.SetStartAnimationInfo(HSceneManager.HEvent.Normal, 2);
          this.CommandProc();
          Singleton<HSceneFlagCtrl>.Instance.BeforeHWait = false;
        }))
      };
      for (int index = 0; index < flagArray.Length; ++index)
      {
        if (flagArray[index])
          commandInfoList.Add(commandInfoArray[index]);
      }
      if (commandInfoList.Count == 0)
        return;
      CommCommandList.CommandInfo[] options = new CommCommandList.CommandInfo[commandInfoList.Count];
      for (int index = 0; index < options.Length; ++index)
        options[index] = commandInfoList[index];
      MapUIContainer.RefreshCommands(0, options);
    }
    else
    {
      bool[] flagArray = new bool[2]
      {
        this.StartListContain(this.startList, this.hSceneManager.height, HSceneManager.HEvent.Normal, 1),
        this.StartListContain(this.startList, this.hSceneManager.height, HSceneManager.HEvent.Normal, 2)
      };
      CommCommandList.CommandInfo[] commandInfoArray = new CommCommandList.CommandInfo[2]
      {
        new CommCommandList.CommandInfo("奉仕させる", (Func<bool>) null, (Action<int>) (x =>
        {
          this.hScene.SetStartAnimationInfo(HSceneManager.HEvent.Normal, 1);
          this.CommandProc();
          Singleton<HSceneFlagCtrl>.Instance.BeforeHWait = false;
        })),
        new CommCommandList.CommandInfo("挿入する", (Func<bool>) null, (Action<int>) (x =>
        {
          this.hScene.SetStartAnimationInfo(HSceneManager.HEvent.Normal, 2);
          this.CommandProc();
          Singleton<HSceneFlagCtrl>.Instance.BeforeHWait = false;
        }))
      };
      for (int index = 0; index < flagArray.Length; ++index)
      {
        if (flagArray[index])
          commandInfoList.Add(commandInfoArray[index]);
      }
      if (commandInfoList.Count == 0)
        return;
      CommCommandList.CommandInfo[] options = new CommCommandList.CommandInfo[commandInfoList.Count];
      for (int index = 0; index < options.Length; ++index)
        options[index] = commandInfoList[index];
      MapUIContainer.RefreshCommands(0, options);
    }
  }

  private void CommandProc()
  {
    MapUIContainer.SetActiveCommandList(false);
    this.beforeChoice = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) MapUIContainer.CommandList.OnCompletedStopAsObservable(), 1), (Action<M0>) (_ =>
    {
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      ((Behaviour) this.hSceneManager.Player.CameraControl).set_enabled(false);
      ((Behaviour) this.hSceneManager.Player.Controller).set_enabled(false);
      ((Behaviour) this.hSceneManager.Player.Animation).set_enabled(false);
      this.hSceneManager.Player.CameraControl.EnabledInput = false;
    }));
    this.StartAnimInfo = this.hScene.StartAnimInfo;
    this.hScene.setCameraLoad(this.StartAnimInfo, true);
  }

  public void MarkerObjSet()
  {
    if (Singleton<Manager.Scene>.Instance.IsNowLoading || Singleton<Manager.Scene>.Instance.IsNowLoadingFade || (this.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    if (!this.hPointCtrl.IsMarker)
    {
      this.hPointCtrl.MarkerObjSet(((UnityEngine.Component) this.ctrlFlag.nowHPoint).get_transform().get_position(), Singleton<Manager.Map>.Instance.MapID, this.hSceneManager.females[0].AreaID);
      this.categories.MainCategoryActive[this.categories.MainCategoryActive.Length - 1] = true;
      GameObject[] gameObjectArray1 = new GameObject[4]
      {
        ((UnityEngine.Component) this.objCloth).get_gameObject(),
        ((UnityEngine.Component) this.objAccessory).get_gameObject(),
        ((UnityEngine.Component) this.objClothCard).get_gameObject(),
        ((UnityEngine.Component) this.categoryMain).get_gameObject()
      };
      for (int index = 0; index < gameObjectArray1.Length; ++index)
      {
        gameObjectArray1[index].SetActive(false);
        if (index == 3)
          this.SetMotionListDraw(false, -1);
        this.charaChoice.CloseChoice();
        ((UnityEngine.Component) this.charaChoice).get_gameObject().SetActive(false);
        this.categories.MainCategoryActive[index] = false;
      }
      this.objClothCard.CloseSort();
      GameObject[] gameObjectArray2 = new GameObject[2]
      {
        this.objHItem,
        this.objLightCategory
      };
      foreach (GameObject gameObject in gameObjectArray2)
        gameObject.SetActive(false);
      for (int index = 0; index < this.categories.SubCategoryActive.Length; ++index)
        this.categories.SubCategoryActive[index] = false;
    }
    else
    {
      this.hPointCtrl.MarkerObjDel();
      this.categories.MainCategoryActive[this.categories.MainCategoryActive.Length - 1] = false;
    }
  }

  private bool CheckMotionLimit(HScene.AnimationListInfo lstAnimInfo)
  {
    if (this.PlayerSex == 0 || this.PlayerSex == 1 && this.ctrlFlag.bFutanari)
    {
      if (Object.op_Equality((Object) this.hSceneManager.females[1], (Object) null))
      {
        if (lstAnimInfo.nPromiscuity == 1)
          return false;
      }
      else if (lstAnimInfo.nPromiscuity != 1)
        return false;
      if (lstAnimInfo.nPromiscuity == 2 || lstAnimInfo.ActionCtrl.Item1 == 3 && !this.NonTokushuCheckIDs.Contains(lstAnimInfo.id) && lstAnimInfo.fileMale == string.Empty)
        return false;
    }
    else if (this.PlayerSex == 1 && lstAnimInfo.nPromiscuity < 2)
      return false;
    if (!this.hSceneManager.bMerchant && (lstAnimInfo.nHentai == 1 && !this.hSceneManager.isHAddTaii[0] || lstAnimInfo.nHentai == 2 && !this.hSceneManager.isHAddTaii[1]))
      return false;
    if (this.hSceneManager.bMerchant)
    {
      if (!lstAnimInfo.bMerchantMotion || lstAnimInfo.nIyaAction == 2)
        return false;
    }
    else if (this.hSceneManager.EventKind != HSceneManager.HEvent.Yobai)
    {
      if (this.hSceneManager.isForce)
      {
        if (lstAnimInfo.nIyaAction == 0)
          return false;
      }
      else if (lstAnimInfo.nIyaAction == 2)
        return false;
    }
    else if (!lstAnimInfo.bSleep)
      return false;
    if (!this.usePoint)
    {
      int num = (int) this.hSceneManager.hitHmesh.Item3;
      if (num != -1 && !lstAnimInfo.nPositons.Contains(num) || num == -1 && !lstAnimInfo.nPositons.Contains(0) && !lstAnimInfo.nPositons.Contains(1))
        return false;
    }
    else if (this.ctrlFlag.HPointID == 0)
    {
      if (!lstAnimInfo.nPositons.Contains((int) this.hPointCtrl.InitNull.Item3))
        return false;
    }
    else
    {
      foreach (HPoint.NotMotionInfo notMotionInfo in ((HPoint) this.hPointCtrl.lstMarker[this.ctrlFlag.HPointID].Item2).notMotion)
      {
        if (notMotionInfo.motionID.Contains(lstAnimInfo.id))
          return false;
      }
      if (!HPointCtrl.DicTupleContainsValue(((HPoint) this.hPointCtrl.lstMarker[this.ctrlFlag.HPointID].Item2)._nPlace, lstAnimInfo.nPositons, 0))
        return false;
    }
    if (lstAnimInfo.isNeedItem && !this.hSceneManager.CheckHadItem((int) lstAnimInfo.ActionCtrl.Item1, lstAnimInfo.id) || lstAnimInfo.nDownPtn == 0 && this.ctrlFlag.isFaintness || lstAnimInfo.nFaintnessLimit == 1 && !this.ctrlFlag.isFaintness)
      return false;
    switch (this.ctrlFlag.initiative)
    {
      case 0:
        if (lstAnimInfo.nInitiativeFemale != 0)
          return false;
        break;
      case 1:
        if (lstAnimInfo.nInitiativeFemale == 0)
          return false;
        break;
      case 2:
        if (lstAnimInfo.nInitiativeFemale != 2)
          return false;
        break;
    }
    return true;
  }

  private bool CheckAutoMotionLimit(HScene.AnimationListInfo lstAnimInfo)
  {
    if (!this.hSceneManager.bMerchant && (this.hSceneManager.EventKind == HSceneManager.HEvent.Yobai || this.hSceneManager.isForce) || (this.ctrlFlag.isFaintness || !this.hSceneManager.HSkil.ContainsValue(13)))
      return false;
    if (this.PlayerSex == 0 || this.PlayerSex == 1 && this.ctrlFlag.bFutanari)
    {
      if (Object.op_Equality((Object) this.hSceneManager.females[1], (Object) null))
      {
        if (lstAnimInfo.nPromiscuity == 1)
          return false;
      }
      else if (lstAnimInfo.nPromiscuity != 1)
        return false;
      if (lstAnimInfo.nPromiscuity == 2 || lstAnimInfo.ActionCtrl.Item1 == 3 && !this.NonTokushuCheckIDs.Contains(lstAnimInfo.id) && lstAnimInfo.fileMale == string.Empty)
        return false;
    }
    else if (this.PlayerSex == 1 && lstAnimInfo.nPromiscuity < 2)
      return false;
    if (!this.hSceneManager.bMerchant && (lstAnimInfo.nHentai == 1 && !this.hSceneManager.isHAddTaii[0] || lstAnimInfo.nHentai == 2 && !this.hSceneManager.isHAddTaii[1]) || this.hSceneManager.bMerchant && !lstAnimInfo.bMerchantMotion)
      return false;
    if (!this.usePoint)
    {
      int num = (int) this.hSceneManager.hitHmesh.Item3;
      if (num != -1 && !lstAnimInfo.nPositons.Contains(num) || num == -1 && !lstAnimInfo.nPositons.Contains(0) && !lstAnimInfo.nPositons.Contains(1))
        return false;
    }
    else if (this.ctrlFlag.HPointID == 0)
    {
      if (!lstAnimInfo.nPositons.Contains((int) this.hPointCtrl.InitNull.Item3))
        return false;
    }
    else
    {
      foreach (HPoint.NotMotionInfo notMotionInfo in ((HPoint) this.hPointCtrl.lstMarker[this.ctrlFlag.HPointID].Item2).notMotion)
      {
        if (notMotionInfo.motionID.Contains(lstAnimInfo.id))
          return false;
      }
      if (!HPointCtrl.DicTupleContainsValue(((HPoint) this.hPointCtrl.lstMarker[this.ctrlFlag.HPointID].Item2)._nPlace, lstAnimInfo.nPositons, 0))
        return false;
    }
    if (lstAnimInfo.isNeedItem && !this.hSceneManager.CheckHadItem((int) lstAnimInfo.ActionCtrl.Item1, lstAnimInfo.id) || lstAnimInfo.nFaintnessLimit == 1)
      return false;
    switch (this.ctrlFlag.initiative)
    {
      case 0:
        if (lstAnimInfo.nInitiativeFemale == 0)
          return false;
        break;
      case 1:
        if (lstAnimInfo.nInitiativeFemale == 0)
          return false;
        break;
      case 2:
        if (lstAnimInfo.nInitiativeFemale != 2)
          return false;
        break;
    }
    return true;
  }

  private bool StartListContain(
    List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>> _startList,
    HSceneManager.HEvent target)
  {
    using (List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>>.Enumerator enumerator = _startList.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        if ((HSceneManager.HEvent) enumerator.Current.Item1 == target)
          return true;
      }
    }
    return false;
  }

  private bool StartListContain(
    List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>> _startList,
    int target,
    HSceneManager.HEvent hEvent = HSceneManager.HEvent.Normal,
    int category = -1)
  {
    using (List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>>.Enumerator enumerator = _startList.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion> current = enumerator.Current;
        if ((HSceneManager.HEvent) current.Item1 == hEvent && current.Item2 == target && (category < 0 || ((HScene.StartMotion) current.Item3).mode == category))
          return true;
      }
    }
    return false;
  }

  private bool StartListContain(
    List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>> _startList,
    HScene.StartMotion target)
  {
    using (List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>>.Enumerator enumerator = _startList.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        if (enumerator.Current.Item3 == target)
          return true;
      }
    }
    return false;
  }

  public void ChangeStateAllEquip()
  {
    this.objAccessory.ChangeStateAllEquip(this.AllEquip);
    HSceneSprite hsceneSprite = this;
    hsceneSprite.AllEquip = ((hsceneSprite.AllEquip ? 1 : 0) ^ 1) != 0;
  }

  private void OnDisable()
  {
    this.UIGroup.set_blocksRaycasts(false);
    this.UIGroup.set_alpha(0.0f);
    this.HItemCtrl.EndProc();
    this.endFade = -1;
    this.usePoint = false;
    this.objGaugeLockF.set_isOn(false);
    this.objGaugeLockM.set_isOn(false);
    this.categoryFinish.SetActive(true, -1);
    ((UnityEngine.Component) this.categoryFinish).get_gameObject().SetActive(true);
    this.objAccessory.EndProc();
    this.charaChoice.EndProc();
    this.categoryMain.EndProc();
    this.hPointCtrl.EndProc();
    ((UnityEngine.Component) this.objCloth).get_gameObject().SetActive(false);
    ((UnityEngine.Component) this.objAccessory).get_gameObject().SetActive(false);
    ((UnityEngine.Component) this.objClothCard).get_gameObject().SetActive(false);
    ((UnityEngine.Component) this.categoryMain).get_gameObject().SetActive(false);
    this.objHItem.SetActive(false);
    this.objLightCategory.SetActive(false);
    this.objMotionListPanel.SetActive(false);
    this.charaChoice.CloseChoice();
    ((UnityEngine.Component) this.charaChoice).get_gameObject().SetActive(false);
    this.HelpBase.SetActive(false);
    this.ReSetHelpText();
    for (int index = 0; index < this.categories.MainCategoryActive.Length; ++index)
      this.categories.MainCategoryActive[index] = false;
    this.isLeaveItToYou = false;
    this.SetMotionListDraw(false, -1);
    this.SetEnableCategoryMain(false);
    this.SetEnableHItem(false);
    this.CategoryScroll.ListClear();
    this.MotionScroll.GetList().Clear();
    this.MotionScroll.ClearBlank();
    this.objClothCard.EndProc();
    ((Selectable) this.categoryMainButton).set_interactable(true);
    ((Selectable) this.hPointButton).set_interactable(true);
    List<GameObject> list = this.hSceneMotionPool.GetList();
    using (List<GameObject>.Enumerator enumerator = list.GetEnumerator())
    {
      while (enumerator.MoveNext())
        Object.Destroy((Object) enumerator.Current);
    }
    list.Clear();
    this.chaFemales = (ChaControl[]) null;
    if (this.beforeChoice != null)
      this.beforeChoice.Dispose();
    this.beforeChoice = (IDisposable) null;
    if (!((Behaviour) this).get_enabled())
      return;
    ((Behaviour) this).set_enabled(false);
  }

  private void SetHelpActive(bool _active)
  {
    if (this.HelpBase.get_activeSelf() == _active)
      return;
    this.HelpBase.SetActive(_active);
  }

  private void SetHelpText(string _text)
  {
    if (Object.op_Equality((Object) this.HelpTxt, (Object) null))
      return;
    this.HelpTxt.set_text(_text);
  }

  private void ReSetHelpText()
  {
    if (Object.op_Equality((Object) this.HelpTxt, (Object) null))
      return;
    this.HelpTxt.set_text("エッチを開始する");
  }

  public void GuidProc(AnimatorStateInfo ai)
  {
    if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 3 && this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item2 == 5)
    {
      this.SetHelpActive(false);
    }
    else
    {
      int num = -1;
      for (int index1 = 0; index1 < this.GuidTiming.Length; ++index1)
      {
        for (int index2 = 0; index2 < this.GuidTiming[index1].Length; ++index2)
        {
          if (((AnimatorStateInfo) ref ai).IsName(this.GuidTiming[index1][index2]))
          {
            num = index1;
            break;
          }
        }
        if (num >= 0)
          break;
      }
      if (num < 0)
        this.SetHelpActive(false);
      else if (num != 1 && this.hScene.ctrlVoice.nowVoices[0].state == HVoiceCtrl.VoiceKind.voice || (this.hScene.ctrlVoice.nowVoices[0].state == HVoiceCtrl.VoiceKind.startVoice || this.hScene.ctrlVoice.nowVoices[1].state == HVoiceCtrl.VoiceKind.voice) || (this.hScene.ctrlVoice.nowVoices[1].state == HVoiceCtrl.VoiceKind.startVoice || this.ctrlFlag.voice.playStart > 4))
        this.SetHelpActive(false);
      else if (this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 2 && this.hScene.GetProcBase() is Sonyu procBase && procBase.nowInsert)
        this.SetHelpActive(false);
      else if (this.hScene.NowChangeAnim)
        this.SetHelpActive(false);
      else if (num != 1 && this.ctrlFlag.initiative != 0)
      {
        this.SetHelpActive(false);
      }
      else
      {
        switch (num)
        {
          case 0:
            this.ReSetHelpText();
            this.SetHelpActive(true);
            break;
          case 1:
            this.SetHelpText("上で速くなる\n下で遅くなる");
            this.SetHelpActive(true);
            break;
          case 2:
            this.SetHelpText("上で続ける\n下で抜く");
            this.SetHelpActive(true);
            break;
          case 3:
            this.SetHelpText("続ける");
            this.SetHelpActive(true);
            break;
          default:
            this.SetHelpActive(false);
            break;
        }
      }
    }
  }

  public void HelpBaseActive(bool _active)
  {
    if (Object.op_Equality((Object) this.HelpBaseConfig, (Object) null) || this.HelpBaseConfig.get_activeSelf() == _active)
      return;
    this.HelpBaseConfig.SetActive(_active);
  }

  public bool GetHelpActive()
  {
    return !Object.op_Equality((Object) this.HelpBaseConfig, (Object) null) && this.HelpBaseConfig.get_activeSelf();
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
  }

  public enum FadeKind
  {
    Out,
    In,
    OutIn,
  }

  public enum FadeKindProc
  {
    None,
    Out,
    OutEnd,
    In,
    InEnd,
    OutIn,
    OutInEnd,
  }
}

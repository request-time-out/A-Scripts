// Decompiled with JetBrains decompiler
// Type: Studio.MPCharCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class MPCharCtrl : MonoBehaviour
  {
    [SerializeField]
    private MPCharCtrl.RootButtonInfo[] rootButtonInfo;
    [SerializeField]
    private AnimeGroupList animeGroupList;
    [SerializeField]
    private AnimeControl animeControl;
    [SerializeField]
    private VoiceControl voiceControl;
    [SerializeField]
    private MPCharCtrl.StateInfo stateInfo;
    [SerializeField]
    private MPCharCtrl.FKInfo fkInfo;
    [SerializeField]
    private MPCharCtrl.IKInfo ikInfo;
    [SerializeField]
    private MPCharCtrl.LookAtInfo lookAtInfo;
    [SerializeField]
    private MPCharCtrl.NeckInfo neckInfo;
    [SerializeField]
    private MPCharCtrl.PoseInfo poseInfo;
    [SerializeField]
    private MPCharCtrl.EtcInfo etcInfo;
    [SerializeField]
    private MPCharCtrl.HandInfo handInfo;
    [SerializeField]
    private Button[] buttonKinematic;
    [SerializeField]
    private MPCharCtrl.CostumeInfo costumeInfo;
    [SerializeField]
    private MPCharCtrl.JointInfo jointInfo;
    private OCIChar m_OCIChar;
    private int kinematic;
    private int select;
    private SingleAssignmentDisposable disposableFK;
    private SingleAssignmentDisposable disposableIK;

    public MPCharCtrl()
    {
      base.\u002Ector();
    }

    public OCIChar ociChar
    {
      get
      {
        return this.m_OCIChar;
      }
      set
      {
        this.m_OCIChar = value;
        if (this.m_OCIChar == null)
          return;
        this.UpdateInfo();
      }
    }

    public bool active
    {
      get
      {
        return ((Component) this).get_gameObject().get_activeSelf();
      }
      set
      {
        if (((Component) this).get_gameObject().get_activeSelf() == value)
          return;
        ((Component) this).get_gameObject().SetActive(value);
        if (((Component) this).get_gameObject().get_activeSelf())
          return;
        this.OnClickRoot(-1);
      }
    }

    public void OnClickRoot(int _idx)
    {
      this.select = _idx;
      for (int index = 0; index < this.rootButtonInfo.Length; ++index)
        this.rootButtonInfo[index].active = index == _idx;
      this.animeControl.active = _idx == 2;
      this.voiceControl.active = _idx == 3;
      switch (_idx)
      {
        case 0:
          this.stateInfo.UpdateInfo(this.m_OCIChar);
          break;
        case 1:
          this.fkInfo.UpdateInfo(this.m_OCIChar);
          this.ikInfo.UpdateInfo(this.m_OCIChar);
          this.lookAtInfo.UpdateInfo(this.m_OCIChar);
          this.neckInfo.UpdateInfo(this.m_OCIChar);
          this.poseInfo.UpdateInfo(this.m_OCIChar);
          this.etcInfo.UpdateInfo(this.m_OCIChar);
          this.handInfo.UpdateInfo(this.m_OCIChar);
          break;
        case 2:
          this.animeGroupList.InitList((AnimeGroupList.SEX) this.m_OCIChar.oiCharInfo.sex);
          this.animeControl.objectCtrlInfo = (ObjectCtrlInfo) this.m_OCIChar;
          break;
        case 3:
          this.voiceControl.ociChar = this.m_OCIChar;
          break;
        case 4:
          this.costumeInfo.UpdateInfo(this.m_OCIChar);
          break;
        case 5:
          this.jointInfo.UpdateInfo(this.m_OCIChar);
          break;
      }
    }

    public void OnClickKinematic(int _idx)
    {
      if (this.kinematic == _idx)
        return;
      MPCharCtrl.CommonInfo[] commonInfoArray = new MPCharCtrl.CommonInfo[8]
      {
        (MPCharCtrl.CommonInfo) this.fkInfo,
        (MPCharCtrl.CommonInfo) this.ikInfo,
        (MPCharCtrl.CommonInfo) this.lookAtInfo,
        (MPCharCtrl.CommonInfo) this.neckInfo,
        null,
        (MPCharCtrl.CommonInfo) this.etcInfo,
        (MPCharCtrl.CommonInfo) this.handInfo,
        (MPCharCtrl.CommonInfo) this.poseInfo
      };
      if (MathfEx.RangeEqualOn<int>(0, this.kinematic, commonInfoArray.Length - 1) && commonInfoArray[this.kinematic] != null)
      {
        commonInfoArray[this.kinematic].active = false;
        ((Graphic) ((Selectable) this.buttonKinematic[this.kinematic]).get_image()).set_color(Color.get_white());
      }
      this.kinematic = _idx;
      if (commonInfoArray[this.kinematic] == null)
        return;
      commonInfoArray[this.kinematic].active = true;
      commonInfoArray[this.kinematic].UpdateInfo(this.m_OCIChar);
      ((Graphic) ((Selectable) this.buttonKinematic[this.kinematic]).get_image()).set_color(Color.get_green());
    }

    public void LoadAnime(AnimeGroupList.SEX _sex, int _group, int _category, int _no)
    {
      this.m_OCIChar.LoadAnime(_group, _category, _no, 0.0f);
      this.animeControl.UpdateInfo();
    }

    public bool Deselect(OCIChar _ociChar)
    {
      if (this.m_OCIChar != _ociChar)
        return false;
      this.ociChar = (OCIChar) null;
      this.active = false;
      return true;
    }

    private void UpdateInfo()
    {
      this.OnClickRoot(this.select);
    }

    private void SetCopyBoneFK(OIBoneInfo.BoneGroup _group)
    {
      if (this.disposableFK != null)
      {
        this.disposableFK.Dispose();
        this.disposableFK = (SingleAssignmentDisposable) null;
      }
      this.disposableFK = new SingleAssignmentDisposable();
      this.disposableFK.set_Disposable(ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) ObservableTriggerExtensions.LateUpdateAsObservable((Component) this), 1), (Action<M0>) (_ => this.CopyBoneFK(_group)), (Action) (() =>
      {
        this.disposableFK.Dispose();
        this.disposableFK = (SingleAssignmentDisposable) null;
      })));
    }

    private void SetCopyBoneIK(OIBoneInfo.BoneGroup _group)
    {
      if (this.disposableIK != null)
      {
        this.disposableIK.Dispose();
        this.disposableIK = (SingleAssignmentDisposable) null;
      }
      this.disposableIK = new SingleAssignmentDisposable();
      this.disposableIK.set_Disposable(ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) ObservableTriggerExtensions.LateUpdateAsObservable((Component) this), 1), (Action<M0>) (_ => this.CopyBoneIK(_group)), (Action) (() =>
      {
        this.disposableIK.Dispose();
        this.disposableIK = (SingleAssignmentDisposable) null;
      })));
    }

    private void CopyBoneFK(OIBoneInfo.BoneGroup _group)
    {
      if (this.m_OCIChar == null)
        return;
      this.m_OCIChar.fkCtrl.CopyBone(_group);
    }

    private void CopyBoneIK(OIBoneInfo.BoneGroup _group)
    {
      if (this.m_OCIChar == null)
        return;
      this.m_OCIChar.ikCtrl.CopyBone(_group);
    }

    private void Awake()
    {
      this.fkInfo.Init();
      // ISSUE: method pointer
      ((UnityEvent) this.fkInfo.buttonAnime.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__0)));
      // ISSUE: method pointer
      ((UnityEvent) this.fkInfo.buttonAnimeSingle[0].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__1)));
      // ISSUE: method pointer
      ((UnityEvent) this.fkInfo.buttonAnimeSingle[1].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__2)));
      // ISSUE: method pointer
      ((UnityEvent) this.fkInfo.buttonAnimeSingle[2].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__3)));
      // ISSUE: method pointer
      ((UnityEvent) this.fkInfo.buttonAnimeSingle[3].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__4)));
      // ISSUE: method pointer
      ((UnityEvent) this.fkInfo.buttonReflectIK.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__5)));
      this.ikInfo.Init();
      // ISSUE: method pointer
      ((UnityEvent) this.ikInfo.buttonAnime.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__6)));
      // ISSUE: method pointer
      ((UnityEvent) this.ikInfo.buttonAnimeSingle[0].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__7)));
      // ISSUE: method pointer
      ((UnityEvent) this.ikInfo.buttonAnimeSingle[1].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__8)));
      // ISSUE: method pointer
      ((UnityEvent) this.ikInfo.buttonAnimeSingle[2].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__9)));
      // ISSUE: method pointer
      ((UnityEvent) this.ikInfo.buttonAnimeSingle[3].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__A)));
      // ISSUE: method pointer
      ((UnityEvent) this.ikInfo.buttonAnimeSingle[4].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__B)));
      // ISSUE: method pointer
      ((UnityEvent) this.ikInfo.buttonReflectFK.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Em__C)));
      this.stateInfo.Init();
      this.lookAtInfo.Init();
      this.neckInfo.Init();
      this.etcInfo.Init();
      this.handInfo.Init();
      this.costumeInfo.Init();
      this.jointInfo.Init();
      this.select = -1;
    }

    [Serializable]
    private class CommonInfo
    {
      public GameObject objRoot;

      public virtual bool active
      {
        set
        {
          if (this.objRoot.get_activeSelf() == value)
            return;
          this.objRoot.SetActive(value);
        }
      }

      public bool isUpdateInfo { get; set; }

      public OCIChar ociChar { get; set; }

      public virtual void Init()
      {
        this.isUpdateInfo = false;
      }

      public virtual void UpdateInfo(OCIChar _char)
      {
        this.ociChar = _char;
      }
    }

    [Serializable]
    private class RootButtonInfo
    {
      public Button button;
      public GameObject root;

      public bool active
      {
        get
        {
          return this.root.get_activeSelf();
        }
        set
        {
          if (this.root.get_activeSelf() == value)
            return;
          this.root.SetActive(value);
          ((Graphic) ((Selectable) this.button).get_image()).set_color(!this.root.get_activeSelf() ? Color.get_white() : Color.get_green());
        }
      }
    }

    [Serializable]
    public class StateCommonInfo
    {
      private bool m_Open = true;
      public Button buttonOpen;
      public GameObject objOpen;

      public Sprite[] spriteVisible { get; set; }

      public bool isOpen
      {
        get
        {
          return this.m_Open;
        }
        set
        {
          if (!Utility.SetStruct<bool>(ref this.m_Open, value))
            return;
          this.Change();
        }
      }

      public bool active
      {
        set
        {
          GameObject gameObject = ((Component) ((Component) this.buttonOpen).get_transform().get_parent()).get_gameObject();
          if (gameObject.get_activeSelf() == value)
            return;
          gameObject.SetActive(value);
          bool flag = value & this.m_Open;
          if (this.objOpen.get_activeSelf() == flag)
            return;
          this.objOpen.SetActive(flag);
        }
      }

      public virtual void Init(Sprite[] _spriteVisible)
      {
        this.spriteVisible = _spriteVisible;
        // ISSUE: method pointer
        ((UnityEvent) this.buttonOpen.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClick)));
        this.m_Open = true;
      }

      public virtual void UpdateInfo(OCIChar _char)
      {
      }

      private void OnClick()
      {
        this.isOpen = !this.isOpen;
      }

      private void Change()
      {
        if (this.objOpen.get_activeSelf() != this.m_Open)
          this.objOpen.SetActive(this.m_Open);
        ((Selectable) this.buttonOpen).get_image().set_sprite(this.spriteVisible[!this.m_Open ? 0 : 1]);
      }
    }

    [Serializable]
    public class StateButtonInfo
    {
      public GameObject root;
      public Button[] buttons;

      public bool interactable
      {
        set
        {
          for (int index = 0; index < this.buttons.Length; ++index)
            ((Selectable) this.buttons[index]).set_interactable(value);
        }
      }

      public int select
      {
        set
        {
          int num = Mathf.Clamp(value, 0, this.buttons.Length - 1);
          for (int index = 0; index < this.buttons.Length; ++index)
            ((Graphic) ((Selectable) this.buttons[index]).get_image()).set_color(!((Selectable) this.buttons[index]).get_interactable() || index != num ? Color.get_white() : Color.get_green());
        }
      }

      public bool active
      {
        set
        {
          if (!Object.op_Implicit((Object) this.root) || this.root.get_activeSelf() == value)
            return;
          this.root.SetActive(value);
        }
      }

      public void Interactable(int _state, bool _flag)
      {
        ((Selectable) this.buttons[_state]).set_interactable(_flag);
      }

      public void Interactable(params int[] _state)
      {
        if (((IList<int>) _state).IsNullOrEmpty<int>())
        {
          this.interactable = false;
        }
        else
        {
          for (int index = 0; index < this.buttons.Length; ++index)
            ((Selectable) this.buttons[index]).set_interactable(((IEnumerable<int>) _state).Contains<int>(index));
        }
      }
    }

    [Serializable]
    public class StateSliderInfo
    {
      public GameObject root;
      public Slider slider;

      public bool active
      {
        set
        {
          if (!Object.op_Implicit((Object) this.root) || this.root.get_activeSelf() == value)
            return;
          this.root.SetActive(value);
        }
      }
    }

    [Serializable]
    public class StateToggleInfo
    {
      public GameObject root;
      public Toggle toggle;

      public bool active
      {
        set
        {
          if (!Object.op_Implicit((Object) this.root) || this.root.get_activeSelf() == value)
            return;
          this.root.SetActive(value);
        }
      }
    }

    [Serializable]
    public class ClothingDetailsInfo : MPCharCtrl.StateCommonInfo
    {
      public MPCharCtrl.StateButtonInfo top = new MPCharCtrl.StateButtonInfo();
      public MPCharCtrl.StateButtonInfo buttom = new MPCharCtrl.StateButtonInfo();
      public MPCharCtrl.StateButtonInfo bra = new MPCharCtrl.StateButtonInfo();
      public MPCharCtrl.StateButtonInfo shorts = new MPCharCtrl.StateButtonInfo();
      public MPCharCtrl.StateButtonInfo pantyhose = new MPCharCtrl.StateButtonInfo();
      public MPCharCtrl.StateButtonInfo gloves = new MPCharCtrl.StateButtonInfo();
      public MPCharCtrl.StateButtonInfo socks = new MPCharCtrl.StateButtonInfo();
      public MPCharCtrl.StateButtonInfo cloth = new MPCharCtrl.StateButtonInfo();
      public MPCharCtrl.StateButtonInfo shoes = new MPCharCtrl.StateButtonInfo();
      private OCIChar ociChar;

      private MPCharCtrl.StateButtonInfo[] infoArray
      {
        get
        {
          return new MPCharCtrl.StateButtonInfo[8]
          {
            this.top,
            this.buttom,
            this.bra,
            this.shorts,
            this.gloves,
            this.pantyhose,
            this.socks,
            this.shoes
          };
        }
      }

      public void Init(Sprite[] _spriteVisible, MPCharCtrl.ClothingDetailsInfo.OnClickFunc _func)
      {
        this.Init(_spriteVisible);
        MPCharCtrl.StateButtonInfo[] infoArray = this.infoArray;
        for (int index = 0; index < infoArray.Length; ++index)
        {
          int _id = index;
          this.SetFunc(infoArray[index], _func, _id);
        }
      }

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        this.ociChar = _char;
        MPCharCtrl.StateButtonInfo[] infoArray = this.infoArray;
        for (int clothesKind = 0; clothesKind < infoArray.Length; ++clothesKind)
        {
          infoArray[clothesKind].active = true;
          if (clothesKind == 3)
          {
            Dictionary<byte, string> clothesStateKind = _char.charInfo.GetClothesStateKind(clothesKind);
            infoArray[clothesKind].Interactable(clothesStateKind == null ? (int[]) null : clothesStateKind.Keys.Select<byte, int>((System.Func<byte, int>) (v => (int) v)).ToArray<int>());
          }
          else
            infoArray[clothesKind].interactable = _char.charInfo.IsClothesStateKind(clothesKind);
          infoArray[clothesKind].select = (int) _char.charFileStatus.clothesState[clothesKind];
        }
      }

      private void SetFunc(
        MPCharCtrl.StateButtonInfo _info,
        MPCharCtrl.ClothingDetailsInfo.OnClickFunc _func,
        int _id)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MPCharCtrl.ClothingDetailsInfo.\u003CSetFunc\u003Ec__AnonStorey0 funcCAnonStorey0 = new MPCharCtrl.ClothingDetailsInfo.\u003CSetFunc\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        funcCAnonStorey0._func = _func;
        // ISSUE: reference to a compiler-generated field
        funcCAnonStorey0._id = _id;
        for (int index = 0; index < _info.buttons.Length; ++index)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: method pointer
          ((UnityEvent) _info.buttons[index].get_onClick()).AddListener(new UnityAction((object) new MPCharCtrl.ClothingDetailsInfo.\u003CSetFunc\u003Ec__AnonStorey1()
          {
            \u003C\u003Ef__ref\u00240 = funcCAnonStorey0,
            state = (byte) index
          }, __methodptr(\u003C\u003Em__0)));
          // ISSUE: method pointer
          ((UnityEvent) _info.buttons[index].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(UpdateState)));
        }
      }

      private void UpdateState()
      {
        if (this.ociChar == null)
          return;
        MPCharCtrl.StateButtonInfo[] infoArray = this.infoArray;
        for (int index = 0; index < infoArray.Length; ++index)
          infoArray[index].select = (int) this.ociChar.charFileStatus.clothesState[index];
      }

      public delegate void OnClickFunc(int _id, byte _state);
    }

    [Serializable]
    public class AccessoriesInfo : MPCharCtrl.StateCommonInfo
    {
      public MPCharCtrl.StateButtonInfo[] slots = new MPCharCtrl.StateButtonInfo[20];

      public void Init(Sprite[] _spriteVisible, MPCharCtrl.AccessoriesInfo.OnClickFunc _func)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MPCharCtrl.AccessoriesInfo.\u003CInit\u003Ec__AnonStorey0 initCAnonStorey0 = new MPCharCtrl.AccessoriesInfo.\u003CInit\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        initCAnonStorey0._func = _func;
        // ISSUE: reference to a compiler-generated field
        initCAnonStorey0.\u0024this = this;
        this.Init(_spriteVisible);
        for (int index1 = 0; index1 < this.slots.Length; ++index1)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          MPCharCtrl.AccessoriesInfo.\u003CInit\u003Ec__AnonStorey1 initCAnonStorey1 = new MPCharCtrl.AccessoriesInfo.\u003CInit\u003Ec__AnonStorey1();
          // ISSUE: reference to a compiler-generated field
          initCAnonStorey1.id = index1;
          for (int index2 = 0; index2 < 2; ++index2)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            MPCharCtrl.AccessoriesInfo.\u003CInit\u003Ec__AnonStorey2 initCAnonStorey2 = new MPCharCtrl.AccessoriesInfo.\u003CInit\u003Ec__AnonStorey2();
            // ISSUE: reference to a compiler-generated field
            initCAnonStorey2.\u003C\u003Ef__ref\u00240 = initCAnonStorey0;
            // ISSUE: reference to a compiler-generated field
            initCAnonStorey2.\u003C\u003Ef__ref\u00241 = initCAnonStorey1;
            // ISSUE: reference to a compiler-generated field
            initCAnonStorey2.flag = index2 == 0;
            // ISSUE: method pointer
            ((UnityEvent) this.slots[index1].buttons[index2].get_onClick()).AddListener(new UnityAction((object) initCAnonStorey2, __methodptr(\u003C\u003Em__0)));
            // ISSUE: reference to a compiler-generated field
            initCAnonStorey2.state = index2;
            // ISSUE: method pointer
            ((UnityEvent) this.slots[index1].buttons[index2].get_onClick()).AddListener(new UnityAction((object) initCAnonStorey2, __methodptr(\u003C\u003Em__1)));
          }
        }
      }

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        for (int index = 0; index < this.slots.Length; ++index)
          this.slots[index].interactable = Object.op_Inequality((Object) _char.charInfo.objAccessory[index], (Object) null);
        for (int index = 0; index < this.slots.Length; ++index)
          this.slots[index].select = !_char.charFileStatus.showAccessory[index] ? 1 : 0;
      }

      public delegate void OnClickFunc(int _id, bool _flag);
    }

    [Serializable]
    public class LiquidInfo : MPCharCtrl.StateCommonInfo
    {
      public MPCharCtrl.StateButtonInfo face = new MPCharCtrl.StateButtonInfo();
      public MPCharCtrl.StateButtonInfo breast = new MPCharCtrl.StateButtonInfo();
      public MPCharCtrl.StateButtonInfo back = new MPCharCtrl.StateButtonInfo();
      public MPCharCtrl.StateButtonInfo belly = new MPCharCtrl.StateButtonInfo();
      public MPCharCtrl.StateButtonInfo hip = new MPCharCtrl.StateButtonInfo();

      public void Init(Sprite[] _spriteVisible, MPCharCtrl.LiquidInfo.OnClickFunc _func)
      {
        this.Init(_spriteVisible);
        this.SetFunc(this.face, _func, ChaFileDefine.SiruParts.SiruKao);
        this.SetFunc(this.breast, _func, ChaFileDefine.SiruParts.SiruFrontTop);
        this.SetFunc(this.back, _func, ChaFileDefine.SiruParts.SiruBackTop);
        this.SetFunc(this.belly, _func, ChaFileDefine.SiruParts.SiruFrontBot);
        this.SetFunc(this.hip, _func, ChaFileDefine.SiruParts.SiruBackBot);
      }

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        if (_char.oiCharInfo.sex == 1)
        {
          this.active = true;
          this.face.select = (int) _char.GetSiruFlags(ChaFileDefine.SiruParts.SiruKao);
          this.breast.select = (int) _char.GetSiruFlags(ChaFileDefine.SiruParts.SiruFrontTop);
          this.back.select = (int) _char.GetSiruFlags(ChaFileDefine.SiruParts.SiruBackTop);
          this.belly.select = (int) _char.GetSiruFlags(ChaFileDefine.SiruParts.SiruFrontBot);
          this.hip.select = (int) _char.GetSiruFlags(ChaFileDefine.SiruParts.SiruBackBot);
        }
        else
          this.active = false;
      }

      private void SetFunc(
        MPCharCtrl.StateButtonInfo _info,
        MPCharCtrl.LiquidInfo.OnClickFunc _func,
        ChaFileDefine.SiruParts _parts)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MPCharCtrl.LiquidInfo.\u003CSetFunc\u003Ec__AnonStorey0 funcCAnonStorey0 = new MPCharCtrl.LiquidInfo.\u003CSetFunc\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        funcCAnonStorey0._func = _func;
        // ISSUE: reference to a compiler-generated field
        funcCAnonStorey0._parts = _parts;
        // ISSUE: reference to a compiler-generated field
        funcCAnonStorey0._info = _info;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < funcCAnonStorey0._info.buttons.Length; ++index)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          MPCharCtrl.LiquidInfo.\u003CSetFunc\u003Ec__AnonStorey1 funcCAnonStorey1 = new MPCharCtrl.LiquidInfo.\u003CSetFunc\u003Ec__AnonStorey1();
          // ISSUE: reference to a compiler-generated field
          funcCAnonStorey1.\u003C\u003Ef__ref\u00240 = funcCAnonStorey0;
          // ISSUE: reference to a compiler-generated field
          funcCAnonStorey1.state = (byte) index;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          ((UnityEvent) funcCAnonStorey0._info.buttons[index].get_onClick()).AddListener(new UnityAction((object) funcCAnonStorey1, __methodptr(\u003C\u003Em__0)));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          ((UnityEvent) funcCAnonStorey0._info.buttons[index].get_onClick()).AddListener(new UnityAction((object) funcCAnonStorey1, __methodptr(\u003C\u003Em__1)));
        }
      }

      public delegate void OnClickFunc(ChaFileDefine.SiruParts _parts, byte _state);
    }

    [Serializable]
    public class OtherInfo : MPCharCtrl.StateCommonInfo
    {
      public MPCharCtrl.StateSliderInfo tears = new MPCharCtrl.StateSliderInfo();
      public MPCharCtrl.StateSliderInfo cheek = new MPCharCtrl.StateSliderInfo();
      public MPCharCtrl.StateSliderInfo nipple = new MPCharCtrl.StateSliderInfo();
      public MPCharCtrl.StateSliderInfo skin = new MPCharCtrl.StateSliderInfo();
      public MPCharCtrl.StateSliderInfo wet = new MPCharCtrl.StateSliderInfo();
      public MPCharCtrl.StateToggleInfo single = new MPCharCtrl.StateToggleInfo();
      public MPCharCtrl.StateButtonInfo color = new MPCharCtrl.StateButtonInfo();
      public MPCharCtrl.StateToggleInfo son = new MPCharCtrl.StateToggleInfo();
      public MPCharCtrl.StateSliderInfo sonLen = new MPCharCtrl.StateSliderInfo();

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        bool flag = _char.oiCharInfo.sex == 1;
        this.nipple.active = flag;
        this.skin.active = true;
        this.wet.active = true;
        this.single.active = true;
        this.color.active = true;
        this.son.active = true;
        this.sonLen.active = true;
        this.tears.slider.set_value(_char.GetTears());
        this.cheek.slider.set_value(_char.GetHohoAkaRate());
        this.son.toggle.set_isOn(_char.oiCharInfo.visibleSon);
        this.sonLen.slider.set_value(_char.GetSonLength());
        if (flag)
          this.nipple.slider.set_value(_char.oiCharInfo.nipple);
        this.skin.slider.set_value(_char.oiCharInfo.SkinTuyaRate);
        this.wet.slider.set_value(_char.oiCharInfo.WetRate);
        this.single.toggle.set_isOn(_char.GetVisibleSimple());
        this.SetSimpleColor(_char.oiCharInfo.simpleColor);
      }

      public void SetSimpleColor(Color _color)
      {
        ((Graphic) ((Selectable) this.color.buttons[0]).get_image()).set_color(_color);
      }

      public delegate void OnClickTears(byte _state);
    }

    [Serializable]
    private class StateInfo : MPCharCtrl.CommonInfo
    {
      public MPCharCtrl.ClothingDetailsInfo clothingDetailsInfo = new MPCharCtrl.ClothingDetailsInfo();
      public MPCharCtrl.AccessoriesInfo accessoriesInfo = new MPCharCtrl.AccessoriesInfo();
      public MPCharCtrl.LiquidInfo liquidInfo = new MPCharCtrl.LiquidInfo();
      public MPCharCtrl.OtherInfo otherInfo = new MPCharCtrl.OtherInfo();
      public Sprite[] spriteVisible;
      public Button[] buttonCosState;

      public override void Init()
      {
        base.Init();
        // ISSUE: method pointer
        ((UnityEvent) this.buttonCosState[0].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__0)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonCosState[1].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__1)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonCosState[2].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__2)));
        this.clothingDetailsInfo.Init(this.spriteVisible, new MPCharCtrl.ClothingDetailsInfo.OnClickFunc(this.OnClickClothingDetails));
        this.accessoriesInfo.Init(this.spriteVisible, new MPCharCtrl.AccessoriesInfo.OnClickFunc(this.OnClickAccessories));
        this.liquidInfo.Init(this.spriteVisible, new MPCharCtrl.LiquidInfo.OnClickFunc(this.OnClickLiquid));
        this.otherInfo.Init(this.spriteVisible);
        // ISSUE: method pointer
        ((UnityEvent<float>) this.otherInfo.tears.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedTears)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.otherInfo.cheek.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedCheek)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.otherInfo.nipple.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedNipple)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.otherInfo.skin.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedSkin)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.otherInfo.wet.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedWet)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.otherInfo.single.toggle.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedSimple)));
        // ISSUE: method pointer
        ((UnityEvent) this.otherInfo.color.buttons[0].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickSimpleColor)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.otherInfo.son.toggle.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedSon)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.otherInfo.sonLen.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedSonLength)));
      }

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        this.isUpdateInfo = true;
        this.clothingDetailsInfo.UpdateInfo(_char);
        this.accessoriesInfo.UpdateInfo(_char);
        this.liquidInfo.UpdateInfo(_char);
        this.otherInfo.UpdateInfo(_char);
        this.isUpdateInfo = false;
      }

      private void OnClickCosState(int _value)
      {
        this.ociChar.SetClothesStateAll(_value);
        this.clothingDetailsInfo.UpdateInfo(this.ociChar);
      }

      private void OnClickClothingDetails(int _id, byte _state)
      {
        this.ociChar.SetClothesState(_id, _state);
      }

      private void OnClickAccessories(int _id, bool _flag)
      {
        this.ociChar.ShowAccessory(_id, _flag);
      }

      private void OnClickLiquid(ChaFileDefine.SiruParts _parts, byte _state)
      {
        this.ociChar.SetSiruFlags(_parts, _state);
      }

      private void OnValueChangedTears(float _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.SetTears(_value);
      }

      private void OnValueChangedCheek(float _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.SetHohoAkaRate(_value);
      }

      private void OnValueChangedNipple(float _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.SetNipStand(_value);
      }

      private void OnValueChangedSkin(float _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.SetTuyaRate(_value);
      }

      private void OnValueChangedWet(float _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.SetWetRate(_value);
      }

      private void OnValueChangedSimple(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.SetVisibleSimple(_value);
      }

      private void OnClickSimpleColor()
      {
        Singleton<Studio.Studio>.Instance.colorPalette.Setup("単色", this.ociChar.oiCharInfo.simpleColor, new Action<Color>(this.OnValueChangeSimpleColor), true);
        Singleton<Studio.Studio>.Instance.colorPalette.visible = true;
      }

      private void OnValueChangeSimpleColor(Color _color)
      {
        this.ociChar.SetSimpleColor(_color);
        this.otherInfo.SetSimpleColor(_color);
      }

      private void OnValueChangedSon(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.SetVisibleSon(_value);
      }

      private void OnValueChangedSonLength(float _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.SetSonLength(_value);
      }
    }

    [Serializable]
    private class FKInfo : MPCharCtrl.CommonInfo
    {
      public Toggle toggleFunction;
      public Toggle toggleHair;
      public Toggle toggleNeck;
      public Toggle toggleBreast;
      public Toggle toggleBody;
      public Toggle toggleRightHand;
      public Toggle toggleLeftHand;
      public Toggle toggleSkirt;
      public Slider sliderSize;
      public Button buttonAnime;
      public Button buttonReflectIK;
      public Button[] buttonAnimeSingle;
      public Button[] buttonInitSingle;
      [Space]
      public Toggle toggleVisible;
      private Toggle[] array;

      public override void Init()
      {
        base.Init();
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleFunction.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnChangeValueFunction)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleHair.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__0)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleNeck.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__1)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleBreast.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__2)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleBody.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__3)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleRightHand.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__4)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleLeftHand.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__5)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleSkirt.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__6)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.sliderSize.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedSize)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonInitSingle[0].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__7)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonInitSingle[1].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__8)));
        ((Component) this.toggleVisible).get_gameObject().SetActive(false);
        this.array = new Toggle[7]
        {
          this.toggleHair,
          this.toggleNeck,
          this.toggleBreast,
          this.toggleBody,
          this.toggleRightHand,
          this.toggleLeftHand,
          this.toggleSkirt
        };
      }

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        this.isUpdateInfo = true;
        this.toggleFunction.set_isOn(_char.oiCharInfo.enableFK);
        for (int index = 0; index < this.array.Length; ++index)
          this.array[index].set_isOn(_char.oiCharInfo.activeFK[index]);
        ((Selectable) this.buttonReflectIK).set_interactable(_char.oiCharInfo.enableFK);
        ((Selectable) this.toggleHair).set_interactable(_char.oiCharInfo.sex != 0 || _char.IsFKGroup(OIBoneInfo.BoneGroup.Hair));
        ((Selectable) this.toggleBreast).set_interactable(_char.oiCharInfo.sex != 0 || _char.IsFKGroup(OIBoneInfo.BoneGroup.Breast));
        ((Selectable) this.toggleSkirt).set_interactable(_char.oiCharInfo.sex != 0 || _char.IsFKGroup(OIBoneInfo.BoneGroup.Skirt));
        this.isUpdateInfo = false;
      }

      private void OnChangeValueFunction(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.ActiveKinematicMode(OICharInfo.KinematicMode.FK, _value, false);
        ((Selectable) this.buttonReflectIK).set_interactable(_value);
      }

      private void OnChangeValueIndividual(OIBoneInfo.BoneGroup _group, bool _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.ActiveFK(_group, _value, false);
      }

      private void OnValueChangedSize(float _value)
      {
        if (this.isUpdateInfo)
          return;
        int count = this.ociChar.listBones.Count;
        for (int index = 0; index < count; ++index)
          this.ociChar.listBones[index].scaleRate = _value;
      }

      private void OnClickInitSingle(OIBoneInfo.BoneGroup _group)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.InitFKBone(_group);
      }
    }

    [Serializable]
    private class IKInfo : MPCharCtrl.CommonInfo
    {
      public Toggle toggleFunction;
      public Toggle toggleAll;
      public Toggle toggleBody;
      public Toggle toggleRightHand;
      public Toggle toggleLeftHand;
      public Toggle toggleRightLeg;
      public Toggle toggleLeftLeg;
      public Slider sliderSize;
      public Button buttonAnime;
      public Button buttonReflectFK;
      public Button[] buttonAnimeSingle;
      [Space]
      public Toggle toggleVisible;
      private Toggle[] array;

      public override void Init()
      {
        base.Init();
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleFunction.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnChangeValueFunction)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleAll.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedAll)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleBody.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__0)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleRightLeg.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__1)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleLeftLeg.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__2)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleRightHand.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__3)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleLeftHand.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__4)));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.sliderSize.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedSize)));
        ((Component) this.toggleVisible).get_gameObject().SetActive(false);
        this.array = new Toggle[5]
        {
          this.toggleBody,
          this.toggleRightLeg,
          this.toggleLeftLeg,
          this.toggleRightHand,
          this.toggleLeftHand
        };
      }

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        this.isUpdateInfo = true;
        this.toggleFunction.set_isOn(this.ociChar.oiCharInfo.enableIK);
        bool flag = false;
        for (int index = 0; index < 5; ++index)
        {
          this.array[index].set_isOn(this.ociChar.oiCharInfo.activeIK[index]);
          flag |= this.ociChar.oiCharInfo.activeIK[index];
        }
        this.toggleAll.set_isOn(flag);
        ((Selectable) this.buttonReflectFK).set_interactable(this.ociChar.oiCharInfo.enableIK);
        ((Selectable) this.toggleAll).set_interactable(this.ociChar.oiCharInfo.enableIK);
        for (int index = 0; index < 5; ++index)
          ((Selectable) this.array[index]).set_interactable(this.ociChar.oiCharInfo.enableIK);
        this.isUpdateInfo = false;
      }

      private void OnChangeValueFunction(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.ActiveKinematicMode(OICharInfo.KinematicMode.IK, _value, false);
        ((Selectable) this.buttonReflectFK).set_interactable(_value);
        ((Selectable) this.toggleAll).set_interactable(_value);
        for (int index = 0; index < 5; ++index)
          ((Selectable) this.array[index]).set_interactable(_value);
      }

      private void OnValueChangedAll(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        for (int index = 0; index < 5; ++index)
          this.array[index].set_isOn(_value);
      }

      private void OnChangeValueIndividual(int _no, bool _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.ActiveIK((OIBoneInfo.BoneGroup) (1 << _no), _value, false);
        this.isUpdateInfo = true;
        bool flag = false;
        for (int index = 0; index < 5; ++index)
          flag |= this.ociChar.oiCharInfo.activeIK[index];
        this.toggleAll.set_isOn(flag);
        this.isUpdateInfo = false;
      }

      private void OnValueChangedSize(float _value)
      {
        if (this.isUpdateInfo)
          return;
        int count = this.ociChar.listIKTarget.Count;
        for (int index = 0; index < count; ++index)
          this.ociChar.listIKTarget[index].scaleRate = _value;
      }
    }

    [Serializable]
    private class LookAtInfo : MPCharCtrl.CommonInfo
    {
      public Button[] buttonMode;
      public Slider sliderSize;

      public override void Init()
      {
        base.Init();
        for (int index = 0; index < this.buttonMode.Length; ++index)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: method pointer
          ((UnityEvent) this.buttonMode[index].get_onClick()).AddListener(new UnityAction((object) new MPCharCtrl.LookAtInfo.\u003CInit\u003Ec__AnonStorey0()
          {
            \u0024this = this,
            no = index
          }, __methodptr(\u003C\u003Em__0)));
        }
        // ISSUE: method pointer
        ((UnityEvent<float>) this.sliderSize.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnVauleChangeSize)));
      }

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        this.isUpdateInfo = true;
        int eyesLookPtn = this.ociChar.charFileStatus.eyesLookPtn;
        for (int index = 0; index < this.buttonMode.Length; ++index)
          ((Graphic) ((Selectable) this.buttonMode[index]).get_image()).set_color(index != eyesLookPtn ? Color.get_white() : Color.get_green());
        this.sliderSize.set_value(_char.lookAtInfo.guideObject.scaleRate);
        ((Selectable) this.sliderSize).set_interactable(_char.charFileStatus.eyesLookPtn == 4);
        this.isUpdateInfo = false;
      }

      private void OnClick(int _no)
      {
        int eyesLookPtn = this.ociChar.charFileStatus.eyesLookPtn;
        this.ociChar.ChangeLookEyesPtn(_no, false);
        ((Selectable) this.sliderSize).set_interactable(_no == 4);
        ((Graphic) ((Selectable) this.buttonMode[eyesLookPtn]).get_image()).set_color(Color.get_white());
        ((Graphic) ((Selectable) this.buttonMode[_no]).get_image()).set_color(Color.get_green());
      }

      private void OnVauleChangeSize(float _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.lookAtInfo.guideObject.scaleRate = _value;
      }
    }

    [Serializable]
    private class NeckInfo : MPCharCtrl.CommonInfo
    {
      private int[] patterns = new int[4]{ 0, 1, 3, 4 };
      public Button[] buttonMode;

      public override void Init()
      {
        base.Init();
        for (int index = 0; index < this.buttonMode.Length; ++index)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: method pointer
          ((UnityEvent) this.buttonMode[index].get_onClick()).AddListener(new UnityAction((object) new MPCharCtrl.NeckInfo.\u003CInit\u003Ec__AnonStorey0()
          {
            \u0024this = this,
            no = index
          }, __methodptr(\u003C\u003Em__0)));
        }
      }

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        this.isUpdateInfo = true;
        int no = this.ociChar.charFileStatus.neckLookPtn;
        no = Array.FindIndex<int>(this.patterns, (Predicate<int>) (v => v == no));
        for (int index = 0; index < this.buttonMode.Length; ++index)
        {
          ((Graphic) ((Selectable) this.buttonMode[index]).get_image()).set_color(index != no ? Color.get_white() : Color.get_green());
          ((Selectable) this.buttonMode[index]).set_interactable((!_char.oiCharInfo.enableFK ? 0 : (_char.oiCharInfo.activeFK[1] ? 1 : 0)) == 0);
        }
        this.isUpdateInfo = false;
      }

      private void OnClick(int _idx)
      {
        int old = this.ociChar.charFileStatus.neckLookPtn;
        old = Array.FindIndex<int>(this.patterns, (Predicate<int>) (v => v == old));
        this.ociChar.ChangeLookNeckPtn(this.patterns[_idx]);
        ((Graphic) ((Selectable) this.buttonMode[old]).get_image()).set_color(Color.get_white());
        ((Graphic) ((Selectable) this.buttonMode[_idx]).get_image()).set_color(Color.get_green());
      }
    }

    [Serializable]
    public class PatternInfo
    {
      public Button[] buttons = new Button[2];
      private int m_Ptn = -1;
      public TextMeshProUGUI textPtn;
      public MPCharCtrl.PatternInfo.OnClickFunc onClickFunc;

      public int ptn
      {
        get
        {
          return this.m_Ptn;
        }
        set
        {
          if (!Utility.SetStruct<int>(ref this.m_Ptn, value))
            return;
          ((TMP_Text) this.textPtn).set_text(string.Format("{00:0}", (object) this.m_Ptn));
        }
      }

      public int num { get; set; }

      public void Init()
      {
        // ISSUE: method pointer
        ((UnityEvent) this.buttons[0].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__0)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttons[1].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__1)));
      }

      private void OnClick(int _add)
      {
        int num = this.m_Ptn + _add;
        this.ptn = num >= 0 ? num % this.num : this.num - 1;
        if (this.onClickFunc == null)
          return;
        this.onClickFunc(this.m_Ptn);
      }

      public delegate void OnClickFunc(int _no);
    }

    [Serializable]
    private class EtcInfo : MPCharCtrl.CommonInfo
    {
      public MPCharCtrl.PatternInfo piEyebrows = new MPCharCtrl.PatternInfo();
      public MPCharCtrl.PatternInfo piEyes = new MPCharCtrl.PatternInfo();
      public MPCharCtrl.PatternInfo piMouth = new MPCharCtrl.PatternInfo();
      public Slider sliderEyesOpen;
      public Toggle toggleBlink;
      public Slider sliderMouthOpen;
      public Toggle toggleLipSync;
      private int[] eyebrowsKeys;
      private int[] eyesKeys;

      public override void Init()
      {
        base.Init();
        this.piEyebrows.Init();
        this.piEyebrows.onClickFunc += new MPCharCtrl.PatternInfo.OnClickFunc(this.ChangeEyebrowsPtn);
        this.piEyes.Init();
        this.piEyes.onClickFunc += new MPCharCtrl.PatternInfo.OnClickFunc(this.ChangeEyesPtn);
        // ISSUE: method pointer
        ((UnityEvent<float>) this.sliderEyesOpen.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedEyesOpen)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleBlink.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedEyesBlink)));
        this.piMouth.Init();
        this.piMouth.onClickFunc += new MPCharCtrl.PatternInfo.OnClickFunc(this.ChangeMouthPtn);
        // ISSUE: method pointer
        ((UnityEvent<float>) this.sliderMouthOpen.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedMouthOpen)));
        // ISSUE: method pointer
        ((UnityEvent<bool>) this.toggleLipSync.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedLipSync)));
      }

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        this.isUpdateInfo = true;
        FBSCtrlEyebrow eyebrowCtrl = _char.charInfo.eyebrowCtrl;
        Dictionary<int, ListInfoBase> categoryInfo1 = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(_char.sex != 0 ? ChaListDefine.CategoryNo.custom_eyebrow_f : ChaListDefine.CategoryNo.custom_eyebrow_m);
        this.eyebrowsKeys = categoryInfo1.Keys.ToArray<int>();
        this.piEyebrows.num = categoryInfo1.Count;
        this.piEyebrows.ptn = Mathf.Clamp(Array.FindIndex<int>(this.eyebrowsKeys, (Predicate<int>) (_i => _i == _char.charInfo.GetEyebrowPtn())), 0, this.eyebrowsKeys.Length - 1);
        FBSCtrlEyes eyesCtrl = _char.charInfo.eyesCtrl;
        Dictionary<int, ListInfoBase> categoryInfo2 = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(_char.sex != 0 ? ChaListDefine.CategoryNo.custom_eye_f : ChaListDefine.CategoryNo.custom_eye_m);
        this.eyesKeys = categoryInfo2.Keys.ToArray<int>();
        this.piEyes.num = categoryInfo2.Count;
        this.piEyes.ptn = Mathf.Clamp(Array.FindIndex<int>(this.eyesKeys, (Predicate<int>) (_i => _i == _char.charInfo.GetEyesPtn())), 0, this.eyesKeys.Length - 1);
        this.sliderEyesOpen.set_value(_char.charFileStatus.eyesOpenMax);
        this.toggleBlink.set_isOn(_char.charFileStatus.eyesBlink);
        FBSCtrlMouth mouthCtrl = _char.charInfo.mouthCtrl;
        this.piMouth.num = mouthCtrl.GetMaxPtn();
        this.piMouth.ptn = _char.charInfo.GetMouthPtn();
        this.sliderMouthOpen.set_value(mouthCtrl.FixedRate);
        this.toggleLipSync.set_isOn(_char.oiCharInfo.lipSync);
        this.isUpdateInfo = false;
      }

      private void ChangeEyebrowsPtn(int _no)
      {
        this.ociChar.charInfo.ChangeEyebrowPtn(this.eyebrowsKeys[Mathf.Clamp(_no, 0, this.eyebrowsKeys.Length - 1)], true);
      }

      private void ChangeEyesPtn(int _no)
      {
        this.ociChar.charInfo.ChangeEyesPtn(this.eyesKeys[Mathf.Clamp(_no, 0, this.eyesKeys.Length - 1)], true);
      }

      private void OnValueChangedEyesOpen(float _value)
      {
        this.ociChar.ChangeEyesOpen(_value);
      }

      private void OnValueChangedEyesBlink(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.ChangeBlink(_value);
      }

      private void ChangeMouthPtn(int _no)
      {
        this.ociChar.charInfo.ChangeMouthPtn(_no, true);
      }

      private void OnValueChangedMouthOpen(float _value)
      {
        this.ociChar.ChangeMouthOpen(_value);
      }

      private void OnValueChangedLipSync(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        this.ociChar.ChangeLipSync(_value);
      }
    }

    [Serializable]
    private class HandInfo : MPCharCtrl.CommonInfo
    {
      public MPCharCtrl.PatternInfo piRightHand = new MPCharCtrl.PatternInfo();
      public MPCharCtrl.PatternInfo piLeftHand = new MPCharCtrl.PatternInfo();

      public override void Init()
      {
        base.Init();
        this.piRightHand.Init();
        this.piRightHand.onClickFunc += new MPCharCtrl.PatternInfo.OnClickFunc(this.ChangeRightHandAnime);
        this.piLeftHand.Init();
        this.piLeftHand.onClickFunc += new MPCharCtrl.PatternInfo.OnClickFunc(this.ChangeLeftHandAnime);
      }

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        this.isUpdateInfo = true;
        this.piRightHand.num = _char.HandAnimeNum + 1;
        this.piRightHand.ptn = _char.oiCharInfo.handPtn[1];
        this.piLeftHand.num = _char.HandAnimeNum + 1;
        this.piLeftHand.ptn = _char.oiCharInfo.handPtn[0];
        this.isUpdateInfo = false;
      }

      private void ChangeLeftHandAnime(int _no)
      {
        this.ociChar.ChangeHandAnime(0, _no);
      }

      private void ChangeRightHandAnime(int _no)
      {
        this.ociChar.ChangeHandAnime(1, _no);
      }
    }

    [Serializable]
    private class PoseInfo : MPCharCtrl.CommonInfo
    {
      public PauseRegistrationList pauseRegistrationList;

      public override bool active
      {
        set
        {
          this.pauseRegistrationList.active = value;
        }
      }

      public override void Init()
      {
        base.Init();
      }

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        this.isUpdateInfo = true;
        this.pauseRegistrationList.ociChar = _char;
        this.isUpdateInfo = false;
      }
    }

    [Serializable]
    private class CostumeInfo : MPCharCtrl.CommonInfo
    {
      public CharaFileSort fileSort = new CharaFileSort();
      private int sex = -1;
      public GameObject prefabNode;
      public RawImage imageThumbnail;
      public Button[] buttonSort;
      public Button buttonLoad;

      public override void Init()
      {
        base.Init();
        // ISSUE: method pointer
        ((UnityEvent) this.buttonSort[0].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__0)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonSort[1].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__1)));
        // ISSUE: method pointer
        ((UnityEvent) this.buttonLoad.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickLoad)));
        this.sex = -1;
      }

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        this.isUpdateInfo = true;
        this.InitList(_char.oiCharInfo.sex);
        this.isUpdateInfo = false;
      }

      private void InitList(int _sex)
      {
        if (this.sex == _sex)
          return;
        this.fileSort.DeleteAllNode();
        this.InitFileList(_sex);
        int count = this.fileSort.cfiList.Count;
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          MPCharCtrl.CostumeInfo.\u003CInitList\u003Ec__AnonStorey0 listCAnonStorey0 = new MPCharCtrl.CostumeInfo.\u003CInitList\u003Ec__AnonStorey0()
          {
            \u0024this = this,
            info = this.fileSort.cfiList[index]
          };
          // ISSUE: reference to a compiler-generated field
          listCAnonStorey0.info.index = index;
          GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.prefabNode);
          if (!gameObject.get_activeSelf())
            gameObject.SetActive(true);
          gameObject.get_transform().SetParent(this.fileSort.root, false);
          // ISSUE: reference to a compiler-generated field
          listCAnonStorey0.info.node = (ListNode) gameObject.GetComponent<ListNode>();
          // ISSUE: reference to a compiler-generated field
          listCAnonStorey0.info.button = (Button) gameObject.GetComponent<Button>();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          listCAnonStorey0.info.node.AddActionToButton(new UnityAction((object) listCAnonStorey0, __methodptr(\u003C\u003Em__0)));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          listCAnonStorey0.info.node.text = listCAnonStorey0.info.name;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          listCAnonStorey0.info.node.listEnterAction.Add(new UnityAction((object) listCAnonStorey0, __methodptr(\u003C\u003Em__1)));
        }
        this.sex = _sex;
        this.fileSort.Sort(0, false);
        ((Selectable) this.buttonLoad).set_interactable(false);
        ((Graphic) this.imageThumbnail).set_color(Color.get_clear());
      }

      private void InitFileList(int _sex)
      {
        List<string> files = new List<string>();
        Illusion.Utils.File.GetAllFiles(UserData.Path + (_sex != 0 ? "coordinate/female/" : "coordinate/male/"), "*.png", ref files);
        this.fileSort.cfiList.Clear();
        int count = files.Count;
        ChaFileCoordinate chaFileCoordinate = new ChaFileCoordinate();
        for (int index = 0; index < count; ++index)
        {
          if (chaFileCoordinate.LoadFile(files[index]))
            this.fileSort.cfiList.Add(new CharaFileInfo(files[index], chaFileCoordinate.coordinateName)
            {
              time = System.IO.File.GetLastWriteTime(files[index])
            });
        }
      }

      private void OnSelect(int _idx)
      {
        if (this.fileSort.select == _idx)
          return;
        this.fileSort.select = _idx;
        ((Selectable) this.buttonLoad).set_interactable(true);
      }

      private void LoadImage(int _idx)
      {
        this.imageThumbnail.set_texture((Texture) PngAssist.LoadTexture(this.fileSort.cfiList[_idx].file));
        ((Graphic) this.imageThumbnail).set_color(Color.get_white());
        Resources.UnloadUnusedAssets();
        GC.Collect();
      }

      private void OnClickLoad()
      {
        this.ociChar.LoadClothesFile(this.fileSort.selectPath);
      }

      private void OnClickSort(int _type)
      {
        this.fileSort.select = -1;
        ((Selectable) this.buttonLoad).set_interactable(false);
        this.fileSort.Sort(_type);
      }
    }

    [Serializable]
    private class JointInfo : MPCharCtrl.CommonInfo
    {
      public Toggle[] toggles;

      public override void Init()
      {
        base.Init();
        for (int index = 0; index < this.toggles.Length; ++index)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: method pointer
          ((UnityEvent<bool>) this.toggles[index].onValueChanged).AddListener(new UnityAction<bool>((object) new MPCharCtrl.JointInfo.\u003CInit\u003Ec__AnonStorey0()
          {
            \u0024this = this,
            idx = index
          }, __methodptr(\u003C\u003Em__0)));
        }
      }

      public override void UpdateInfo(OCIChar _char)
      {
        base.UpdateInfo(_char);
        this.isUpdateInfo = true;
        for (int index = 0; index < this.toggles.Length; ++index)
          this.toggles[index].set_isOn(this.ociChar.oiCharInfo.expression[index]);
        this.isUpdateInfo = false;
      }

      private void OnValueChanged(int _group, bool _value)
      {
        this.ociChar.EnableExpressionCategory(_group, _value);
      }
    }
  }
}

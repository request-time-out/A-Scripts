// Decompiled with JetBrains decompiler
// Type: Studio.AnimeControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio
{
  public class AnimeControl : MonoBehaviour
  {
    [SerializeField]
    private Slider sliderSpeed;
    [SerializeField]
    private EventTrigger eventSpeed;
    [SerializeField]
    private InputField inputSpeed;
    [SerializeField]
    private Slider sliderPattern;
    [SerializeField]
    private EventTrigger eventPattern;
    [SerializeField]
    private InputField inputPattern;
    [SerializeField]
    private Slider[] sliderOptionParam;
    [SerializeField]
    private EventTrigger[] eventOptionParam;
    [SerializeField]
    private InputField[] inputOptionParam;
    [SerializeField]
    private Toggle toggleOption;
    [SerializeField]
    private Toggle toggleLoop;
    [SerializeField]
    private Button[] buttons;
    private ObjectCtrlInfo m_ObjectCtrlInfo;
    private bool isUpdateInfo;
    private ObjectCtrlInfo[] arrayTarget;
    private int num;
    private float[] oldValue;
    private int sex;
    private OICharInfo.AnimeInfo animeInfo;

    public AnimeControl()
    {
      base.\u002Ector();
    }

    public ObjectCtrlInfo objectCtrlInfo
    {
      get
      {
        return this.m_ObjectCtrlInfo;
      }
      set
      {
        this.m_ObjectCtrlInfo = value;
        if (this.m_ObjectCtrlInfo == null)
          return;
        this.UpdateInfo();
      }
    }

    public bool active
    {
      set
      {
        if (((Component) this).get_gameObject().get_activeSelf() == value)
          return;
        ((Component) this).get_gameObject().SetActive(value);
      }
    }

    private void OnValueChangedSpeed(float _value)
    {
      if (this.isUpdateInfo)
        return;
      if (((IList<ObjectCtrlInfo>) this.arrayTarget).IsNullOrEmpty<ObjectCtrlInfo>())
        this.OnPointerDownSpeed((BaseEventData) null);
      for (int index = 0; index < this.num; ++index)
        this.arrayTarget[index].animeSpeed = _value;
      this.inputSpeed.set_text(_value.ToString("0.00"));
    }

    private void OnPointerDownSpeed(BaseEventData _data)
    {
      if (!((IList<ObjectCtrlInfo>) this.arrayTarget).IsNullOrEmpty<ObjectCtrlInfo>())
        return;
      this.arrayTarget = ((IEnumerable<ObjectCtrlInfo>) Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectObjectCtrl).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 0 || v.kind == 1)).ToArray<ObjectCtrlInfo>();
      this.num = this.arrayTarget.Length;
      this.oldValue = ((IEnumerable<ObjectCtrlInfo>) this.arrayTarget).Select<ObjectCtrlInfo, float>((Func<ObjectCtrlInfo, float>) (v => v.animeSpeed)).ToArray<float>();
    }

    private void OnPointerUpSpeed(BaseEventData _data)
    {
      if (this.arrayTarget.Length == 0)
        return;
      Singleton<UndoRedoManager>.Instance.Do((ICommand) new AnimeCommand.SpeedCommand(((IEnumerable<ObjectCtrlInfo>) this.arrayTarget).Select<ObjectCtrlInfo, int>((Func<ObjectCtrlInfo, int>) (v => v.objectInfo.dicKey)).ToArray<int>(), this.sliderSpeed.get_value(), this.oldValue));
      this.arrayTarget = (ObjectCtrlInfo[]) null;
      this.num = 0;
    }

    private void OnEndEditSpeed(string _text)
    {
      float _speed = Mathf.Clamp(Utility.StringToFloat(_text), 0.0f, 3f);
      this.arrayTarget = ((IEnumerable<ObjectCtrlInfo>) Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectObjectCtrl).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 0 || v.kind == 1)).ToArray<ObjectCtrlInfo>();
      this.num = this.arrayTarget.Length;
      this.oldValue = ((IEnumerable<ObjectCtrlInfo>) this.arrayTarget).Select<ObjectCtrlInfo, float>((Func<ObjectCtrlInfo, float>) (v => v.animeSpeed)).ToArray<float>();
      Singleton<UndoRedoManager>.Instance.Do((ICommand) new AnimeCommand.SpeedCommand(((IEnumerable<ObjectCtrlInfo>) this.arrayTarget).Select<ObjectCtrlInfo, int>((Func<ObjectCtrlInfo, int>) (v => v.objectInfo.dicKey)).ToArray<int>(), _speed, this.oldValue));
      this.isUpdateInfo = true;
      this.sliderSpeed.set_value(_speed);
      this.inputSpeed.set_text(_speed.ToString("0.00"));
      this.isUpdateInfo = false;
      this.arrayTarget = (ObjectCtrlInfo[]) null;
      this.num = 0;
    }

    private void OnValueChangedPattern(float _value)
    {
      if (this.isUpdateInfo)
        return;
      if (((IList<ObjectCtrlInfo>) this.arrayTarget).IsNullOrEmpty<ObjectCtrlInfo>())
        this.OnPointerDownPattern((BaseEventData) null);
      float num = Mathf.Lerp(0.0f, 1f, _value);
      for (int index = 0; index < this.num; ++index)
        (this.arrayTarget[index] as OCIChar).animePattern = num;
      this.inputPattern.set_text(num.ToString("0.00"));
    }

    private void OnPointerDownPattern(BaseEventData _data)
    {
      if (!((IList<ObjectCtrlInfo>) this.arrayTarget).IsNullOrEmpty<ObjectCtrlInfo>())
        return;
      this.arrayTarget = ((IEnumerable<ObjectCtrlInfo>) Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectObjectCtrl).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 0)).ToArray<ObjectCtrlInfo>();
      this.num = this.arrayTarget.Length;
      this.oldValue = ((IEnumerable<ObjectCtrlInfo>) this.arrayTarget).Select<ObjectCtrlInfo, float>((Func<ObjectCtrlInfo, float>) (v => (v as OCIChar).animePattern)).ToArray<float>();
    }

    private void OnPointerUpPattern(BaseEventData _data)
    {
      if (this.arrayTarget.Length == 0)
        return;
      Singleton<UndoRedoManager>.Instance.Do((ICommand) new AnimeCommand.PatternCommand(((IEnumerable<ObjectCtrlInfo>) this.arrayTarget).Select<ObjectCtrlInfo, int>((Func<ObjectCtrlInfo, int>) (v => v.objectInfo.dicKey)).ToArray<int>(), Mathf.Lerp(0.0f, 1f, this.sliderPattern.get_value()), this.oldValue));
    }

    private void OnEndEditPattern(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), 0.0f, 1f);
      this.arrayTarget = ((IEnumerable<ObjectCtrlInfo>) Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectObjectCtrl).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 0)).ToArray<ObjectCtrlInfo>();
      this.num = this.arrayTarget.Length;
      this.oldValue = ((IEnumerable<ObjectCtrlInfo>) this.arrayTarget).Select<ObjectCtrlInfo, float>((Func<ObjectCtrlInfo, float>) (v => (v as OCIChar).animePattern)).ToArray<float>();
      Singleton<UndoRedoManager>.Instance.Do((ICommand) new AnimeCommand.PatternCommand(((IEnumerable<ObjectCtrlInfo>) this.arrayTarget).Select<ObjectCtrlInfo, int>((Func<ObjectCtrlInfo, int>) (v => v.objectInfo.dicKey)).ToArray<int>(), num, this.oldValue));
      this.isUpdateInfo = true;
      this.sliderPattern.set_value(Mathf.InverseLerp(0.0f, 1f, num));
      this.inputPattern.set_text(num.ToString("0.00"));
      this.isUpdateInfo = false;
      this.arrayTarget = (ObjectCtrlInfo[]) null;
      this.num = 0;
    }

    private void ChangedOptionParam(float _value, int _kind)
    {
      if (this.isUpdateInfo)
        return;
      if (((IList<ObjectCtrlInfo>) this.arrayTarget).IsNullOrEmpty<ObjectCtrlInfo>())
        this.DownOptionParam((BaseEventData) null, _kind);
      float num = Mathf.Lerp(0.0f, 1f, _value);
      for (int index = 0; index < this.num; ++index)
      {
        OCIChar ociChar = this.arrayTarget[index] as OCIChar;
        if (_kind == 0)
          ociChar.animeOptionParam1 = num;
        else
          ociChar.animeOptionParam2 = num;
      }
      this.inputOptionParam[_kind].set_text(num.ToString("0.00"));
    }

    private void DownOptionParam(BaseEventData _data, int _kind)
    {
      if (!((IList<ObjectCtrlInfo>) this.arrayTarget).IsNullOrEmpty<ObjectCtrlInfo>())
        return;
      this.arrayTarget = ((IEnumerable<ObjectCtrlInfo>) Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectObjectCtrl).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 0)).ToArray<ObjectCtrlInfo>();
      this.num = this.arrayTarget.Length;
      this.oldValue = ((IEnumerable<ObjectCtrlInfo>) this.arrayTarget).Select<ObjectCtrlInfo, float>((Func<ObjectCtrlInfo, float>) (v => (v as OCIChar).animeOptionParam[_kind])).ToArray<float>();
    }

    private void UpOptionParam(BaseEventData _data, int _kind)
    {
      if (this.arrayTarget.Length == 0)
        return;
      Singleton<UndoRedoManager>.Instance.Do((ICommand) new AnimeCommand.OptionParamCommand(((IEnumerable<ObjectCtrlInfo>) this.arrayTarget).Select<ObjectCtrlInfo, int>((Func<ObjectCtrlInfo, int>) (v => v.objectInfo.dicKey)).ToArray<int>(), Mathf.Lerp(0.0f, 1f, this.sliderOptionParam[_kind].get_value()), this.oldValue, _kind));
    }

    private void EndEditOptionParam(string _text, int _kind)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), 0.0f, 1f);
      this.arrayTarget = ((IEnumerable<ObjectCtrlInfo>) Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectObjectCtrl).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 0)).ToArray<ObjectCtrlInfo>();
      this.num = this.arrayTarget.Length;
      this.oldValue = ((IEnumerable<ObjectCtrlInfo>) this.arrayTarget).Select<ObjectCtrlInfo, float>((Func<ObjectCtrlInfo, float>) (v => (v as OCIChar).animeOptionParam[_kind])).ToArray<float>();
      Singleton<UndoRedoManager>.Instance.Do((ICommand) new AnimeCommand.OptionParamCommand(((IEnumerable<ObjectCtrlInfo>) this.arrayTarget).Select<ObjectCtrlInfo, int>((Func<ObjectCtrlInfo, int>) (v => v.objectInfo.dicKey)).ToArray<int>(), num, this.oldValue, _kind));
      this.isUpdateInfo = true;
      this.sliderOptionParam[_kind].set_value(num);
      this.inputOptionParam[_kind].set_text(num.ToString("0.00"));
      this.isUpdateInfo = false;
      this.arrayTarget = (ObjectCtrlInfo[]) null;
      this.num = 0;
    }

    private void OnValueChangedOption(bool _value)
    {
      if (this.isUpdateInfo || !(this.m_ObjectCtrlInfo is OCIChar objectCtrlInfo))
        return;
      objectCtrlInfo.optionItemCtrl.visible = _value;
    }

    private void OnValueChangedLoop(bool _value)
    {
      if (this.isUpdateInfo || !(this.m_ObjectCtrlInfo is OCIChar objectCtrlInfo))
        return;
      objectCtrlInfo.charAnimeCtrl.isForceLoop = _value;
    }

    private void OnClickRestart()
    {
      foreach (OCIChar ociChar in ((IEnumerable<ObjectCtrlInfo>) Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectObjectCtrl).Select<ObjectCtrlInfo, OCIChar>((Func<ObjectCtrlInfo, OCIChar>) (v => v as OCIChar)).Where<OCIChar>((Func<OCIChar, bool>) (v => v != null)).ToArray<OCIChar>())
        ociChar.RestartAnime();
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectObjectCtrl).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)).Where<OCIItem>((Func<OCIItem, bool>) (v => v != null)).ToArray<OCIItem>())
        ociItem.RestartAnime();
    }

    private void OnClickAllRestart()
    {
      foreach (OCIChar ociChar in Singleton<Studio.Studio>.Instance.dicObjectCtrl.Where<KeyValuePair<int, ObjectCtrlInfo>>((Func<KeyValuePair<int, ObjectCtrlInfo>, bool>) (v => v.Value.kind == 0)).Select<KeyValuePair<int, ObjectCtrlInfo>, OCIChar>((Func<KeyValuePair<int, ObjectCtrlInfo>, OCIChar>) (v => v.Value as OCIChar)).ToArray<OCIChar>())
        ociChar.RestartAnime();
      foreach (OCIItem ociItem in Singleton<Studio.Studio>.Instance.dicObjectCtrl.Where<KeyValuePair<int, ObjectCtrlInfo>>((Func<KeyValuePair<int, ObjectCtrlInfo>, bool>) (v => v.Value.kind == 1)).Select<KeyValuePair<int, ObjectCtrlInfo>, OCIItem>((Func<KeyValuePair<int, ObjectCtrlInfo>, OCIItem>) (v => v.Value as OCIItem)).ToArray<OCIItem>())
        ociItem.RestartAnime();
    }

    private void OnClickCopy()
    {
      OCIChar[] array = ((IEnumerable<ObjectCtrlInfo>) Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectObjectCtrl).Select<ObjectCtrlInfo, OCIChar>((Func<ObjectCtrlInfo, OCIChar>) (v => v as OCIChar)).Where<OCIChar>((Func<OCIChar, bool>) (v => v != null)).ToArray<OCIChar>();
      if (array.Length == 0)
        return;
      this.animeInfo.Copy(array[0].oiCharInfo.animeInfo);
      this.sex = array[0].oiCharInfo.sex;
    }

    private void OnClickPaste()
    {
      if (this.sex == -1 || !this.animeInfo.exist)
        return;
      foreach (OCIChar ociChar in ((IEnumerable<ObjectCtrlInfo>) Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectObjectCtrl).Select<ObjectCtrlInfo, OCIChar>((Func<ObjectCtrlInfo, OCIChar>) (v => v as OCIChar)).Where<OCIChar>((Func<OCIChar, bool>) (v => v != null)).ToArray<OCIChar>())
        ociChar.LoadAnime(this.animeInfo.group, this.animeInfo.category, this.animeInfo.no, 0.0f);
    }

    private void Init()
    {
      // ISSUE: method pointer
      ((UnityEvent<float>) this.sliderSpeed.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedSpeed)));
      // ISSUE: method pointer
      this.AddEventTrigger(this.eventSpeed, (EventTriggerType) 2, new UnityAction<BaseEventData>((object) this, __methodptr(OnPointerDownSpeed)));
      // ISSUE: method pointer
      this.AddEventTrigger(this.eventSpeed, (EventTriggerType) 3, new UnityAction<BaseEventData>((object) this, __methodptr(OnPointerUpSpeed)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputSpeed.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditSpeed)));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.sliderPattern.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedPattern)));
      // ISSUE: method pointer
      this.AddEventTrigger(this.eventPattern, (EventTriggerType) 2, new UnityAction<BaseEventData>((object) this, __methodptr(OnPointerDownPattern)));
      // ISSUE: method pointer
      this.AddEventTrigger(this.eventPattern, (EventTriggerType) 3, new UnityAction<BaseEventData>((object) this, __methodptr(OnPointerUpPattern)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputPattern.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditPattern)));
      for (int index = 0; index < 2; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AnimeControl.\u003CInit\u003Ec__AnonStorey2 initCAnonStorey2 = new AnimeControl.\u003CInit\u003Ec__AnonStorey2();
        // ISSUE: reference to a compiler-generated field
        initCAnonStorey2.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        initCAnonStorey2.kind = index;
        // ISSUE: method pointer
        ((UnityEvent<float>) this.sliderOptionParam[index].get_onValueChanged()).AddListener(new UnityAction<float>((object) initCAnonStorey2, __methodptr(\u003C\u003Em__0)));
        // ISSUE: method pointer
        this.AddEventTrigger(this.eventOptionParam[index], (EventTriggerType) 2, new UnityAction<BaseEventData>((object) initCAnonStorey2, __methodptr(\u003C\u003Em__1)));
        // ISSUE: method pointer
        this.AddEventTrigger(this.eventOptionParam[index], (EventTriggerType) 3, new UnityAction<BaseEventData>((object) initCAnonStorey2, __methodptr(\u003C\u003Em__2)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.inputOptionParam[index].get_onEndEdit()).AddListener(new UnityAction<string>((object) initCAnonStorey2, __methodptr(\u003C\u003Em__3)));
      }
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.toggleOption.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedOption)));
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.toggleLoop.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedLoop)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttons[0].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickRestart)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttons[1].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickAllRestart)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttons[2].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickCopy)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttons[3].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickPaste)));
    }

    public void UpdateInfo()
    {
      this.isUpdateInfo = true;
      this.arrayTarget = (ObjectCtrlInfo[]) null;
      bool flag1 = this.m_ObjectCtrlInfo.kind == 0;
      OCIChar objectCtrlInfo = this.m_ObjectCtrlInfo as OCIChar;
      bool flag2 = flag1 && objectCtrlInfo.isHAnime;
      this.sliderSpeed.set_value(this.m_ObjectCtrlInfo.animeSpeed);
      this.inputSpeed.set_text(this.m_ObjectCtrlInfo.animeSpeed.ToString("0.00"));
      ((Selectable) this.sliderPattern).set_interactable(flag1);
      ((Selectable) this.inputPattern).set_interactable(flag1);
      this.sliderPattern.set_value(!flag1 ? 0.5f : Mathf.InverseLerp(0.0f, 1f, objectCtrlInfo.animePattern));
      this.inputPattern.set_text(!flag1 ? "-" : objectCtrlInfo.animePattern.ToString("0.00"));
      for (int index = 0; index < 2; ++index)
      {
        ((Selectable) this.sliderOptionParam[index]).set_interactable(flag2);
        ((Selectable) this.inputOptionParam[index]).set_interactable(flag2);
        this.sliderOptionParam[index].set_value(!flag2 ? 0.5f : objectCtrlInfo.animeOptionParam[index]);
        this.inputOptionParam[index].set_text(!flag2 ? "-" : objectCtrlInfo.animeOptionParam[index].ToString("0.00"));
      }
      ((Selectable) this.toggleOption).set_interactable(flag1);
      this.toggleOption.set_isOn(flag1 && objectCtrlInfo.optionItemCtrl.visible);
      ((Selectable) this.toggleLoop).set_interactable(flag1);
      this.toggleLoop.set_isOn(flag1 && objectCtrlInfo.charAnimeCtrl.isForceLoop);
      ((Selectable) this.buttons[2]).set_interactable(flag1);
      ((Selectable) this.buttons[3]).set_interactable(flag1);
      this.isUpdateInfo = false;
    }

    private void AddEventTrigger(
      EventTrigger _event,
      EventTriggerType _type,
      UnityAction<BaseEventData> _action)
    {
      EventTrigger.Entry entry = new EventTrigger.Entry();
      entry.eventID = (__Null) _type;
      ((UnityEvent<BaseEventData>) entry.callback).AddListener(_action);
      _event.get_triggers().Add(entry);
    }

    private void Awake()
    {
      this.arrayTarget = (ObjectCtrlInfo[]) null;
      this.num = 0;
      this.Init();
      ((Component) this).get_gameObject().SetActive(false);
    }
  }
}

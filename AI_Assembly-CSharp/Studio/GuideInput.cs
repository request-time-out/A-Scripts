// Decompiled with JetBrains decompiler
// Type: Studio.GuideInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio
{
  public class GuideInput : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
  {
    [SerializeField]
    protected TMP_InputField[] inputPos;
    [SerializeField]
    protected TMP_InputField[] inputRot;
    [SerializeField]
    protected TMP_InputField[] inputScale;
    [SerializeField]
    private Button[] buttonInit;
    [Space]
    [SerializeField]
    private Canvas canvasParent;
    private HashSet<GuideObject> hashSelectObject;
    private BoolReactiveProperty _outsideVisible;
    private BoolReactiveProperty _visible;
    public GuideInput.OnVisible onVisible;
    private int _selectIndex;

    public GuideInput()
    {
      base.\u002Ector();
    }

    public GuideObject guideObject
    {
      set
      {
        if (!this.SetGuideObject(value))
          return;
        this.UpdateUI();
      }
    }

    public GuideObject deselectObject
    {
      set
      {
        if (!this.DeselectGuideObject(value))
          return;
        this.UpdateUI();
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

    public bool visible
    {
      get
      {
        return ((ReactiveProperty<bool>) this._visible).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._visible).set_Value(value);
      }
    }

    public int selectIndex
    {
      get
      {
        return this._selectIndex;
      }
      set
      {
        this._selectIndex = this._selectIndex != value ? value : -1;
      }
    }

    private TMP_InputField[] arrayInput { get; set; }

    public void Stop()
    {
      this.hashSelectObject.Clear();
      this.visible = false;
    }

    public void UpdateUI()
    {
      if (this.hashSelectObject.Count != 0)
        this.SetInputText();
      else
        this.visible = false;
    }

    public void OnEndEditPos(int _target)
    {
      if (this.hashSelectObject.Count == 0)
        return;
      float num = this.InputToFloat(this.inputPos[_target]);
      List<GuideCommand.EqualsInfo> self = new List<GuideCommand.EqualsInfo>();
      foreach (GuideObject guideObject in this.hashSelectObject)
      {
        if (guideObject.enablePos)
        {
          Vector3 pos1 = guideObject.changeAmount.pos;
          if ((double) ((Vector3) ref pos1).get_Item(_target) != (double) num)
          {
            ((Vector3) ref pos1).set_Item(_target, num);
            Vector3 pos2 = guideObject.changeAmount.pos;
            guideObject.changeAmount.pos = pos1;
            self.Add(new GuideCommand.EqualsInfo()
            {
              dicKey = guideObject.dicKey,
              oldValue = pos2,
              newValue = pos1
            });
          }
        }
      }
      if (!self.IsNullOrEmpty<GuideCommand.EqualsInfo>())
        Singleton<UndoRedoManager>.Instance.Push((ICommand) new GuideCommand.MoveEqualsCommand(self.ToArray()));
      this.SetInputTextPos();
    }

    public void OnEndEditRot(int _target)
    {
      if (this.hashSelectObject.Count == 0)
        return;
      float num = this.InputToFloat(this.inputRot[_target]) % 360f;
      List<GuideCommand.EqualsInfo> self = new List<GuideCommand.EqualsInfo>();
      foreach (GuideObject guideObject in this.hashSelectObject)
      {
        if (guideObject.enableRot)
        {
          Vector3 rot1 = guideObject.changeAmount.rot;
          if ((double) ((Vector3) ref rot1).get_Item(_target) != (double) num)
          {
            ((Vector3) ref rot1).set_Item(_target, num);
            Vector3 rot2 = guideObject.changeAmount.rot;
            guideObject.changeAmount.rot = rot1;
            self.Add(new GuideCommand.EqualsInfo()
            {
              dicKey = guideObject.dicKey,
              oldValue = rot2,
              newValue = rot1
            });
          }
        }
      }
      if (!self.IsNullOrEmpty<GuideCommand.EqualsInfo>())
        Singleton<UndoRedoManager>.Instance.Push((ICommand) new GuideCommand.RotationEqualsCommand(self.ToArray()));
      this.SetInputTextRot();
    }

    public void OnEndEditScale(int _target)
    {
      if (this.hashSelectObject.Count == 0)
        return;
      float num = Mathf.Max(this.InputToFloat(this.inputScale[_target]), 0.01f);
      List<GuideCommand.EqualsInfo> self = new List<GuideCommand.EqualsInfo>();
      foreach (GuideObject guideObject in this.hashSelectObject)
      {
        if (guideObject.enableScale)
        {
          Vector3 scale1 = guideObject.changeAmount.scale;
          if ((double) ((Vector3) ref scale1).get_Item(_target) != (double) num)
          {
            ((Vector3) ref scale1).set_Item(_target, num);
            Vector3 scale2 = guideObject.changeAmount.scale;
            guideObject.changeAmount.scale = scale1;
            self.Add(new GuideCommand.EqualsInfo()
            {
              dicKey = guideObject.dicKey,
              oldValue = scale2,
              newValue = scale1
            });
          }
        }
      }
      if (!self.IsNullOrEmpty<GuideCommand.EqualsInfo>())
        Singleton<UndoRedoManager>.Instance.Push((ICommand) new GuideCommand.ScaleEqualsCommand(self.ToArray()));
      this.SetInputTextScale(Vector3.get_zero());
    }

    public void SetInputText()
    {
      this.visible = true;
      bool flag1 = this.hashSelectObject.Any<GuideObject>((Func<GuideObject, bool>) (v => !v.enablePos));
      bool flag2 = this.hashSelectObject.Any<GuideObject>((Func<GuideObject, bool>) (v => !v.enableRot));
      bool flag3 = this.hashSelectObject.Any<GuideObject>((Func<GuideObject, bool>) (v => !v.enableScale));
      this.SetInputTextPos();
      for (int index = 0; index < 3; ++index)
        ((Selectable) this.inputPos[index]).set_interactable(!flag1);
      this.SetInputTextRot();
      for (int index = 0; index < 3; ++index)
        ((Selectable) this.inputRot[index]).set_interactable(!flag2);
      this.SetInputTextScale(Vector3.get_zero());
      for (int index = 0; index < 3; ++index)
        ((Selectable) this.inputScale[index]).set_interactable(!flag3);
    }

    public void AddSelectMultiple(GuideObject _object)
    {
      if (this.hashSelectObject.Contains(_object))
        return;
      this.AddObject(_object);
      this.SetInputText();
    }

    private bool SetGuideObject(GuideObject _object)
    {
      bool flag = Input.GetKey((KeyCode) 306) || Input.GetKey((KeyCode) 305);
      bool key = Input.GetKey((KeyCode) 120);
      if (flag && !key)
      {
        if (this.hashSelectObject.Contains(_object))
          return false;
        this.AddObject(_object);
      }
      else
      {
        foreach (GuideObject guideObject in this.hashSelectObject)
        {
          ChangeAmount changeAmount = guideObject.changeAmount;
          changeAmount.onChangePos -= new Action(this.SetInputTextPos);
          changeAmount.onChangeRot -= new Action(this.SetInputTextRot);
          changeAmount.onChangeScale -= new Action<Vector3>(this.SetInputTextScale);
        }
        this.hashSelectObject.Clear();
        this.AddObject(_object);
      }
      return true;
    }

    private bool DeselectGuideObject(GuideObject _object)
    {
      if (Object.op_Equality((Object) _object, (Object) null) || !this.hashSelectObject.Contains(_object))
        return false;
      ChangeAmount changeAmount = _object.changeAmount;
      changeAmount.onChangePos -= new Action(this.SetInputTextPos);
      changeAmount.onChangeRot -= new Action(this.SetInputTextRot);
      changeAmount.onChangeScale -= new Action<Vector3>(this.SetInputTextScale);
      this.hashSelectObject.Remove(_object);
      return true;
    }

    private void AddObject(GuideObject _object)
    {
      if (Object.op_Equality((Object) _object, (Object) null))
        return;
      ChangeAmount changeAmount = _object.changeAmount;
      changeAmount.onChangePos += new Action(this.SetInputTextPos);
      changeAmount.onChangeRot += new Action(this.SetInputTextRot);
      changeAmount.onChangeScale += new Action<Vector3>(this.SetInputTextScale);
      this.hashSelectObject.Add(_object);
    }

    private void SetInputTextPos()
    {
      GuideObject guideObject = this.hashSelectObject.ElementAtOrDefault<GuideObject>(0);
      Vector3 baseValue = !Object.op_Inequality((Object) guideObject, (Object) null) ? Vector3.get_zero() : guideObject.changeAmount.pos;
      IEnumerable<Vector3> source = this.hashSelectObject.Select<GuideObject, Vector3>((Func<GuideObject, Vector3>) (v => v.changeAmount.pos));
      for (int i = 0; i < 3; ++i)
        this.inputPos[i].set_text(!source.All<Vector3>((Func<Vector3, bool>) (_v => Mathf.Approximately(((Vector3) ref _v).get_Item(i), ((Vector3) ref baseValue).get_Item(i)))) ? "-" : ((Vector3) ref baseValue).get_Item(i).ToString("0.#####"));
    }

    private void SetInputTextRot()
    {
      GuideObject guideObject = this.hashSelectObject.ElementAtOrDefault<GuideObject>(0);
      Vector3 baseValue = !Object.op_Inequality((Object) guideObject, (Object) null) ? Vector3.get_zero() : guideObject.changeAmount.rot;
      IEnumerable<Vector3> source = this.hashSelectObject.Select<GuideObject, Vector3>((Func<GuideObject, Vector3>) (v => v.changeAmount.rot));
      for (int i = 0; i < 3; ++i)
        this.inputRot[i].set_text(!source.All<Vector3>((Func<Vector3, bool>) (_v => Mathf.Approximately(((Vector3) ref _v).get_Item(i), ((Vector3) ref baseValue).get_Item(i)))) ? "-" : ((Vector3) ref baseValue).get_Item(i).ToString("0.#####"));
    }

    private void SetInputTextScale(Vector3 _value)
    {
      GuideObject guideObject = this.hashSelectObject.ElementAtOrDefault<GuideObject>(0);
      Vector3 baseValue = !Object.op_Inequality((Object) guideObject, (Object) null) ? Vector3.get_zero() : guideObject.changeAmount.scale;
      IEnumerable<Vector3> source = this.hashSelectObject.Select<GuideObject, Vector3>((Func<GuideObject, Vector3>) (v => v.changeAmount.scale));
      for (int i = 0; i < 3; ++i)
        this.inputScale[i].set_text(!source.All<Vector3>((Func<Vector3, bool>) (_v => Mathf.Approximately(((Vector3) ref _v).get_Item(i), ((Vector3) ref baseValue).get_Item(i)))) ? "-" : ((Vector3) ref baseValue).get_Item(i).ToString("0.#####"));
    }

    private float InputToFloat(TMP_InputField _input)
    {
      float result = 0.0f;
      return float.TryParse(_input.get_text(), out result) ? result : 0.0f;
    }

    private bool Vector3Equals(ref Vector3 _a, ref Vector3 _b)
    {
      return _a.x == _b.x && _a.y == _b.y && _a.z == _b.z;
    }

    private void SetVisible()
    {
      bool flag = this.outsideVisible & this.visible;
      if (Object.op_Implicit((Object) this.canvasParent))
        ((Behaviour) this.canvasParent).set_enabled(flag);
      else
        ((Component) this).get_gameObject().SetActive(flag);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      SortCanvas.select = this.canvasParent;
    }

    private void ChangeTarget()
    {
      if (Input.GetKey((KeyCode) 304) | Input.GetKey((KeyCode) 303) | Input.GetKeyDown((KeyCode) 304) | Input.GetKeyDown((KeyCode) 303))
      {
        --this.selectIndex;
        if (this.selectIndex < 0)
          this.selectIndex = this.arrayInput.Length - 1;
      }
      else
        this.selectIndex = (this.selectIndex + 1) % this.arrayInput.Length;
      if (!Object.op_Implicit((Object) this.arrayInput[this.selectIndex]))
        return;
      ((Selectable) this.arrayInput[this.selectIndex]).Select();
    }

    private void OnClickInitPos()
    {
      if (this.hashSelectObject.Count == 0)
        return;
      GuideCommand.EqualsInfo[] array = this.hashSelectObject.Where<GuideObject>((Func<GuideObject, bool>) (v => v.enablePos)).Select<GuideObject, GuideCommand.EqualsInfo>((Func<GuideObject, GuideCommand.EqualsInfo>) (v => new GuideCommand.EqualsInfo()
      {
        dicKey = v.dicKey,
        oldValue = v.changeAmount.pos,
        newValue = Vector3.get_zero()
      })).ToArray<GuideCommand.EqualsInfo>();
      if (!((IList<GuideCommand.EqualsInfo>) array).IsNullOrEmpty<GuideCommand.EqualsInfo>())
        Singleton<UndoRedoManager>.Instance.Do((ICommand) new GuideCommand.MoveEqualsCommand(array));
      this.SetInputTextPos();
    }

    private void OnClickInitRot()
    {
      if (this.hashSelectObject.Count == 0)
        return;
      GuideCommand.EqualsInfo[] array = this.hashSelectObject.Where<GuideObject>((Func<GuideObject, bool>) (v => v.enableRot)).Select<GuideObject, GuideCommand.EqualsInfo>((Func<GuideObject, GuideCommand.EqualsInfo>) (v => new GuideCommand.EqualsInfo()
      {
        dicKey = v.dicKey,
        oldValue = v.changeAmount.rot,
        newValue = v.changeAmount.defRot
      })).ToArray<GuideCommand.EqualsInfo>();
      if (!((IList<GuideCommand.EqualsInfo>) array).IsNullOrEmpty<GuideCommand.EqualsInfo>())
        Singleton<UndoRedoManager>.Instance.Do((ICommand) new GuideCommand.RotationEqualsCommand(array));
      this.SetInputTextRot();
    }

    private void OnClickInitScale()
    {
      if (this.hashSelectObject.Count == 0)
        return;
      GuideCommand.EqualsInfo[] array = this.hashSelectObject.Where<GuideObject>((Func<GuideObject, bool>) (v => v.enableScale)).Select<GuideObject, GuideCommand.EqualsInfo>((Func<GuideObject, GuideCommand.EqualsInfo>) (v => new GuideCommand.EqualsInfo()
      {
        dicKey = v.dicKey,
        oldValue = v.changeAmount.scale,
        newValue = Vector3.get_one()
      })).ToArray<GuideCommand.EqualsInfo>();
      if (!((IList<GuideCommand.EqualsInfo>) array).IsNullOrEmpty<GuideCommand.EqualsInfo>())
        Singleton<UndoRedoManager>.Instance.Do((ICommand) new GuideCommand.ScaleEqualsCommand(array));
      this.SetInputTextScale(Vector3.get_zero());
    }

    private void Awake()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._outsideVisible, (Action<M0>) (_ => this.SetVisible()));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._visible, (Action<M0>) (_b =>
      {
        this.SetVisible();
        if (this.onVisible == null)
          return;
        this.onVisible(_b);
      }));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonInit[0].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickInitPos)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonInit[1].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickInitRot)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonInit[2].get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickInitScale)));
      this.visible = false;
      List<TMP_InputField> tmpInputFieldList = new List<TMP_InputField>();
      tmpInputFieldList.AddRange((IEnumerable<TMP_InputField>) this.inputPos);
      tmpInputFieldList.AddRange((IEnumerable<TMP_InputField>) this.inputRot);
      tmpInputFieldList.AddRange((IEnumerable<TMP_InputField>) this.inputScale);
      this.arrayInput = tmpInputFieldList.ToArray();
      this.selectIndex = -1;
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this), (Func<M0, bool>) (_ => this.selectIndex != -1)), (Func<M0, bool>) (_ => Input.GetKeyDown((KeyCode) 9))), (Action<M0>) (_ => this.ChangeTarget()));
    }

    public delegate void OnVisible(bool _value);
  }
}

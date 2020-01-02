// Decompiled with JetBrains decompiler
// Type: Studio.GuideObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Studio
{
  public class GuideObject : MonoBehaviour
  {
    public Transform transformTarget;
    private int m_DicKey;
    protected ChangeAmount m_ChangeAmount;
    [SerializeField]
    protected GameObject[] roots;
    [SerializeField]
    protected GameObject objectSelect;
    [SerializeField]
    private GuideBase[] guide;
    [SerializeField]
    private GameObject m_objCenter;
    [SerializeField]
    private MeshRenderer rendererCenter;
    [SerializeField]
    protected bool[] m_Enables;
    [SerializeField]
    private bool _calcScale;
    public GuideObject.IsActiveFunc isActiveFunc;
    protected bool m_IsActive;
    protected float m_ScaleRate;
    protected float m_ScaleRot;
    protected float m_ScaleSelect;
    public GuideObject parentGuide;
    protected BoolReactiveProperty _visible;
    private BoolReactiveProperty _visibleOutside;
    public GuideObject.Mode mode;
    public Transform parent;
    private GuideMove.MoveCalc _moveCalc;
    public bool nonconnect;

    public GuideObject()
    {
      base.\u002Ector();
    }

    public int dicKey
    {
      get
      {
        return this.m_DicKey;
      }
      set
      {
        if (!Utility.SetStruct<int>(ref this.m_DicKey, value))
          return;
        this.changeAmount = Studio.Studio.GetChangeAmount(this.m_DicKey);
      }
    }

    public ChangeAmount changeAmount
    {
      get
      {
        return this.m_ChangeAmount;
      }
      private set
      {
        this.m_ChangeAmount = value;
        if (this.m_ChangeAmount == null)
          return;
        this.m_ChangeAmount.onChangePos += new Action(this.CalcPosition);
        this.m_ChangeAmount.onChangeRot += new Action(this.CalcRotation);
        this.m_ChangeAmount.onChangeScale += new Action<Vector3>(this.CalcScale);
      }
    }

    public GuideMove[] guideMove
    {
      get
      {
        return ((IEnumerable<GuideBase>) this.guide).Skip<GuideBase>(1).Take<GuideBase>(3).Select<GuideBase, GuideMove>((Func<GuideBase, GuideMove>) (g => g as GuideMove)).ToArray<GuideMove>();
      }
    }

    public GuideSelect guideSelect
    {
      get
      {
        return this.guide[11] as GuideSelect;
      }
    }

    public GameObject objCenter
    {
      get
      {
        return this.m_objCenter;
      }
    }

    public bool[] enables
    {
      get
      {
        return this.m_Enables;
      }
    }

    public bool enablePos
    {
      get
      {
        return this.m_Enables[0];
      }
      set
      {
        if (!Utility.SetStruct<bool>(ref this.m_Enables[0], value))
          return;
        this.roots[0].SetActive(this.isActive && this.m_Enables[0]);
      }
    }

    public bool enableRot
    {
      get
      {
        return this.m_Enables[1];
      }
      set
      {
        if (!Utility.SetStruct<bool>(ref this.m_Enables[1], value))
          return;
        this.roots[1].SetActive(this.isActive && this.m_Enables[1]);
      }
    }

    public bool enableScale
    {
      get
      {
        return this.m_Enables[2];
      }
      set
      {
        if (!Utility.SetStruct<bool>(ref this.m_Enables[2], value))
          return;
        this.roots[2].SetActive(this.isActive && this.m_Enables[2]);
      }
    }

    public bool calcScale
    {
      get
      {
        return this._calcScale;
      }
      set
      {
        this._calcScale = value;
      }
    }

    public bool enableMaluti { get; set; }

    public bool isActive
    {
      get
      {
        return this.m_IsActive;
      }
      set
      {
        if (!Utility.SetStruct<bool>(ref this.m_IsActive, value))
          return;
        this.SetMode(GuideObjectManager.GetMode(), true);
      }
    }

    public float scaleRate
    {
      get
      {
        return this.m_ScaleRate;
      }
      set
      {
        if (!Utility.SetStruct<float>(ref this.m_ScaleRate, value))
          return;
        this.SetScale();
      }
    }

    public float scaleRot
    {
      get
      {
        return this.m_ScaleRot;
      }
      set
      {
        if (!Utility.SetStruct<float>(ref this.m_ScaleRot, value))
          return;
        this.SetScale();
      }
    }

    public float scaleSelect
    {
      get
      {
        return this.m_ScaleSelect;
      }
      set
      {
        if (!Utility.SetStruct<float>(ref this.m_ScaleSelect, value))
          return;
        this.SetScale();
      }
    }

    public bool isChild
    {
      get
      {
        return Object.op_Inequality((Object) this.parentGuide, (Object) null);
      }
    }

    public int layer
    {
      get
      {
        return this.isChild ? ((Component) this.parentGuide).get_gameObject().get_layer() : ((Component) this).get_gameObject().get_layer();
      }
    }

    public void SetMode(int _mode, bool _layer = true)
    {
      for (int index = 0; index < 3; ++index)
      {
        if (!Object.op_Equality((Object) this.roots[index], (Object) null))
          this.roots[index].SetActive(this.isActive && this.m_Enables[index] && _mode == index);
      }
      bool _active = ((this.isActive ? 0 : (((IEnumerable<bool>) this.m_Enables).Any<bool>((Func<bool, bool>) (b => b)) ? 1 : 0)) | (!this.isActive ? 0 : (!this.m_Enables[_mode] ? 1 : 0))) != 0;
      this.objectSelect.SetActive(_active);
      if (!_layer)
        return;
      this.SetLayer(((Component) this).get_gameObject(), !this.isChild ? LayerMask.NameToLayer(!_active ? "Studio/Select" : "Studio/Col") : this.layer);
      if (this.isActiveFunc == null)
        return;
      this.isActiveFunc(_active);
    }

    public void SetActive(bool _active, bool _layer = true)
    {
      this.m_IsActive = _active;
      this.SetMode(GuideObjectManager.GetMode(), _layer);
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

    public bool visibleOutside
    {
      get
      {
        return ((ReactiveProperty<bool>) this._visibleOutside).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._visibleOutside).set_Value(value);
      }
    }

    public bool visibleCenter
    {
      get
      {
        return ((Renderer) this.rendererCenter).get_enabled();
      }
      set
      {
        ((Renderer) this.rendererCenter).set_enabled(value);
      }
    }

    public bool visibleTranslation
    {
      set
      {
        foreach (Component component in ((IEnumerable<GuideBase>) this.guide).Skip<GuideBase>(13))
          component.get_gameObject().SetActive(value);
      }
    }

    public GuideMove.MoveCalc moveCalc
    {
      get
      {
        return this._moveCalc;
      }
      set
      {
        this._moveCalc = value;
        foreach (GuideMove guideMove in this.guideMove)
          guideMove.moveCalc = value;
      }
    }

    private bool isQuit { get; set; }

    private void CalcPosition()
    {
      if (!this.m_Enables[0] || !Object.op_Implicit((Object) this.transformTarget))
        return;
      if (Object.op_Implicit((Object) this.parent) && this.nonconnect)
        this.transformTarget.set_position(this.parent.TransformPoint(this.changeAmount.pos));
      else
        this.transformTarget.set_localPosition(this.changeAmount.pos);
    }

    private void CalcRotation()
    {
      if (!this.m_Enables[1] || !Object.op_Implicit((Object) this.transformTarget))
        return;
      if (Object.op_Implicit((Object) this.parent) && this.nonconnect)
        this.transformTarget.set_rotation(Quaternion.op_Multiply(this.parent.get_rotation(), Quaternion.Euler(this.changeAmount.rot)));
      else
        this.transformTarget.set_localRotation(Quaternion.Euler(this.changeAmount.rot));
    }

    private void CalcScale(Vector3 _value)
    {
      if (!Object.op_Implicit((Object) this.transformTarget) || !Object.op_Implicit((Object) this.parent) || !this.nonconnect)
        return;
      this.transformTarget.set_localScale(this.changeAmount.scale);
    }

    public void SetScale()
    {
      this.roots[0].get_transform().set_localScale(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_one(), this.m_ScaleRate), Studio.Studio.optionSystem.manipulateSize));
      this.roots[1].get_transform().set_localScale(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_one(), 15f), this.m_ScaleRate), 1.1f), this.m_ScaleRot), Studio.Studio.optionSystem.manipulateSize));
      this.roots[2].get_transform().set_localScale(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_one(), this.m_ScaleRate), Studio.Studio.optionSystem.manipulateSize));
      this.objectSelect.get_transform().set_localScale(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_one(), this.m_ScaleRate), this.m_ScaleSelect), Studio.Studio.optionSystem.manipulateSize));
      this.m_objCenter.get_transform().set_localScale(Vector3.op_Multiply(Vector3.op_Multiply(new Vector3(0.01f, 0.01f, 0.01f), this.m_ScaleRate), Studio.Studio.optionSystem.manipulateSize));
    }

    public void SetLayer(GameObject _object, int _layer)
    {
      if (Object.op_Equality((Object) _object, (Object) null))
        return;
      _object.set_layer(_layer);
      Transform transform = _object.get_transform();
      int childCount = transform.get_childCount();
      for (int index = 0; index < childCount; ++index)
        this.SetLayer(((Component) transform.GetChild(index)).get_gameObject(), _layer);
    }

    public GuideCommand.EqualsInfo SetWorldPos(Vector3 _pos)
    {
      Vector3 pos = this.m_ChangeAmount.pos;
      if (Object.op_Implicit((Object) this.parent) && this.nonconnect)
      {
        this.m_ChangeAmount.pos = this.parent.InverseTransformPoint(_pos);
      }
      else
      {
        this.transformTarget.set_position(_pos);
        this.m_ChangeAmount.pos = this.transformTarget.get_localPosition();
      }
      return new GuideCommand.EqualsInfo()
      {
        dicKey = this.dicKey,
        oldValue = pos,
        newValue = this.m_ChangeAmount.pos
      };
    }

    public void MoveWorld(Vector3 _value)
    {
      if (Object.op_Implicit((Object) this.parent) && this.nonconnect)
      {
        this.m_ChangeAmount.pos = Vector3.op_Addition(this.m_ChangeAmount.pos, this.parent.InverseTransformVector(_value));
      }
      else
      {
        this.transformTarget.set_position(Vector3.op_Addition(this.transformTarget.get_position(), _value));
        this.m_ChangeAmount.pos = this.transformTarget.get_localPosition();
      }
    }

    public void MoveLocal(Vector3 _value, bool _snap, GuideMove.MoveAxis _axis)
    {
      switch (this.moveCalc)
      {
        case GuideMove.MoveCalc.TYPE1:
          Vector3 _src1 = Vector3.op_Addition(this.m_ChangeAmount.pos, _value);
          this.m_ChangeAmount.pos = !_snap ? _src1 : this.Parse(_src1, _axis);
          break;
        case GuideMove.MoveCalc.TYPE2:
        case GuideMove.MoveCalc.TYPE3:
          if (Object.op_Implicit((Object) this.parent) && this.nonconnect)
          {
            Vector3 _src2 = Vector3.op_Addition(this.m_ChangeAmount.pos, this.parent.InverseTransformVector(_value));
            this.m_ChangeAmount.pos = !_snap ? _src2 : this.Parse(_src2, _axis);
            break;
          }
          Vector3 _src3 = Vector3.op_Addition(this.transformTarget.get_position(), _value);
          this.transformTarget.set_position(!_snap ? _src3 : this.Parse(_src3, _axis));
          this.m_ChangeAmount.pos = this.transformTarget.get_localPosition();
          break;
      }
    }

    private Vector3 Parse(Vector3 _src, GuideMove.MoveAxis _axis)
    {
      string format = string.Format("F{0}", (object) (2 - Studio.Studio.optionSystem.snap));
      ((Vector3) ref _src).set_Item((int) _axis, float.Parse(((Vector3) ref _src).get_Item((int) _axis).ToString(format)));
      return _src;
    }

    public void MoveLocal(Vector3 _value)
    {
      this.m_ChangeAmount.pos = Vector3.op_Addition(this.m_ChangeAmount.pos, ((Component) this).get_transform().InverseTransformVector(_value));
    }

    public void Rotation(Vector3 _axis, float _angle)
    {
      this.transformTarget.Rotate(_axis, _angle, (Space) 0);
      this.m_ChangeAmount.rot = this.transformTarget.get_localEulerAngles();
    }

    public void ForceUpdate()
    {
      this.CalcPosition();
      this.CalcRotation();
    }

    public void SetEnable(int _pos = -1, int _rot = -1, int _scale = -1)
    {
      if (_pos != -1)
        this.m_Enables[0] = _pos == 1;
      if (_rot != -1)
        this.m_Enables[1] = _rot == 1;
      if (_scale != -1)
        this.m_Enables[2] = _scale == 1;
      this.SetMode(GuideObjectManager.GetMode(), true);
    }

    public void SetVisibleCenter(bool _value)
    {
      this.m_objCenter.SetActive(_value);
    }

    private void Awake()
    {
      this.m_DicKey = -1;
      this.isActiveFunc = (GuideObject.IsActiveFunc) null;
      this.parentGuide = (GuideObject) null;
      this.enableMaluti = true;
      this.calcScale = true;
      this.visibleTranslation = Singleton<Studio.Studio>.Instance.workInfo.visibleAxisTranslation;
      this.visibleCenter = Singleton<Studio.Studio>.Instance.workInfo.visibleAxisCenter;
      this.SetVisibleCenter(false);
      Renderer component = (Renderer) this.objectSelect.GetComponent<Renderer>();
      if (Object.op_Implicit((Object) component))
        component.get_material().set_renderQueue(3500);
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._visible, (Action<M0>) (_b =>
      {
        foreach (GuideBase guideBase in this.guide)
          guideBase.draw = _b & this.visibleOutside;
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._visibleOutside, (Action<M0>) (_b =>
      {
        foreach (GuideBase guideBase in this.guide)
          guideBase.draw = _b & this.visible;
      }));
      for (int index = 0; index < this.guide.Length; ++index)
        this.guide[index].guideObject = this;
    }

    private void Start()
    {
      this.isQuit = false;
      ((ReactiveProperty<bool>) this._visible).set_Value(true);
    }

    private void LateUpdate()
    {
      if (Object.op_Implicit((Object) this.parent) && this.nonconnect)
      {
        this.CalcPosition();
        this.CalcRotation();
      }
      ((Component) this).get_transform().set_position(this.transformTarget.get_position());
      ((Component) this).get_transform().set_rotation(this.transformTarget.get_rotation());
      switch (this.mode)
      {
        case GuideObject.Mode.Local:
          this.roots[0].get_transform().set_eulerAngles(!Object.op_Implicit((Object) this.parent) ? Vector3.get_zero() : this.parent.get_eulerAngles());
          break;
        case GuideObject.Mode.LocalIK:
          this.roots[0].get_transform().set_localEulerAngles(Vector3.get_zero());
          break;
        case GuideObject.Mode.World:
          this.roots[0].get_transform().set_eulerAngles(Vector3.get_zero());
          break;
      }
      if (!this.calcScale)
        return;
      Vector3 localScale = this.transformTarget.get_localScale();
      Vector3 lossyScale = this.transformTarget.get_lossyScale();
      Vector3 vector3 = !this.enableScale ? Vector3.get_one() : this.changeAmount.scale;
      this.transformTarget.set_localScale(new Vector3((float) (localScale.x / lossyScale.x * vector3.x), (float) (localScale.y / lossyScale.y * vector3.y), (float) (localScale.z / lossyScale.z * vector3.z)));
    }

    private void OnApplicationQuit()
    {
      this.isQuit = true;
    }

    private void OnDestroy()
    {
      if (this.isQuit || Scene.isGameEnd || !Singleton<GuideObjectManager>.IsInstance())
        return;
      Singleton<GuideObjectManager>.Instance.Delete(this, false);
    }

    public delegate void IsActiveFunc(bool _active);

    public enum Mode
    {
      Local,
      LocalIK,
      World,
    }
  }
}

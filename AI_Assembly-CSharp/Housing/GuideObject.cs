// Decompiled with JetBrains decompiler
// Type: Housing.GuideObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Housing
{
  public class GuideObject : MonoBehaviour
  {
    [SerializeField]
    private GameObject[] roots;
    [SerializeField]
    private GuideBase[] guide;
    [SerializeField]
    private GameObject m_objCenter;
    [SerializeField]
    private MeshRenderer rendererCenter;
    [SerializeField]
    [Header("サイズ補助")]
    private Camera mainCamera;
    [SerializeField]
    private float minDis;
    [SerializeField]
    private float maxDis;
    [SerializeField]
    private float minScale;
    [SerializeField]
    private float maxScale;
    [ReadOnly]
    [Header("サイズ関係")]
    public Vector3 min;
    [ReadOnly]
    public Vector3 max;
    private FloatReactiveProperty _scaleRate;
    private BoolReactiveProperty _visible;
    private BoolReactiveProperty _visibleOutside;

    public GuideObject()
    {
      base.\u002Ector();
    }

    public Transform TransformTarget { get; private set; }

    public ObjectCtrl ObjectCtrl { get; private set; }

    public GuideMove[] guideMove
    {
      get
      {
        return ((IEnumerable<GuideBase>) this.guide).Skip<GuideBase>(1).Take<GuideBase>(3).Select<GuideBase, GuideMove>((Func<GuideBase, GuideMove>) (g => g as GuideMove)).ToArray<GuideMove>();
      }
    }

    public GuideBase[] guides
    {
      get
      {
        return this.guide;
      }
    }

    public GameObject objCenter
    {
      get
      {
        return this.m_objCenter;
      }
    }

    public float scaleRate
    {
      get
      {
        return ((ReactiveProperty<float>) this._scaleRate).get_Value();
      }
      set
      {
        ((ReactiveProperty<float>) this._scaleRate).set_Value(value);
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

    public void SetTarget(ObjectCtrl _objectCtrl)
    {
      if (_objectCtrl is OCFolder)
        _objectCtrl = (ObjectCtrl) null;
      this.ObjectCtrl = _objectCtrl;
      this.TransformTarget = this.ObjectCtrl?.GameObject.get_transform();
      this.TransformTarget.SafeProc<Transform>((Action<Transform>) (_t => ((Component) this).get_transform().SetPositionAndRotation(_t.get_position(), _t.get_rotation())));
      this.visible = _objectCtrl != null;
    }

    public void SetScale()
    {
    }

    public void SetLayer(GameObject _object, int _layer)
    {
      if (Object.op_Equality((Object) _object, (Object) null))
        return;
      _object.set_layer(_layer);
      IEnumerator enumerator = _object.get_transform().GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          this.SetLayer(((Component) enumerator.Current).get_gameObject(), _layer);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    public void SetVisibleCenter(bool _value)
    {
      this.m_objCenter.SetActive(_value);
    }

    private void Awake()
    {
      foreach (GuideBase guideBase in this.guide)
        guideBase.Init(this);
      this.visibleCenter = true;
      if (Object.op_Equality((Object) this.mainCamera, (Object) null))
        this.mainCamera = Camera.get_main();
      ObservableExtensions.Subscribe<float>((IObservable<M0>) this._scaleRate, (Action<M0>) (_f => this.SetScale()));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._visible, (Action<M0>) (_b =>
      {
        foreach (GuideBase guideBase in this.guide)
          guideBase.Draw = _b & this.visibleOutside;
        Singleton<CraftScene>.Instance.UICtrl.ManipulateUICtrl.Visible = _b & this.visibleOutside;
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._visibleOutside, (Action<M0>) (_b =>
      {
        foreach (GuideBase guideBase in this.guide)
          guideBase.Draw = _b & this.visible;
        Singleton<CraftScene>.Instance.UICtrl.ManipulateUICtrl.Visible = _b & this.visible;
      }));
    }

    private void Update()
    {
      ((Component) this).get_transform().set_localScale(Vector3.op_Multiply(Vector3.get_one(), Mathf.Lerp(this.minScale, this.maxScale, Mathf.InverseLerp(this.minDis, this.maxDis, Vector3.Distance(((Component) this.mainCamera).get_transform().get_position(), ((Component) this).get_transform().get_position())))));
    }

    private void LateUpdate()
    {
      this.TransformTarget.SafeProc<Transform>((Action<Transform>) (_t => ((Component) this).get_transform().SetPositionAndRotation(_t.get_position(), _t.get_rotation())));
      if (Singleton<GuideManager>.IsInstance() && Object.op_Implicit((Object) Singleton<GuideManager>.Instance.TransformRoot))
        this.roots[0].get_transform().set_rotation(Singleton<GuideManager>.Instance.TransformRoot.get_rotation());
      else
        this.roots[0].get_transform().set_eulerAngles(Vector3.get_zero());
    }
  }
}

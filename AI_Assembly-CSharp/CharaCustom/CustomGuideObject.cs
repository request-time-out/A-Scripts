// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomGuideObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharaCustom
{
  public class CustomGuideObject : MonoBehaviour
  {
    public CustomGuideObject.Amount amount;
    public bool isDrag;
    [Range(0.01f, 3f)]
    public float scaleAxis;
    [Range(0.01f, 3f)]
    public float speedMove;
    public CameraControl_Ver2 ccv2;
    [SerializeField]
    protected ObjectCategoryBehaviour roots;
    protected float m_ScaleRate;
    protected float m_ScaleRot;
    protected float m_ScaleSelect;

    public CustomGuideObject()
    {
      base.\u002Ector();
    }

    public int ctrlAxisType { get; set; }

    public float scaleRate
    {
      get
      {
        return this.m_ScaleRate;
      }
      set
      {
        if ((double) this.m_ScaleRate == (double) value)
          return;
        this.m_ScaleRate = value;
        this.UpdateScale();
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
        if ((double) this.m_ScaleRot == (double) value)
          return;
        this.m_ScaleRot = value;
        this.UpdateScale();
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
        if ((double) this.m_ScaleSelect == (double) value)
          return;
        this.m_ScaleSelect = value;
        this.UpdateScale();
      }
    }

    public void SetMode(int _mode)
    {
      this.roots.SetActiveToggle(_mode);
    }

    public void UpdateScale()
    {
      this.roots.GetObject(0).get_transform().set_localScale(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_one(), this.m_ScaleRate), this.scaleAxis));
      this.roots.GetObject(1).get_transform().set_localScale(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_one(), 15f), this.m_ScaleRate), 1.1f), this.m_ScaleRot), this.scaleAxis));
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

    private void Awake()
    {
      List<CustomGuideBase> customGuideBaseList = new List<CustomGuideBase>();
      for (int _array = 0; _array < 2; ++_array)
        customGuideBaseList.AddRange((IEnumerable<CustomGuideBase>) ((IEnumerable<CustomGuideBase>) this.roots.GetObject(_array).GetComponentsInChildren<CustomGuideBase>()).ToList<CustomGuideBase>());
      foreach (CustomGuideBase customGuideBase in customGuideBaseList.ToArray())
        customGuideBase.guideObject = this;
      this.SetMode(0);
      this.UpdateScale();
    }

    private void LateUpdate()
    {
      ((Component) this).get_transform().set_localPosition(((Component) this).get_transform().InverseTransformVector(this.amount.position));
      this.roots.GetObject(1).SafeProcObject<GameObject>((Action<GameObject>) (o => o.get_transform().set_localRotation(Quaternion.Euler(this.amount.rotation))));
    }

    public class Amount
    {
      public Vector3 position = Vector3.get_zero();
      public Vector3 rotation = Vector3.get_zero();
    }
  }
}

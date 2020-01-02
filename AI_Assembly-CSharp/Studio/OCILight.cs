// Decompiled with JetBrains decompiler
// Type: Studio.OCILight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace Studio
{
  public class OCILight : ObjectCtrlInfo
  {
    public GameObject objectLight;
    protected Light m_Light;
    public Info.LightLoadInfo.Target lightTarget;
    public LightColor lightColor;

    public OILightInfo lightInfo
    {
      get
      {
        return this.objectInfo as OILightInfo;
      }
    }

    public Light light
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Light, (Object) null))
          this.m_Light = (Light) this.objectLight.GetComponentInChildren<Light>();
        return this.m_Light;
      }
    }

    public LightType lightType
    {
      get
      {
        return Object.op_Inequality((Object) this.light, (Object) null) ? this.light.get_type() : (LightType) 1;
      }
    }

    public void SetColor(Color _color)
    {
      this.lightInfo.color = _color;
      this.light.set_color(this.lightInfo.color);
      if (!Object.op_Implicit((Object) this.lightColor))
        return;
      this.lightColor.color = this.lightInfo.color;
    }

    public bool SetIntensity(float _value, bool _force = false)
    {
      if (!Utility.SetStruct<float>(ref this.lightInfo.intensity, _value) && !_force)
        return false;
      if (Object.op_Implicit((Object) this.light))
        this.light.set_intensity(this.lightInfo.intensity);
      return true;
    }

    public bool SetRange(float _value, bool _force = false)
    {
      if (!Utility.SetStruct<float>(ref this.lightInfo.range, _value) && !_force)
        return false;
      if (Object.op_Implicit((Object) this.light))
        this.light.set_range(this.lightInfo.range);
      return true;
    }

    public bool SetSpotAngle(float _value, bool _force = false)
    {
      if (!Utility.SetStruct<float>(ref this.lightInfo.spotAngle, _value) && !_force)
        return false;
      if (Object.op_Implicit((Object) this.light))
        this.light.set_spotAngle(this.lightInfo.spotAngle);
      return true;
    }

    public bool SetEnable(bool _value, bool _force = false)
    {
      if (!Utility.SetStruct<bool>(ref this.lightInfo.enable, _value) && !_force)
        return false;
      if (Object.op_Implicit((Object) this.light))
        ((Behaviour) this.light).set_enabled(this.lightInfo.enable);
      return true;
    }

    public bool SetDrawTarget(bool _value, bool _force = false)
    {
      if (!Utility.SetStruct<bool>(ref this.lightInfo.drawTarget, _value) && !_force)
        return false;
      Singleton<GuideObjectManager>.Instance.drawLightLine.SetEnable(this.light, this.lightInfo.drawTarget);
      this.guideObject.visible = this.lightInfo.drawTarget;
      return true;
    }

    public bool SetShadow(bool _value, bool _force = false)
    {
      if (!Utility.SetStruct<bool>(ref this.lightInfo.shadow, _value) && !_force)
        return false;
      if (Object.op_Implicit((Object) this.light))
        this.light.set_shadows(!this.lightInfo.shadow ? (LightShadows) 0 : (LightShadows) 2);
      return true;
    }

    public void Update()
    {
      this.SetColor(this.lightInfo.color);
      this.SetIntensity(this.lightInfo.intensity, true);
      this.SetRange(this.lightInfo.range, true);
      this.SetSpotAngle(this.lightInfo.spotAngle, true);
      this.SetEnable(this.lightInfo.enable, true);
      this.SetDrawTarget(this.lightInfo.drawTarget, true);
      this.SetShadow(this.lightInfo.shadow, true);
    }

    public override void OnDelete()
    {
      Singleton<GuideObjectManager>.Instance.Delete(this.guideObject, true);
      Object.Destroy((Object) this.objectLight);
      if (this.parentInfo != null)
        this.parentInfo.OnDetachChild((ObjectCtrlInfo) this);
      Studio.Studio.DeleteInfo(this.objectInfo, true);
    }

    public override void OnAttach(TreeNodeObject _parent, ObjectCtrlInfo _child)
    {
    }

    public override void OnLoadAttach(TreeNodeObject _parent, ObjectCtrlInfo _child)
    {
    }

    public override void OnDetach()
    {
      this.parentInfo.OnDetachChild((ObjectCtrlInfo) this);
      this.guideObject.parent = (Transform) null;
      Studio.Studio.AddInfo(this.objectInfo, (ObjectCtrlInfo) this);
      this.objectLight.get_transform().SetParent(Singleton<Scene>.Instance.commonSpace.get_transform());
      this.objectInfo.changeAmount.pos = this.objectLight.get_transform().get_localPosition();
      this.objectInfo.changeAmount.rot = this.objectLight.get_transform().get_localEulerAngles();
      this.treeNodeObject.ResetVisible();
    }

    public override void OnDetachChild(ObjectCtrlInfo _child)
    {
    }

    public override void OnSelect(bool _select)
    {
    }

    public override void OnSavePreprocessing()
    {
      base.OnSavePreprocessing();
    }

    public override void OnVisible(bool _visible)
    {
    }
  }
}

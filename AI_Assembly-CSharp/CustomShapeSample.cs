// Decompiled with JetBrains decompiler
// Type: CustomShapeSample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

public class CustomShapeSample : MonoBehaviour
{
  public CustomShapeSample.CustomCtrl cctrl;
  public Transform trfPanel;
  private Slider[] sldCustom;
  public Transform trfSample;
  public Transform trfDemo;
  private Animator anmDemo;
  public WireFrameRender wfr;

  public CustomShapeSample()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (this.cctrl != null)
      this.cctrl.Initialize();
    if (Object.op_Implicit((Object) this.trfPanel))
    {
      for (int no = 0; no < ShapeSampleDefine.shapename.Length; ++no)
      {
        Transform transform1 = ((Component) this.trfPanel).get_transform().Find("Parts" + no.ToString("00"));
        if (!Object.op_Equality((Object) null, (Object) this.trfPanel))
        {
          Transform transform2 = transform1.Find("Slider");
          if (!Object.op_Equality((Object) null, (Object) transform2))
          {
            this.sldCustom[no] = (Slider) ((Component) transform2).GetComponent<Slider>();
            if (this.cctrl != null && this.cctrl.CheckInitEnd())
              this.sldCustom[no].set_value(this.cctrl.GetValue(no));
          }
        }
      }
    }
    if (!Object.op_Implicit((Object) this.trfDemo))
      return;
    this.anmDemo = (Animator) ((Component) this.trfDemo).GetComponent<Animator>();
    if (!Object.op_Implicit((Object) this.anmDemo))
      return;
    Animator anmDemo = this.anmDemo;
    AnimatorStateInfo animatorStateInfo = this.anmDemo.GetCurrentAnimatorStateInfo(0);
    int fullPathHash = ((AnimatorStateInfo) ref animatorStateInfo).get_fullPathHash();
    anmDemo.Play(fullPathHash, 0, 0.5f);
  }

  public void OnWireFrameDraw(Toggle tgl)
  {
    if (!Object.op_Implicit((Object) this.wfr))
      return;
    this.wfr.wireFrameDraw = tgl.get_isOn();
  }

  public void OnObjectPosition(Toggle tgl)
  {
    float[] numArray = new float[2];
    if (tgl.get_isOn())
    {
      numArray[0] = -0.2f;
      numArray[1] = 0.1f;
    }
    else
    {
      numArray[0] = 0.0f;
      numArray[1] = 0.0f;
    }
    if (Object.op_Implicit((Object) this.trfSample))
      this.trfSample.set_position(new Vector3(numArray[0], 0.0f, 0.0f));
    if (!Object.op_Implicit((Object) this.trfDemo))
      return;
    this.trfDemo.set_position(new Vector3(numArray[1], 0.0f, 0.0f));
  }

  public void OnPushButton(int id)
  {
    float num1 = 0.0f;
    switch (id)
    {
      case 1:
        num1 = 0.5f;
        break;
      case 2:
        num1 = 1f;
        break;
    }
    for (int index = 0; index < ShapeSampleDefine.shapename.Length; ++index)
      this.sldCustom[index].set_value(num1);
    if (!Object.op_Implicit((Object) this.anmDemo))
      return;
    Animator anmDemo = this.anmDemo;
    AnimatorStateInfo animatorStateInfo = this.anmDemo.GetCurrentAnimatorStateInfo(0);
    int fullPathHash = ((AnimatorStateInfo) ref animatorStateInfo).get_fullPathHash();
    double num2 = (double) num1;
    anmDemo.Play(fullPathHash, 0, (float) num2);
  }

  private void Update()
  {
    if (this.cctrl == null || !this.cctrl.CheckInitEnd())
      return;
    for (int no = 0; no < ShapeSampleDefine.shapename.Length; ++no)
    {
      if (Object.op_Inequality((Object) null, (Object) this.sldCustom[no]))
        this.cctrl.SetValue(no, this.sldCustom[no].get_value());
    }
    this.cctrl.Update();
  }

  [Serializable]
  public class CustomCtrl
  {
    private bool InitEnd;
    public GameObject objSample;
    private ShapeInfoBase sibSample;
    private float[] value;

    public bool CheckInitEnd()
    {
      return this.InitEnd;
    }

    public void Update()
    {
      if (this.sibSample == null)
        return;
      this.sibSample.Update();
    }

    public void SetValue(int no, float val)
    {
      this.value[no] = val;
      if (this.sibSample == null)
        return;
      this.sibSample.ChangeValue(no, val);
    }

    public float GetValue(int no)
    {
      return this.value[no];
    }

    public void Initialize()
    {
      this.sibSample = (ShapeInfoBase) new ShapeInfoSample();
      int length = ShapeSampleDefine.shapename.Length;
      this.value = new float[length];
      if (this.sibSample != null && Object.op_Inequality((Object) null, (Object) this.objSample))
      {
        this.sibSample.InitShapeInfo(string.Empty, "sample.unity3d", "sample.unity3d", "anmShapeSample", "customSample", this.objSample.get_transform());
        for (int no = 0; no < length; ++no)
          this.SetValue(no, 0.5f);
        this.sibSample.Update();
      }
      this.InitEnd = true;
    }
  }
}

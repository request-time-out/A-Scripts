// Decompiled with JetBrains decompiler
// Type: UVTextureAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

internal class UVTextureAnimator : MonoBehaviour
{
  public Material[] AnimatedMaterialsNotInstance;
  public int Rows;
  public int Columns;
  public float Fps;
  public int OffsetMat;
  public Vector2 SelfTiling;
  public bool IsLoop;
  public bool IsReverse;
  public bool IsRandomOffsetForInctance;
  public bool IsBump;
  public bool IsHeight;
  public bool IsCutOut;
  private bool isInizialised;
  private int index;
  private int count;
  private int allCount;
  private float deltaFps;
  private bool isVisible;
  private bool isCorutineStarted;
  private Renderer currentRenderer;
  private Material instanceMaterial;

  public UVTextureAnimator()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.InitMaterial();
    this.InitDefaultVariables();
    this.isInizialised = true;
    this.isVisible = true;
    this.StartCoroutine(this.UpdateCorutine());
  }

  public void SetInstanceMaterial(Material mat, Vector2 offsetMat)
  {
    this.instanceMaterial = mat;
    this.InitDefaultVariables();
  }

  private void InitDefaultVariables()
  {
    this.allCount = 0;
    this.deltaFps = 1f / this.Fps;
    this.count = this.Rows * this.Columns;
    this.index = this.Columns - 1;
    Vector2 vector2_1;
    ((Vector2) ref vector2_1).\u002Ector((float) this.index / (float) this.Columns - (float) (this.index / this.Columns), (float) (1.0 - (double) (this.index / this.Columns) / (double) this.Rows));
    this.OffsetMat = this.IsRandomOffsetForInctance ? Random.Range(0, this.count) : this.OffsetMat - this.OffsetMat / this.count * this.count;
    Vector2 vector2_2 = !Vector2.op_Equality(this.SelfTiling, Vector2.get_zero()) ? this.SelfTiling : new Vector2(1f / (float) this.Columns, 1f / (float) this.Rows);
    if (this.AnimatedMaterialsNotInstance.Length > 0)
    {
      foreach (Material material in this.AnimatedMaterialsNotInstance)
      {
        material.SetTextureScale("_MainTex", vector2_2);
        material.SetTextureOffset("_MainTex", Vector2.get_zero());
        if (this.IsBump)
        {
          material.SetTextureScale("_BumpMap", vector2_2);
          material.SetTextureOffset("_BumpMap", Vector2.get_zero());
        }
        if (this.IsHeight)
        {
          material.SetTextureScale("_HeightMap", vector2_2);
          material.SetTextureOffset("_HeightMap", Vector2.get_zero());
        }
        if (this.IsCutOut)
        {
          material.SetTextureScale("_CutOut", vector2_2);
          material.SetTextureOffset("_CutOut", Vector2.get_zero());
        }
      }
    }
    else if (Object.op_Inequality((Object) this.instanceMaterial, (Object) null))
    {
      this.instanceMaterial.SetTextureScale("_MainTex", vector2_2);
      this.instanceMaterial.SetTextureOffset("_MainTex", vector2_1);
      if (this.IsBump)
      {
        this.instanceMaterial.SetTextureScale("_BumpMap", vector2_2);
        this.instanceMaterial.SetTextureOffset("_BumpMap", vector2_1);
      }
      if (this.IsBump)
      {
        this.instanceMaterial.SetTextureScale("_HeightMap", vector2_2);
        this.instanceMaterial.SetTextureOffset("_HeightMap", vector2_1);
      }
      if (!this.IsCutOut)
        return;
      this.instanceMaterial.SetTextureScale("_CutOut", vector2_2);
      this.instanceMaterial.SetTextureOffset("_CutOut", vector2_1);
    }
    else
    {
      if (!Object.op_Inequality((Object) this.currentRenderer, (Object) null))
        return;
      this.currentRenderer.get_material().SetTextureScale("_MainTex", vector2_2);
      this.currentRenderer.get_material().SetTextureOffset("_MainTex", vector2_1);
      if (this.IsBump)
      {
        this.currentRenderer.get_material().SetTextureScale("_BumpMap", vector2_2);
        this.currentRenderer.get_material().SetTextureOffset("_BumpMap", vector2_1);
      }
      if (this.IsHeight)
      {
        this.currentRenderer.get_material().SetTextureScale("_HeightMap", vector2_2);
        this.currentRenderer.get_material().SetTextureOffset("_HeightMap", vector2_1);
      }
      if (!this.IsCutOut)
        return;
      this.currentRenderer.get_material().SetTextureScale("_CutOut", vector2_2);
      this.currentRenderer.get_material().SetTextureOffset("_CutOut", vector2_1);
    }
  }

  private void InitMaterial()
  {
    if (Object.op_Inequality((Object) ((Component) this).GetComponent<Renderer>(), (Object) null))
    {
      this.currentRenderer = (Renderer) ((Component) this).GetComponent<Renderer>();
    }
    else
    {
      Projector component = (Projector) ((Component) this).GetComponent<Projector>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      if (!((Object) component.get_material()).get_name().EndsWith("(Instance)"))
      {
        Projector projector = component;
        Material material1 = new Material(component.get_material());
        ((Object) material1).set_name(((Object) component.get_material()).get_name() + " (Instance)");
        Material material2 = material1;
        projector.set_material(material2);
      }
      this.instanceMaterial = component.get_material();
    }
  }

  private void OnEnable()
  {
    if (!this.isInizialised)
      return;
    this.InitDefaultVariables();
    this.isVisible = true;
    if (this.isCorutineStarted)
      return;
    this.StartCoroutine(this.UpdateCorutine());
  }

  private void OnDisable()
  {
    this.isCorutineStarted = false;
    this.isVisible = false;
    this.StopAllCoroutines();
  }

  private void OnBecameVisible()
  {
    this.isVisible = true;
    if (this.isCorutineStarted)
      return;
    this.StartCoroutine(this.UpdateCorutine());
  }

  private void OnBecameInvisible()
  {
    this.isVisible = false;
  }

  [DebuggerHidden]
  private IEnumerator UpdateCorutine()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new UVTextureAnimator.\u003CUpdateCorutine\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void UpdateCorutineFrame()
  {
    if (Object.op_Equality((Object) this.currentRenderer, (Object) null) && Object.op_Equality((Object) this.instanceMaterial, (Object) null) && this.AnimatedMaterialsNotInstance.Length == 0)
      return;
    ++this.allCount;
    if (this.IsReverse)
      --this.index;
    else
      ++this.index;
    if (this.index >= this.count)
      this.index = 0;
    if (this.AnimatedMaterialsNotInstance.Length > 0)
    {
      for (int index = 0; index < this.AnimatedMaterialsNotInstance.Length; ++index)
      {
        int num1 = index * this.OffsetMat + this.index + this.OffsetMat;
        int num2 = num1 - num1 / this.count * this.count;
        Vector2 vector2;
        ((Vector2) ref vector2).\u002Ector((float) num2 / (float) this.Columns - (float) (num2 / this.Columns), (float) (1.0 - (double) (num2 / this.Columns) / (double) this.Rows));
        this.AnimatedMaterialsNotInstance[index].SetTextureOffset("_MainTex", vector2);
        if (this.IsBump)
          this.AnimatedMaterialsNotInstance[index].SetTextureOffset("_BumpMap", vector2);
        if (this.IsHeight)
          this.AnimatedMaterialsNotInstance[index].SetTextureOffset("_HeightMap", vector2);
        if (this.IsCutOut)
          this.AnimatedMaterialsNotInstance[index].SetTextureOffset("_CutOut", vector2);
      }
    }
    else
    {
      Vector2 vector2;
      if (this.IsRandomOffsetForInctance)
      {
        int num = this.index + this.OffsetMat;
        ((Vector2) ref vector2).\u002Ector((float) num / (float) this.Columns - (float) (num / this.Columns), (float) (1.0 - (double) (num / this.Columns) / (double) this.Rows));
      }
      else
        ((Vector2) ref vector2).\u002Ector((float) this.index / (float) this.Columns - (float) (this.index / this.Columns), (float) (1.0 - (double) (this.index / this.Columns) / (double) this.Rows));
      if (Object.op_Inequality((Object) this.instanceMaterial, (Object) null))
      {
        this.instanceMaterial.SetTextureOffset("_MainTex", vector2);
        if (this.IsBump)
          this.instanceMaterial.SetTextureOffset("_BumpMap", vector2);
        if (this.IsHeight)
          this.instanceMaterial.SetTextureOffset("_HeightMap", vector2);
        if (!this.IsCutOut)
          return;
        this.instanceMaterial.SetTextureOffset("_CutOut", vector2);
      }
      else
      {
        if (!Object.op_Inequality((Object) this.currentRenderer, (Object) null))
          return;
        this.currentRenderer.get_material().SetTextureOffset("_MainTex", vector2);
        if (this.IsBump)
          this.currentRenderer.get_material().SetTextureOffset("_BumpMap", vector2);
        if (this.IsHeight)
          this.currentRenderer.get_material().SetTextureOffset("_HeightMap", vector2);
        if (!this.IsCutOut)
          return;
        this.currentRenderer.get_material().SetTextureOffset("_CutOut", vector2);
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ProceduralImage.ProceduralImage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace UnityEngine.UI.ProceduralImage
{
  [ExecuteInEditMode]
  [AddComponentMenu("UI/Procedural Image")]
  public class ProceduralImage : Image
  {
    [SerializeField]
    private float borderWidth;
    private ProceduralImageModifier modifier;
    private static Material materialInstance;
    [SerializeField]
    private float falloffDistance;

    public ProceduralImage()
    {
      base.\u002Ector();
    }

    public float BorderWidth
    {
      get
      {
        return this.borderWidth;
      }
      set
      {
        this.borderWidth = value;
        ((Graphic) this).SetVerticesDirty();
      }
    }

    public float FalloffDistance
    {
      get
      {
        return this.falloffDistance;
      }
      set
      {
        this.falloffDistance = value;
        ((Graphic) this).SetVerticesDirty();
      }
    }

    protected ProceduralImageModifier Modifier
    {
      get
      {
        if (Object.op_Equality((Object) this.modifier, (Object) null))
        {
          this.modifier = (ProceduralImageModifier) ((Component) this).GetComponent<ProceduralImageModifier>();
          if (Object.op_Equality((Object) this.modifier, (Object) null))
            this.ModifierType = typeof (FreeModifier);
        }
        return this.modifier;
      }
      set
      {
        this.modifier = value;
      }
    }

    public System.Type ModifierType
    {
      get
      {
        return ((object) this.Modifier).GetType();
      }
      set
      {
        if (Object.op_Inequality((Object) ((Component) this).GetComponent<ProceduralImageModifier>(), (Object) null))
          Object.DestroyImmediate((Object) ((Component) this).GetComponent<ProceduralImageModifier>());
        ((Component) this).get_gameObject().AddComponent(value);
        this.Modifier = (ProceduralImageModifier) ((Component) this).GetComponent<ProceduralImageModifier>();
        ((Graphic) this).SetAllDirty();
      }
    }

    protected virtual void OnEnable()
    {
      base.OnEnable();
      this.Init();
    }

    private void Init()
    {
      this.set_preserveAspect(false);
      if (Object.op_Equality((Object) this.get_sprite(), (Object) null))
        this.set_sprite(EmptySprite.Get());
      if (Object.op_Equality((Object) UnityEngine.UI.ProceduralImage.ProceduralImage.materialInstance, (Object) null))
        UnityEngine.UI.ProceduralImage.ProceduralImage.materialInstance = new Material(Shader.Find("UI/Procedural UI Image"));
      ((Graphic) this).set_material(UnityEngine.UI.ProceduralImage.ProceduralImage.materialInstance);
    }

    private Vector4 FixRadius(Vector4 vec)
    {
      Rect rect = ((Graphic) this).get_rectTransform().get_rect();
      ((Vector4) ref vec).\u002Ector(Mathf.Max((float) vec.x, 0.0f), Mathf.Max((float) vec.y, 0.0f), Mathf.Max((float) vec.z, 0.0f), Mathf.Max((float) vec.w, 0.0f));
      float num = Mathf.Min(new float[5]
      {
        ((Rect) ref rect).get_width() / (float) (vec.x + vec.y),
        ((Rect) ref rect).get_width() / (float) (vec.z + vec.w),
        ((Rect) ref rect).get_height() / (float) (vec.x + vec.w),
        ((Rect) ref rect).get_height() / (float) (vec.z + vec.y),
        1f
      });
      return Vector4.op_Multiply(vec, num);
    }

    protected virtual void OnPopulateMesh(VertexHelper toFill)
    {
      base.OnPopulateMesh(toFill);
      this.EncodeAllInfoIntoVertices(toFill, this.CalculateInfo());
    }

    private ProceduralImageInfo CalculateInfo()
    {
      if (Object.op_Equality((Object) this.get_sprite(), (Object) null))
        this.set_sprite(EmptySprite.Get());
      Rect pixelAdjustedRect = ((Graphic) this).GetPixelAdjustedRect();
      Vector3[] vector3Array = new Vector3[4];
      ((Graphic) this).get_rectTransform().GetWorldCorners(vector3Array);
      float pixelSize = Vector3.Distance(vector3Array[1], vector3Array[2]) / ((Rect) ref pixelAdjustedRect).get_width() / Mathf.Max(0.0f, this.falloffDistance);
      Vector4 vector4 = this.FixRadius(this.Modifier.CalculateRadius(pixelAdjustedRect));
      float num = Mathf.Min(((Rect) ref pixelAdjustedRect).get_width(), ((Rect) ref pixelAdjustedRect).get_height());
      return new ProceduralImageInfo(((Rect) ref pixelAdjustedRect).get_width() + this.falloffDistance, ((Rect) ref pixelAdjustedRect).get_height() + this.falloffDistance, this.falloffDistance, pixelSize, Vector4.op_Division(vector4, num), (float) ((double) this.borderWidth / (double) num * 2.0));
    }

    private void EncodeAllInfoIntoVertices(VertexHelper vh, ProceduralImageInfo info)
    {
      UIVertex uiVertex = (UIVertex) null;
      Vector2 vector2_1;
      ((Vector2) ref vector2_1).\u002Ector(info.width, info.height);
      Vector2 vector2_2;
      ((Vector2) ref vector2_2).\u002Ector(this.EncodeFloats_0_1_16_16((float) info.radius.x, (float) info.radius.y), this.EncodeFloats_0_1_16_16((float) info.radius.z, (float) info.radius.w));
      Vector2 vector2_3;
      ((Vector2) ref vector2_3).\u002Ector((double) info.borderWidth != 0.0 ? Mathf.Clamp01(info.borderWidth) : 1f, info.pixelSize);
      for (int index = 0; index < vh.get_currentVertCount(); ++index)
      {
        vh.PopulateUIVertex(ref uiVertex, index);
        ref UIVertex local = ref uiVertex;
        local.position = (__Null) Vector3.op_Addition((Vector3) local.position, Vector3.op_Multiply(Vector3.op_Subtraction(Vector2.op_Implicit((Vector2) uiVertex.uv0), new Vector3(0.5f, 0.5f)), info.fallOffDistance));
        uiVertex.uv1 = (__Null) vector2_1;
        uiVertex.uv2 = (__Null) vector2_2;
        uiVertex.uv3 = (__Null) vector2_3;
        vh.SetUIVertex(uiVertex, index);
      }
    }

    private float EncodeFloats_0_1_16_16(float a, float b)
    {
      Vector2 vector2;
      ((Vector2) ref vector2).\u002Ector(1f, 1.525902E-05f);
      return Vector2.Dot(new Vector2(Mathf.Floor(a * 65534f) / (float) ushort.MaxValue, Mathf.Floor(b * 65534f) / (float) ushort.MaxValue), vector2);
    }
  }
}

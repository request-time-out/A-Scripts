// Decompiled with JetBrains decompiler
// Type: PlaceholderSoftware.WetStuff.Demos.Demo_Assets.Wipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace PlaceholderSoftware.WetStuff.Demos.Demo_Assets
{
  [RequireComponent(typeof (PlaceholderSoftware.WetStuff.WetStuff))]
  public class Wipe : MonoBehaviour
  {
    private Mesh _mesh;
    private Material _material;
    private PlaceholderSoftware.WetStuff.WetStuff _decals;
    [Range(0.0f, 1f)]
    public float Progress;

    public Wipe()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this._material = new Material(Shader.Find("Demo/ExcludeWetness"));
      this._mesh = Wipe.CreateFullscreenQuad();
      this._decals = (PlaceholderSoftware.WetStuff.WetStuff) ((Component) this).GetComponent<PlaceholderSoftware.WetStuff.WetStuff>();
      this._decals.add_AfterDecalRender(new Action<CommandBuffer>(this.RecordCommandBuffer));
    }

    private static Mesh CreateFullscreenQuad()
    {
      Mesh mesh1 = new Mesh();
      mesh1.set_vertices(new Vector3[4]
      {
        new Vector3(-1f, -1f, 0.0f),
        new Vector3(-1f, 1f, 0.0f),
        new Vector3(1f, 1f, 0.0f),
        new Vector3(1f, -1f, 0.0f)
      });
      mesh1.set_uv(new Vector2[4]
      {
        new Vector2(0.0f, 1f),
        new Vector2(0.0f, 0.0f),
        new Vector2(1f, 0.0f),
        new Vector2(1f, 1f)
      });
      Mesh mesh2 = mesh1;
      mesh2.SetIndices(new int[6]{ 0, 1, 2, 2, 3, 0 }, (MeshTopology) 0, 0);
      return mesh2;
    }

    private void OnDestroy()
    {
      if (!Object.op_Inequality((Object) this._decals, (Object) null))
        return;
      this._decals.remove_AfterDecalRender(new Action<CommandBuffer>(this.RecordCommandBuffer));
    }

    private void RecordCommandBuffer(CommandBuffer cmd)
    {
      float num = Mathf.Lerp(-2f, 2f, this.Progress);
      cmd.SetRenderTarget(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 0), RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 2));
      cmd.DrawMesh(this._mesh, Matrix4x4.Translate(new Vector3(num, 0.0f, 0.0f)), this._material);
    }

    public void OnGUI()
    {
      Rect rect1 = GUILayoutUtility.GetRect(float.MaxValue, 1f);
      float width = ((Rect) ref rect1).get_width();
      Rect rect2;
      ((Rect) ref rect2).\u002Ector(170f, 20f, width - 190f, 30f);
      this.Progress = GUI.HorizontalSlider(rect2, this.Progress, 0.0f, 0.5f);
      Rect rect3;
      ((Rect) ref rect3).\u002Ector(20f, 15f, 160f, 30f);
      GUI.Label(rect3, "Remove Wetness Effect:");
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: MeshBrush.Examples.RuntimeExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace MeshBrush.Examples
{
  public class RuntimeExample : MonoBehaviour
  {
    [SerializeField]
    private MeshBrush.MeshBrush meshbrushInstance;
    [SerializeField]
    private Transform circleBrush;
    [SerializeField]
    private Slider radiusSlider;
    [SerializeField]
    private Slider scatteringSlider;
    [SerializeField]
    private Slider densitySlider;
    private Transform mainCamera;

    public RuntimeExample()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.mainCamera = ((Component) Camera.get_main()).get_transform();
    }

    private void Update()
    {
      this.meshbrushInstance.radius = this.radiusSlider.get_value();
      this.meshbrushInstance.scatteringRange = new Vector2(this.scatteringSlider.get_value(), this.scatteringSlider.get_value());
      this.meshbrushInstance.densityRange = new Vector2(this.densitySlider.get_value(), this.densitySlider.get_value());
      RaycastHit brushLocation;
      if (!Physics.Raycast(Camera.get_main().ScreenPointToRay(Input.get_mousePosition()), ref brushLocation))
        return;
      this.circleBrush.set_position(((RaycastHit) ref brushLocation).get_point());
      this.circleBrush.set_forward(Vector3.op_UnaryNegation(((RaycastHit) ref brushLocation).get_normal()));
      this.circleBrush.set_localScale(new Vector3(this.meshbrushInstance.radius, this.meshbrushInstance.radius, 1f));
      if (Input.GetKey(this.meshbrushInstance.paintKey))
        this.meshbrushInstance.PaintMeshes(brushLocation);
      if (Input.GetKey(this.meshbrushInstance.deleteKey))
        this.meshbrushInstance.DeleteMeshes(brushLocation);
      if (!Input.GetKey(this.meshbrushInstance.randomizeKey))
        return;
      this.meshbrushInstance.RandomizeMeshes(brushLocation);
    }
  }
}

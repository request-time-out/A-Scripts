// Decompiled with JetBrains decompiler
// Type: Highlight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Vectrosity;

public class Highlight : MonoBehaviour
{
  public int lineWidth;
  public int energyLineWidth;
  public float selectionSize;
  public float force;
  public int pointsInEnergyLine;
  private VectorLine line;
  private VectorLine energyLine;
  private RaycastHit hit;
  private int selectIndex;
  private float energyLevel;
  private bool canClick;
  private GameObject[] spheres;
  private double timer;
  private int ignoreLayer;
  private int defaultLayer;
  private bool fading;

  public Highlight()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    Time.set_fixedDeltaTime(0.01f);
    this.spheres = new GameObject[((MakeSpheres) ((Component) this).GetComponent<MakeSpheres>()).numberOfSpheres];
    this.ignoreLayer = LayerMask.NameToLayer("Ignore Raycast");
    this.defaultLayer = LayerMask.NameToLayer("Default");
    this.line = new VectorLine("Line", new List<Vector2>(), (float) this.lineWidth);
    this.line.set_color(Color32.op_Implicit(Color.get_green()));
    this.line.set_capLength((float) this.lineWidth * 0.5f);
    this.energyLine = new VectorLine("Energy", new List<Vector2>(this.pointsInEnergyLine), (Texture) null, (float) this.energyLineWidth, (LineType) 0);
    this.SetEnergyLinePoints();
  }

  private void SetEnergyLinePoints()
  {
    for (int index = 0; index < this.energyLine.get_points2().Count; ++index)
    {
      float num = Mathf.Lerp(70f, (float) (Screen.get_width() - 20), (float) index / (float) this.energyLine.get_points2().Count);
      this.energyLine.get_points2()[index] = new Vector2(num, (float) Screen.get_height() * 0.1f);
    }
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0) && (Input.get_mousePosition().x > 50.0 && !this.fading))
    {
      if (!Input.GetKey((KeyCode) 304) && !Input.GetKey((KeyCode) 303) && this.selectIndex > 0)
        this.ResetSelection(true);
      if (Physics.Raycast(Camera.get_main().ScreenPointToRay(Input.get_mousePosition()), ref this.hit))
      {
        this.spheres[this.selectIndex] = ((Component) ((RaycastHit) ref this.hit).get_collider()).get_gameObject();
        this.spheres[this.selectIndex].set_layer(this.ignoreLayer);
        ((Renderer) this.spheres[this.selectIndex].GetComponent<Renderer>()).get_material().EnableKeyword("_EMISSION");
        ++this.selectIndex;
        this.line.Resize(this.selectIndex * 10);
      }
    }
    for (int index = 0; index < this.selectIndex; ++index)
    {
      float num = (float) ((double) Screen.get_height() * (double) this.selectionSize / ((Component) Camera.get_main()).get_transform().InverseTransformPoint(this.spheres[index].get_transform().get_position()).z);
      Vector3 screenPoint = Camera.get_main().WorldToScreenPoint(this.spheres[index].get_transform().get_position());
      Rect rect;
      ((Rect) ref rect).\u002Ector((float) screenPoint.x - num, (float) screenPoint.y - num, num * 2f, num * 2f);
      this.line.MakeRect(rect, index * 10);
      this.line.get_points2()[index * 10 + 8] = new Vector2(((Rect) ref rect).get_x() - (float) this.lineWidth * 0.25f, ((Rect) ref rect).get_y() + num);
      this.line.get_points2()[index * 10 + 9] = new Vector2(35f, Mathf.Lerp(65f, (float) (Screen.get_height() - 25), this.energyLevel));
      ((Renderer) this.spheres[index].GetComponent<Renderer>()).get_material().SetColor("_EmissionColor", new Color(this.energyLevel, this.energyLevel, this.energyLevel));
    }
  }

  private void FixedUpdate()
  {
    int index;
    for (index = 0; index < this.energyLine.get_points2().Count - 1; ++index)
      this.energyLine.get_points2()[index] = new Vector2((float) this.energyLine.get_points2()[index].x, (float) this.energyLine.get_points2()[index + 1].y);
    this.timer += (double) Time.get_deltaTime() * (double) Mathf.Lerp(5f, 20f, this.energyLevel);
    this.energyLine.get_points2()[index] = new Vector2((float) this.energyLine.get_points2()[index].x, (float) Screen.get_height() * (float) (0.100000001490116 + (double) Mathf.Sin((float) this.timer) * 0.0799999982118607 * (double) this.energyLevel));
  }

  private void LateUpdate()
  {
    this.line.Draw();
    this.energyLine.Draw();
  }

  private void ResetSelection(bool instantFade)
  {
    if ((double) this.energyLevel > 0.0)
      this.StartCoroutine(this.FadeColor(instantFade));
    this.selectIndex = 0;
    this.energyLevel = 0.0f;
    this.line.get_points2().Clear();
    this.line.Draw();
    foreach (GameObject sphere in this.spheres)
    {
      if (Object.op_Implicit((Object) sphere))
        sphere.set_layer(this.defaultLayer);
    }
  }

  [DebuggerHidden]
  private IEnumerator FadeColor(bool instantFade)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new Highlight.\u003CFadeColor\u003Ec__Iterator0()
    {
      instantFade = instantFade,
      \u0024this = this
    };
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(60f, 20f, 600f, 40f), "Click to select sphere, shift-click to select multiple spheres\nThen change energy level slider and click Go");
    this.energyLevel = GUI.VerticalSlider(new Rect(30f, 20f, 10f, (float) (Screen.get_height() - 80)), this.energyLevel, 1f, 0.0f);
    if (this.selectIndex == 0)
      this.energyLevel = 0.0f;
    if (!GUI.Button(new Rect(20f, (float) (Screen.get_height() - 40), 32f, 20f), "Go"))
      return;
    for (int index = 0; index < this.selectIndex; ++index)
      ((Rigidbody) this.spheres[index].GetComponent<Rigidbody>()).AddRelativeForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_forward(), this.force), this.energyLevel), (ForceMode) 2);
    this.ResetSelection(false);
  }
}

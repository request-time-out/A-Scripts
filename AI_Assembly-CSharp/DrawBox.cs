// Decompiled with JetBrains decompiler
// Type: DrawBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class DrawBox : MonoBehaviour
{
  public float moveSpeed;
  public float explodePower;
  public Camera vectorCam;
  private bool mouseDown;
  private Rigidbody[] rigidbodies;
  private bool canClick;
  private bool boxDrawn;

  public DrawBox()
  {
    base.\u002Ector();
  }

  [DebuggerHidden]
  private IEnumerator Start()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new DrawBox.\u003CStart\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void Update()
  {
    Vector3 mousePosition = Input.get_mousePosition();
    mousePosition.z = (__Null) 1.0;
    Vector3 worldPoint = Camera.get_main().ScreenToWorldPoint(mousePosition);
    if (Input.GetMouseButtonDown(0) && this.canClick)
    {
      ((Renderer) ((Component) this).GetComponent<Renderer>()).set_enabled(true);
      ((Component) this).get_transform().set_position(worldPoint);
      this.mouseDown = true;
    }
    if (this.mouseDown)
      ((Component) this).get_transform().set_localScale(new Vector3((float) (worldPoint.x - ((Component) this).get_transform().get_position().x), (float) (worldPoint.y - ((Component) this).get_transform().get_position().y), 1f));
    if (Input.GetMouseButtonUp(0))
    {
      this.mouseDown = false;
      this.boxDrawn = true;
    }
    ((Component) this).get_transform().Translate(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(Vector3.get_up()), Time.get_deltaTime()), this.moveSpeed), Input.GetAxis("Vertical")));
    ((Component) this).get_transform().Translate(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_right(), Time.get_deltaTime()), this.moveSpeed), Input.GetAxis("Horizontal")));
  }

  private void OnGUI()
  {
    GUI.Box(new Rect(20f, 20f, 320f, 38f), "Draw a box by clicking and dragging with the mouse\nMove the drawn box with the arrow keys");
    Rect rect;
    ((Rect) ref rect).\u002Ector(20f, 62f, 60f, 30f);
    this.canClick = !((Rect) ref rect).Contains(Event.get_current().get_mousePosition());
    if (!this.boxDrawn || !GUI.Button(rect, "Boom!"))
      return;
    foreach (Rigidbody rigidbody in this.rigidbodies)
      rigidbody.AddExplosionForce(this.explodePower, new Vector3(0.0f, -6.5f, -1.5f), 20f, 0.0f, (ForceMode) 2);
  }
}

// Decompiled with JetBrains decompiler
// Type: GameController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class GameController : MonoBehaviour
{
  public GameObject m_Player;

  public GameController()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    this.m_Player.get_transform().Rotate(new Vector3(0.0f, (float) ((double) Input.GetAxis("Horizontal") * (double) Time.get_deltaTime() * 200.0), 0.0f));
    this.m_Player.get_transform().Translate(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_forward(), Input.GetAxis("Vertical")), Time.get_deltaTime()), 4f));
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(50f, 50f, 200f, 20f), "Press arrow key to move");
    Animation componentInChildren = (Animation) this.m_Player.GetComponentInChildren<Animation>();
    ((Behaviour) componentInChildren).set_enabled(GUI.Toggle(new Rect(50f, 70f, 200f, 20f), ((Behaviour) componentInChildren).get_enabled(), "Play Animation"));
    DynamicBone[] components = (DynamicBone[]) this.m_Player.GetComponents<DynamicBone>();
    GUI.Label(new Rect(50f, 100f, 200f, 20f), "Choose dynamic bone:");
    DynamicBone dynamicBone = components[0];
    bool flag = GUI.Toggle(new Rect(50f, 120f, 100f, 20f), ((Behaviour) components[0]).get_enabled(), "Breasts");
    ((Behaviour) components[1]).set_enabled(flag);
    int num = flag ? 1 : 0;
    ((Behaviour) dynamicBone).set_enabled(num != 0);
    ((Behaviour) components[2]).set_enabled(GUI.Toggle(new Rect(50f, 140f, 100f, 20f), ((Behaviour) components[2]).get_enabled(), "Tail"));
  }
}

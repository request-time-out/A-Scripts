// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.PanelThrowObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  public class PanelThrowObject : UseObject
  {
    public GameObject ThrowBox;
    public GameObject[] ThrowObjects;

    private void Start()
    {
    }

    public override void Use()
    {
      base.Use();
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.ThrowObjects[Random.Range(0, this.ThrowObjects.Length)], Vector3.op_Addition(this.ThrowBox.get_transform().get_position(), Vector3.op_Multiply(Vector3.get_up(), 2f)), Quaternion.get_identity());
      gameObject.AddComponent<ThrowObject>();
      gameObject.set_tag("Exploder");
      if (Object.op_Equality((Object) gameObject.GetComponent<Rigidbody>(), (Object) null))
        gameObject.AddComponent<Rigidbody>();
      if (gameObject.GetComponentsInChildren<BoxCollider>().Length == 0)
        gameObject.AddComponent<BoxCollider>();
      Vector3 up = Vector3.get_up();
      up.x = (__Null) (double) Random.Range(-0.2f, 0.2f);
      up.y = (__Null) (double) Random.Range(1f, 0.8f);
      up.z = (__Null) (double) Random.Range(-0.2f, 0.2f);
      ((Vector3) ref up).Normalize();
      ((Rigidbody) gameObject.GetComponent<Rigidbody>()).set_velocity(Vector3.op_Multiply(up, 20f));
      ((Rigidbody) gameObject.GetComponent<Rigidbody>()).set_angularVelocity(Vector3.op_Multiply(Random.get_insideUnitSphere(), 3f));
      ((Rigidbody) gameObject.GetComponent<Rigidbody>()).set_mass(20f);
    }
  }
}

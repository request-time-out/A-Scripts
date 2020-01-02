// Decompiled with JetBrains decompiler
// Type: BehaviorSelection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;

public class BehaviorSelection : MonoBehaviour
{
  public GameObject marker;
  public GameObject mainBot;
  public GameObject flockGroup;
  public GameObject followGroup;
  public GameObject queueGroup;
  public GameObject[] waypoints;
  public GameObject[] waypointsA;
  public GUISkin descriptionGUISkin;
  private Vector3[] flockGroupPosition;
  private Vector3[] followGroupPosition;
  private Vector3[] queueGroupPosition;
  private Quaternion[] flockGroupRotation;
  private Quaternion[] followGroupRotation;
  private Quaternion[] queueGroupRotation;
  private Dictionary<int, BehaviorTree> behaviorTreeGroup;
  private BehaviorSelection.BehaviorSelectionType selectionType;
  private BehaviorSelection.BehaviorSelectionType prevSelectionType;

  public BehaviorSelection()
  {
    base.\u002Ector();
  }

  public void Start()
  {
    BehaviorTree[] components1 = (BehaviorTree[]) this.mainBot.GetComponents<BehaviorTree>();
    for (int index = 0; index < components1.Length; ++index)
      this.behaviorTreeGroup.Add(components1[index].get_Group(), components1[index]);
    BehaviorTree[] components2 = (BehaviorTree[]) ((Component) Camera.get_main()).GetComponents<BehaviorTree>();
    for (int index = 0; index < components2.Length; ++index)
      this.behaviorTreeGroup.Add(components2[index].get_Group(), components2[index]);
    this.flockGroupPosition = new Vector3[this.flockGroup.get_transform().get_childCount()];
    this.flockGroupRotation = new Quaternion[this.flockGroup.get_transform().get_childCount()];
    for (int index = 0; index < this.flockGroup.get_transform().get_childCount(); ++index)
    {
      ((Component) this.flockGroup.get_transform().GetChild(index)).get_gameObject().SetActive(false);
      this.flockGroupPosition[index] = ((Component) this.flockGroup.get_transform().GetChild(index)).get_transform().get_position();
      this.flockGroupRotation[index] = ((Component) this.flockGroup.get_transform().GetChild(index)).get_transform().get_rotation();
    }
    this.followGroupPosition = new Vector3[this.followGroup.get_transform().get_childCount()];
    this.followGroupRotation = new Quaternion[this.followGroup.get_transform().get_childCount()];
    for (int index = 0; index < this.followGroup.get_transform().get_childCount(); ++index)
    {
      ((Component) this.followGroup.get_transform().GetChild(index)).get_gameObject().SetActive(false);
      this.followGroupPosition[index] = ((Component) this.followGroup.get_transform().GetChild(index)).get_transform().get_position();
      this.followGroupRotation[index] = ((Component) this.followGroup.get_transform().GetChild(index)).get_transform().get_rotation();
    }
    this.queueGroupPosition = new Vector3[this.queueGroup.get_transform().get_childCount()];
    this.queueGroupRotation = new Quaternion[this.queueGroup.get_transform().get_childCount()];
    for (int index = 0; index < this.queueGroup.get_transform().get_childCount(); ++index)
    {
      ((Component) this.queueGroup.get_transform().GetChild(index)).get_gameObject().SetActive(false);
      this.queueGroupPosition[index] = ((Component) this.queueGroup.get_transform().GetChild(index)).get_transform().get_position();
      this.queueGroupRotation[index] = ((Component) this.queueGroup.get_transform().GetChild(index)).get_transform().get_rotation();
    }
    this.SelectionChanged();
  }

  public void OnGUI()
  {
    GUILayout.BeginVertical(new GUILayoutOption[1]
    {
      GUILayout.Width(300f)
    });
    GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    if (GUILayout.Button("<-", (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
    {
      this.prevSelectionType = this.selectionType;
      this.selectionType = (BehaviorSelection.BehaviorSelectionType) ((int) (this.selectionType - 1) % 17);
      if (this.selectionType < BehaviorSelection.BehaviorSelectionType.MoveTowards)
        this.selectionType = BehaviorSelection.BehaviorSelectionType.Queue;
      this.SelectionChanged();
    }
    GUILayout.Box(BehaviorSelection.SplitCamelCase(this.selectionType.ToString()), new GUILayoutOption[1]
    {
      GUILayout.Width(220f)
    });
    if (GUILayout.Button("->", (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
    {
      this.prevSelectionType = this.selectionType;
      this.selectionType = (BehaviorSelection.BehaviorSelectionType) ((int) (this.selectionType + 1) % 17);
      this.SelectionChanged();
    }
    GUILayout.EndHorizontal();
    GUILayout.Box(this.Description(), this.descriptionGUISkin.get_box(), (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    if (this.selectionType == BehaviorSelection.BehaviorSelectionType.CanHearObject && GUILayout.Button("Play Sound", (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
      ((AudioSource) this.marker.GetComponent<AudioSource>()).Play();
    GUILayout.EndVertical();
  }

  private string Description()
  {
    string str = string.Empty;
    switch (this.selectionType)
    {
      case BehaviorSelection.BehaviorSelectionType.MoveTowards:
        str = "The Move Towards task will move the agent towards the target (without pathfinding). In this example the green agent is moving towards the red dot.";
        break;
      case BehaviorSelection.BehaviorSelectionType.RotateTowards:
        str = "The Rotate Towards task rotate the agent towards the target. In this example the green agent is rotating towards the red dot.";
        break;
      case BehaviorSelection.BehaviorSelectionType.Seek:
        str = "The Seek task will move the agent towards the target with pathfinding. In this example the green agent is seeking the red dot (which moves).";
        break;
      case BehaviorSelection.BehaviorSelectionType.Flee:
        str = "The Flee task will move the agent away from the target with pathfinding. In this example the green agent is fleeing from red dot (which moves).";
        break;
      case BehaviorSelection.BehaviorSelectionType.Pursue:
        str = "The Pursue task is similar to the Seek task except the Pursue task predicts where the target is going to be in the future. This allows the agent to arrive at the target earlier than it would have with the Seek task.";
        break;
      case BehaviorSelection.BehaviorSelectionType.Evade:
        str = "The Evade task is similar to the Flee task except the Evade task predicts where the target is going to be in the future. This allows the agent to flee from the target earlier than it would have with the Flee task.";
        break;
      case BehaviorSelection.BehaviorSelectionType.Follow:
        str = "The Follow task will allow the agent to continuously follow a GameObject. In this example the green agent is following the red dot.";
        break;
      case BehaviorSelection.BehaviorSelectionType.Patrol:
        str = "The Patrol task moves from waypoint to waypint. In this example the green agent is patrolling with four different waypoints (the white dots).";
        break;
      case BehaviorSelection.BehaviorSelectionType.Cover:
        str = "The Cover task will move the agent into cover from its current position. In this example the agent sees cover in front of it so takes cover behind the wall.";
        break;
      case BehaviorSelection.BehaviorSelectionType.Wander:
        str = "The Wander task moves the agent randomly throughout the map with pathfinding.";
        break;
      case BehaviorSelection.BehaviorSelectionType.Search:
        str = "The Search task will search the map by wandering until it finds the target. It can find the target by seeing or hearing the target. In this example the Search task is looking for the red dot.";
        break;
      case BehaviorSelection.BehaviorSelectionType.WithinDistance:
        str = "The Within Distance task is a conditional task that returns success when another object comes within distance of the current agent. In this example the Within Distance task is paired with the Seek task so the agent will move towards the target as soon as the target within distance.";
        break;
      case BehaviorSelection.BehaviorSelectionType.CanSeeObject:
        str = "The Can See Object task is a conditional task that returns success when it sees an object in front of the current agent. In this example the Can See Object task is paired with the Seek task so the agent will move towards the target as soon as the target is seen.";
        break;
      case BehaviorSelection.BehaviorSelectionType.CanHearObject:
        str = "The Can Hear Object task is a conditional task that returns success when it hears another object. Press the \"Play\" button to emit a sound from the red dot. If the red dot is within range of the agent when the sound is played then the agent will move towards the red dot with the Seek task.";
        break;
      case BehaviorSelection.BehaviorSelectionType.Flock:
        str = "The Flock task moves a group of objects together in a pattern (which can be adjusted). In this example the Flock task is controlling all 30 objects. There are no colliders on the objects - the Flock task is completing managing the position of all of the objects";
        break;
      case BehaviorSelection.BehaviorSelectionType.LeaderFollow:
        str = "The Leader Follow task moves a group of objects behind a leader object. There are two behavior trees running in this example - one for the leader (who is patrolling the area) and one for the group of objects. Again, there is are no colliders on the objects.";
        break;
      case BehaviorSelection.BehaviorSelectionType.Queue:
        str = "The Queue task will move a group of objects through a small space in an organized way. In this example the Queue task is controlling all of the objects. Another way to move all of the objects through the doorway is with the seek task, however with this approach the objects would group up at the doorway.";
        break;
    }
    return str;
  }

  private static string SplitCamelCase(string s)
  {
    s = new Regex("(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace).Replace(s, " ");
    return (char.ToUpper(s[0]).ToString() + s.Substring(1)).Trim();
  }

  private void SelectionChanged()
  {
    this.DisableAll();
    switch (this.selectionType)
    {
      case BehaviorSelection.BehaviorSelectionType.MoveTowards:
        this.marker.get_transform().set_position(new Vector3(20f, 1f, -20f));
        this.marker.SetActive(true);
        this.mainBot.get_transform().set_position(new Vector3(-20f, 1f, -20f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 180f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.RotateTowards:
        this.marker.get_transform().set_position(new Vector3(20f, 1f, 10f));
        this.marker.SetActive(true);
        this.mainBot.get_transform().set_position(new Vector3(0.0f, 1f, -20f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 180f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.Seek:
        this.marker.get_transform().set_position(new Vector3(20f, 1f, 20f));
        this.marker.SetActive(true);
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerSeek").set_time(0.0f);
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerSeek").set_speed(1f);
        ((Animation) this.marker.GetComponent<Animation>()).Play("MarkerSeek");
        this.mainBot.get_transform().set_position(new Vector3(-20f, 1f, -20f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 180f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.Flee:
        this.marker.get_transform().set_position(new Vector3(20f, 1f, 20f));
        this.marker.SetActive(true);
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerFlee").set_time(0.0f);
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerFlee").set_speed(1f);
        ((Animation) this.marker.GetComponent<Animation>()).Play("MarkerFlee");
        this.mainBot.get_transform().set_position(new Vector3(10f, 1f, 18f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 180f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.Pursue:
        this.marker.get_transform().set_position(new Vector3(20f, 1f, 20f));
        this.marker.SetActive(true);
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerPersue").set_time(0.0f);
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerPersue").set_speed(1f);
        ((Animation) this.marker.GetComponent<Animation>()).Play("MarkerPersue");
        this.mainBot.get_transform().set_position(new Vector3(-20f, 1f, 0.0f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 90f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.Evade:
        this.marker.get_transform().set_position(new Vector3(20f, 1f, 20f));
        this.marker.SetActive(true);
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerEvade").set_time(0.0f);
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerEvade").set_speed(1f);
        ((Animation) this.marker.GetComponent<Animation>()).Play("MarkerEvade");
        this.mainBot.get_transform().set_position(new Vector3(0.0f, 1f, 18f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 180f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.Follow:
        this.marker.get_transform().set_position(new Vector3(20f, 1f, 20f));
        this.marker.SetActive(true);
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerFollow").set_time(0.0f);
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerFollow").set_speed(1f);
        ((Animation) this.marker.GetComponent<Animation>()).Play("MarkerFollow");
        this.mainBot.get_transform().set_position(new Vector3(20f, 1f, 15f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 0.0f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.Patrol:
        for (int index = 0; index < this.waypoints.Length; ++index)
          this.waypoints[index].SetActive(true);
        this.mainBot.get_transform().set_position(new Vector3(-20f, 1f, 20f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 180f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.Cover:
        this.mainBot.get_transform().set_position(new Vector3(-5f, 1f, -10f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 0.0f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.Wander:
        this.mainBot.get_transform().set_position(new Vector3(-20f, 1f, -20f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 0.0f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.Search:
        this.marker.get_transform().set_position(new Vector3(20f, 1f, 20f));
        this.marker.SetActive(true);
        this.mainBot.get_transform().set_position(new Vector3(-20f, 1f, -20f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 0.0f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.WithinDistance:
        this.marker.get_transform().set_position(new Vector3(20f, 1f, 20f));
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerPersue").set_time(0.0f);
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerPersue").set_speed(1f);
        ((Animation) this.marker.GetComponent<Animation>()).Play("MarkerPersue");
        this.marker.SetActive(true);
        this.mainBot.get_transform().set_position(new Vector3(-15f, 1f, 2f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 0.0f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.CanSeeObject:
        this.marker.get_transform().set_position(new Vector3(20f, 1f, 20f));
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerPersue").set_time(0.0f);
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerPersue").set_speed(1f);
        ((Animation) this.marker.GetComponent<Animation>()).Play("MarkerPersue");
        this.marker.SetActive(true);
        this.mainBot.get_transform().set_position(new Vector3(-15f, 1f, -10f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 0.0f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.CanHearObject:
        this.marker.get_transform().set_position(new Vector3(20f, 1f, 20f));
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerPersue").set_time(0.0f);
        ((Animation) this.marker.GetComponent<Animation>()).get_Item("MarkerPersue").set_speed(1f);
        ((Animation) this.marker.GetComponent<Animation>()).Play("MarkerPersue");
        this.marker.SetActive(true);
        this.mainBot.get_transform().set_position(new Vector3(-15f, 1f, -10f));
        this.mainBot.get_transform().set_eulerAngles(new Vector3(0.0f, 0.0f, 0.0f));
        this.mainBot.SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.Flock:
        ((Component) Camera.get_main()).get_transform().set_position(new Vector3(0.0f, 90f, -150f));
        for (int index = 0; index < this.flockGroup.get_transform().get_childCount(); ++index)
          ((Component) this.flockGroup.get_transform().GetChild(index)).get_gameObject().SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.LeaderFollow:
        for (int index = 0; index < this.waypointsA.Length; ++index)
          this.waypointsA[index].SetActive(true);
        this.mainBot.get_transform().set_position(new Vector3(0.0f, 1f, -130f));
        this.mainBot.SetActive(true);
        ((Component) Camera.get_main()).get_transform().set_position(new Vector3(0.0f, 90f, -150f));
        for (int index = 0; index < this.followGroup.get_transform().get_childCount(); ++index)
          ((Component) this.followGroup.get_transform().GetChild(index)).get_gameObject().SetActive(true);
        break;
      case BehaviorSelection.BehaviorSelectionType.Queue:
        this.marker.get_transform().set_position(new Vector3(45f, 1f, 0.0f));
        for (int index = 0; index < this.queueGroup.get_transform().get_childCount(); ++index)
          ((Component) this.queueGroup.get_transform().GetChild(index)).get_gameObject().SetActive(true);
        break;
    }
    this.StartCoroutine("EnableBehavior");
  }

  private void DisableAll()
  {
    this.StopCoroutine("EnableBehavior");
    this.behaviorTreeGroup[(int) this.prevSelectionType].DisableBehavior();
    if (this.prevSelectionType == BehaviorSelection.BehaviorSelectionType.LeaderFollow)
      this.behaviorTreeGroup[17].DisableBehavior();
    ((Animation) this.marker.GetComponent<Animation>()).Stop();
    this.marker.SetActive(false);
    this.mainBot.SetActive(false);
    ((Component) Camera.get_main()).get_transform().set_position(new Vector3(0.0f, 90f, 0.0f));
    for (int index = 0; index < this.flockGroup.get_transform().get_childCount(); ++index)
    {
      ((Component) this.flockGroup.get_transform().GetChild(index)).get_gameObject().SetActive(false);
      ((Component) this.flockGroup.get_transform().GetChild(index)).get_transform().set_position(this.flockGroupPosition[index]);
      ((Component) this.flockGroup.get_transform().GetChild(index)).get_transform().set_rotation(this.flockGroupRotation[index]);
    }
    for (int index = 0; index < this.followGroup.get_transform().get_childCount(); ++index)
    {
      ((Component) this.followGroup.get_transform().GetChild(index)).get_gameObject().SetActive(false);
      ((Component) this.followGroup.get_transform().GetChild(index)).get_transform().set_position(this.followGroupPosition[index]);
      ((Component) this.followGroup.get_transform().GetChild(index)).get_transform().set_rotation(this.followGroupRotation[index]);
    }
    for (int index = 0; index < this.queueGroup.get_transform().get_childCount(); ++index)
    {
      ((Component) this.queueGroup.get_transform().GetChild(index)).get_gameObject().SetActive(false);
      ((Component) this.queueGroup.get_transform().GetChild(index)).get_transform().set_position(this.queueGroupPosition[index]);
      ((Component) this.queueGroup.get_transform().GetChild(index)).get_transform().set_rotation(this.queueGroupRotation[index]);
    }
    for (int index = 0; index < this.waypoints.Length; ++index)
      this.waypoints[index].SetActive(false);
    for (int index = 0; index < this.waypointsA.Length; ++index)
      this.waypointsA[index].SetActive(false);
  }

  [DebuggerHidden]
  private IEnumerator EnableBehavior()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new BehaviorSelection.\u003CEnableBehavior\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private enum BehaviorSelectionType
  {
    MoveTowards,
    RotateTowards,
    Seek,
    Flee,
    Pursue,
    Evade,
    Follow,
    Patrol,
    Cover,
    Wander,
    Search,
    WithinDistance,
    CanSeeObject,
    CanHearObject,
    Flock,
    LeaderFollow,
    Queue,
    Last,
  }
}

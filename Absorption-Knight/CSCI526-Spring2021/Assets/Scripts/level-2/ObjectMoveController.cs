using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoveController : MonoBehaviour
{

  //要移动的物体
  public GameObject PlatForm;
  //要移动物体开始的点
  public Transform StartPoint;
  //要移动物体结束的点
  public Transform EndPoint;
  //要移动物体的移动速度
  public float MoveSpeed;
  //要移动到的目标点
  public Vector2 target;

  public float CopySpeed;
  private bool flag = false;
  void Start () {

    //Store the start and the end position. Platform will move between these two points.
    // Debug.Log("StartPosition:   "+ StartPoint.position);
    // Debug.Log("EndPosition:   "+ EndPoint.position);
    target = EndPoint.position;
    CopySpeed = MoveSpeed;
  }

  // Update is called once per frame
  void Update () {
      
      //把物体移动到指定的位置点上
      PlatForm.transform.position = Vector2.MoveTowards(PlatForm.transform.position, target, MoveSpeed*Time.deltaTime);

      //实现物体来回移动

      if (PlatForm.transform.position.x == EndPoint.position.x)
      {
          target = StartPoint.position;
          CopySpeed *= -1;
          flag = true;
      }
      if (PlatForm.transform.position.x == StartPoint.position.x)
      {
          if (flag)
          {
              CopySpeed = Math.Abs(CopySpeed);
          }

          target = EndPoint.position;
      }

  }
  
}

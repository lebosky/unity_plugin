/***
 * 
 * 
 * 
 */ 
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class QxcRayLine : MonoBehaviour {
	
	//一个RaycastHit类型的变量, 可以得到射线碰撞点的信息  
	private RaycastHit hit;  

	bool isTest=false;

	private List<QxcRayDebugPos> debugPos;

	private float baseLineLength = 10000f;

	bool isOnlyDebugRay = false;

	//BallColliderLayer
	private int rayLayerIndex = 8;


	/**
	 * 
	 */ 
	void Start(){
		//Physics.IgnoreCollision(ball.collider, this.collider); 

	}

	void Awake()  
	{  
		debugPos = new List<QxcRayDebugPos>();

	}  


	public void sendTestRay(Vector3 pos,float angle,Color color){
		Vector3 trianglePos = Functions.mathDirectForAngle(Vector3.zero,angle,10f,2);
		pushToRayDebug(pos,trianglePos,color);
	}

	public float getRayDistance(Vector3 pos,float angle,bool isDebug=false){
		Vector3 trianglePos = Functions.mathDirectForAngle(Vector3.zero,angle,10f,2);
		Ray ray = new Ray(pos,trianglePos);
		if (PhysicsRaycast(ray, out hit, this.baseLineLength)) {
			if (isDebug) {
				pushToRayDebug(pos,new Vector3(hit.point.x-pos.x,hit.point.y-pos.y,hit.point.z-pos.z),Color.green);
			}
			return hit.distance;
		}
		return -1;
	}

	protected bool PhysicsRaycast(Ray ray,out RaycastHit hit,float maxDistance){
		if(rayLayerIndex!=-1){
			int layermask=(1<<8);
			layermask=~layermask;
//			Physics.Raycast(ray,hit,maxDistance)
			return 	Physics.Raycast (ray,out hit,maxDistance,layermask);
		}
		//Assets/Script/QuickX/Lib/Component/QxcRayLine.cs(64,32): error CS1503: Argument `#3' cannot convert `object' expression to type `float'
		return Physics.Raycast (ray,out hit,maxDistance);
	}


	public float getRayReflectAngle(Vector3 pos,float hitAngle,float moveAngle=0){
		Vector3 trianglePos = Functions.mathDirectForAngle(Vector3.zero,hitAngle,1f,2);
		Ray ray = new Ray(pos,trianglePos);
		if (PhysicsRaycast(ray, out hit, this.baseLineLength)) {
			Vector3 refectDirection = Vector3.Reflect(ray.direction,hit.normal);
			Ray reflectLine = new Ray(hit.point,refectDirection);
//			pushToRayDebug(hit.point,refectDirection,Color.red);
			Vector3 targetPoint = new Vector3(hit.point.x+refectDirection.x,hit.point.y+refectDirection.y,hit.point.z+refectDirection.z);
			float reflectangle = Functions.mathTriangle (hit.point,targetPoint);
			moveAngle = (float)Math.Round(reflectangle,2);
			moveAngle = moveAngle % 360;
			moveAngle = moveAngle < 0 ? moveAngle + 360 : moveAngle;
			return moveAngle;
		}
		return -1;
	}





	public void pushToRayDebug(Vector3 pos1,Vector3 pos2,Color color){
		if(!isTest){
			return;
		}
		if (isOnlyDebugRay) {
			debugPos.Clear();
			debugPos.Add (new QxcRayDebugPos (pos1, pos2,color));
		} else {
			debugPos.Add (new QxcRayDebugPos (pos1, pos2,color));
		}

	}

	void Update(){
		
		if(isTest){
			//这里创建植入
			int i = 0;
			foreach(QxcRayDebugPos item in debugPos){
				Debug.DrawRay (item.x1,item.x2,item.color);
				i++;
			}
		}
	}




	public class QxcRayDebugPos{
		public Vector3 x1;
		public Vector3 x2;
		public Color color;
		public QxcRayDebugPos(Vector3 a,Vector3 b,Color _color){
			x1 = a;
			x2 = b;
			color = _color;
		}
	}
	/**
	 * 碰撞信息类
	 */ 
	public class QxcRayLineHitInfo{
		public Vector3 hitPos;
		public Vector3 hitNormal;

		public int angle;
	}




}

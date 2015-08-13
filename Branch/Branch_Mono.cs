﻿/// <summary>
/// Yves Wang @ FISH, 2015, All rights reserved
/// Branch_ mono.
/// working in world co-ordinate
/// </summary>
using UnityEngine;
using System.Collections;
using ParticlePhysics2D;

[ExecuteInEditMode]
public class Branch_Mono : MonoBehaviour, IFormLayer {

	[HideInInspector]
	public Branch branch;
	
	[SerializeField]
	Simulation sim;
	public Simulation GetSimulation {
		get {return sim;} 
	}
	
	public float ks = 0.5f;
	
	[Range(20f,150f)]
	public float length = 20f;
	
	public bool debugBranch = false,debugParticlePhysics = false,debugIndex = false;
	
	//branch generation params
	[HideInInspector] public float lengthExitRatio;
	[HideInInspector] public float angleOffsetMin,angleOffsetMax;
	[HideInInspector] public float lengthMin1,lengthMax1,lengthMin2,lengthMax2;
	[HideInInspector] public float lengthBranchAThreshold,lengthBranchBThreshold;
	[HideInInspector] public int maxDepth;
	[HideInInspector] public int leafCount;
	
	void Start() {
		
	}
	
	void Update () {
		
	
	}
	
	void LateUpdate(){
		
	}
	
	//copy branch's topology to simulation
	void CopyBranchTopology(Particle2D p, Branch b,ref Simulation s) {
		
		//if the branch has children
		if (b.branchA!=null || b.branchB!=null) {
			Particle2D temp;
			temp = (b.branchA==null) ? s.makeParticle(b.branchB.Position) : s.makeParticle(b.branchA.Position);
			temp.IsLeaf = false;
			s.makeSpring(p,temp,ks);
			if (b.branchA!=null) CopyBranchTopology (temp,b.branchA,ref s);
			if (b.branchB!=null) CopyBranchTopology (temp,b.branchB,ref s);
		} 
		//if it's a leaf branch
		else {
			float xB = b.GetChildrenBranchPosX;
			float yB = b.GetChildrenBranchPosY;
			Particle2D temp = s.makeParticle(new Vector2(xB,yB));//temp is where the leaf is
			temp.IsLeaf = true;
			leafCount++;
			s.makeSpring(p,temp,ks);
		}
		
	}
	
	//called by the editor script
	public void ReGenerateBranch(){
		//Debug.Log(System.DateTime.Now);
		Branch.branchesCount = 0;
		//branch = new Branch (null,transform.position.x,transform.position.y,angle * Mathf.Deg2Rad,length);
		branch = new Branch (null,0f,0f,0f,length,0);//grow tree in local co-ord
		Debug.Log("Branches : " + Branch.branchesCount);
		if (sim==null)
			sim = new Simulation (IntegrationMedthod.VERLET);
		sim.setGravity(0f,0f);
		sim.clear();
		leafCount = 0;
		Particle2D start = sim.makeParticle (branch.Position);
		CopyBranchTopology(start,branch,ref sim);
		if (Application.isEditor) OnDrawGizmos();
	}
	
	void OnDrawGizmos() {
		
		
		if (debugBranch) {
			if (branch!=null) branch.DebugRender(transform.localToWorldMatrix);
			else Debug.Log("branch is null");
		}
		if (debugParticlePhysics) {
			if (sim!=null) sim.DebugSpring(transform.localToWorldMatrix);
		}
	}
	
	void OnDestroy(){

	}
	
	
}

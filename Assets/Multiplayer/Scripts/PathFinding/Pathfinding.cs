using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour {
	Grid grid;
	public Transform seeker;
	Vector3[] AllPlayersPosition;
	int TotalPlayerCount=0;
	GameObject cube;
	GameObject[] Players;
	private LineRenderer linerender;
	int SeekerID,TargetID;
	void Start() {
		grid = GetComponent<Grid>();
		linerender = GetComponent<LineRenderer> ();
		//linerender.material = new Material(Shader.Find("Standard"));
		//Physics.IgnoreLayerCollision (9,10,true);
	}

	void Update() {
		Players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < Players.Length; i++) {
			if (i + 1 < Players.Length) {
				FindPath(Players [0].transform.position,Players [i+1].transform.position);
			}
			//Debug.Log ("Player "+i+" position is : "+ Players [i].transform.position);
		}
		//Debug.Log ("Total players :" + Players.Length);
		//FindPath(AllOtherPlayers[0].position,AllOtherPlayers[1].position);
//		FindPath(seeker.position,target.position);
	}

	void FindPath(Vector3 startPos, Vector3 targetPos) {
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node currentNode = openSet.RemoveFirst();
			closedSet.Add(currentNode);

			if (currentNode == targetNode) {
				RetracePath(startNode,targetNode);
				//StartCoroutine("ShowDirection");
				DrawCurve();
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
				if (!neighbour.walkable || closedSet.Contains(neighbour)) {
					continue;
				}

				int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
				if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = currentNode;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
					else {
						//openSet.UpdateItem(neighbour);
					}
				}
			}
		}

	}
//	void FindPath(Vector3 startPos, Vector3 targetPos) {
//		Debug.Log (startPos+" : "+targetPos);
//	}
	void DrawCurve()
	{
		List <Node> path;
		path = grid.path;
		Vector3[] Direction=new Vector3[path.Count];
		int count=0;

		if (path != null) {
			foreach (Node n in path) {
				Direction[count] = n.worldPosition;
				count++;
			}
		}
		/*for (int i = 0; i < count; i++) {
			//Debug.Log (Direction [i]);
			linerender.SetPosition (i,Direction[i]);
		}*/
		linerender.SetVertexCount (count);
		linerender.SetPositions (Direction);
		//linerender.SetPosition (0,startNode.worldPosition);
		//linerender.SetPosition (1, endNode.worldPosition);
	}
	IEnumerator ShowDirection()
	{
		yield return new WaitForSeconds(2);
		List <Node> path;
		path = grid.path;
		if (path != null) {
			foreach (Node n in path) {
				GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				cube.tag = "pathTag";
				cube.layer = 9;
				cube.transform.position = n.worldPosition;
				Debug.Log (n.worldPosition);
				cube.transform.localScale = Vector3.one * ((grid.nodeRadius * 2) - .1f);


			}
		}	
			
		yield return new WaitForSeconds(3);

		if (path != null) {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("pathTag");
			foreach(GameObject enemy in enemies)
				GameObject.Destroy(enemy);
		}


	}
	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();
		grid.path = path;

	}

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}


}
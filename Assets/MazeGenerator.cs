using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour{
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] Vector2Int mazeSize;
    [SerializeField] float nodeSize;

    private void Start(){
        StartCoroutine(GenerateMaze(mazeSize));
    }

    IEnumerator GenerateMaze(Vector2Int size){
        List<MazeNode> nodes = new List<MazeNode>();

        // create nodes
        for(int x = 0; x < size.x; x++){
            for(int y = 0; y < size.y; y++){
                Vector3 nodePos = new Vector3(x - (size.x/2f)*nodeSize, 0, y - (size.y / 2f))*nodeSize;       // centers node at 0,0
                MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                nodes.Add(newNode);

                yield return null;

            }
        }

        // get the center node in the maze
        int midNodeIndex = (mazeSize.x * mazeSize.y) / 2;
        transform.LookAt(nodes[midNodeIndex].transform.position);

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        // Choose a random starting node
        currentPath.Add(nodes[Random.Range(0, nodes.Count)]);
        currentPath[0].SetState(NodeState.Current);

        // while we still have nodes to go through 
        while(completedNodes.Count < nodes.Count){
            // Check nodes next to the current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count -1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            if(currentNodeX < size.x - 1){
                // check node to the right of the current node
                if(!completedNodes.Contains(nodes[currentNodeIndex + size.y]) && !currentPath.Contains(nodes[currentNodeIndex + size.y])){
                    // if not in either, it's possible for the node to move here
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }

            if(currentNodeX > 0){
                // Check node to the left of the current node
                if(!completedNodes.Contains(nodes[currentNodeIndex - size.y]) && !currentPath.Contains(nodes[currentNodeIndex - size.y])){
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }

            }

            if(currentNodeY < size.y - 1){
                // Check node above current node
                if(!completedNodes.Contains(nodes[currentNodeIndex + 1]) && !currentPath.Contains(nodes[currentNodeIndex + 1])){
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);

                }
            }

            if(currentNodeY > 0){
                // Check node below current node
                if(!completedNodes.Contains(nodes[currentNodeIndex -1]) && !currentPath.Contains(nodes[currentNodeIndex - 1])){
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Choose next node
            if(possibleDirections.Count > 0){
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                switch(possibleDirections[chosenDirection]){
                    case 1:
                        chosenNode.RemoveWall(1);                           // remove left wall from the chosen node
                        currentPath[currentPath.Count - 1].RemoveWall(0);   // remove the right wall from the current node
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);                           // remove the right wall from the chosen node  
                        currentPath[currentPath.Count -1].RemoveWall(1);    // remove the left wall from the current node
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);                           // remove the top wall from the chosen node
                        currentPath[currentPath.Count -1].RemoveWall(2);    // remove the bottom wall from the current node
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);                           // remove the bottom wall from the chosen node
                        currentPath[currentPath.Count -1].RemoveWall(3);    // remove the top wall from the current node
                        break;
                }

                currentPath.Add(chosenNode);
                chosenNode.SetState(NodeState.Current);
            }
            else{
                completedNodes.Add(currentPath[currentPath.Count - 1]);
                currentPath[currentPath.Count - 1].SetState(NodeState.Completed);
                currentPath.RemoveAt(currentPath.Count - 1);
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Update(){
    }
}

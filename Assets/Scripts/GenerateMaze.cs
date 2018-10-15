using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
	MAZE GENERATOR USING THE KRUSKAL ALGORITHM
*/

public class GenerateMaze : MonoBehaviour {

	// contains the width of the maze
	public int labWidth;

	// contains the height of the maze
	public int labHeight;

	// contains all the paths between the cells ex: [0,2] => [0,3]; [2,3] => [3,3]
	public Dictionary<int[], int[]> paths;

	// each int is a set, and each list contains the coordinates (ex:[0, 3]) of the cells in this set
	private Dictionary<int, List<int[]>> edges;

	// contains the X coordinate of the cell being processed
	private int cellCheckX = 0;

	// contains the Y coordinate of the cell being processed
	private int cellCheckY = 0;

	// contains the set of the cell being processed
	private int cellCheckSet;

	// contains the set of the neighbour of the cell being processed
	private int neighbourSet;

	// contains the coordinates of the neighbour of the cell being processed
	private int[] neighbourCell;

	// Use this for initialization
	void Start () {
	}

	// Initialize the variables used to compute and display the maze
	public void InitializeMaze()
	{
		paths = new Dictionary<int[], int[]>();
		neighbourCell = new int[2];
		edges = new Dictionary<int, List<int[]>>();
		int k = 0; // the sets
		for(int i = 0; i < labHeight; i++) // loops to fill the dictionary edges that contains the sets
		{
			for(int j = 0; j < labWidth; j++)
			{
				int[] tmpArray = new int[2];
				List<int[]> tmpList = new List<int[]>();

				tmpArray[0] = j;
				tmpArray[1] = i;

				tmpList.Add(tmpArray);

				edges.Add(k++, tmpList);
			}
		}
	}

	// Check if the neighbour cell and the current cell are in the same set. If so, they shall not be processed any further
	public bool CheckNotInSameSet(int x, int y)
	{
		if (edges[cellCheckSet].Any(subList => subList.SequenceEqual(new int[2] {x, y})))
		{
			return false;
		}
		return true;
	}

	// Select a random neighbour cell around the current cell.
	// The toCheckArray represents the neighbours and is translated into a toCheck List for ease of use.
	// While it is not empty, we choose randomly a neighbour represented by one of its value.
	// If it is empty and a valid neighbour has not been chosen, the returned value tells that another current cell must be defined.
	public bool FindNeighbourCell()
	{
		int randNeighbour;
		int[] toCheckArray = {0, 1, 2, 3}; // 0: up;    1: right;    2: down;    3: left;
		List<int> toCheck = new List<int>(toCheckArray);
		neighbourCell[0] = -1; // Used to check if a neighbour cell has been chosen
		while (neighbourCell[0] == -1 && toCheck.Count > 0)
		{
			randNeighbour = Random.Range(0, toCheck.Count);
			if (toCheck[randNeighbour] == 0)
			{
				// if the coordinates are inbounds and the cells are not in the same set, validate the neighbour cell and return true
				if (cellCheckY-1 >= 0 && CheckNotInSameSet(cellCheckX, cellCheckY-1))
				{
					neighbourCell[0] = cellCheckX;
					neighbourCell[1] = cellCheckY-1;
					return true;
				}
				// remove an index from the toCheck List so the same neighbour is not check again in futur loops
				toCheck.RemoveAt(randNeighbour);
			}
			else if (toCheck[randNeighbour] == 1)
			{
				// if the coordinates are inbounds and the cells are not in the same set, validate the neighbour cell and return true
				if (cellCheckX+1 < labWidth && CheckNotInSameSet(cellCheckX+1, cellCheckY))
				{
					neighbourCell[0] = cellCheckX+1;
					neighbourCell[1] = cellCheckY;
					return true;
				}
				// remove an index from the toCheck List so the same neighbour is not check again in futur loops
				toCheck.RemoveAt(randNeighbour);
			}
			else if (toCheck[randNeighbour] == 2)
			{
				// if the coordinates are inbounds and the cells are not in the same set, validate the neighbour cell and return true
				if (cellCheckY+1 < labHeight && CheckNotInSameSet(cellCheckX, cellCheckY+1))
				{
					neighbourCell[0] = cellCheckX;
					neighbourCell[1] = cellCheckY+1;
					return true;
				}
				// remove an index from the toCheck List so the same neighbour is not check again in futur loops
				toCheck.RemoveAt(randNeighbour);
			}
			else if (toCheck[randNeighbour] == 3)
			{
				// if the coordinates are inbounds and the cells are not in the same set, validate the neighbour cell and return true
				if (cellCheckX-1 >= 0 && CheckNotInSameSet(cellCheckX-1, cellCheckY))
				{
					neighbourCell[0] = cellCheckX-1;
					neighbourCell[1] = cellCheckY;
					return true;
				}
				// remove an index from the toCheck List so the same neighbour is not check again in futur loops
				toCheck.RemoveAt(randNeighbour);
			}
			else
			{
				neighbourCell[0] = -1;
			}
		}
		return false;
	}

	// Find the set of the current cell, in order to determine a valid neighbour that has to not belong in the same set
	public bool FindCellNeighbourSet()
	{
		// Security, saying that if there is only 1 set left, the maze has been generated properly
		if (edges.Count == 1)
		{
			return true;
		}
		foreach(KeyValuePair<int, List<int[]>> keyval in edges) // parse the sets
		{
			// if a list contains the current cell's coordinates, store its set and find a neighbour
			if (keyval.Value.Any(subList => subList.SequenceEqual(new int[2]{cellCheckX, cellCheckY})))
			{
				cellCheckSet = keyval.Key;
				return FindNeighbourCell();
			}
		}
		return false;
	}

	// Randomly select a cell of the maze.
	// Launch the other methods used to select and validate a neighbour
	// Loops while a valid neighbour has not been found
	public void SelectRandomEdge()
	{
		System.Random rnd = new System.Random();
		do
		{
			cellCheckX = rnd.Next(0, labWidth);
			cellCheckY = rnd.Next(0, labHeight);
		} while (!FindCellNeighbourSet());
	}

	// Add all the cells in the neighbour's set to the current cell's set
	// This is because if one cell of a set A is linked to another set B, all the cells of A are linked to B
	public void AddNeighboursToCellSet()
	{
		foreach(int[] subArr in edges[neighbourSet])
		{
			edges[cellCheckSet].Add(new int[2]{subArr[0], subArr[1]});
		}
	}

	// Used to find the neighbour's set to later add it to the current cell's set
	public void FindNeighbourSet()
	{
		foreach(KeyValuePair<int, List<int[]>> keyval in edges)
		{
			if (keyval.Value.Any(subArr => subArr.SequenceEqual(new int[2]{neighbourCell[0], neighbourCell[1]})))
			{
				neighbourSet = keyval.Key;
				break;
			}
		}
	}

	// Remove a List<int[]> representing the cells's coordinates belonging to a set from the edges object
	public void DeleteNeighboursSet()
	{
		edges.Remove(neighbourSet);
	}

	// Adds the current cell and its neighbour's coordinates to the path array, representing the link between them two
	public void AddCellsToPaths()
	{
		paths.Add(new int[2]{cellCheckX, cellCheckY}, new int[2]{neighbourCell[0], neighbourCell[1]});
	}

	// Change the sets of the current cell and the neighbour cell by calling all the necessary methods
	public void ChangeSets()
	{
		AddCellsToPaths();
		FindNeighbourSet();
		AddNeighboursToCellSet();
		DeleteNeighboursSet();
	}

	// Stops when there is only 1 set left, meaning that the maze is completely generated
	public void MazeLoop()
	{
		while (edges.Count > 1)
		{
			SelectRandomEdge();
			ChangeSets();
		}
	}

	// Method called by the script Maze
	// Retrieves the user inputs, computes the maze, and displays it
	public void ComputeMaze()
	{
		// retrieves the user input for the width
		int.TryParse(GameObject.Find("/Canvas/UI/InputFieldWidth/Text").GetComponent<Text>().text, out labWidth);
		// retrieves the user input for the height
		int.TryParse(GameObject.Find("/Canvas/UI/InputFieldHeight/Text").GetComponent<Text>().text, out labHeight);
		if (labWidth > 0 && labHeight > 0)
		{
			// Instantiate the variables needed
			InitializeMaze();
			// Loop to compute the maze
			MazeLoop();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMaze : MonoBehaviour {

	// array of cells prefabs representing the 3D maze
	public Transform[,] cells;

	public GenerateMaze generatedMaze;

	// the prefab containing the maze cell
	private Transform labCell;

	private GameObject maze;

	public void Draw()
	{
		labCell = (Transform)Resources.Load("Prefabs/LabCell", typeof(Transform));
		cells = new Transform[generatedMaze.labWidth, generatedMaze.labHeight];
		// Create a grid made with prefabs cells
		CreateCells();
		// Break cells' walls to make the maze according to the data calculated
		BreakTheWalls();
		// Resize and replace the objects so it fits the screen
		ResizeAndCenter();
	}

	// Destroys all the cells and the maze game object so a new maze can be displayed
	public void DestroyCells()
	{
		// Retrieve all the game object with the tag "MazeWall" given to all the maze cells
		// One condition to find them all
		if (GameObject.FindGameObjectWithTag("MazeWall"))
		{
			// One loop to destroy them all
			foreach(GameObject wall in GameObject.FindGameObjectsWithTag("MazeWall"))
			{
				Destroy(wall);
			}
		}
		Destroy(maze);
	}

	// Create a grid of cells with size of width and height given by the user
	public void CreateCells() {
		DestroyCells();
		maze = new GameObject();
		maze.name = "Maze";
		// Loop from 0 to the height and width given by the user to create as much as prefabs a needed
		// Two loops to instantiate them all
		for (int x = 0; x < generatedMaze.labWidth; x++)
		{
			for (int y = 0; y < generatedMaze.labHeight; y++)
			{
				// Create a new game object with a prefab labCell and the positions between 0 and the ones given by the user
				cells[x, y] = Instantiate(labCell, new Vector3(x, y, 0), Quaternion.identity);
				// Set the game object maze as the parent of the cells
				// And in the maze bind them
				cells[x, y].parent = maze.transform;
			}
		}
	}

	// Resize and center the maze so it fits well the screen
	public void ResizeAndCenter()
	{
		float posZ; // distance at which the maze must be
		float calcDistX;
		float calcDistY;
		// The following value have been chosen by trial and error
		float startX = -generatedMaze.labWidth/2.0f; // X coordinate to position the maze
		float startY = -generatedMaze.labHeight/2.0f; // Y coordinate to position the maze

		// The following distances have been chosen by testing and writing down values tested for X and Y from 1 to 6 each
		// Like linear regression but less cool
		calcDistY = generatedMaze.labHeight * 0.9f + 0.7f;
		calcDistX = generatedMaze.labWidth * 0.7f + 1f;
		// Choose the highest distance so the display is correct in width and height, then assign the different position to the localPosition
		posZ = calcDistX>calcDistY?calcDistX:calcDistY;
		maze.transform.localPosition = new Vector3(startX, startY, posZ);

		// The following changes the position of the main Camera according to the average position of the cells
		// This allows the camera to face the maze
		// Retrieve all the cells
		// For each cell, add its vector3 value
		// At the end divide it by the number of cells
		// It gives the central position of the maze
		// Place the camera to face there
		GameObject[] selectedUnits = GameObject.FindGameObjectsWithTag("MazeWall");
		Vector3 meanVector3Temp = Vector3.zero;
   		for (int i = 0; i < selectedUnits.Length; i++) {
      		meanVector3Temp += selectedUnits[i].transform.position;
		}
		Vector3 meanVector3Pos = meanVector3Temp/selectedUnits.Length;
		Camera.main.transform.position = new Vector3(meanVector3Pos.x, meanVector3Pos.y, 0);
		// Resize the z localScal to 0.1f so it is more aesthetic with high value like 40x40
		maze.transform.localScale = new Vector3(maze.transform.localScale.x, maze.transform.localScale.y, .1f);
	}

	// Destroy the walls of the cells so the path of the maze is revealed
	// Also creates an entrance and an exit at the first and last cell
	public void BreakTheWalls() //Just another brick in it
	{
		// Breaks first and last cells' wall
		Destroy(cells[0,0].Find("WallDown").gameObject);
		Destroy(cells[generatedMaze.labWidth-1,generatedMaze.labHeight-1].Find("WallUp").gameObject);
		// Loop to check each path in the paths array
		foreach(KeyValuePair<int[], int[]> path in generatedMaze.paths)
		{
			// If the first cell's X coordinate is higher than the second cell's X coordinate
			if (path.Key[0] > path.Value[0])
			{
				// Break the left wall of the first cell
				Destroy(cells[path.Key[0], path.Key[1]].Find("WallLeft").gameObject);
				// Break the right wall of the second cell
				Destroy(cells[path.Value[0], path.Value[1]].Find("WallRight").gameObject);
			}
			// If the first cell's X coordinate is lower than the second cell's X coordinate
			else if (path.Key[0] < path.Value[0])
			{
				// Break the right wall of the first cell
				Destroy(cells[path.Key[0], path.Key[1]].Find("WallRight").gameObject);
				// Break the left wall of the second cell
				Destroy(cells[path.Value[0], path.Value[1]].Find("WallLeft").gameObject);
			}
			// If the first cell's Y coordinate is higher than the second cell's X coordinate
			else if (path.Key[1] < path.Value[1])
			{
				// Break the up wall of the first cell
				Destroy(cells[path.Key[0], path.Key[1]].Find("WallUp").gameObject);
				// Break the down wall of the second cell
				Destroy(cells[path.Value[0], path.Value[1]].Find("WallDown").gameObject);
			}
			// If the first cell's Y coordinate is lower than the second cell's X coordinate
			else if (path.Key[1] > path.Value[1])
			{
				// Break the down wall of the first cell
				Destroy(cells[path.Key[0], path.Key[1]].Find("WallDown").gameObject);
				// Break the up wall of the second cell
				Destroy(cells[path.Value[0], path.Value[1]].Find("WallUp").gameObject);
			}
		}
	}
}

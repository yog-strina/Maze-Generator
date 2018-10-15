using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

	// Called by the button "Generate"
	// It calls the script to generate a maze in memory
	// It calls the script to draw the maze calculated
	public void CreateMaze()
	{
		GenerateMaze generatedMaze = GameObject.Find("/Scripts").GetComponent<GenerateMaze>();
		DrawMaze drawnMaze = GameObject.Find("/Scripts").GetComponent<DrawMaze>();
		generatedMaze.ComputeMaze();
		drawnMaze.generatedMaze = generatedMaze;
		drawnMaze.Draw();
	}
}

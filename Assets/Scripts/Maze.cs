using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Maze : MonoBehaviour {

	// Called by the button "Generate"
	// It calls the script to generate a maze in memory
	// It calls the script to draw the maze calculated
	public void CreateMaze()
	{
		var stopwatchMain = Stopwatch.StartNew();
		//GenerateMaze generatedMaze = GameObject.Find("/Scripts").GetComponent<GenerateMaze>();
		//DrawMaze drawnMaze = GameObject.Find("/Scripts").GetComponent<DrawMaze>();
		/*var stopwatchGeneratedMaze = Stopwatch.StartNew();*/
		GenerateMaze generatedMaze = GameObject.Find("/Scripts").GetComponent<GenerateMaze>();/************/
		generatedMaze.ComputeMaze();
		/*stopwatchGeneratedMaze.Stop();*/
		/*var stopwatchDraw = Stopwatch.StartNew();*/
		DrawMaze drawnMaze = GameObject.Find("/Scripts").GetComponent<DrawMaze>();/************/
		drawnMaze.generatedMaze = generatedMaze;
		drawnMaze.Draw();
		/*stopwatchDraw.Stop();*/
		stopwatchMain.Stop();
		/*Debug.Log("Duration of the generating part: " + (stopwatchGeneratedMaze.Elapsed.TotalMilliseconds/* * 1000000#1#).ToString("0.00 ms"));*/
		/*Debug.Log("Duration of the drawing part: " + (stopwatchDraw.Elapsed.TotalMilliseconds/* * 1000000#1#).ToString("0.00 ms"));*/
		Debug.Log("Duration of the whole program: " + (stopwatchMain.Elapsed.TotalMilliseconds/* * 1000000*/).ToString("0.00 ms"));
	}
}

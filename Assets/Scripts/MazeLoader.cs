using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class MazeLoader : MonoBehaviour {

	// Size of the maze
	private int mazeRows = GlobalVariables.mazeSize;
	private int mazeColumns = GlobalVariables.mazeSize;
    public int size = 10;

    // Strucutre for floor and border wall and an other for common walls if need different textures 
    public GameObject mazeStructure;
	public GameObject wall;

	public GameObject start;
	public GameObject end;
    public GameObject player;
    public GameObject trap;

    private MazeCell[,] mazeCells;

    private int currentRow = 0;
	private int currentColumn = 0;
	private bool runComplete = false;

    private int endRow = -1;
    private int endColumn = -1;

    private bool firstTrap = true;

    void Start () {

		InitializeMaze ();
        HuntAndKill ();
    }

    // Creation of the base of the maze as a grid
	void InitializeMaze () {

		mazeCells = new MazeCell[mazeRows, mazeColumns];

		mazeStructure = Instantiate (mazeStructure, Vector3.zero, Quaternion.identity);
		mazeStructure.name = "Floor";
		mazeStructure.transform.localScale = new Vector3 (mazeRows * size, 0.5f, mazeColumns * 10f);

		mazeStructure = Instantiate (mazeStructure, new Vector3 (0f, 6f, (mazeColumns * size + 1f)/2 ), Quaternion.identity);
		mazeStructure.name = "Top";
		mazeStructure.transform.localScale = new Vector3 (mazeRows * size + 2, 12f, 1f);

		mazeStructure = Instantiate (mazeStructure, new Vector3 (0f , 6f, -((mazeColumns * size + 1f)/2) ), Quaternion.identity);
		mazeStructure.name = "Bottom";
		mazeStructure.transform.localScale = new Vector3 (mazeRows * size + 2, 12f, 1f);

		mazeStructure = Instantiate (mazeStructure, new Vector3 (-((mazeRows * size + 1f)/2) , 6f, 0f ), Quaternion.identity);
		mazeStructure.name = "Left";
		mazeStructure.transform.localScale = new Vector3 (1f, 12f, mazeColumns * size + 2);

		mazeStructure = Instantiate (mazeStructure, new Vector3 ((mazeRows * size + 1f)/2 , 6f, 0f ), Quaternion.identity);
		mazeStructure.name = "Right";
		mazeStructure.transform.localScale = new Vector3 (1f, 12f, mazeColumns * size + 2);

		for (int r = 0; r < mazeRows; r++) {
			for (int c = 0; c < mazeColumns; c++) {

				mazeCells [r, c] = new MazeCell ();

				if (r != (mazeRows - 1)) {
					mazeCells [r, c].WallRight = Instantiate (wall, new Vector3 ((mazeRows * size) / 2 - ((r +1) * size), 3f, (mazeColumns * size) / 2 - size / 2 - (c * size)), Quaternion.Euler(new Vector3(0, 90, 0)));
					mazeCells [r, c].WallRight.name = "right wall " + r + " - " + c;
				}

				if (c != (mazeColumns - 1)) {
					mazeCells [r, c].WallBottom = Instantiate (wall, new Vector3 ((mazeRows * size)/2 - ((r+1) * size - size / 2), 3f, (mazeColumns * size) / 2 -((c +1) * size)), Quaternion.identity);
					mazeCells [r, c].WallBottom.name = "bottom wall " + r + " - " + c;
				}

            }
        }
    }

    /**
     * Instantiate the Start on a random position
     * Use Kill to create the path from the Start to the End
     * Use Hunt to create alternatives paths
     * Place the End
     **/
    private void HuntAndKill () {

        // random currentRow & currentColumn 
        currentRow = RandomNumber.getRandomInRange(mazeRows);
        currentColumn = RandomNumber.getRandomInRange(mazeColumns);

        float startX = (mazeRows * size) / 2 + size / 2 - ((currentRow + 1) * size);
        float startZ = (mazeColumns * size) / 2 - currentColumn * size - size / 2;

        Instantiate(start, new Vector3(startX, 0.33f, startZ), Quaternion.identity);

        Vector3 startPosition = new Vector3(startX, 4f, startZ);
        GlobalVariables.startPosition = startPosition;
        player.transform.position = startPosition;

        mazeCells[currentRow, currentColumn].visited = true;

		while (!runComplete) {

			Kill ();
			Hunt ();
		}

        float endX = (mazeRows * size) / 2 + size / 2 - ((endRow + 1) * size);
        float endZ = (mazeColumns * size) / 2 - endColumn * size - size / 2;

        Instantiate (end, new Vector3(endX, 2.7f, endZ), Quaternion.identity);
    }

    // Create the path by destroying wall in a random direction
	private void Kill () {
		while (IsAvailableDirection (currentRow, currentColumn)) {

			int direction = RandomNumber.getDirection ();

            if (direction == 1 && CellAvailable (currentRow - 1, currentColumn)) {
				// Up
				DestroyWallAndPlaceTrap (mazeCells [currentRow - 1, currentColumn].WallRight);
				currentRow--;
			} else if (direction == 2 && CellAvailable (currentRow + 1, currentColumn)) {
				// Down
				DestroyWallAndPlaceTrap (mazeCells [currentRow, currentColumn].WallRight);
				currentRow++;
			} else if (direction == 3 && CellAvailable (currentRow, currentColumn +1)) {
				// Right
				DestroyWallAndPlaceTrap (mazeCells [currentRow, currentColumn].WallBottom);
				currentColumn++;
			} else if (direction == 4 && CellAvailable (currentRow, currentColumn -1)) {
				// Left
				DestroyWallAndPlaceTrap (mazeCells [currentRow, currentColumn -1].WallBottom);
				currentColumn--;
			}

			mazeCells [currentRow, currentColumn].visited = true;
		}
        
        if (endRow == -1 && endColumn == -1)
        {
            endRow = currentRow;
            endColumn = currentColumn;
        }
	}

    // Transform the remains of the grid into alternative paths by finding all the unvisited cell for restart the kill methode on a new spot
	private void Hunt () {
		runComplete = true;

		for (int r = 0; r < mazeRows; r++) {
			for (int c = 0; c < mazeColumns; c++) {

                if (!mazeCells [r, c].visited && IsAnAdjacentVisitedCell (r, c)) {
                    runComplete = false;
					currentRow = r;
					currentColumn = c;
					DestroyAdjacentWall (currentRow, currentColumn);
					mazeCells [currentRow, currentColumn].visited = true;
					return;
				}
			}
		}
	}

    // check if we are on a dead end or not
	private bool IsAvailableDirection (int row, int column) {
		int availableDirection = 0;

		if (row > 0 && !mazeCells [row - 1, column].visited) {
			availableDirection++;
		}
		if (row < mazeRows -1 && !mazeCells [row + 1, column].visited) {
			availableDirection++;
		}
		if (column > 0 && !mazeCells [row, column -1].visited) {
			availableDirection++;
		}
		if (column < mazeColumns -1 && !mazeCells [row, column  +1].visited) {
			availableDirection++;
		}

		return availableDirection > 0;
	}

    // Call by Kill to destroy the walls along the way and place trap instead 
	private void DestroyWallAndPlaceTrap (GameObject wall) {
        GameObject clone;

        if (wall != null) {
            if (firstTrap)
            {
                firstTrap = false;
            } else
            {
                if (RandomNumber.getRandomInRange (3) != 2)
                {

                    float x;
                    float z;

                    if (RandomNumber.getRandomInRange (1) == 0)
                    {
                        x = wall.transform.position.x + (RandomNumber.getRandomInRange (20) / 10);

                        if (RandomNumber.getRandomInRange (1) == 0)
                        {
                            z = wall.transform.position.z + (RandomNumber.getRandomInRange (20) / 10);
                        } else
                        {
                            z = wall.transform.position.z - (RandomNumber.getRandomInRange (20) / 10);
                        }

                    } else
                    {
                        x = wall.transform.position.x - (RandomNumber.getRandomInRange (20) / 10);

                        if (RandomNumber.getRandomInRange (1) == 0)
                        {
                            z = wall.transform.position.z + (RandomNumber.getRandomInRange (20) / 10);
                        } else
                        {
                            z = wall.transform.position.z - (RandomNumber.getRandomInRange (20) / 10);
                        }
                    }

                    clone = Instantiate (trap, new Vector3 (x, -6.7f, z), Quaternion.identity);
                }
            }
            GameObject.Destroy (wall);
		}
	}

    // Check if there is an adjacent visited cell 
	private bool IsAnAdjacentVisitedCell (int row, int column) {
		int visitedCells = 0;

		// look up
		if (row > 0 && mazeCells [row -1, column].visited) {
			visitedCells++;
		}
		// look down
		if (row < (mazeRows -2) && mazeCells [row +1, column].visited) {
			visitedCells++;
		}
		// Look left
		if (column > 0 && mazeCells [row, column -1].visited) {
			visitedCells++;
		}
		// Look right
		if (column < (mazeColumns -2) && mazeCells [row, column +1].visited) {
			visitedCells++;
		}

		return visitedCells > 0;
	}

    // check if the current cell is visited or not
	public bool CellAvailable (int row, int column) {
		if (row >= 0 && row < mazeRows && column >= 0 && column < mazeColumns && !mazeCells [row, column].visited) {
			return true;
		} else {
			return false;
		}
	}

    // call by Hunt to destroy the wall between the path create by Kill and the new unvisited cell find by Hunt
	private void DestroyAdjacentWall (int row, int column) {

		bool isWalldestroyed = false;

		while (!isWalldestroyed) {

			int direction = RandomNumber.getDirection ();

			if (direction == 1 && row > 0 && mazeCells [row - 1, column].visited) {
				DestroyWallAndPlaceTrap (mazeCells [row - 1, column].WallRight);
				isWalldestroyed = true;
			} 
			else if (direction == 2 && row < (mazeRows -2) && mazeCells [row + 1, column].visited) {
				DestroyWallAndPlaceTrap (mazeCells [row, column].WallRight);
				isWalldestroyed = true;
			} 
			else if (direction == 3 && column > 0 && mazeCells [row, column -1].visited) {
				DestroyWallAndPlaceTrap (mazeCells [row, column -1].WallBottom);
				isWalldestroyed = true;
			} 
			else if (direction == 4 && column < (mazeColumns -2) && mazeCells [row, column +1].visited) {
				DestroyWallAndPlaceTrap (mazeCells [row, column].WallBottom);
				isWalldestroyed = true;
			} 
		}
	}

}

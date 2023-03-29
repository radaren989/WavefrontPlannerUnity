using System.Collections.Generic;

public static class PathFinding
{
    public static void CalculateMapDigits(
        int[,] digitMap,
        int agentX,
        int agentZ,
        int destinationX,
        int destinationZ
    )
    {
        Queue<Location> digitMapQueue = new Queue<Location>();
        digitMapQueue.Enqueue(new Location(destinationX, destinationZ));
        while (digitMapQueue.Count > 0)
        {
            Location currGridCell = digitMapQueue.Dequeue();
            //Left
            if (
                currGridCell.x - 1 >= 0
                && digitMap[currGridCell.x - 1, currGridCell.z] != 1
                && (
                    digitMap[currGridCell.x, currGridCell.z] + 1
                        < digitMap[currGridCell.x - 1, currGridCell.z]
                    || digitMap[currGridCell.x - 1, currGridCell.z] == 0
                    || digitMap[currGridCell.x - 1, currGridCell.z] == -1
                )
            )
            {
                digitMap[currGridCell.x - 1, currGridCell.z] =
                    digitMap[currGridCell.x, currGridCell.z] + 1;
                digitMapQueue.Enqueue(new Location(currGridCell.x - 1, currGridCell.z));
            }
            //Down
            if (
                currGridCell.z - 1 >= 0
                && digitMap[currGridCell.x, currGridCell.z - 1] != 1
                && (
                    digitMap[currGridCell.x, currGridCell.z] + 1
                        < digitMap[currGridCell.x, currGridCell.z - 1]
                    || digitMap[currGridCell.x, currGridCell.z - 1] == 0
                    || digitMap[currGridCell.x, currGridCell.z - 1] == -1
                )
            )
            {
                digitMap[currGridCell.x, currGridCell.z - 1] =
                    digitMap[currGridCell.x, currGridCell.z] + 1;
                digitMapQueue.Enqueue(new Location(currGridCell.x, currGridCell.z - 1));
            }
            //Right
            if (
                currGridCell.x + 1 < digitMap.GetLength(0)
                && digitMap[currGridCell.x + 1, currGridCell.z] != 1
                && (
                    digitMap[currGridCell.x, currGridCell.z] + 1
                        < digitMap[currGridCell.x + 1, currGridCell.z]
                    || digitMap[currGridCell.x + 1, currGridCell.z] == 0
                    || digitMap[currGridCell.x + 1, currGridCell.z] == -1
                )
            )
            {
                digitMap[currGridCell.x + 1, currGridCell.z] =
                    digitMap[currGridCell.x, currGridCell.z] + 1;
                digitMapQueue.Enqueue(new Location(currGridCell.x + 1, currGridCell.z));
            }
            //Up
            if (
                currGridCell.z + 1 < digitMap.GetLength(1)
                && digitMap[currGridCell.x, currGridCell.z + 1] != 1
                && (
                    digitMap[currGridCell.x, currGridCell.z] + 1
                        < digitMap[currGridCell.x, currGridCell.z + 1]
                    || digitMap[currGridCell.x, currGridCell.z + 1] == 0
                    || digitMap[currGridCell.x, currGridCell.z + 1] == -1
                )
            )
            {
                digitMap[currGridCell.x, currGridCell.z + 1] =
                    digitMap[currGridCell.x, currGridCell.z] + 1;
                digitMapQueue.Enqueue(new Location(currGridCell.x, currGridCell.z + 1));
            }
        }
    }

    public static Location[] GetPath(
        int[,] digitMap,
        int agentX,
        int agentZ,
        int destinationX,
        int destinationZ
    )
    {
        int pathLength = digitMap[agentX, agentZ] - digitMap[destinationX, destinationZ];
        Location[] path = new Location[pathLength];
        int pathIndex = 0;
        Location cursor = new Location(agentX, agentZ);
        while (cursor.x != destinationX || cursor.z != destinationZ)
        {
            cursor = GetSurroundingCellLocation(cursor, digitMap);
            path[pathIndex] = cursor;
            pathIndex++;
        }
        return path;
    }

    private static Location GetSurroundingCellLocation(Location cursor, int[,] digitMap)
    {
        Location next = new Location(0, 0);
        int value = 9999;
        //checks up
        if (
            cursor.z + 1 < digitMap.GetLength(1)
            && digitMap[cursor.x, cursor.z + 1] != 1
            && digitMap[cursor.x, cursor.z] > digitMap[cursor.x, cursor.z + 1]
        )
        {
            next = new Location(cursor.x, cursor.z + 1);
            value = digitMap[cursor.x, cursor.z + 1];
        }
        //checks right
        if (
            cursor.x + 1 < digitMap.GetLength(0)
            && digitMap[cursor.x + 1, cursor.z] != 1
            && digitMap[cursor.x, cursor.z] > digitMap[cursor.x + 1, cursor.z]
            && value > digitMap[cursor.x + 1, cursor.z]
        )
        {
            next = new Location(cursor.x + 1, cursor.z);
            value = digitMap[cursor.x + 1, cursor.z];
        }
        //checks down
        if (
            cursor.z - 1 >= 0
            && digitMap[cursor.x, cursor.z - 1] != 1
            && digitMap[cursor.x, cursor.z] > digitMap[cursor.x, cursor.z - 1]
            && value > digitMap[cursor.x, cursor.z - 1]
        )
        {
            next = new Location(cursor.x, cursor.z - 1);
            value = digitMap[cursor.x, cursor.z - 1];
        }
        //checks left
        if (
            cursor.x - 1 >= 0
            && digitMap[cursor.x - 1, cursor.z] != 1
            && digitMap[cursor.x, cursor.z] > digitMap[cursor.x - 1, cursor.z]
            && value > digitMap[cursor.x - 1, cursor.z]
        )
        {
            next = new Location(cursor.x - 1, cursor.z);
        }
        return next;
    }
}

public class Map
{
    public int[,] digitMap;
    public Location agentLocation;
    public Location destinationLocation;

    public Map(int width, int height, int agentX, int agentZ, int destinationX, int destinationZ)
    {
        digitMap = new int[width, height];
        agentLocation = new Location(agentX, agentZ);
        destinationLocation = new Location(destinationX, destinationZ);
        digitMap[destinationX, destinationZ] = 2;
        digitMap[agentX, agentZ] = -1;
    }
}

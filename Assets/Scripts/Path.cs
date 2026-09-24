using UnityEngine;

public class Path : MonoBehaviour
{

    public GameObject[] waypoints;

    public Vector3 getPosition(int index)
    {
        return waypoints[index].transform.position;
    }

    private void OnDrawGizmos()
    {
        if (waypoints.Length > 0)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (i < waypoints.Length - 1)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
                }
            }
        }
    }

}

using UnityEngine;

public class HelixCarFollower : MonoBehaviour
{
    public Transform directionArrow; // Assuming this is your AI car's transform
    public float followDistance = 5f;
    //public float stopDistance = 5f;
    public float speed = 5f;
     float rotationSpeed = 5f;

    public GameObject nearestObject;
    private float minDistance = float.MaxValue;

    [SerializeField] PlayerID playerID;

  
    void FixedUpdate()
    {

        if (SocketPlayerManager.Instance && SocketPlayerManager.Instance.MyPlayer && SocketPlayerManager.Instance.MyPlayer.helixPlayerInfo.isRoomHostUser)
        {

            // Find the nearest object on every frame
            if (nearestObject == null && playerID.gunControllerinAi.currentEnemy)
                nearestObject = playerID.gunControllerinAi.currentEnemy.gameObject;

            if (nearestObject == null)
            {
                nearestObject = FindNearestObject();
            }
            else if (!nearestObject.activeSelf)
            {
                nearestObject = null;
            }




            // Check if there is a nearest object
            if (nearestObject != null)
            {
                // Calculate the direction to the nearest object
                Vector3 direction = (nearestObject.transform.position - transform.position).normalized;

                // Rotate towards the nearest object
                //Quaternion toRotation = Quaternion.LookRotation(new Vector3(direction.x,transform.rotation.y ,direction.z));
                //if (direction != Vector3.zero)
                //{
                    Quaternion toRotation = Quaternion.LookRotation(direction);
                //Debug.Log("::::>>>>>>>>>>>" + toRotation.ToString());


                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                //transform.rotation = toRotation;

                // Move towards the nearest object if within follow distance
                float distanceToNearestObject = Vector3.Distance(transform.position, nearestObject.transform.position);
                    if (distanceToNearestObject > playerID.gunControllerinAi.Range)
                    {
                        //  transform.Translate(Vector3.forward * speed /** Time.deltaTime*/);
                        if (playerID.gunControllerinAi.currentEnemy && playerID.gunControllerinAi.currentEnemy == nearestObject)
                            playerID.gunControllerinAi.currentEnemy = null;
                        nearestObject = null;
                    }
                //}
            }
            //else
            //{
            //    nearestObject = FindNearestObject();
            //}
        }
    }

    //GameObject nearObject = null;
    GameObject FindNearestObject()
    {
        //GameObject[] generatedMapObjects = GameObject.FindGameObjectsWithTag("Player"); // Replace "YourObjectTag" with the tag of your objects

        //Debug.Log("aaa");

        GameObject nearObject = null;
        minDistance = float.MaxValue;

        ////GameObject currentObj = null;

        //if (playerID.currentGunController != null)
        //{
        foreach (PlayerID obj in SocketPlayerManager.Instance.allPlayers.Values)
        {
            //currentObj = obj;

            if (obj != null && obj.gameObject.activeSelf && playerID != obj)
            {
                float distance = Vector3.Distance(directionArrow.position, obj.transform.position);
                if ((playerID.helixPlayerInfo.teamA && !obj.helixPlayerInfo.teamA) || (!playerID.helixPlayerInfo.teamA && obj.helixPlayerInfo.teamA))
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearObject = obj.gameObject;
                    }
                }
                else if ((playerID.helixPlayerInfo.teamB && !obj.helixPlayerInfo.teamB) || (!playerID.helixPlayerInfo.teamB && obj.helixPlayerInfo.teamB))
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearObject = obj.gameObject;
                    }
                }
            }
        }
        //}
        //else
        //{
        //    foreach (PowerUp obj in GameplayManager.Instance.allPowerUps.Values)
        //    {
        //        if (obj.gameObject.activeSelf && obj.isSecondaryPowerUp == false)
        //        {
        //            float distance = Vector3.Distance(directionArrow.position, obj.transform.position);

        //            if (distance < minDistance)
        //            {
        //                minDistance = distance;
        //                nearObject = obj.gameObject;
        //            }
        //        }
        //    }
        //}



        return nearObject;

    }
}



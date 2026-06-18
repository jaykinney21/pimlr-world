using UnityEngine;

namespace CarControllerwithShooting
{
    public class MissileScript : MonoBehaviour
    {
        public GameObject explosionPrefab;
        int DamagePower = 25;
        public Transform particle_following;
        //public bool isEnemyMissile = false;
        public Rigidbody rb;
        public MissileControl missileControl;
        internal bool teamA, teamB;
        public BoxCollider boxCollider;

        internal string shooterId;
        internal string missileId;



        private void Start()
        {
            if (missileControl == null)
                missileControl = GetComponent<MissileControl>();
        }

        private void FixedUpdate()
        {
            if (!string.IsNullOrEmpty(shooterId) && shooterId == SocketPlayerManager.Instance.helixPlayerInfo.userID)
            {

                //Debug.Log("::::::::::::" + shooterId + ":::::::::::::::::" + SocketPlayerManager.Instance.helixPlayerInfo.userID);
                SocketNetworkManager.Instance.EmitFireUpdate("missile", this.gameObject.transform, 0, shooterId, missileId, false);
            }
        }
        private void Update()
        {
            //if (!string.IsNullOrEmpty(shooterId) && shooterId == SocketPlayerManager.Instance.helixPlayerInfo.userID)
            //    SocketNetworkManager.Instance.EmitFireUpdate("missile", this.gameObject.transform, 0, shooterId, missileId, false);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(shooterId) && other.name != "Wheel_F_L")
            {
                //Debug.Log("???????////////////////::::::"+other.name);
                if (other.CompareTag("Enemy"))
                {
                    PlayerID playerIDComponent = other.GetComponent<PlayerID>();

                    if (playerIDComponent != null && playerIDComponent.playerID != SocketNetworkManager.Instance.playerID && (playerIDComponent.helixPlayerInfo.teamA != teamA || playerIDComponent.helixPlayerInfo.teamB != teamB))
                    {
                        SocketNetworkManager.Instance.EmitHealth(shooterId, playerIDComponent.playerID, DamagePower.ToString());
                    }
                }
                if (other.CompareTag("Player"))
                {
                    PlayerID playerIDComponent = other.GetComponent<PlayerID>();

                    if (playerIDComponent != null && playerIDComponent.playerID != SocketNetworkManager.Instance.playerID && (playerIDComponent.helixPlayerInfo.teamA != teamA || playerIDComponent.helixPlayerInfo.teamB != teamB))
                    {
                        SocketNetworkManager.Instance.EmitHealth(shooterId, playerIDComponent.playerID, DamagePower.ToString());
                    }
                }

                if (other.gameObject.CompareTag("OilBarrel")/*&&f3DFXController && f3DFXController.playerid*/)
                {
                    //Debug.Log("Other Gameobject Name::" + other.gameObject.name);
                    other.gameObject.GetComponent<OilBarrel>().Blast(shooterId);
                }

                //Debug.Log(other.gameObject.name + "&&&&&&&&&||||||||||||||");
                ExplodeAndDestroy();
            }
        }

        internal void ExplodeAndDestroy()
        {


            if (!string.IsNullOrEmpty(shooterId))
            {
                if (shooterId == SocketPlayerManager.Instance.helixPlayerInfo.userID)
                {
                    SocketNetworkManager.Instance.EmitFireUpdate("missile", this.gameObject.transform, 0, shooterId, missileId, true);


                }
                if (GameplayManager.Instance.allmissiles.ContainsKey(missileId))
                {
                    GameplayManager.Instance.allmissiles.Remove(missileId);
                }


                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }


        public void UpdatePosRot(Vector3 pos, Vector3 rot)
        {
            //Debug.Log("::::::%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%::::::::"+ rot);
            this.transform.position = pos;
            this.transform.eulerAngles = rot;
        }

    }
}

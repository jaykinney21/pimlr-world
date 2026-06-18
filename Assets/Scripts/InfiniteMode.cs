using JUTPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteMode : Singleton<InfiniteMode>
{







    [SerializeField] internal int currentWave = 1;
   [SerializeField] private int normalEnemyCount = 5;
    [SerializeField] private int bossCount = 0;

    public JUHealth normalEnemyPrefab, bossEnemyPrefab;

    public Transform normalEnemiesHolder;
    public Transform bossEnemiesHolder;


    public Vector3 SpawnArea;


   public int currentEnemyCount;

    void Start()
    {
        StartCoroutine(SpawnWave());
        SceneManagerScript.Instance.goalPanel.gameObject.SetActive(false);
    }


    public void CompleteWave()
    {
        CoinManager.Instance.SetCoins(CoinManager.Instance.GetCoins() + 50 * currentWave);
        currentEnemyCount = 0;
        currentWave++;
        if (currentWave % 2 == 1)
            normalEnemyCount += 5;
        else
            bossCount += 1;

        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(3.5f);
        currentEnemyCount = 0;
        Debug.Log(currentWave % 2);
        yield return StartCoroutine(GameExecutionManager.Instance.WaitForscreenfadeOut("0/" + (currentWave % 2 == 1 ? normalEnemyCount : bossCount)));
     
        SceneManagerScript.Instance.goalPanel.SetCurrentKillInfo(currentEnemyCount.ToString() + "/" + (currentWave % 2 == 1 ? normalEnemyCount : bossCount));
       
        Vector3 randomPosOnArea = transform.position;
        randomPosOnArea.x += Random.Range(-SpawnArea.x, SpawnArea.x);
        randomPosOnArea.y += Random.Range(-SpawnArea.y, SpawnArea.y);
        randomPosOnArea.z += Random.Range(-SpawnArea.z, SpawnArea.z);

        if (currentWave % 2 == 1)
        {

            for (int i = 0; i < normalEnemyCount; i++)
            {
                randomPosOnArea = transform.position;
                randomPosOnArea.x += Random.Range(-SpawnArea.x, SpawnArea.x);
                randomPosOnArea.y += Random.Range(-SpawnArea.y, SpawnArea.y);
                randomPosOnArea.z += Random.Range(-SpawnArea.z, SpawnArea.z);
                JUHealth enemy;

                enemy = Instantiate(normalEnemyPrefab, normalEnemiesHolder);


                enemy.transform.position = randomPosOnArea;
                enemy.transform.rotation = Quaternion.Euler(0, Random.Range(-360, 360), 0);

                enemy.Health = enemy.MaxHealth;

                enemy.gameObject.SetActive(true);

                yield return new WaitForSeconds(0.3f);
            }
        }
        else
        {

            for (int i = 0; i < bossCount; i++)
            {
                randomPosOnArea = transform.position;
                randomPosOnArea.x += Random.Range(-SpawnArea.x, SpawnArea.x);
                randomPosOnArea.y += Random.Range(-SpawnArea.y, SpawnArea.y);
                randomPosOnArea.z += Random.Range(-SpawnArea.z, SpawnArea.z);
                JUHealth enemy;

                enemy = Instantiate(bossEnemyPrefab, bossEnemiesHolder);


                enemy.transform.position = randomPosOnArea;
                enemy.transform.rotation = Quaternion.Euler(0, Random.Range(-360, 360), 0);

                enemy.Health = enemy.MaxHealth;

                enemy.gameObject.SetActive(true);

            }
        }
    }


    public void OnKillEnemy()
    {
        currentEnemyCount++;

        SceneManagerScript.Instance.goalPanel.SetCurrentKillInfo(currentEnemyCount.ToString() + "/" + (currentWave % 2 == 1 ? normalEnemyCount : bossCount));
        if (currentWave % 2 == 1)
        {
            if (normalEnemyCount <= currentEnemyCount)
            {
                CompleteWave();
                return;
            }
        }
        else
        {
            if (bossCount <= currentEnemyCount)
            {
                CompleteWave();
            }
        }
    }

}

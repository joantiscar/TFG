using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;


public class Game : MonoBehaviour
{

    public static int EASY_MODE = 0;
    public static int NORMAL_MODE = 1;
    public static int HARD_MODE = 2;

    public GameObject[] planesEasy;
    public GameObject[] planesNormal;
    public GameObject[] planesHard;

    public int difficulty = EASY_MODE;

    public GameObject prefavBala;
    public Transform player;

    public float TIME_ROUND = 60;
    public float TIME_BETWEEN_ROUNDS = 15;

    public int round;
    public float time;
    public int score;
    public bool betweenRounds;
    public float interval;
    private bool gameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        difficulty = Singleton.GetInstance().GetDifficulty();
        round = 0;
        time = TIME_BETWEEN_ROUNDS;
        score = 0;
        betweenRounds = true;
        interval = 2;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if(time <= 0)
        {
            if(betweenRounds)
            {
                round++;
                time = TIME_ROUND;
                InvokeRepeating("Shoot", 0, interval);
            }
            else
            {
                CancelInvoke("Shoot");
                time = TIME_BETWEEN_ROUNDS;
                interval *= 0.95f;
            }
            betweenRounds = !betweenRounds;
        }
    }

    void Shoot()
    {
        List<GameObject> planes = SelectPlanes();
        int randomPlane = Random.Range(0, planes.ToArray().Length);
        Vector3 pos = GetARandomTreePos(planes[randomPlane]);
        GameObject bala = Instantiate(prefavBala, pos, Quaternion.identity);

        Vector3 randTran = player.position;
        randTran += new Vector3(0, Random.Range(-1.0f, 0.5f), 0);
        bala.transform.LookAt(randTran);
        bala.transform.Rotate(new Vector3(90, 0, 0));
        bala.GetComponent<Bala>().target = randTran;
        bala.GetComponent<Bala>().game = this;
    }

    private List<GameObject> SelectPlanes()
    {
        List<GameObject> planes = new List<GameObject>(planesEasy);
        if (difficulty.Equals(NORMAL_MODE))
            planes.AddRange(planesNormal);
        else if (difficulty.Equals(HARD_MODE))
        {
            planes.AddRange(planesNormal);
            planes.AddRange(planesHard);
        }
        return planes;
    }

    private Vector3 GetARandomTreePos(GameObject plane)
    {
        Mesh planeMesh = plane.GetComponent<MeshFilter>().mesh;
        Bounds bounds = planeMesh.bounds;
        float minX = plane.transform.localScale.x * bounds.size.x * 0.5f;
        float minZ = plane.transform.localScale.z * bounds.size.z * 0.5f;

        float randX = Random.Range(minX, -minX);
        float randZ = randX * Mathf.Sin(Mathf.Deg2Rad * plane.transform.eulerAngles.y);
        randX *= Mathf.Cos(Mathf.Deg2Rad * plane.transform.eulerAngles.y);
        float randY = Random.Range(minZ, -minZ);
        Vector3 newVec = new Vector3(randX + plane.transform.position.x,
            randY + plane.transform.position.y, plane.transform.position.z - randZ);

        return newVec;
    }

    public void AddScore()
    {
        score += ((difficulty + 1) * 100);
    }

    public void GameOver(){
        if (!gameOver){
            gameOver = true;
            CancelInvoke("Shoot");
            Singleton.GetInstance().AddLeaderBoardEntry(Singleton.GetInstance().GetCurrentName(), score, difficulty);
            SceneManager.LoadScene("ScoreScene");
        }
    }

}

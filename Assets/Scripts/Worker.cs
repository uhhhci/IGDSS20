using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    #region Manager References
    JobManager _jobManager; //Reference to the JobManager
    GameManager _gameManager;//Reference to the GameManager
    #endregion

    public float _age; // The age of this worker
    public float _happiness; // The happiness of this worker

    public Job _job; //The job this worker is assigned to
    public HousingBuilding _home; //The house this worker is assigned to

    public float _agingInterval = 15; //The time in seconds it takes for a worker to age by one year
    private float _agingProgress; //The counter that needs to be incrementally increased during a production cycle
    private bool _becameOfAge = false;
    private bool _retired = false;

    public Tile _navigationTarget;
    public NavigationManager.Map _currentMap;
    public bool _currentlyCommuting;
    public bool _currentlyCommutingHome;
    public bool _currentlyCommutingToWork;
    private float _speed = 10.0f;

    public Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Age();
        Move();
    }

    private void Age()
    {
        _agingProgress += Time.deltaTime;
        if (_agingProgress > _agingInterval)
        {
            _agingProgress = 0;
            _age++;
            ConsumeResourcesAndCalculateHappiness();
            ChanceOfDeath();
        }

        if (_age > 14 && !_becameOfAge)
        {
            BecomeOfAge();
        }

        if (_age > 64 && !_retired)
        {
            Retire();
        }

        if (_age > 100)
        {
            Die();
        }
    }

    private void Move()
    {
        if (_currentlyCommuting)
        {
            MoveToNavigationTarget();
        }
    }

    private void ConsumeResourcesAndCalculateHappiness()
    {
        bool fish = _gameManager.RemoveResourceFromWarehouse(GameManager.ResourceTypes.Fish, 2);
        bool clothes = _gameManager.RemoveResourceFromWarehouse(GameManager.ResourceTypes.Clothes, 2);
        bool schnapps = _gameManager.RemoveResourceFromWarehouse(GameManager.ResourceTypes.Schnapps, 2);
        bool job = _job != null;

        float happinessTarget = (fish ? 25 : 0) + (clothes ? 25 : 0) + (schnapps ? 25 : 0) + (job ? 25 : 10);
        _happiness = (happinessTarget + _happiness) / 2;
    }

    private void ChanceOfDeath()
    {
        float chanceOfDeath = _age * 0.1f * (100f / _happiness);

        float rng = Random.Range(0f, 100f);

        if (rng < chanceOfDeath)
        {
            Die();
        }
    }

    public void AssignToJob(Job job)
    {
        _job = job;
        CommuteToWork();
    }

    public void AssignToHome(HousingBuilding home)
    {
        _home = home;
        _navigationTarget = _home._tile;
    }

    public void BeBorn()
    {
        _age = 0;
        gameObject.name = "Child";
    }

    public void BecomeOfAge()
    {
        _becameOfAge = true;

        _jobManager = JobManager.Instance;
        _jobManager.RegisterWorker(this);
        gameObject.name = "Worker";
    }

    private void Retire()
    {
        _retired = true;
        _jobManager.RemoveWorker(this);
        gameObject.name = "Retiree";
    }

    private void Die()
    {
        _jobManager.RemoveWorker(this);
        _home.RemoveWorker(this);
        GameManager.Instance.RemoveWorker(this);
        print("A " + gameObject.name + " has died");

        _currentlyCommuting = false;
        _currentlyCommutingHome = false;
        _currentlyCommutingToWork = false;

        Destroy(this.gameObject, 1f);
    }

    private void CommuteToWork()
    {
        _currentMap = _job._building._map;
        _currentlyCommuting = true;
        _currentlyCommutingToWork = true;
    }

    private void CommuteHome()
    {
        _currentMap = _home._map;
        _currentlyCommuting = true;
        _currentlyCommutingHome = true;
    }

    IEnumerator ArrivedAtWork(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        CommuteHome();
    }

    IEnumerator ArrivedAtHome(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        CommuteToWork();
    }

    private void SetNextNavigationTarget(Tile currentTile)
    {
        List<Tile> neighbors = currentTile._neighborTiles;
        Tile bestTile = currentTile;
        int bestValue = 1000000;
        for (int i = 0; i < neighbors.Count; i++)
        {
            int currentValue = _currentMap.GetValue(neighbors[i]._coordinateWidth, neighbors[i]._coordinateHeight);
            if (currentValue < bestValue)
            {
                bestTile = neighbors[i];
                bestValue = currentValue;
            }
        }
        _navigationTarget = bestTile;
    }

    private void MoveToNavigationTarget()
    {
        //_animator.Play();
        float step = _speed * Time.deltaTime; // calculate distance to move
        transform.LookAt(_navigationTarget.transform);
        transform.position = Vector3.MoveTowards(transform.position, _navigationTarget.transform.position, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, _navigationTarget.transform.position) < 0.001f)
        {
            ArrivedAtNavigationTarget();
        }
    }

    private void ArrivedAtNavigationTarget()
    {
        if (_currentlyCommutingHome && _navigationTarget == _home._tile)
        {
            _currentlyCommuting = false;
            _currentlyCommutingHome = false;
            _currentlyCommutingToWork = false;
            StartCoroutine(ArrivedAtHome(20 + Random.Range(0f, 2f)));
        }
        else if (_currentlyCommutingToWork && _navigationTarget == _job._building._tile)
        {
            _currentlyCommuting = false;
            _currentlyCommutingHome = false;
            _currentlyCommutingToWork = false;
            StartCoroutine(ArrivedAtWork(20 + Random.Range(0f, 2f)));
        }
        else
        {
            SetNextNavigationTarget(_navigationTarget);
        }
    }
}

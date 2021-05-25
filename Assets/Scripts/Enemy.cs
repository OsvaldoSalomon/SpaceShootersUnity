using UnityEngine;
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;

    private Player _player;

    private Animator _anim;
    
    [SerializeField]
    private GameObject _laserPrefab;
    
    // [SerializeField] 
    // private GameObject _explosionPrefab;
    private BoxCollider2D _boxCollider2D;


    private float _fireRate = 3.0f;
    private float _canFire = -1;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        if (_player == null)
        {
            Debug.LogError("The player is null");
        }

        if (_boxCollider2D == null)
        {
            Debug.LogError("Collider2D for Enemy is NULL.");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The animator is null");
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 6f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
                Destroy(enemyLaser, 1.0f);
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Hit: " + other.transform.name);

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            // var explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _boxCollider2D.enabled = false;
            // Destroy(explosion, 2.8f);
            Destroy(this.gameObject, 0.5f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }

            // var explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _boxCollider2D.enabled = false;
            // Destroy(explosion, 2.8f);
            Destroy(this.gameObject, 0.5f);
        }
    }
}
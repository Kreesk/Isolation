using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Animator _enemy;
    [SerializeField] Transform _target;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] AudioSource _source;
    [SerializeField] float distance = 2f;
    

    private bool _applyWalk = false;
    private bool _setMovement = false;
    private void ApplyMovement(bool t)
    {
        _enemy.SetBool("isWalk", t);
        _applyWalk = t;
        _agent.isStopped = !t;
    }

    void OnEnable() => _setMovement = true; 
    void OnDisable() => _setMovement = false;

    private void Distanation()
    {
        float dist = (_agent.transform.position - _target.transform.position).magnitude;


        if(dist <= distance && _applyWalk)
        {
            ApplyMovement(false);
            gameObject.SetActive(false);
        }

        if(dist > distance && !_applyWalk)
        {
            Debug.Log("check");
            ApplyMovement(true);
        }
        if(_applyWalk)
        _agent.SetDestination(_target.position);
    }

    private void SetSound()
    {
        if(_source.isPlaying)return;

        _source.Play();
    }

    void Update()
    {
        if(!_setMovement)return;
        Distanation();
        SetSound();
    }
}

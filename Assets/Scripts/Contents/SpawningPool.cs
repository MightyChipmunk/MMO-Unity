using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0;
    int _reserveCount = 0; // �ڷ�ƾ�� ������ �� ���� ����� �ڷ�ƾ�� ����� �Ǵ��ϱ� ����
    [SerializeField] 
    int _keepMonsterCount = 0;
    [SerializeField]
    Vector3 _spawnPos;
    [SerializeField]
    float _spawnRadius = 15.0f;
    [SerializeField]
    float _spawnTime = 5.0f;

    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }

    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, "DogPolyart");
        // _monsterCount�� �� �Լ����� �ø��� �ʾƵ� GameManagerEx���� Spawn�Լ��� ����� �� Invoke�� _monsterCount�� �÷��ش�.
        NavMeshAgent nma =  obj.GetOrAddComponent<NavMeshAgent>();

        Vector3 randPos;

        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);
            randDir.y = 0;
            randPos = _spawnPos + randDir;
            // ������ ���⺤�͸� �����Ѵ�.
            // ���ʹ� ��鿡 ��ġ�ؾ� �ϱ� ������ ������ ���������� y��ǥ�� 0���� ������ش�.

            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randPos, path))
                break;
            // ������ �������Ϳ� �� �� �ֳ� ���� ���
        }

        obj.transform.position = randPos;
        _reserveCount--;
    }
}

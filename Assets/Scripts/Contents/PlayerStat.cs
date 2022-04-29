using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp; 
    [SerializeField]
    protected int _gold;

    public int Exp 
    { 
        get { return _exp; } 
        set 
        { 
            _exp = value;
            int level = Level;
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                    break; // 다음 레벨의 스탯을 불러온다. 만약 다음 레벨의 스탯이 없다면 break
                if (_exp < stat.totalExp)
                    break; // 다음 레벨의 스탯을 불러온다. 만약 다음 레벨의 totalExp보다 현재 exp가 낮다면 break
                level++;   // 다음 레벨의 totalExp보다 현재 exp가 더 높다면 레벨을 올린다.
            }              // 이를 더는 레벨업을 못하는 상황이 될 때까지 반복한다.

            if (level != Level) 
            {
                Debug.Log("Level Up!");
                Level = level;  // 위 반복문을 통해 레벨이 변화한 상태라면 다시 프로퍼티를 사용해 레벨을 저장한다.
                SetStat(level); // 변화된 레벨에 스탯을 맞춘다.
            }
        } 
    }

    public int Gold { get { return _gold; } set { _gold = value; } }
    private void Start()
    {
        _level = 1;
        _exp = 0;
        _defense = 5;
        _moveSpeed = 5.0f;
        _gold = 0;

        SetStat(_level);
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[level];
        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}

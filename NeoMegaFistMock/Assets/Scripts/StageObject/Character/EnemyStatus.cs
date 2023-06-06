using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(1)]
public class EnemyStatus : MonoBehaviour
{
    [SerializeField] private Character target;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider stunGaugeSlider;

    private void Start()
    {
        target.OnChangeHP += ChangeHP;
        target.OnChangeStunGauge += ChangeStunGauge;
        hpSlider.maxValue = target.MaxHP;
        hpSlider.value = target.HP;
        stunGaugeSlider.maxValue = target.MaxStunGauge;
        stunGaugeSlider.value = target.StunGauge;
    }

    private void ChangeHP(int hp)
    {
        hpSlider.value = hp;
    }

    private void ChangeStunGauge(int stunGauge)
    {
        stunGaugeSlider.value = stunGauge;
    }
}

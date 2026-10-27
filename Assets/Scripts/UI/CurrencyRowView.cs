using Core;
using Economics;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CurrencyRowView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text amount;

        public void Set(Currency currency, int count) {
            amount.text = count.ToString();

            CurrencySettings settings = AssetProvider.Instance.GetCurrency(currency);
            icon.enabled = true;
            icon.sprite = settings.icon;
        }
    }
}

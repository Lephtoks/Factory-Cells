using Core;
using Economics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cloud
{
    public class UICloudInfo : MonoSingleton<UICloudInfo>
    {
        [SerializeField] private Image[] icons;

        private int index;
        public void ResetIcons() {
            foreach (var icon in icons) {
                icon.gameObject.SetActive(false);
            }
            index = 0;
        }

        public bool TryAddIcon(ItemStack stack) {
            if (stack.IsEmpty()) return false;
            
            if (index < icons.Length) {
                icons[index].gameObject.SetActive(true);
                icons[index].sprite = AssetProvider.Instance.GetCurrency(stack.CurrencyType).icon;
                index++;
                return true;
            }
            return false;
        }
    }
}

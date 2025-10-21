
using UnityEngine;
using TMPro;

namespace UI
{
    public class HUDController : MonoBehaviour
    {
        public static HUDController Instance;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI fragmentsText;
        public TextMeshProUGUI characterText;

        void Awake()
        {
            Instance = this;
        }

        public void UpdateHealth(int current, int max)
        {
            if (healthText != null)
                healthText.text = $"Vida: {current}/{max}";
        }

        public void UpdateFragments(int have, int need)
        {
            if (fragmentsText != null)
                fragmentsText.text = $"Fragmentos: {have}/{need}";
        }

        public void UpdateCharacter(string name)
        {
            if (characterText != null)
                characterText.text = name;
        }
    }
}

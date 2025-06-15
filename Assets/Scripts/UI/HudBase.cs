using UnityEngine;

public class HudBase : MonoBehaviour
{
   public void Show()
   {
      gameObject.SetActive(true);
   }

   public void Hide()
   {
      gameObject.SetActive(false);
   }
}
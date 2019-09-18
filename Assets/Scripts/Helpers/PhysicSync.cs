using UnityEngine;

namespace zs.Helpers
{
    public class PhysicSync : MonoBehaviour
    {
        private bool _improvedCharacterMovement;

        void Start()
        {
            if (PlayerPrefs.HasKey("ImprovedCharacterMovement"))
            {
                _improvedCharacterMovement = PlayerPrefs.GetInt("ImprovedCharacterMovement") == 1;
            }

            if (!_improvedCharacterMovement)
            {
                Physics2D.autoSimulation = true;
            }
            else
            {
                // Hint: Physics2D.Simulate() will be called once at the end of the Update-Cycle in the PhysicSync Component.
                Physics2D.autoSimulation = false;
            }

            Debug.Log("Physics2D.autoSimulation: " + Physics2D.autoSimulation);
        }

        void Update()
        {
            if (_improvedCharacterMovement)
            {
                // Hint: Physics2D.Simulate() will be called once at the end of the Update-Cycle.
                //
                // Hint: The PhysicSync Update() method is the last Update() method called during a frame, enforced through
                //       the "Script Execution Order" of the project settings. 

                Physics2D.Simulate(Time.deltaTime);
            }
        }
    }
}

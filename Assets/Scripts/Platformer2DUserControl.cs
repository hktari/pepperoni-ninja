using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private RhytmManager m_RhytmManager;
        public GameObject Shuriken;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            m_RhytmManager = GameObject.Find("RhytmManager").GetComponent<RhytmManager>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                if (m_Jump && !m_RhytmManager.TryPerformAction())
                {
                    m_Jump = false;
                }
                if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                {
                Instantiate(Shuriken, this.transform.position, this.transform.rotation);

                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Debug.Log("restart");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }
    }
}

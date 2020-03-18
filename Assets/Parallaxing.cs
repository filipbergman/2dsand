using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {
    public Transform[] backgrounds;     // Array of all back- and foregrounds to be parallaxed.
    private float[] parallaxScales;     // Proportion of the cameras movement to move the backgrounds by.
    public float smoothing = 1f;        // How smooth the parallax is going to be. Must be above 0.

    private Transform cam;              // Reference to the main cameras transform.
    private Vector3 previousCamPos;     // The position of the camera in the previous frame.

    // Called before Start() great for references
    void Awake () {
        // Set up the camera reference
        cam = Camera.main.transform;
    }


    // Start is called before the first frame update
    void Start() {
        // The previous fram had the current frames camera position
        previousCamPos = cam.position;
        parallaxScales = new float[backgrounds.Length];

        // Assigning corresponding parallaxScales
        for(int i = 0; i < backgrounds.Length; i++) {
            parallaxScales[i] = backgrounds[i].position.z*-1;
        }
    }

    // Update is called once per frame
    void Update() {
        
        // For each background
        for(int i = 0; i < backgrounds.Length; i++) {
            // The parallax is the opposite of the cameramovement because the previous frame multiplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            //float yParallax = (cam.position.y - previousCamPos.y) * parallaxScales[i];

            // Set a target x position which is the current position + the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            //float backgroundTargetPosY = backgrounds[i].position.y + yParallax;

            // Create a target position which is the backgrounds current position with its target x position, z is the same
            Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // Fade between current position and the target position using lerp, time gör så att det inte är olika för olika datorer(istället för frames)
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // Set the previousCamPos to the cameras position at the end of the frames(update)
        previousCamPos = cam.position;
    }
}

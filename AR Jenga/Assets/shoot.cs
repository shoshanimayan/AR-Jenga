using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class shoot : MonoBehaviour
{

	private ARSessionOrigin aROrigin;
	private ARRaycastManager m_RaycastManager;
    public Material Material1;
    public Material Material2;
    bool hitting=false;
    GameObject hit;
    GameObject previous;
    bool directionChosen=false;
    private Vector3 startPosition;
    private Vector3 direction;
    private float startTime;
    private float endTime;
    private float duration;
    Camera ARCam;

    public float m_ThrowForce = 100f;

    // X and Y axis damping factors for the throw direction
    public float m_ThrowDirectionX = 0.17f;
    public float m_ThrowDirectionY = 0.67f;

    private void Awake()
    {
        ARCam = Camera.main;
    }



    private void Update()
	{
       // Debug.Log(hitting);
		//var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
		//Ray ray = Camera.main.ScreenPointToRay(screenCenter);
		RaycastHit hits;
		// if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space))
		//  {
		

		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hits, 100.0f))
		{

			 hit = hits.transform.gameObject;
            if (hit.transform.tag == "block")
            {
                if (previous == default) { previous = hit; }
                if (hit != previous) { previous.GetComponent<MeshRenderer>().material = Material2; }


               // Debug.Log(hit.name);
                hitting = true;
                hit.GetComponent<MeshRenderer>().material = Material1;
                previous = hit;
            }
            else {
                hit = null;
                hitting = false;
              //  Debug.Log("miss");
                previous.GetComponent<MeshRenderer>().material = Material2;
            }
            

		}
		else {
            hit = null;
            hitting = false;
          //  Debug.Log("miss");
            previous.GetComponent<MeshRenderer>().material = Material2;

        }
        if (hitting) {
            if (Input.GetMouseButtonDown(0))
            { // Works for both Mouse and Touch on Mobile, when we press/touch
                startPosition = Input.mousePosition;
                startTime = Time.time;
                directionChosen = false;
                Debug.Log("down");
            }
            // We've ended the touch of the screen, which will end collecting info about the ball throw
            else if (Input.GetMouseButtonUp(0))
            { // Works for both Mouse and Touch, when we release click/touch
                endTime = Time.time;
                duration = endTime - startTime;
                direction = Input.mousePosition - startPosition;
                directionChosen = true;
                Debug.Log("up");
            }

            // Direction was chosen, which will release/throw the ball
            if (directionChosen)
            {
                hit.transform.GetComponent<Rigidbody>().isKinematic = false;
                hit.transform.GetComponent<Rigidbody>().mass = .5f;
                hit.transform.GetComponent<Rigidbody>().useGravity = true;
                Debug.Log(direction);


                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    if (direction.x > 0)
                        direction = new Vector3(1, 0, 0);
                    else
                        direction = new Vector3(-1, 0, 0);

                }
                else {

                    if (direction.y > 0)
                        direction = new Vector3(0, 1, 0);
                    else
                        direction = new Vector3(0, -1, 0);
                }
                Debug.Log(direction.magnitude);
                if (direction.magnitude==0f) {
                    direction = new Vector3(0, 0, 1);
                }
                Debug.Log(direction);

                hit.transform.GetComponent<Rigidbody>().AddForce(direction*250);

                /*hit.transform.GetComponent<Rigidbody>().AddForce(
                    ARCam.transform.forward * m_ThrowForce / duration +
                    ARCam.transform.up * direction.y * m_ThrowDirectionY +
                    ARCam.transform.right * direction.x * m_ThrowDirectionX);
*/
                startTime = 0.0f;
                duration = 0.0f;

                startPosition = new Vector3(0, 0, 0);
                direction = new Vector3(0, 0, 0);

                directionChosen = false;
            }


        }

		// }


	}


}
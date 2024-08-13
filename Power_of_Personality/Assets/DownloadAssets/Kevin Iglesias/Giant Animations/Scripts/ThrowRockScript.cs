using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinIglesias
{
    public class ThrowRockScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject rock;
        
        [SerializeField]
        private Transform throwingHand;
        
        [SerializeField]
        private Transform spawnPoint;
        
        [SerializeField]
        private Transform target;
        
        [SerializeField]
        private float speed;
        
        [SerializeField]
        private float height;
        
        private IEnumerator throwRockCoroutine;
        
        public void SpawnRock()
        {
            rock.transform.position = spawnPoint.position;
            rock.transform.rotation = Quaternion.identity;
            rock.transform.SetParent(transform);
            rock.SetActive(true);
        }
        
        public void GrabRock()
        {
            rock.transform.SetParent(throwingHand);
        }
        
        public void ThrowRock()
        {
            rock.transform.SetParent(null);
            if(throwRockCoroutine != null)
            {
                StopCoroutine(throwRockCoroutine);
            }
            throwRockCoroutine = ThrowRockCoroutine();
            StartCoroutine(throwRockCoroutine);
        }
        
        public void RemoveRock()
        {
            if(throwRockCoroutine != null)
            {
                StopCoroutine(throwRockCoroutine);
            }
            rock.SetActive(false);
        }
        
        IEnumerator ThrowRockCoroutine()
        {
            Vector3 initPosition = rock.transform.position;
            
            Vector3 midPoint = (initPosition + target.position) / 2f;
            midPoint.y += height;

            float i = 0;
            while(i < 1)
            {
                i += Time.deltaTime * speed;
                
                //Movement
                rock.transform.position = Parabola(initPosition, midPoint, target.position, i);
                
                //Rotation
                Vector3 forwardVector = CalculateParabolaDirection(initPosition, midPoint, target.position, i);
                rock.transform.rotation = Quaternion.LookRotation(forwardVector, Vector3.up);

                yield return null;
            }
        }
        

        private Vector3 Parabola(Vector3 start, Vector3 mid, Vector3 end, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 result = (uu * start) + (2 * u * t * mid) + (tt * end);
            return result;
        }
        
        private Vector3 CalculateParabolaDirection(Vector3 start, Vector3 mid, Vector3 end, float t)
        {
            float u = 1 - t;
            float uu = u * u;
            float tt = t * t;

            Vector3 tangent = 2 * ((u * (mid - start)) + (t * (end - mid)));

            return tangent.normalized;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GamePlayer : MonoBehaviourPunCallbacks
{
    GameObject othelloManager;
    byte phase = 2;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.NickName = "Master";
        }
        else
        {
            PhotonNetwork.NickName = "Local";
        }

        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        othelloManager = GameObject.Find("OthelloManager(Clone)");
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray();
            RaycastHit2D hit = new RaycastHit2D();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit.collider.CompareTag("Othello"))
            {
                if (phase == 2)
                {
                    byte pos = hit.collider.gameObject.GetComponent<Othello>().GetId();

                    byte x, y;
                    x = (byte)(pos % 8);
                    y = (byte)(pos / 8);

                    othelloManager.GetComponent<OthelloManager>().PutBomb(x, y);
                }
                else if (phase == 3)
                {
                    byte pos = hit.collider.gameObject.GetComponent<Othello>().OnUserPush();

                    if (pos != 100)
                    {
                        byte x, y;
                        x = (byte)(pos % 8);
                        y = (byte)(pos / 8);

                        othelloManager.GetComponent<OthelloManager>().PushOthello(PhotonNetwork.NickName, x, y);
                    }
                }
            }
        }
    }
}

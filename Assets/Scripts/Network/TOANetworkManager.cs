using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TOANetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        TOANetworkPlayer player = conn.identity.GetComponent<TOANetworkPlayer>();
        player.SetDisplayName($"Player {numPlayers}");

        //TODO: make sure no other player has the same color
        Color displayColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        player.SetDisplayColor(displayColor);

    }
}

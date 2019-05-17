using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameIntefaceController : MonoBehaviour {

    public TextMeshProUGUI TopPanelRecordLabel;

    public TextMeshProUGUI TopPanelPointsLabel;
    public TextMeshProUGUI ResultPanelPointsLabel;

    public TextMeshProUGUI TopPanelLifesLabel;

    public void UpdatePoints(int points)
    {
        TopPanelPointsLabel.text = ResultPanelPointsLabel.text = points.ToString();
    }

    public void UpdateLifes(int lifes)
    {
        TopPanelLifesLabel.text = lifes.ToString();
    }

    public void UpdateRecord(int points)
    {
        TopPanelRecordLabel.text = points.ToString();
    }
}

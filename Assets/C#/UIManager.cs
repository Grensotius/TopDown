using UnityEngine;
using UnityEngine.UI;
public class UIManager: MonoBehaviour
{
    [SerializeField]
    private SOData _sOData;
    [SerializeField]
    private Text _text1;
    [SerializeField]
    private Text _text2;
    [SerializeField]
    private Text _textRound;
    
    private void Start()
    {
        _text1.text = _sOData._score1.ToString();
        _text2.text = _sOData._score2.ToString();
        _textRound.text = _sOData._round.ToString();
    }
    public void AddScore()
    {
        _text1.text = _sOData._score1.ToString();
        _text2.text = _sOData._score2.ToString();
        _textRound.text = _sOData._round.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLineManager : MonoBehaviour
{
    //Static　参照
    public static RenderLineManager RenderLineManagerInstance;

    [SerializeField] List<LineRenderer> L_PieceLine = null;

    public bool ClearFlag = false;

    public Color ClaerLineColor =  new Color();

    [System.Serializable]
    //inspector　でも見れる多段階配列
    public class LinePosition {

        public List<Vector3> L_LinePositionData = new List<Vector3>();

        public LinePosition()
        {
        }

        public LinePosition(List<Vector3> listData)
        {
            L_LinePositionData = listData;
        }
    }

    //終了時の線の位置データ
    public List<LinePosition> LinePositions = new List<LinePosition>();

    private void Awake()
    {
        if(RenderLineManagerInstance == null)
        {
            RenderLineManagerInstance = this;
            
        } 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            foreach (var Data in L_PieceLine)
            {
                for(int i = 0; i < Data.positionCount;i++)
                {
                   Debug.Log(Data.GetPosition(i));
                }
                
            }
        }

        if (ClearFlag == true)
        {
            ClearFlag = false;


            for (int j = 0 ; j < L_PieceLine.Count ;j++)
            {
                LinePosition linevec = new LinePosition();

                for (int i = 0; i < L_PieceLine[j].positionCount; i++)
                {
                    linevec.L_LinePositionData.Add(L_PieceLine[j].GetPosition(i));
                }

                LinePositions.Add(linevec);

            }

            foreach (var Data in L_PieceLine)
            {
                for (int i = 0; i < Data.positionCount; i++)
                {

                    Data.startColor = ClaerLineColor;
                    Data.endColor = ClaerLineColor;
                }

            }

            StartCoroutine(Move());
        }
    }
    IEnumerator Move()
    {
        while (true)
        {
            //Debug.Log("コルーチン回ってます。");
            for (int j = 0; j < L_PieceLine.Count; j++)
            {

                L_PieceLine[j].positionCount = LinePositions[j].L_LinePositionData.Count;

                var l_lotation =  L_PieceLine[j].transform.localRotation;

                l_lotation = Quaternion.AngleAxis(0.1f,Vector3.down);

                L_PieceLine[j].transform.localRotation = L_PieceLine[j].transform.localRotation *l_lotation;

                for (int i = 0; i < L_PieceLine[j].positionCount; i++)
                {
                    

                    Vector3 point = L_PieceLine[j].transform.localToWorldMatrix * new Vector4(LinePositions[j].L_LinePositionData[i].x, LinePositions[j].L_LinePositionData[i].y, LinePositions[j].L_LinePositionData[i].z, 1);
                    L_PieceLine[j].SetPosition(i, point);
                }

            }
            yield return null;
        }

        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;
public class Character : MonoBehaviour
{

    private Data GameData;
    private CharacterData MyData;

    public int Index = -1;

    private GameObject GameController;
    private GameObject Caffe;

    private int Distance = 0;
    private int Direction;
    private float[] Position = new float[3];
    private void Moving(int direction)
    {
        // caffe �߽��� ��������
        // x �� +- 500
        // y �� +- 900
        if (Caffe != null)
        {
            switch (direction)
            {
                // �� o
                case 0:
                    transform.Translate(0.0f, 0.0f, 0.005f * 1.0f);
                    transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                    break;
                // �»� o
                case 1:
                    transform.Translate(0.005f * -0.75f, 0.005f * 0.75f, 0.0f);
                    transform.rotation = Quaternion.Euler(0.0f, -45.0f, 0.0f);
                    break;
                // �� o
                case 2:
                    transform.Translate(0.0f, 0.005f * 1.0f, 0.0f);
                    transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    break;
                // ��� o
                case 3:
                    transform.Translate(0.005f * 0.75f, 0.005f * 0.75f, 0.0f);
                    transform.rotation = Quaternion.Euler(0.0f, 45.0f, 0.0f);
                    break;
                // �� o
                case 4:
                    transform.Translate(0.0f, 0.0f, 0.005f * 1.0f);
                    transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                    break;
                // ���� o
                case 5:
                    transform.Translate(0.005f * -0.25f, 0.005f * -0.75f, 0.005f * 0.25f);
                    transform.rotation = Quaternion.Euler(0.0f, 135.0f, 0.0f);
                    // x -x -z
                    // y y
                    // z -x z
                    break;
                // �� o
                case 6:
                    transform.Translate(0.0f, 0.005f * -1.0f, 0.0f);
                    transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    break;
                // ���� o
                case 7:
                    transform.Translate(0.005f * 0.25f, 0.005f * -0.75f, 0.005f * 0.25f);
                    transform.rotation = Quaternion.Euler(0.0f, -135.0f, 0.0f);
                    break;
                // ���� o
                case 8:
                    transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    break;
                // ���ۺ���
                case 9:
                    transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
                    break;
                default:
                    break;
            }

            // Ʋ ���� ����� �ʵ��� ��
            if (transform.localPosition.y <= -750.0f)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, -750.0f, 0.0f);
                Distance = -1;
            }
            if (transform.localPosition.y >= 500.0f)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, 500.0f, 0.0f);
                Distance = -1;
            }
            if (transform.localPosition.x >= 500.0f)
            {
                transform.localPosition = new Vector3(500.0f, transform.localPosition.y, 0.0f);
                Distance = -1;
            }
            if (transform.localPosition.x <= -500.0f)
            {
                transform.localPosition = new Vector3(-500.0f, transform.localPosition.y, 0.0f);
                Distance = -1;
            }

            // ĳ������ ��ġ ������ ����
            Position[0] = transform.localPosition.x;
            Position[1] = transform.localPosition.y;
            Position[2] = transform.localPosition.z;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.Find("GameController").gameObject;
        GameData = GameController.GetComponent<GameController>().GAMEDATA;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (GameController == null) GameController = GameObject.Find("GameController").gameObject;
        else
        {
            if (GameData == null) GameData = GameController.GetComponent<GameController>().GAMEDATA;
            else
            {
                if (MyData == null && Index != -1) MyData = GameData.CharacterDatas[Index];
                else
                {
                    if (Caffe == null) Caffe = GameObject.Find("Caffe").gameObject;

                    if (GameController.GetComponent<GameController>().NOWCHARACTER == gameObject) { }
                    else
                    {
                        if (Distance <= 0)
                        {
                            Distance = Random.Range(500, 1600);
                            Direction = Random.Range(0, 9);
                        }
                        // �������� �־��� �Ÿ���ŭ�� ���� �������� �̵�
                        Distance -= 1;
                        Moving(Direction);

                        MyData.Position = Position;
                    }
                }
            }
        }
    }
}

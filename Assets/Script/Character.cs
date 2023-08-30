using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;
public class Character : MonoBehaviour
{
    private GameObject GameController;
    private GameObject Caffe;

    private int Distance = 0;
    private int Direction;
    private void Moving(int direction)
    {
        // caffe �߽��� ��������
        // x �� +- 500
        // y �� +- 900
        if (Caffe != null)
        {
            switch(direction)
            {
                // ��
                case 0:
                    transform.Translate(0.005f * -1.0f, 0.0f, 0.0f);
                    transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                    break;
                // �»�
                case 1:
                    transform.Translate(0.005f * -0.75f, 0.005f * 0.75f, 0.0f);
                    transform.rotation = Quaternion.Euler(0.0f, -45.0f, 0.0f);
                    break;
                // ��
                case 2:
                    transform.Translate(0.0f, 0.005f * 1.0f, 0.0f);
                    transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    break;
                // ���
                case 3:
                    transform.Translate(0.005f * 0.75f, 0.005f * 0.75f, 0.0f);
                    transform.rotation = Quaternion.Euler(0.0f, 45.0f, 0.0f);
                    break;
                // ��
                case 4:
                    transform.Translate(0.0f, 0.005f *  1.0f, 0.0f);
                    transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                    break;
                // ����
                case 5:
                    transform.Translate(0.005f * -0.75f, 0.005f * 0.75f, 0.0f);
                    transform.rotation = Quaternion.Euler(0.0f, 135.0f, 0.0f);
                    break;
                // ��
                case 6:
                    transform.Translate(0.0f, 0.005f * -1.0f, 0.0f);
                    transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    break;
                // ����
                case 7:
                    transform.Translate(0.005f * -0.75f, 0.005f * -0.75f, 0.0f);
                    transform.rotation = Quaternion.Euler(0.0f, -135.0f, 0.0f);
                    break;
                // ����
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

            if (transform.localPosition.y <= -750.0f) transform.localPosition = new Vector3(transform.localPosition.x, -750.0f, 0.0f);
            if (transform.localPosition.y >= 888.0f) transform.localPosition = new Vector3(transform.localPosition.x, 888.0f, 0.0f);
            if (transform.localPosition.x >= 500.0f) transform.localPosition = new Vector3(500.0f, transform.localPosition.y, 0.0f);
            if (transform.localPosition.x <= -500.0f) transform.localPosition = new Vector3(-500.0f, transform.localPosition.y, 0.0f);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.Find("GameController").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController == null) GameController = GameObject.Find("GameController").gameObject;
        else
        {
            if (Caffe == null) Caffe = GameObject.Find("Caffe").gameObject;

            if (GameController.GetComponent<GameController>().NOWCHARACTER == gameObject) { }
            else
            {
                if (Distance <= 0)
                {
                    Distance = Random.Range(50, 1600);
                    Direction = Random.Range(0, 9);
                }
                Distance -= 1;
                Moving(Direction);
            }
        }
    }
}

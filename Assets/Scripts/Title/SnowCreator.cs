using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SnowCreator : MonoBehaviour
{
    float time = 0;
    const float PARTICLE_TIME = 0.1f;
    int count = 0;
    [SerializeField] GameObject snow;
    [SerializeField] RectTransform Parent;
    Snowfall[] objects = new Snowfall[128];
    Vector2 ScreenSize = Vector2.zero;
    private void Awake()
    {
        ScreenSize.x = Screen.width;
        ScreenSize.y = Screen.height;
    }
    void Update()
    {
        if (time > PARTICLE_TIME)
        {
            var gameObj = getObj();
            if (gameObj != null)
                gameObj.InitObj(0, 0, ScreenSize.x, ScreenSize.y);
            time = 0f;
        }
        time += Time.deltaTime;
    }
    public Snowfall getObj()
    {
        for (int i = 0; i < count; i++)
        {
            if (!objects[i].gameObject.activeSelf)
            {
                objects[i].gameObject.SetActive(true);
                return objects[i];
            }
        }
        if (objects.Length < count) return null;
        var gameObj = Instantiate(snow);
        RectTransform rc = gameObj.GetComponent<RectTransform>();
        rc.SetParent(Parent);
        int j = count;
        count++;
        objects[j] = gameObj.GetComponent<Snowfall>();
        return objects[j];
    }
}

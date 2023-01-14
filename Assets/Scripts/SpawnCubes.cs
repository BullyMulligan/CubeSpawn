using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCubes : MonoBehaviour
{
    //инстанциируем поле префаба кубика и времени, за которое спавнится кубик
    [SerializeField] GameObject prefab; 
    
    [SerializeField] float spawnInterval = 0.04f;
    
    //ниже переменные со значением времени шага смены цвета и перехода цвета
    private float _stepRecoloring = 0.2f;
    
    private float _recoloringTime = 0.5f;

    //переменная с объектами кубов
    private List<GameObject> _cubes = new List<GameObject>();
    
    //со стартом программы запускаем корутину SpawnObjects
    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    //метод, который спавнит кубики между кадрами, то есть, корутина
    IEnumerator SpawnObjects()
    { 
        //инстанциируем внутри двух циклов, чтобы задать объектам позиции в квадрате
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                //создаем позицию со значениями, создаем ГО с заданной позицией и шаблоном префаба
                var position = new Vector3(i, 0.48f, j);
                var cube =Instantiate(prefab, position, Quaternion.identity);
                yield return new WaitForSeconds(spawnInterval);
                
                //добавляем созданный объект в массив
                _cubes.Add(cube);
            }
        }
    }

    //запускаем корутину с изменением цветов через ивент Click, запускаемый после нажатия на кнопку
    public void StartRecoloringCubes()
    {
        StartCoroutine(ChangeColors());
    }

    //корутина, меняющая света массиву объектов, который мы создали ранее
    IEnumerator ChangeColors()
    {
        //создаем рандомный цвет вне цикла, чтобы он был един для всех кубов на одно нажатие
        Color newColor = new Color(Random.value, Random.value, Random.value); // Генерируем случайный цвет

        //запускаем цикл, в котором запускаем корутину с переходом цвета каждого куба
        for (int i = 0; i < _cubes.Count; i++)
        {
            StartCoroutine(LerpColor(_cubes[i].GetComponent<Renderer>().material, newColor, _recoloringTime));
            yield return new WaitForSeconds(_stepRecoloring); // Ждём время перехода перед изменением цвета следующего кубика
        }
    }

    //корутина с переходом из цвета в цвет
    IEnumerator LerpColor(Material material, Color targetColor, float duration)
    {
        Color currentColor = material.color;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            material.color = Color.Lerp(currentColor, targetColor, t);
            yield return null;
        }
    }
}

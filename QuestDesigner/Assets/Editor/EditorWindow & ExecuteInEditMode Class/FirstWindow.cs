using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //para trabajar con cosas de editor
using System;

public class FirstWindow : EditorWindow //necesita heredar de EditorWindow
{
    private bool _toogleGroupActivated;

    private bool boolEx1;
    private string stringEx1;
    private float floatEx1;
    private int pressedTimes;
    private int newWindowAmount;
    private GameObject _toPreview;
    private Texture2D _preview;

    [MenuItem("CustomTools/MyFirstWindow")] //la ubicacion (la "ruta") de la opcion de menu que abre la ventana
    public static void OpenWindow() //el método al que accede el menú tiene que ser "static"
    {
        //consigue una instancia de una ventana del tipo especificado y la abre.
        //GetWindow(typeof(FirstWindow)).Show();

        //Si necesito guardar la referencia, GetWindow me devuelve una EditorWindow, asi que hay que castear.
        FirstWindow myWindow = (FirstWindow)GetWindow(typeof(FirstWindow));
        myWindow.wantsMouseMove = true; //necesario para que podamos detectar el mouse
        myWindow.Show();
    }

    //todas las opciones que usamos aca tambien pueden usarse en CustomInspectors
    private void OnGUI() //aca adentro vamos a hacer todo
    {
        MiscStuff();
        OptionsAndMoreOptions();
    }

    private void MiscStuff()
    {
        EditorGUILayout.LabelField("Esta es mi ventana", EditorStyles.largeLabel);
        EditorGUILayout.LabelField("es la primera", EditorStyles.centeredGreyMiniLabel);

        EditorGUILayout.Space();

        /*
         LOS TOOGLE GROUPS PERMITEN DESACTIVAR O ACTIVAR OPCIONES (DEJANDOLAS EN GRIS), SEGÚN UN BOOLEANO.
         FIJENSE QUE HAY QUE GUARDAR LAS VARIABLES EN ALGUN LADO, SI NO LOS CAMBIOS NO SE MANTIENEN!
        todos los grupos inician con BeginToggleGroup(...) y terminan con EndToggleGroup(); lo del medio es lo que se activa/desactiva
        */
        _toogleGroupActivated = EditorGUILayout.BeginToggleGroup("Un toogle group", _toogleGroupActivated);
        boolEx1 = EditorGUILayout.Toggle("Un bool", boolEx1);
        stringEx1 = EditorGUILayout.TextField("un texto", stringEx1);
        floatEx1 = EditorGUILayout.FloatField("un float", floatEx1);
        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.LabelField("Apreté EL boton " + pressedTimes + " veces");
        var buttonRes = GUILayout.Button("Esto es un botón");
        if (buttonRes)
            pressedTimes++;

        //como GUILayout.Button devuelve un bool, lo puedo meter directo dentro del if
        if (GUILayout.Button("ESTE SACA CLICKS", GUILayout.Width(200), GUILayout.ExpandWidth(false), GUILayout.Height(100)))
            pressedTimes--;
    }


    private void OptionsAndMoreOptions()
    {
        //para cerrar la ventana, usamos la función Close() --> viene de EditorWindow
        if (GUILayout.Button("Cerrar la ventana"))
            Close();


        //puedo abrir otra ventana de varias formas
        if (GUILayout.Button("Abrir otra ventana"))
        {
            //ver dentro de MoveAlongWindow para explicacion del por que almacenamos este parámetro aca
            MoveAlongWindow.OpenWindow(newWindowAmount);
            newWindowAmount++;
        }

        if (GUILayout.Button("Otra forma de abrir ventanas"))
            GetWindow(typeof(EmptyWindow)).Show();


        //las ventanas abiertas como pop up no tienen marco y no son draggeables
        if(GUILayout.Button("Y hacer que ventanas actúen como POP-UPS"))
        {
            var w = ScriptableObject.CreateInstance<EmptyWindow>();
            w.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
            w.ShowPopup();
        }

        EditorGUILayout.Space();

        //Podemos saber si el usuario tiene el foco en esta ventana
        if (focusedWindow == this)
            EditorGUILayout.LabelField("TENGO EL FOCO EN ESTA VENTANA");
        //o si tiene el mouse sobre esta ventana
        if (mouseOverWindow == this)
            EditorGUILayout.LabelField("EL MOUSE ESTA SOBRE ESTA VENTANA");

        //conseguimos la posicion del mouse y la imprimimos.
        EditorGUILayout.LabelField("Posición del mouse: ", Event.current.mousePosition.ToString());
        //si detectamos que el mouse se mueve, reimprimimos. REQUIERE EL wantsMouseMove = true;
        if (Event.current.type == EventType.MouseMove) Repaint();

        //podemos modificar el tamaño de la ventana a uno específico
        maxSize = new Vector2(300, 500);
        minSize = new Vector2(300, 500);
        //podemos obtener el rect actual de la ventana mediante la propiedad position

        //podemos mostrar un preview
        _toPreview = (GameObject)EditorGUILayout.ObjectField("Mostrar: ", _toPreview, typeof(GameObject), true);
        var prevState = _preview;
        _preview = AssetPreview.GetAssetPreview(_toPreview);
        if (_preview != null)
        {
            if(prevState == null)
                Repaint();
            EditorGUILayout.BeginHorizontal();
            GUI.DrawTexture(GUILayoutUtility.GetRect(50, 50, 50, 50), _preview, ScaleMode.ScaleToFit);
            GUILayout.Label(_toPreview.name);
            GUILayout.Label(AssetDatabase.GetAssetPath(_toPreview));
            EditorGUILayout.EndHorizontal();
        }
        else
            EditorGUILayout.LabelField("NOTHING");

    }

    void OnDestroy() { } // Se llama cuando se cierra la ventana
    void OnFocus() { } // Se llama cuando pongo el foco
    void OnLostFocus() { } // Se llama cuando se pierde el foco
    void OnHierarchyChange() { } // Se llama cuando cambia la jeraquía
    void OnInspectorUpdate() { } // Se llama 10 frames por segundo para que se pueda updatear el inspector
    void Update() { } // Se llama varias veces por segundo en todas las ventanas visibles
}
